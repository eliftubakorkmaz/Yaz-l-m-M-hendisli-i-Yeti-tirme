using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameworkCore.BenchMark.Core;
internal class ApplicationDbContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Data Source=ETUBAS;Initial Catalog=RepositoryPatternDb;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False");
    }
    public DbSet<ShoppingCart> shoppingCarts { get; set; }
    public DbSet<Product> products { get; set; }
}

public sealed class ShoppingCart 
{
    public int ProductId { get; set; }
    public Product? Product { get; set; }
    public int Quantity { get; set; }
}

public sealed class Product 
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}