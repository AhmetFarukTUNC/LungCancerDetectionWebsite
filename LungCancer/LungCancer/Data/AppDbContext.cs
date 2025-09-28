using LungCancer.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace LungCancer.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Prediction> Predictions { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<ContactMessage> ContactMessages { get; set; }


    }
}
