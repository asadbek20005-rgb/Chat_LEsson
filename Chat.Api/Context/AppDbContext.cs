using Chat.Api.Entities;
using Chat.Api.Helper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Chat.Api.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Entities.Chat> Chats { get; set; }
        public DbSet<User_Chat> User_Chats { get; set; }
        public DbSet<Message> Messages { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            User user = new User
            {
                Id = Guid.NewGuid(),
                FirstName = "Asadbek",
                LastName = "Shermatov",
                Username = "Spawn777",
                Gender = Constants.Male,
                Age = 19,
                Bio = "Never give up",
                Role = Constants.Admin,
            };
            string password = "asadbek_945631282";
            var passwordHash = new PasswordHasher<User>().HashPassword(user, password);
            user.PasswordHash = passwordHash;
            modelBuilder.Entity<User>()
                .HasData(user);
        }
    }
}
