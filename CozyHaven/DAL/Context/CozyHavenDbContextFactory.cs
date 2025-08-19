using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Context
{
    internal class CozyHavenDbContextFactory : IDesignTimeDbContextFactory<CozyHavenDbContext>
    {
        public CozyHavenDbContext CreateDbContext(string[] args)
        {
            var connectionString = DataBaseHelper.GetConnectionString();

            var optionsBuilder = new DbContextOptionsBuilder<CozyHavenDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new CozyHavenDbContext(optionsBuilder.Options);
        }
    }
}
