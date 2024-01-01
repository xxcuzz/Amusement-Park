using Lab06.BLL.EntitiesDTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lab06.BLL.Services.Interfaces
{
    public interface IParkAttractionsService
    {
        Task<ParkAttractionDto> Create(ParkAttractionDto item);

        ParkAttractionDto Update(ParkAttractionDto item);

        Task<int> DeleteAsync(int id);

        IEnumerable<ParkAttractionDto> GetAll();

        Task<ParkAttractionDto> GetByIdOrNullAsync(int id);

        Task<decimal> GetPriceAsync(int parkAttractionId);
    }
}
