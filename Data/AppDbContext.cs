using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ReservationHotel.Models;

namespace ReservationHotel.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<HotelImage> HotelImages { get; set; }
        public DbSet<Reservation> Reservations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);   // ← Très important de garder ceci en premier

            // ====================== FIX pour .NET 10 + Identity Passkeys ======================
            modelBuilder.Ignore<IdentityPasskeyData>();
            modelBuilder.Ignore<IdentityUserPasskey<string>>();

            // =================================================================================

            // Configurations de tes entités
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