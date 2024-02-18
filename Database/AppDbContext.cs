using IsMyPlantSickApp.Models;
using Microsoft.EntityFrameworkCore;

namespace IsMyPlantSickApp.Database;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options) {
    public DbSet<User> Users { get; set; }

    public DbSet<Diagnosis> Diagnoses { get; set; }
}
