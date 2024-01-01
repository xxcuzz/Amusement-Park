using AutoMapper;
using Lab06.BLL.EntitiesDTO;
using Lab06.BLL.Services.Interfaces;
using Lab06.BLL.Validation;
using Lab06.DAL.Entities;
using Lab06.DAL.Uow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab06.BLL.Services
{
    public class ParkAttractionsService : IParkAttractionsService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public ParkAttractionsService(
            IUnitOfWork uow,
            IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<ParkAttractionDto> Create(ParkAttractionDto item)
        {
            if (item == null)
            {
                return null;
            }

            if (AttractionValidator.Validate(item) == false)
            {
                return null;
            }

            var parkAttraction = _mapper.Map<ParkAttractionDto, ParkAttraction>(item);

            parkAttraction.AttractionImage.Name = parkAttraction.Name + DateTime.Now;

            var addedParkAttraction = await _uow.Repository<ParkAttraction>().AddAsync(parkAttraction);
            _uow.Complete();

            var addedParkAttractionDto = _mapper.Map<ParkAttraction, ParkAttractionDto>(addedParkAttraction);
            return addedParkAttractionDto;
        }

        public IEnumerable<ParkAttractionDto> GetAll()
        {
            IEnumerable<ParkAttraction> parkAttractions = _uow.Repository<ParkAttraction>().GetAll();

            if (parkAttractions == null)
            {
                return Enumerable.Empty<ParkAttractionDto>();
            }

            foreach (var parkAttraction in parkAttractions)
            {
                var image = _uow.Repository<AttractionImage>().FindByPredicate(x => x.AttractionId == parkAttraction.Id);
                parkAttraction.AttractionImage = image.Single();
            }

            var parkAttractionsDtos = _mapper.Map<List<ParkAttraction>, List<ParkAttractionDto>>(parkAttractions.ToList());

            return parkAttractionsDtos;
        }

        public async Task<ParkAttractionDto> GetByIdOrNullAsync(int id)
        {
            var parkAttraction = await _uow.Repository<ParkAttraction>().GetByIdAsync(id);

            if (parkAttraction == null)
            {
                return null;
            }

            var attractionImage = _uow.Repository<AttractionImage>().FindByPredicate(x => x.AttractionId == id);
            if (attractionImage != null)
            {
                parkAttraction.AttractionImage = attractionImage.Single();
            }

            var parkAttractionDto = _mapper.Map<ParkAttraction, ParkAttractionDto>(parkAttraction);

            return parkAttractionDto;
        }

        /// <summary>
        /// Finds entity with given id, if an entity is found, returns the value of the price property.
        /// </summary>
        /// <param name="parkAttractionId"></param>
        /// <returns>Price, or zero.</returns>
        public async Task<decimal> GetPriceAsync(int parkAttractionId)
        {
            var parkAttraction = await _uow.Repository<ParkAttraction>().GetByIdAsync(parkAttractionId);

            if (parkAttraction == null)
            {
                return 0.00M;
            }

            return parkAttraction.Price;
        }

        public ParkAttractionDto Update(ParkAttractionDto item)
        {
            if (item == null)
            {
                return null;
            }

            if (AttractionValidator.Validate(item) == false)
            {
                return null;
            }

            var parkAttraction = _mapper.Map<ParkAttractionDto, ParkAttraction>(item);

            var oldImage = _uow.Repository<AttractionImage>().FindByPredicate(x => x.AttractionId == parkAttraction.Id).Single();
            _uow.Repository<AttractionImage>().Delete(oldImage);
            _uow.Complete();

            parkAttraction.AttractionImage.Name = parkAttraction.Name;
            parkAttraction.AttractionImage.AttractionId = parkAttraction.Id;

            var updatedItem = _uow.Repository<ParkAttraction>().Update(parkAttraction);
            _uow.Complete();

            var updatedItemDto = _mapper.Map<ParkAttraction, ParkAttractionDto>(updatedItem);

            return item;
        }

        public async Task<int> DeleteAsync(int id)
        {
            var parkAttraction = await _uow.Repository<ParkAttraction>().GetByIdAsync(id);

            if (parkAttraction == null)
            {
                return 0;
            }

            _uow.Repository<ParkAttraction>().Delete(parkAttraction);
            return _uow.Complete();
        }
    }
}
