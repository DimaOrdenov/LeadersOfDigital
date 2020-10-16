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
        public MainContext(DbContextOptions<MainContext> options) : base(options)
        {
        }
        public DbSet<Facility> Facilities { get; set; }
        public DbSet<Barrier> Barriers { get; set; }
        public DbSet<Api.Data.Models.Disability> Disability { get; set; }
    }
}
