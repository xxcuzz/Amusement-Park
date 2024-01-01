using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using Lab06.BLL.EntitiesDTO;
using Lab06.BLL.Services.Interfaces;
using Lab06.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using static Lab06.BLL.Helpers.ImageConverterHelper;

namespace Lab06.Web.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class ManageAttractionsController : Controller
    {
        private readonly IParkAttractionsService _parkAttractionsService;
        private readonly ILogger<ManageAttractionsController> _logger;
        private readonly INotyfService _notyfService;
        private readonly IMapper _mapper;

        public ManageAttractionsController(
            IParkAttractionsService parkAttractionsService,
            ILogger<ManageAttractionsController> logger,
            INotyfService notyfService,
            IMapper mapper)
        {
            _parkAttractionsService = parkAttractionsService;
            _logger = logger;
            _notyfService = notyfService;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Add()
        {
            _logger.LogInformation("Requesting Add attraction GET view");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddParkAttractionViewModel model)
        {
            _logger.LogInformation("Start Adding attraction with name: {0}", model.Name);
            if (!ModelState.IsValid)
            {
                _notyfService.Error("Model is not valid");
                _logger.LogWarning("Model is not valid");
                return RedirectToAction("Add", "ManageAttractions");
            }

            _logger.LogInformation("Adding attraction {0}", model.Name);
            var parkAttractionDto = _mapper.Map<AddParkAttractionViewModel, ParkAttractionDto>(model);

            var createdAttraction = await _parkAttractionsService.Create(parkAttractionDto);

            if (createdAttraction == null)
            {
                _notyfService.Warning("Attraction was not added");
                return RedirectToAction("Add", "ManageAttractions");
            }

            _logger.LogInformation($"Attraction {createdAttraction.Name} added");
            _notyfService.Success($"Attraction {createdAttraction.Name} added");

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            _logger.LogInformation("Start Edit GET method");
            if (id == null)
            {
                _notyfService.Error("Id is null");
                _logger.LogWarning($"Id is null");
                return RedirectToAction("Index", "Home");
            }

            var parkAttractionDto = await _parkAttractionsService.GetByIdOrNullAsync(id.Value);

            if (parkAttractionDto == null)
            {
                _notyfService.Warning("Attraction with Id: {0} not found", id.Value);
                _logger.LogWarning($"Attraction with Id: {id.Value} not found");
                return RedirectToAction("Index", "Home");
            }

            _logger.LogInformation("Attraction to edit: {0}", parkAttractionDto.Name);

            var model = _mapper.Map<ParkAttractionDto, EditParkAttractionViewModel>(parkAttractionDto);
            _logger.LogInformation("Requesting edit view...");
            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(EditParkAttractionViewModel model, IFormFile attractionImage)
        {
            _logger.LogInformation("Started Updating attraction with id: {0}", model.Id);
            if (!ModelState.IsValid)
            {
                _notyfService.Error("Model is not valid");
                _logger.LogWarning("Model is not valid");
                return RedirectToAction("Index", "Home");
            }

            if (attractionImage == null)
            {
                model.Image = FromStringToFormFile(model.ImagePath);
            }
            else
            {
                model.Image = attractionImage;
            }

            var parkAttractionDto = _mapper.Map<EditParkAttractionViewModel, ParkAttractionDto>(model);

            var updatedAttraction = _parkAttractionsService.Update(parkAttractionDto);

            if (updatedAttraction == null)
            {
                _notyfService.Warning("Attraction was not updated");

                return RedirectToAction("Index", "Home");
            }

            _notyfService.Success("Attraction updated");
            _logger.LogInformation("Attraction updated");

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            _logger.LogInformation("Start Deleting attraction with id: {0}", id);
            if (id == null)
            {
                _notyfService.Error("Model not exist");
                _logger.LogWarning("Can't find model with model id: {0}", id);
                return RedirectToAction("Index", "Home");
            }

            var parkAttractionDto = await _parkAttractionsService.GetByIdOrNullAsync(id.Value);
            _logger.LogInformation("Attraction name: {0}", parkAttractionDto.Name);

            if (parkAttractionDto != null)
            {
                _notyfService.Error("Attraction with id {0} was not found", id.Value);
                _logger.LogWarning($"Attraction with id {id.Value} was not found");

                return RedirectToAction("Index", "Home");
            }

            var affectedRows = await _parkAttractionsService.DeleteAsync(parkAttractionDto.Id);

            if(affectedRows <= 0)
            {
                _notyfService.Error("Attraction was not deleted");
                _logger.LogWarning("Attraction was not deleted");

                return RedirectToAction("Index", "Home");
            }

            _notyfService.Success("Attraction deleted");
            _logger.LogInformation("Attraction deleted");
            return RedirectToAction("Index", "Home");
            
        }
    }
}
