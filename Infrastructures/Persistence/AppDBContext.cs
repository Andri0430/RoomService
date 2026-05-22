using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructures.Persistence
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {
            
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<HistoryReservationRoom> HistoryReservationRooms { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .Property(u => u.Role)
                .HasConversion<string>();

            Seed.SeedDataUser(modelBuilder);

            modelBuilder.Entity<HistoryReservationRoom>()
                .HasOne(reservation => reservation.Room)
                .WithMany()
                .HasForeignKey(reservation => reservation.RoomId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
