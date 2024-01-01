using AutoMapper;
using Lab06.BLL.EntitiesDTO;
using Lab06.BLL.Services.Interfaces;
using Lab06.Web.Models;
using Lab06.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Lab06.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IParkAttractionsService _parkAttractions;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public HomeController(
            ILogger<HomeController> logger,
            IParkAttractionsService parkAttractions,
            IUserService userService,
            IMapper mapper)
        {
            _logger = logger;
            _parkAttractions = parkAttractions;
            _userService = userService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index(string searchString)
        {
            if (HttpContext.User.IsInRole("Employee"))
            {
                var users = await _userService.GetAllUsersWithUserRoleAsync();

                var userViewModels = new List<UserViewModel>();

                if (!string.IsNullOrEmpty(searchString))
                {
                    users = users.Where(s => s.UserName.Contains(searchString, StringComparison.CurrentCultureIgnoreCase)
                        || s.FirstName.Contains(searchString, StringComparison.CurrentCultureIgnoreCase)
                        || s.Surname.Contains(searchString, StringComparison.CurrentCultureIgnoreCase));
                }

                foreach (var user in users)
                {
                    userViewModels.Add(new UserViewModel
                    {
                        UserId = user.Id,
                        FirstName = user.FirstName,
                        Surname = user.Surname,
                        Username = user.UserName,
                        Email = user.Email,
                    });
                }
                _logger.LogInformation("User is in role \"Employee\"");
                return View("EmployeeIndex", userViewModels);
            }

            _logger.LogDebug("Getting Index.cshtml");
            var parkAttractionsDtos = _parkAttractions.GetAll();

            var ivmList = _mapper.Map<List<ParkAttractionDto>, List<IndexViewModel>>(parkAttractionsDtos.ToList());

            if (HttpContext.User.IsInRole("Administrator"))
            {
                _logger.LogInformation("User is in role \"Administrator\"");
                return View("AdministratorIndex", ivmList);
            }

            if (HttpContext.User.IsInRole("User"))
            {
                _logger.LogInformation("User is in role \"User\"");
                return View("UserIndex", ivmList);
            }

            return View(ivmList);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
