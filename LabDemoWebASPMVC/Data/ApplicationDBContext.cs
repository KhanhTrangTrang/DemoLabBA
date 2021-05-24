using LabDemoWebASPMVC.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LabDemoWebASPMVC.Data
{
    /// <summary>
    ///   Data context dùng thể thao tác với database
    /// </summary>
    /// <Modified>
    /// Name Date Comments
    /// khanhnn 5/24/2021 created
    /// </Modified>
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {

        }

        public DbSet<Employees> Employees { get; set; }
        public DbSet<Users> Users { get; set; }
    }
}
