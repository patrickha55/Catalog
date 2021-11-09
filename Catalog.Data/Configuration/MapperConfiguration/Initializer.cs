using AutoMapper;
using Catalog.Data.Entities;
using Catalog.Data.DTOs;

namespace Catalog.Data.Configuration.MapperConfiguration
{
    public class Initializer : Profile
    {
        public Initializer()
        {
            CreateMap<Item, ItemDTO>().ReverseMap();
            CreateMap<Item, ManageItemDTO>().ReverseMap();
        }
    }
}