using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication1
{
    public class AppContext:DbContext
    {
        public DbSet<DeviceListModel> DeviceLists { get; set; }

        public DbSet<SignalModel> SignalList { get; set; }

        public DbSet<BKIModel> BKIList { get; set; }

        public AppContext(DbContextOptions<AppContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
