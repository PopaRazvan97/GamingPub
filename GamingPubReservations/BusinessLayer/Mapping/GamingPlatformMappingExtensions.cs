using BusinessLayer.Dtos;
using DataAccessLayer.Entities;

namespace BusinessLayer.Mapping
{
    public static class GamingPlatformMappingExtensions
    {
        public static GamingPlatform ToGamingPlatform(this AddGamingPlatformDto addGamingPubDto)
        {
            return new GamingPlatform
            {
                Name = addGamingPubDto.Name
            };
        }
    }
}
