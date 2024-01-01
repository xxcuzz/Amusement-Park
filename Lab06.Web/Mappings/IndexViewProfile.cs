using AutoMapper;
using Lab06.BLL.EntitiesDTO;
using Lab06.Web.ViewModels;
using static Lab06.BLL.Helpers.ImageConverterHelper;

namespace Lab06.Web.Mappings
{
    public class IndexViewProfile : Profile
    {
        public IndexViewProfile()
        {
            CreateMap<ParkAttractionDto, IndexViewModel>().ForMember(
                d => d.Image,
                opt => opt.MapFrom(src => FromByteArrayToString(src.Image)));
        }
    }
}
