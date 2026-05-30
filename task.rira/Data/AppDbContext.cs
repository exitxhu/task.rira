using Microsoft.EntityFrameworkCore;
using System.Runtime.Serialization;

namespace task.rira.Data;

public class AppDbContext : DbContext
{
    public DbSet<Person> People => Set<Person>();

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }
}

public class Person
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string NationalId { get; set; } = string.Empty;
    public DateTime BirthDate { get; set; }
}