using AutoMapper;
using Lab06.BLL.EntitiesDTO;
using Lab06.Web.ViewModels;
using static Lab06.BLL.Helpers.ImageConverterHelper;

namespace Lab06.Web.Mappings
{
    public class TicketsProfile : Profile
    {
        public TicketsProfile()
        {
            CreateMap<ParkAttractionDto, TicketViewModel>().ForMember(
               d => d.ImagePath,
               opt => opt.MapFrom(src => FromByteArrayToString(src.Image)))
               .ForMember(
               d => d.AttractionName,
               opt => opt.MapFrom(src => src.Name))
               .ForMember(
               d => d.AttractionId,
               opt => opt.MapFrom(src => src.Id));
        }
    }
}
