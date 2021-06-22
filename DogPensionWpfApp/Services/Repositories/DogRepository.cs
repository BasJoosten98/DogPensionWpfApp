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
    public class DogRepository : GenericRepository<Dog, DogPensionDbContext>
    {
        public DogRepository(DogPensionDbContext context) : base(context)
        {

        }

        public async Task<Dog> GetByIdIncludingProfilePicture(int id)
        {
            return await Context.Dogs.Include(d => d.ProfilePicture).FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<IEnumerable<Dog>> GetAllDogsWithOwnerId(int ownerId)
        {
            return await Context.Dogs.Where(dog => dog.OwnerId == ownerId).ToListAsync();
        }

        public async Task<IEnumerable<Dog>> GetAllDogsWithOwnerIdIncludingProfilePicture(int ownerId)
        {
            return await Context.Dogs.Where(dog => dog.OwnerId == ownerId).Include(d => d.ProfilePicture).ToListAsync();
        }

        public async Task<IEnumerable<Dog>> GetAllDogsAuthenticated(Authenticator auth)
        {
            return await Context.Dogs.Where(dog => dog.Owner.AccountId == auth.Account.Id).ToListAsync();
        }
        public async Task<IEnumerable<Dog>> GetAllDogsAuthenticatedIncludingProfilePicture(Authenticator auth)
        {
            return await Context.Dogs.Where(dog => dog.Owner.AccountId == auth.Account.Id).Include(d => d.ProfilePicture).ToListAsync();
        }
    }
}
