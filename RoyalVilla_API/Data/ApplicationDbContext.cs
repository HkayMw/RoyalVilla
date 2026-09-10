using Microsoft.EntityFrameworkCore;
using RoyalVilla_API.Models;

namespace RoyalVilla_API.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        public DbSet<Villa> Villas { get; set; }
    }
}       
