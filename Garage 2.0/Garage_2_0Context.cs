using Microsoft.EntityFrameworkCore;

public class Garage_2_0Context(DbContextOptions<Garage_2_0Context> options) : DbContext(options)
{
    public DbSet<Garage_2._0.Models.ParkedVehicle> ParkedVehicle { get; set; } = default!;
}
