using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using Lab06.BLL.EntitiesDTO;
using Lab06.BLL.Services.Interfaces;
using Lab06.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Lab06.Web.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private readonly ILogger<UsersController> _logger;
        private readonly IParkAttractionsService _parkAttractionsService;
        private readonly IUserTicketService _userTicketService;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly INotyfService _notyfService;

        public UsersController(
            ILogger<UsersController> logger,
            IParkAttractionsService parkAttractionsService,
            IUserTicketService userTicketService,
            IUserService userService,
            IMapper mapper,
            INotyfService notyfService)
        {
            _logger = logger;
            _parkAttractionsService = parkAttractionsService;
            _userTicketService = userTicketService;
            _userService = userService;
            _mapper = mapper;
            _notyfService = notyfService;
        }

        [HttpGet]
        public IActionResult TopUpBalance()
        {
            _logger.LogInformation("Requesting Top Up Balance page");
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> TopUpBalance(string balance)
        {
            _logger.LogInformation("Adding amount of money: {0}BYN", balance);
            decimal d;
            try
            {
                balance = balance.Replace(",", CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator);
                d = decimal.Parse(balance, CultureInfo.InvariantCulture);
                if (d < 0)
                {
                    _notyfService.Error("Balance value must be positive");
                    _logger.LogInformation("Entered negative balance value");
                    return RedirectToAction("TopUpBalance", "Users");
                }
            }
            catch
            {
                _notyfService.Error("Something went wrong!");
                _logger.LogWarning("Error parsing balance value");
                return RedirectToAction("Index", "Home");
            }

            var user = await _userService.GetUserAsync(User);
            user.Balance += d;
            if (user.Balance >= 1000.00M)
            {
                _notyfService.Error("Balance cannot be more than 1000BYN");
                _logger.LogInformation("User balance cannot be more than 1000BYN");
                return RedirectToAction("TopUpBalance", "Users");
            }

            await _userService.UpdateAsync(user);
            _notyfService.Success("Balance updated!");
            _logger.LogInformation("Balance updated");
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> ShowTickets(string userId)
        {
            _logger.LogInformation("Showing user tickets");
            var user = await _userService.GetUserById(userId);

            _logger.LogInformation("Current user is {0}", user.Email);

            var ticketsByUser = _userTicketService.GetByUserId(user.Id);

            _logger.LogInformation("Count of tickets for user: {0}", ticketsByUser.Count());

            var ticketViewModels = new List<TicketViewModel>();

            foreach (var ticket in ticketsByUser)
            {
                var parkAttractionDto = await _parkAttractionsService.GetByIdOrNullAsync(ticket.ParkAttractionId);
                var ticketViewModel = _mapper.Map<ParkAttractionDto, TicketViewModel>(parkAttractionDto);
                ticketViewModel.UserId = userId;
                ticketViewModel.PurchaseTime = ticket.PurchaseTime;
                ticketViewModels.Add(ticketViewModel);
            }

            return View(ticketViewModels.AsEnumerable());
        }
    }
}
