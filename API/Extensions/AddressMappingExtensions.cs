using System;
using API.Dtos;
using Core.Entities;

namespace API.Extensions
{
    public static class AddressMappingExtensions
    {
<<<<<<< HEAD
        public static AddressDto ToDto(this Address address)
        {
            if (address is null) throw new ArgumentNullException(nameof(address));

#pragma warning disable CS8601 // Possible null reference assignment.
=======
        // Return nullable DTO so GetUserInfo can safely return null when no address is stored
        public static AddressDto? ToDto(this Address? address)
        {
            if (address is null) return null;

>>>>>>> 808235488e37d5290f340be4b215e065c4e1be6e
            return new AddressDto
            {
                Line1 = address.Line1,
                Line2 = address.Line2,
                City = address.City,
                State = address.State,
                PostalCode = address.PostalCode,
                Country = address.Country
            };
<<<<<<< HEAD
#pragma warning restore CS8601 // Possible null reference assignment.
=======
>>>>>>> 808235488e37d5290f340be4b215e065c4e1be6e
        }

        public static Address ToEntity(this AddressDto dto)
        {
            if (dto is null) throw new ArgumentNullException(nameof(dto));

            return new Address
            {
                Line1 = dto.Line1,
                Line2 = dto.Line2,
                City = dto.City,
                State = dto.State,
                PostalCode = dto.PostalCode,
                Country = dto.Country
            };
        }

        public static void UpdateFromDto(this Address address, AddressDto dto)
        {
            if (address is null) throw new ArgumentNullException(nameof(address));
            if (dto is null) throw new ArgumentNullException(nameof(dto));

            address.Line1 = dto.Line1;
            address.Line2 = dto.Line2;
            address.City = dto.City;
            address.State = dto.State;
            address.PostalCode = dto.PostalCode;
            address.Country = dto.Country;
        }
    }
}
