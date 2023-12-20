using AutoMapper;
using HotelListing.API.Data;
using HotelListing.API.Models.Country;

namespace HotelListing.API.Configurations
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<Country, CreateCountryDto>().ReverseMap(); // ReverseMap() is used to map in both direction
        }
    }
}
