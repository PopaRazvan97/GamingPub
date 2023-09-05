using BusinessLayer.Dtos;
using BusinessLayer.Mapping;
using DataAccessLayer;
using DataAccessLayer.Entities;
using Infrastructure.Exceptions;

namespace BusinessLayer.Services
{
    public class UserService
    {
        private readonly UnitOfWork unitOfWork;
        private readonly AuthorizationService authorizationService;

        public UserService(UnitOfWork unitOfWork, AuthorizationService authorizationService)
        {
            this.unitOfWork = unitOfWork;
            this.authorizationService = authorizationService;
        }

        public List<User> GetAll()
        {
            var customers = unitOfWork.Users.GetAll();
            foreach (var user in customers)
            {
                user.Address = user.AddressId.HasValue ? unitOfWork.Address.GetById(user.AddressId.Value) : null;
            }
            return customers;
        }

        public bool Register(RegisterDto registerUser)
        {
            var foundUser = unitOfWork.Users.GetUserByEmail(registerUser.Email);

            var passwordHash = authorizationService.HashPassword(registerUser.Password);

            registerUser.Password = passwordHash;

            if (foundUser != null)
            {
                throw new ForbiddenException("Email is already in use");
            }

            User newUser = registerUser.ToUser();

            unitOfWork.Users.AddUser(newUser);
            unitOfWork.SaveChanges();

            return true;
        }

        public string ValidateLogin(LoginDto loginData)
        {
            var user = unitOfWork.Users.GetUserByEmail(loginData.Email);
            if (user == null)
            {
                throw new ForbiddenException("Wrong email or password");
            }

            var isPasswordOk = authorizationService.VerifyHashedPassword(user.PasswordHash, loginData.Password);
            if (isPasswordOk)
            {
                var role = user.Role;
                return authorizationService.GetToken(user, role);
            }
            else
            {
                throw new ForbiddenException("Wrong email or password");
            }
        }

        public bool RemoveUserById(IdDto customer)
        {
            var foundUser = unitOfWork.Users.GetUserById(customer.Id);
            if (foundUser != null)
            {
                if (foundUser.AddressId.HasValue)
                {
                    foundUser.Address = unitOfWork.Address.GetById(foundUser.AddressId.Value);
                    unitOfWork.Address.Remove(foundUser.Address);
                }
                unitOfWork.Users.RemoveUser(foundUser);
                unitOfWork.SaveChanges();
                return true;
            }

            throw new ResourceMissingException($"User with id {customer.Id} not found");
        }

        public bool UpdateUser(UpdateUserDto updatedUser)
        {
            var foundUser = unitOfWork.Users.GetUserById(updatedUser.Id);
            if (foundUser == null)
            {
                throw new ResourceMissingException($"User with id {updatedUser.Id} not found");
            }
            var otherUser = unitOfWork.Users.GetUserByEmail(updatedUser.Email);
            if (otherUser != null)
            {
                throw new ForbiddenException("New email already in use");
            }
            var foundAdress = foundUser.AddressId.HasValue ? unitOfWork.Address.GetById(foundUser.AddressId.Value) : null;

            if (!string.IsNullOrEmpty(updatedUser.FirstName))
            {
                foundUser.FirstName = updatedUser.FirstName;
            }
            if (!string.IsNullOrEmpty(updatedUser.LastName))
            {
                foundUser.LastName = updatedUser.LastName;
            }
            if (!string.IsNullOrEmpty(updatedUser.PhoneNumber))
            {
                foundUser.PhoneNumber = updatedUser.PhoneNumber;
            }
            if (!string.IsNullOrEmpty(updatedUser.Password))
            {
                var passwordHash = authorizationService.HashPassword(updatedUser.Password);
                foundUser.PasswordHash = passwordHash;
            }
            if (!string.IsNullOrEmpty(updatedUser.Email))
            {
                foundUser.Email = updatedUser.Email;
            }
            if (updatedUser.WillUpdateAdress)
            {
                foundAdress.Country = updatedUser.AddAdressDto.Country;
                foundAdress.City = updatedUser.AddAdressDto.City;
                foundAdress.Street = updatedUser.AddAdressDto.Street;
                foundAdress.ZipCode = updatedUser.AddAdressDto.ZipCode;
                foundAdress.Number = updatedUser.AddAdressDto.Number;
            }

            unitOfWork.SaveChanges();
            return true;
        }
    }
}