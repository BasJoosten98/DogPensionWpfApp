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
    public class OwnerRepository : GenericRepository<Owner, DogPensionDbContext>
    {
        public OwnerRepository(DogPensionDbContext context) : base(context)
        {
                
        }

        public async Task<Owner> GetOwnerByIdIncludingProfilePicture(int id)
        {
            return await Context.Owners.Include(o => o.ProfilePicture).FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<IEnumerable< Owner>> GetAllOwnersIncludingProfilePicture(Authenticator auth)
        {
            return await Context.Owners.Include(o => o.ProfilePicture).Where(o => o.AccountId == auth.Account.Id).ToListAsync();
        }

    }
}
