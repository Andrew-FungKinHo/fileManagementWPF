using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using windowsApp.Models;

namespace windowsApplication.Data
{
    public class windowsAppContext :DbContext
    {
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<AppFile> AppFiles { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost;Database=master;Trusted_Connection=True;TrustServerCertificate=True;");
        }

    }
}
