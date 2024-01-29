using IsMyPlantSickApp.Models;
using Microsoft.EntityFrameworkCore;

namespace IsMyPlantSickApp.Database;

public class AppDbContext : DbContext {
    public DbSet<User> Users { get; set; }

    public DbSet<Diagnosis> Diagnoses { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
        optionsBuilder.UseNpgsql("Host=my_host;Database=my_db;Username=my_user;Password=my_pw");
}
