using AutoMapper;
using Lab06.BLL.EntitiesDTO;
using Lab06.DAL.Entities;

namespace Lab06.BLL.Mapper
{
    public class BllMapperProfile : Profile
    {
        public BllMapperProfile()
        {
            CreateMap<ParkAttraction, ParkAttractionDto>()
                .ForMember(
                d => d.Image,
                opt => opt.MapFrom(src => src.AttractionImage.Payload))
                .ReverseMap();

            CreateMap<UserTicket, UserTicketDto>()
                .ReverseMap();
        }
    }
}
