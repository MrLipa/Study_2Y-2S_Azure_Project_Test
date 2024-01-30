using AutoMapper;
using Project.Models;

namespace Project.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles() 
        {
            CreateMap<AppUser, AppUserDto>().ReverseMap();
            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<Meal, MealDto>().ReverseMap();
            CreateMap<UserMeal, UserMealDto>().ReverseMap();
            CreateMap<MealProduct, MealProductDto>().ReverseMap();
        }
    }
}
