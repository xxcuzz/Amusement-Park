using AutoMapper;
using Lab06.BLL.EntitiesDTO;
using Lab06.Web.ViewModels;
using static Lab06.BLL.Helpers.ImageConverterHelper;

namespace Lab06.Web.Mappings
{
    public class EditParkAttractionProfile : Profile
    {
        public EditParkAttractionProfile()
        {
            CreateMap<EditParkAttractionViewModel, ParkAttractionDto>().ForMember(
                d => d.Image,
                opt => opt.MapFrom(src => FromFormFileToByteArray(src.Image)));

            CreateMap<ParkAttractionDto, EditParkAttractionViewModel>().ForMember(
                d => d.Image,
                opt => opt.MapFrom(src => FromByteArrayToFormFile(src.Image)))
                .ForMember(
                d => d.ImagePath,
                opt => opt.MapFrom(src => FromByteArrayToString(src.Image)));
        }
    }
}
