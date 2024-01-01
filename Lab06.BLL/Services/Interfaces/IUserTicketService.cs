using Lab06.BLL.EntitiesDTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lab06.BLL.Services.Interfaces
{
    public interface IUserTicketService
    {
        Task<UserTicketDto> CreateTicket(string userId, int parkAttractionId);

        IEnumerable<UserTicketDto> GetByUserId(string userId);

        int DeteleTicket(string userId, int parkAttractionId);
    }
}
