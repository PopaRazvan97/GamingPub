using BusinessLayer.Dtos;
using BusinessLayer.Mapping;
using DataAccessLayer;
using DataAccessLayer.Entities;
using Infrastructure.Exceptions;

namespace BusinessLayer.Services
{
    public class GamingPlatformService
    {
        private readonly UnitOfWork unitOfWork;
        public GamingPlatformService(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public List<GamingPlatform> GetAll()
        {
            return unitOfWork.GamingPlatforms.GetAll();
        }

        public GamingPlatform GetPlatform(int id)
        {
            var platform = unitOfWork.GamingPlatforms.GetById(id);
            if(platform == null)
            {
                throw new ResourceMissingException($"Gaming platform with id {id} not found");
            }
            return platform;
        }

        public bool AddGamingPlatform(AddGamingPlatformDto gamingPlatform)
        {
            var foundPlatform = unitOfWork.GamingPlatforms.GetAll().Where(x => x.Name == gamingPlatform.Name).FirstOrDefault();
            if (foundPlatform != null)
            {
                throw new ForbiddenException($"'{gamingPlatform.Name}' gaming platform already exists");
            }
            GamingPlatform newGamingPlatform = gamingPlatform.ToGamingPlatform();
            unitOfWork.GamingPlatforms.Insert(newGamingPlatform);
            unitOfWork.SaveChanges();
            return true;
        }

        public bool RemovePlatformById(IdDto platform)
        {
            var foundPlatform = unitOfWork.GamingPlatforms.GetById(platform.Id);
            if (foundPlatform == null)
            {
                throw new ResourceMissingException($"Gaming platform with id {platform.Id} not found");
            }
            
            unitOfWork.GamingPlatforms.Remove(foundPlatform);
            unitOfWork.SaveChanges();
            return true;
        }

        public bool UpdatePlatform(UpdateGamingPlatformDto gamingPlatform)
        {
            var foundPlatform = unitOfWork.GamingPlatforms.GetById(gamingPlatform.Id);
            if (foundPlatform == null)
            {
                throw new ResourceMissingException($"Gaming platform with id {gamingPlatform.Id} not found");
            }
            foundPlatform.Name = gamingPlatform.Name;
            unitOfWork.SaveChanges();
            return true;
        }
    }
}
