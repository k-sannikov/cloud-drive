using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Migrations;

public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Before migration");
        new AppDbContextFactory().CreateDbContext(args).Database.Migrate();
        Console.WriteLine("After migration");
    }
}