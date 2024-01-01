using AutoMapper;
using Lab06.BLL.EntitiesDTO;
using Lab06.Web.ViewModels;
using static Lab06.BLL.Helpers.ImageConverterHelper;

namespace Lab06.Web.Mappings
{
    public class AddParkAttractionProfile : Profile
    { 
        public AddParkAttractionProfile()
        {
            CreateMap<AddParkAttractionViewModel, ParkAttractionDto>().ForMember(
                d => d.Image,
                opt => opt.MapFrom(src => FromFormFileToByteArray(src.Image)));

            CreateMap<ParkAttractionDto, AddParkAttractionViewModel>().ForMember(
                d => d.Image,
                opt => opt.MapFrom(src => FromByteArrayToFormFile(src.Image)));

        }
    }
}
