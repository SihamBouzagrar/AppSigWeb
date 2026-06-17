using HotelReservation.Models;
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

        // Vérifie la disponibilité d'une chambre
       public async Task<bool> IsRoomAvailable(
    int roomId,
    DateTime checkIn,
    DateTime checkOut)
{
    var conflict = await _context.Reservations.AnyAsync(r =>
        r.RoomId == roomId &&
        r.Status != ReservationStatus.Rejected &&
        checkIn < r.CheckOut &&
        checkOut > r.CheckIn);

    return !conflict;
}

        // ✅ NOUVELLE MÉTHODE : Récupérer les réservations d'un client
// Récupérer les réservations d'un client
public async Task<List<ReservationConfirmDto>> GetUserReservationsAsync(string userId)
{
    return await _context.Reservations
        .Include(r => r.Room)
            .ThenInclude(room => room!.Hotel)
        .Include(r => r.User)
        .Where(r => r.UserId == userId)
        .OrderByDescending(r => r.CheckIn)
        .Select(r => new ReservationConfirmDto
        {
            Id = r.Id,
            RoomId = r.RoomId,
            RoomType = r.Room != null ? r.Room.RoomType : "Chambre inconnue",
            HotelName = r.Room != null && r.Room.Hotel != null ? r.Room.Hotel.Nom : "Hôtel inconnu",
            UserFullName = r.User != null ? r.User.FullName : "Utilisateur inconnu",
            CheckIn = r.CheckIn,
            CheckOut = r.CheckOut,
            Guests = r.Guests,
            Nights = (r.CheckOut - r.CheckIn).Days,
            TotalPrice = r.TotalPrice,
            Status = r.Status.ToString()
        })
        .ToListAsync();
}

// Récupérer toutes les réservations (Admin)
public async Task<List<ReservationConfirmDto>> GetAllReservationsAsync()
{
    return await _context.Reservations
        .Include(r => r.Room)
            .ThenInclude(room => room!.Hotel)
        .Include(r => r.User)
        .OrderByDescending(r => r.CheckIn)
        .Select(r => new ReservationConfirmDto
        {
            Id = r.Id,
            RoomId = r.RoomId,
            RoomType = r.Room != null ? r.Room.RoomType : "Chambre inconnue",
            HotelName = r.Room != null && r.Room.Hotel != null ? r.Room.Hotel.Nom : "Hôtel inconnu",
            UserFullName = r.User != null ? r.User.FullName : "Utilisateur inconnu",
            CheckIn = r.CheckIn,
            CheckOut = r.CheckOut,
            Guests = r.Guests,
            Nights = (r.CheckOut - r.CheckIn).Days,
            TotalPrice = r.TotalPrice,
            Status = r.Status.ToString()
        })
        .ToListAsync();
}
     // Créer une réservation (✅ Version corrigée avec UserId)
        public async Task<ReservationConfirmDto> CreateReservation(
            string userId, int roomId, DateTime checkIn, DateTime checkOut, int guests)
        {
            var room = await _context.Rooms
                .Include(r => r.Hotel)
                .FirstOrDefaultAsync(r => r.Id == roomId);

            if (room == null)
                throw new InvalidOperationException($"Chambre {roomId} introuvable.");

            int nights = (checkOut - checkIn).Days;
            if (nights <= 0)
                throw new ArgumentException("La durée du séjour doit être d'au moins 1 nuit.");

            var reservation = new Reservation
            {
                UserId = userId,
                RoomId = roomId,
                CheckIn = checkIn,
                CheckOut = checkOut,
                Guests = guests,
                TotalPrice = room.PricePerNight * nights,
                Status = ReservationStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };

            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();

           return new ReservationConfirmDto
{
    Id = reservation.Id,
    RoomId = roomId,
    RoomType = room.RoomType,
    HotelName = room.Hotel != null ? room.Hotel.Nom : "Hôtel inconnu",
    CheckIn = checkIn,
    CheckOut = checkOut,
    Guests = guests,
    Nights = nights,
    TotalPrice = reservation.TotalPrice,
    Status = reservation.Status.ToString()
};
        }
    }
}