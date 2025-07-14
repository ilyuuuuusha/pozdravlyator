using Microsoft.EntityFrameworkCore;

public class BirthdayContext : DbContext
{
    public DbSet<BirthdayEntry> Birthdays { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=birthdays.db");
    }
}
