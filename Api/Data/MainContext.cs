using Api.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Data
{
    public class MainContext : DbContext
    {
        public DbSet<Facility> Facilities { get; set; }
        public DbSet<Barrier> Barriers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite("Data Source=city.db");
    }
}
