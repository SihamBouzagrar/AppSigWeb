using HotelReservation.Controllers;
using Microsoft.AspNetCore.Mvc;
using ReservationHotel.Services;
using ReservationHotel.ViewModels;

namespace ReservationHotel.Controllers
{
    public class ReservationsController : Controller
    {
        private readonly ReservationService _reservationService;

        public ReservationsController(ReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        // GET /Reservations/Create?roomId=3&checkIn=...&checkOut=...&guests=2
       [HttpGet]
public IActionResult Create(int roomId, DateTime checkIn, DateTime checkOut, int guests)
{
    if (roomId <= 0)
        return BadRequest("Chambre invalide.");

    if (checkIn < DateTime.Today || checkIn >= checkOut)
        return BadRequest("Dates invalides.");

    var model = new ReservationCreateViewModel
    {
        RoomId = roomId,
        CheckIn = checkIn,
        CheckOut = checkOut,
        Guests = guests
    };

    // Optionnel : charger le nom de la chambre pour l'afficher
    ViewBag.RoomInfo = "Chambre sélectionnée";

    return View(model);
}
[HttpGet]
public async Task<IActionResult> MyReservations()
{
    var reservations = await _reservationService.GetAllReservationsAsync();
    
    return View(reservations);
}
        // POST /Reservations/Create
        [HttpPost]
        [ValidateAntiForgeryToken]  // ✅ FIX: protection CSRF ajoutée
        public async Task<IActionResult> Create(ReservationCreateViewModel model)
        {
            // ✅ FIX: validation métier des dates en plus de ModelState
            if (model.CheckIn < DateTime.Today)
                ModelState.AddModelError(nameof(model.CheckIn), "La date d'arrivée ne peut pas être dans le passé.");

            if (model.CheckIn >= model.CheckOut)
                ModelState.AddModelError(nameof(model.CheckOut), "La date de départ doit être après la date d'arrivée.");

            if (!ModelState.IsValid)
                return View(model);

            var available = await _reservationService.IsRoomAvailable(
                model.RoomId,
                model.CheckIn,
                model.CheckOut
            );

            if (!available)
            {
                ModelState.AddModelError("", "Cette chambre n'est pas disponible pour les dates sélectionnées.");
                return View(model);
            }

            var reservation = await _reservationService.CreateReservation(
                model.RoomId,
                model.CheckIn,
                model.CheckOut,
                model.Guests
            );

            // ✅ FIX: Post-Redirect-Get pattern — RedirectToAction évite la soumission double si F5
            return RedirectToAction(nameof(Success), new { id = reservation.Id });
        }

        // GET /Reservations/Success/12
        [HttpGet]
        public IActionResult Success(int id)
        {
            if (id <= 0)
                return RedirectToAction(nameof(HomeController.Index), "Home");

            ViewBag.ReservationId = id;
            return View();
        }
    }
}