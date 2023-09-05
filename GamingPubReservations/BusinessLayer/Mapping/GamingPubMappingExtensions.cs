using BusinessLayer.Dtos;
using DataAccessLayer.Entities;

namespace BusinessLayer.Mapping
{
    public static class GamingPubMappingExtensions
    {
        public static GamingPub ToGamingPub(this AddGamingPubDto addGamingPubDto)
        {
            return new GamingPub
            {
                Name = addGamingPubDto.Name,
                Address = addGamingPubDto.AddAdressDto.ToAddress(),
                PhoneNumber = addGamingPubDto.PhoneNumber
            };
        }
    }
}