using BusinessLayer.Dtos;
using BusinessLayer.Mapping;
using DataAccessLayer;
using DataAccessLayer.Entities;
using Infrastructure.Exceptions;

namespace BusinessLayer.Services
{
    public class GamingPubService
    {
        private readonly UnitOfWork unitOfWork;

        public GamingPubService(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public GamingPub GetPub(int id)
        {
            var pub = unitOfWork.GamingPubs.GetById(id);
            if(pub == null)
            {
                throw new ResourceMissingException($"Gaming pub with id {id} not found");
            }
            pub.Address = unitOfWork.Address.GetById(pub.AddressId);
            return pub;
        }

        public List<GamingPub> GetAll()
        {
            var gamingPubs = unitOfWork.GamingPubs.GetAll();
            foreach (var pub in gamingPubs)
            {
                pub.Address = unitOfWork.Address.GetById(pub.AddressId);
            }
            return gamingPubs;
        }

        public bool AddGamingPub(AddGamingPubDto gamingPubDto)
        {
            var foundGamingPub = unitOfWork.GamingPubs.GetPubByName(gamingPubDto.Name);

            if (foundGamingPub != null)
            {
                throw new ForbiddenException($"'{gamingPubDto.Name}' gaming pub already exists");
            }

            GamingPub newGamingPub = gamingPubDto.ToGamingPub();

            if (newGamingPub.Address != null)
            {
                unitOfWork.Address.Insert(newGamingPub.Address);
            }

            unitOfWork.GamingPubs.Insert(newGamingPub);

            unitOfWork.SaveChanges();

            return true;
        }

        public bool UpdateGamingPub(int gamingPubId, UpdateGamingPubDto gamingPubDto)
        {
            var foundGamingPub = unitOfWork.GamingPubs.GetById(gamingPubId);

            if (foundGamingPub == null)
            {
                throw new ResourceMissingException($"Gaming pub with id {gamingPubId} not found");
            }

            if (!string.IsNullOrEmpty(gamingPubDto.Name))
            {
                foundGamingPub.Name = gamingPubDto.Name;
            }

            if (gamingPubDto.AddAdressDto != null)
            {
                foundGamingPub.Address = gamingPubDto.AddAdressDto.ToAddress();
            }

            if (!string.IsNullOrEmpty(gamingPubDto.PhoneNumber))
            {
                foundGamingPub.PhoneNumber = gamingPubDto.PhoneNumber;
            }

            unitOfWork.SaveChanges();

            return true;
        }

        public bool DeleteGamingPub(int gamingPubId)
        {
            var foundGamingPub = unitOfWork.GamingPubs.GetById(gamingPubId);

            if (foundGamingPub == null)
            {
                new ResourceMissingException($"Gaming pub with id {gamingPubId} not found");
            }

            if (foundGamingPub.AddressId.HasValue)
            {
                foundGamingPub.Address = unitOfWork.Address.GetById(foundGamingPub.AddressId.Value);
                unitOfWork.Address.Remove(foundGamingPub.Address);
            }

            foundGamingPub.Reservations = unitOfWork.Reservations.GetAllReservations(foundGamingPub);

            unitOfWork.GamingPubs.Remove(foundGamingPub);

            unitOfWork.SaveChanges();

            return true;
        }
    }
}