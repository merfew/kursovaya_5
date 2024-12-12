using Microsoft.EntityFrameworkCore;

namespace kursovaya_transfer.Model
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> User { get; set; }
        public DbSet<Card> Card { get; set; }
        public DbSet<Transfer> Transfer { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("server=localhost;database=kursovay;username=postgres;password=12345678;");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>().HasKey(h => h.user_id);
            modelBuilder.Entity<Card>().HasKey(h => h.card_id);
            modelBuilder.Entity<Transfer>().HasKey(h => h.transfer_id);
        }
    }
}
