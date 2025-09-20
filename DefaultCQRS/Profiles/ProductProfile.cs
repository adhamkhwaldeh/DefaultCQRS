using AutoMapper;
using DefaultCQRS.DTOs;
using DefaultCQRS.Entities;

namespace DefaultCQRS.Profiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductDto>();
            CreateMap<CreateProductDto, Product>();
            CreateMap<UpdateProductDto, Product>();
        }
    }
}
