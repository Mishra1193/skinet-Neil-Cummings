using System;
using API.Dtos;
using Core.Entities;

namespace API.Extensions
{
    public static class AddressMappingExtensions
    {
        public static AddressDto ToDto(this Address address)
        {
            if (address is null) throw new ArgumentNullException(nameof(address));

#pragma warning disable CS8601 // Possible null reference assignment.
            return new AddressDto
            {
                Line1 = address.Line1,
                Line2 = address.Line2,
                City = address.City,
                State = address.State,
                PostalCode = address.PostalCode,
                Country = address.Country
            };
#pragma warning restore CS8601 // Possible null reference assignment.
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
