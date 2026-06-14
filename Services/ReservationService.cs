using Microsoft.EntityFrameworkCore;
using ReservationHotel.Data;
using ReservationHotel.DTOs;
using ReservationHotel.Models;

namespace ReservationHotel.Services
{
    public class ReservationService
    {
        private readonly AppDbContext _context;

        public ReservationService(AppDbContext context)
        {
            _context = context;
        }

        // ─────────────────────────────────────────────────────────────
        // Vérifie la disponibilité d'une chambre sur une plage de dates
        // ✅ FIX: validation des paramètres ajoutée
        // ─────────────────────────────────────────────────────────────
        public async Task<bool> IsRoomAvailable(int roomId, DateTime checkIn, DateTime checkOut)
        {
            if (roomId <= 0 || checkIn >= checkOut)
                return false;

            // Vérifie qu'aucune réservation existante ne chevauche la période demandée
            var hasConflict = await _context.Reservations.AnyAsync(r =>
                r.RoomId == roomId &&
                checkIn < r.CheckOut &&
                checkOut > r.CheckIn
            );

            return !hasConflict;
        }
// Récupérer toutes les réservations (avec info chambre + hôtel)
public async Task<List<ReservationConfirmDto>> GetAllReservationsAsync()
{
    return await _context.Reservations
        .Include(r => r.Room)
            .ThenInclude(room => room!.Hotel)   // Important pour charger l'hôtel
        .OrderByDescending(r => r.CheckIn)
        .Select(r => new ReservationConfirmDto
        {
            Id = r.Id,
            RoomId = r.RoomId,
            RoomType = r.Room != null ? r.Room.RoomType : "Chambre inconnue",
            HotelName = r.Room != null && r.Room.Hotel != null 
                        ? r.Room.Hotel.Nom 
                        : "Hôtel inconnu",
            CheckIn = r.CheckIn,
            CheckOut = r.CheckOut,
            Guests = r.Guests,
            Nights = (r.CheckOut - r.CheckIn).Days,
            TotalPrice = r.TotalPrice
        })
        .ToListAsync();
}

        // ─────────────────────────────────────────────────────────────
        // Crée une réservation et retourne un DTO (pas l'entité brute)
        // ✅ FIX 1: TotalPrice calculé automatiquement à partir du prix de la chambre
        // ✅ FIX 2: retourne ReservationDto au lieu de Reservation (entité EF)
        //           → évite d'exposer des champs internes via le contrôleur
        // ✅ FIX 3: vérifie que la chambre existe avant de créer la réservation
        // ─────────────────────────────────────────────────────────────
        public async Task<ReservationConfirmDto> CreateReservation(
            int roomId, DateTime checkIn, DateTime checkOut, int guests)
        {
            var room = await _context.Rooms.FindAsync(roomId)
                ?? throw new InvalidOperationException($"Chambre {roomId} introuvable.");

            int nights = (checkOut - checkIn).Days;
            if (nights <= 0)
                throw new ArgumentException("La durée du séjour doit être d'au moins 1 nuit.");

            var reservation = new Reservation
            {
                RoomId = roomId,
                CheckIn = checkIn,
                CheckOut = checkOut,
                Guests = guests,
                // ✅ FIX: TotalPrice calculé ici, plus besoin de le faire côté contrôleur
                TotalPrice = room.PricePerNight * nights
            };

            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();

            return new ReservationConfirmDto
            {
                Id = reservation.Id,
                RoomId = roomId,
                RoomType = room.RoomType,
                CheckIn = checkIn,
                CheckOut = checkOut,
                Guests = guests,
                Nights = nights,
                TotalPrice = reservation.TotalPrice
            };
        }
    }
}