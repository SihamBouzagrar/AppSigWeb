using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ReservationHotel.Models;
using ReservationHotel.Services;
using ReservationHotel.ViewModels;

namespace ReservationHotel.Controllers
{
    [Authorize(Roles = "Client")]   // Seulement les clients
    public class ReservationsController : Controller
    {
        private readonly ReservationService _reservationService;
        private readonly UserManager<ApplicationUser> _userManager;

        public ReservationsController(ReservationService reservationService, UserManager<ApplicationUser> userManager)
        {
            _reservationService = reservationService;
            _userManager = userManager;
        }

        // GET: Reservations/Create
        [HttpGet]
        public IActionResult Create(int roomId, DateTime checkIn, DateTime checkOut, int guests = 1)
        {
            if (roomId <= 0 || checkIn < DateTime.Today || checkIn >= checkOut)
                return BadRequest("Paramètres invalides.");

            var model = new ReservationCreateViewModel
            {
                RoomId = roomId,
                CheckIn = checkIn,
                CheckOut = checkOut,
                Guests = guests
            };

            return View(model);
        }

        // POST: Reservations/Create
 [HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Create(ReservationCreateViewModel model)
{
    // Validation des dates
    if (model.CheckIn < DateTime.Today)
        ModelState.AddModelError(nameof(model.CheckIn), "La date d'arrivée ne peut pas être dans le passé.");

    if (model.CheckIn >= model.CheckOut)
        ModelState.AddModelError(nameof(model.CheckOut), "La date de départ doit être après l'arrivée.");

    if (!ModelState.IsValid)
        return View(model);

    var user = await _userManager.GetUserAsync(User);
    if (user == null) 
        return Unauthorized();

    // Vérifier la disponibilité
    var available = await _reservationService.IsRoomAvailable(
        model.RoomId, model.CheckIn, model.CheckOut);

    if (!available)
    {
        ModelState.AddModelError("", "Cette chambre n'est pas disponible pour ces dates.");
        return View(model);
    }

    // Créer la réservation
    var reservation = await _reservationService.CreateReservation(
        user.Id, 
        model.RoomId, 
        model.CheckIn, 
        model.CheckOut, 
        model.Guests);

    return RedirectToAction(nameof(Success), new { id = reservation.Id });
}
   // GET: Reservations/MyReservations
[HttpGet]
public async Task<IActionResult> MyReservations()
{
    var user = await _userManager.GetUserAsync(User);
    if (user == null) 
        return Unauthorized();

    var reservations = await _reservationService.GetUserReservationsAsync(user.Id);
    return View(reservations);
}

        // GET: Reservations/Success/5
        [HttpGet]
        public IActionResult Success(int id)
        {
            ViewBag.ReservationId = id;
            return View();
        }
    }
}