using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TenisRanking.Models;

namespace TenisRanking.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Match> Matches { get; set; }

        public DbSet<Player> Players { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}
