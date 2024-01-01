using AspNetCoreHero.ToastNotification.Abstractions;
using Lab06.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Lab06.Web.Controllers
{
    [Authorize(Roles = "User, Employee")]
    public class TicketController : Controller
    {
        private readonly ILogger<TicketController> _logger;
        private readonly IUserTicketService _userTicketService;
        private readonly IParkAttractionsService _parkService;
        private readonly IUserService _userService;
        private readonly INotyfService _notyfService;
        public TicketController(
            ILogger<TicketController> logger,
            IUserTicketService userTicketService,
            IParkAttractionsService parkService,
            IUserService userService,
            INotyfService notyfService)
        {
            _logger = logger;
            _userTicketService = userTicketService;
            _parkService = parkService;
            _userService = userService;
            _notyfService = notyfService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        public async Task Purchase(int parkAttractionId, int numberOfTickets)
        {
            _logger.LogInformation("Purchase POST started");
            _logger.LogInformation("Id of attraction: {0}", parkAttractionId);
            _logger.LogInformation("Number of tickets: {0}", numberOfTickets);

            if (numberOfTickets > 9 || numberOfTickets < 1)
            {
                _notyfService.Error("The number of tickets must be from 0 to 10");
                _logger.LogWarning("Attempt to purchase invalid number of tickets");
                return;
            }

            var price = await _parkService.GetPriceAsync(parkAttractionId);

            if (price == 0.00M)
            {
                _logger.LogWarning("Park attraction with id = {0} does not exists", parkAttractionId);
                return;
            }

            var fullPrice = price * numberOfTickets;

            _logger.LogInformation("Price: {0}; Number of tickets: {1}, Full Price: {3}", price, numberOfTickets, fullPrice);

            var user = await _userService.GetUserAsync(User);

            if (fullPrice > user.Balance)
            {
                _notyfService.Error("Insufficient funds");
                return;
            }

            for (int i = 0; i < numberOfTickets; i++)
            {
                await _userTicketService.CreateTicket(user.Id, parkAttractionId);
            }

            _logger.LogInformation("Ticket created");
            _logger.LogInformation("User balance prev {0}", user.Balance);
            user.Balance -= fullPrice;
            await _userService.UpdateAsync(user);

            _logger.LogInformation("User balance now {0}", user.Balance);

            _notyfService.Success("Purchased successfully");
        }

        [HttpPost]
        [Authorize(Roles = "Employee")]
        public IActionResult Delete(string userId, int parkAttractionId)
        {
            var affectedRows = _userTicketService.DeteleTicket(userId, parkAttractionId);

            if (affectedRows <= 0)
            {
                _notyfService.Error("Ticket was not deleted");
                return RedirectToAction("ShowTickets", "Users", new { UserId = userId });
            }

            _notyfService.Success("Ticket Deleted");
            return RedirectToAction("ShowTickets", "Users", new { UserId = userId });
        }
    }
}
