using HotelReservation.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReservationHotel.Data;


namespace ReservationHotel.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminReservationController : Controller
    {
        private readonly AppDbContext _context;

        public AdminReservationController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Reservation/Index → Toutes les réservations
        public async Task<IActionResult> Index()
        {
            var reservations = await _context.Reservations
                .Include(r => r.User)
                .Include(r => r.Room)
                    .ThenInclude(room => room!.Hotel)
                .OrderByDescending(r => r.CheckIn)
                .ToListAsync();

            return View(reservations);
        }

        // Approuver une réservation
        [HttpPost]
        public async Task<IActionResult> Approve(int id)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null) return NotFound();

            reservation.Status = ReservationStatus.Confirmed;
            await _context.SaveChangesAsync();

            TempData["Success"] = "Réservation approuvée avec succès !";
            return RedirectToAction(nameof(Index));
        }

        // Refuser une réservation
        [HttpPost]
        public async Task<IActionResult> Reject(int id)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null) return NotFound();

            reservation.Status = ReservationStatus.Rejected;
            await _context.SaveChangesAsync();

            TempData["Success"] = "Réservation refusée.";
            return RedirectToAction(nameof(Index));
        }
    }
}