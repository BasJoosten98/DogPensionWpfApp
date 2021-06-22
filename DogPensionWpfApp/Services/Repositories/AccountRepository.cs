using DogPensionWpfApp.EntityFramework;
using DogPensionWpfApp.Models.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogPensionWpfApp.Services.Repositories
{
    public class AccountRepository : GenericRepository<Account, DogPensionDbContext>
    {
        public AccountRepository(DogPensionDbContext context)
            : base(context)
        {

        }

        public async Task<Account> GetByName(string name)
        {
            return await Context.Accounts.Where(a => a.Name == name).FirstOrDefaultAsync();
        }

        public async Task<Account> GetByEmail(string email)
        {
            return await Context.Accounts.Where(a => a.Email == email).FirstOrDefaultAsync();
        }
    }
}
