using BusinessLayer.Dtos;
using DataAccessLayer.Entities;

namespace BusinessLayer.Mapping
{
    public static class AddressMappingExtensions
    {
        public static Address ToAddress(this AddAddressDto addressDto)
        {
            Address address = new Address
            {
                Country = addressDto.Country,
                City = addressDto.City,
                Street = addressDto.Street,
                ZipCode = addressDto.ZipCode,
                Number = addressDto.Number
            };

            return address;
        }
    }
}