using AutoMapper;
using Lab06.BLL.EntitiesDTO;
using Lab06.BLL.Services.Interfaces;
using Lab06.DAL.Entities;
using Lab06.DAL.Uow;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab06.BLL.Services
{
    public class UserTicketService : IUserTicketService
    {
        private readonly IUserService _userService;
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public UserTicketService(
            IUserService userService,
            IUnitOfWork uow,
            IMapper mapper)
        {
            _userService = userService;
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<UserTicketDto> CreateTicket(string userId, int parkAttractionId)
        {
            var ticket = CreateUserTicketFromIds(userId, parkAttractionId);
            if (await _uow.Repository<ParkAttraction>().GetByIdAsync(parkAttractionId) == null)
            {
                return null;
            }

            var user = await _userService.GetUserById(userId);

            if (user == null)
            {
                return null;
            }

            var createdUserTicket = await _uow.Repository<UserTicket>().AddAsync(ticket);
            _uow.Complete();

            var createdUserTickeetDto = _mapper.Map<UserTicket, UserTicketDto>(createdUserTicket);

            return createdUserTickeetDto;
        }

        public int DeteleTicket(string userId, int parkAttractionId)
        {
            var tickets = GetUserTicketByUserIdAndAttractionId(userId, parkAttractionId);
            if (tickets == null || !tickets.Any())
            {
                return 0;
            }

            var ticket = tickets.First();
            
            _uow.Repository<UserTicket>().Delete(ticket);
            return _uow.Complete();
        }

        public IEnumerable<UserTicketDto> GetByUserId(string userId)
        {
            var userTickets = _uow.Repository<UserTicket>().FindByPredicate(x => x.ApplicationUserId == userId);

            if (userTickets == null)
            {
                return null;
            }
            
            var userTicketDtos = _mapper.Map<List<UserTicket>, List<UserTicketDto>>(userTickets.ToList());

            return userTicketDtos;
        }

        private UserTicket CreateUserTicketFromIds(string userId, int parkAttractionId)
        {
            return new UserTicket
            {
                ApplicationUserId = userId,
                ParkAttractionId = parkAttractionId,
            };
        }

        private IEnumerable<UserTicket> GetUserTicketByUserIdAndAttractionId(string userId, int parkAttractionId)
        {
            return _uow.Repository<UserTicket>().FindByPredicate(
                x => x.ApplicationUserId == userId
                && x.ParkAttractionId == parkAttractionId);
        }
    }
}
