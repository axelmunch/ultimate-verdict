using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Database;

public class DatabaseContext : DbContext
{
    public DbSet<Options> Options { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        var connectionString = "Host=localhost;Port=5432;Database=db;Username=user;Password=password";
        options.UseNpgsql(connectionString);
    }

}

public class Options
{
    public int id { get; set; }
    public string name { get; set; }
}
