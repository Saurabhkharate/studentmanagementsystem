using Microsoft.EntityFrameworkCore;
using studentmanagementsystem.Models;

namespace studentmanagementsystem.DatabaseContext
{
    public class AppDatabaseContext : DbContext
    {
        public AppDatabaseContext(DbContextOptions<AppDatabaseContext> options) : base(options) { }
        public DbSet<StudentInfo> Students { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<City> Cities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // prevent multiple cascade delete error
            modelBuilder.Entity<StudentInfo>()
                .HasOne(s => s.Country)
                .WithMany()
                .HasForeignKey(s => s.CountryId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<StudentInfo>()
                .HasOne(s => s.State)
                .WithMany()
                .HasForeignKey(s => s.StateId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<StudentInfo>()
                .HasOne(s => s.City)
                .WithMany()
                .HasForeignKey(s => s.CityId)
                .OnDelete(DeleteBehavior.NoAction);

            // decimal fix
            modelBuilder.Entity<StudentInfo>()
                .Property(p => p.Amount)
                .HasColumnType("decimal(18,2)");
        }

    }
}
