using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DogPensionWpfApp.EntityFramework
{
    public class DogPensionDbContextFactory
    {
        private readonly Action<DbContextOptionsBuilder> _configureDbContext;

        public DogPensionDbContextFactory(Action<DbContextOptionsBuilder> configureDbContext)
        {
            _configureDbContext = configureDbContext;
        }

        public DogPensionDbContext CreateDbContext()
        {
            DbContextOptionsBuilder<DogPensionDbContext> options = new DbContextOptionsBuilder<DogPensionDbContext>();

            _configureDbContext(options);

            return new DogPensionDbContext(options.Options);
        }
    }
}
