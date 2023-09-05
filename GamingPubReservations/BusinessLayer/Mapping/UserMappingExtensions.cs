using BusinessLayer.Dtos;
using DataAccessLayer.Entities;

namespace BusinessLayer.Mapping
{
    public static class UserMappingExtensions
    {
        public static User ToUser(this RegisterDto registerDto)
        {
            return new User
            {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                Email = registerDto.Email,
                DateOfBirth = registerDto.DateOfBirth,
                PasswordHash = registerDto.Password,
                PhoneNumber = registerDto.PhoneNumber,
                Role = registerDto.Role,
                Address = registerDto.AddAdressDto.ToAddress()
            };
        }
    }
}