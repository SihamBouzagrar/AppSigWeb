// CS0105 FIX : supprimé le 'using ReservationHotel.Models' en double
using Microsoft.EntityFrameworkCore;
using ReservationHotel.Models;

namespace ReservationHotel.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<HotelImage> HotelImages { get; set; }
        public DbSet<Reservation> Reservations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Hotel>()
                .Property(h => h.PrixParNuit)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Room>()
                .Property(r => r.PricePerNight)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Reservation>()
                .Property(r => r.TotalPrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Reservation>()
                .HasIndex(r => new { r.RoomId, r.CheckIn, r.CheckOut })
                .HasDatabaseName("IX_Reservations_RoomId_Dates");
        }
    }
}