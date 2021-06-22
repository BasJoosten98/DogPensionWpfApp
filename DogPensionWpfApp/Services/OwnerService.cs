using DogPensionWpfApp.EntityFramework;
using DogPensionWpfApp.Models.Database;
using DogPensionWpfApp.Services.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogPensionWpfApp.Services
{
    public class OwnerService : ICRUD<Owner>
    {
        private readonly DogPensionDbContextFactory factory;
        private readonly Authenticator authenticator;

        public OwnerService(DogPensionDbContextFactory factory, Authenticator authenticator)
        {
            this.factory = factory;
            this.authenticator = authenticator;
        }

        public async Task<IEnumerable<Owner>> GetAllOwners()
        {
            using(OwnerRepository repo = new OwnerRepository(factory.CreateDbContext()))
            {
                var results = await repo.GetAllAsync();
                return results.Where(owner => owner.AccountId == authenticator.Account.Id);
            }
        }

        public async Task<IEnumerable<Owner>> GetAllOwnersIncludingProfilePicture()
        {
            using (OwnerRepository repo = new OwnerRepository(factory.CreateDbContext()))
            {
                return await repo.GetAllOwnersIncludingProfilePicture(authenticator);
            }
        }

        public async Task<Owner> GetOwnerById(int id)
        {
            using (OwnerRepository repo = new OwnerRepository(factory.CreateDbContext()))
            {
                return await repo.GetByIdAsync(id);
            }
        }

        public async Task<Owner> GetOwnerByIdIncludingProfilePicture(int id)
        {
            using (OwnerRepository repo = new OwnerRepository(factory.CreateDbContext()))
            {
                return await repo.GetOwnerByIdIncludingProfilePicture(id);
            }
        }

        public async Task<Owner> Create(Owner model)
        {
            if(authenticator.Account == null) { throw new Exception("No user has been logged in. Cannot create new Owner"); }
            model.Id = 0;
            model.AccountId = authenticator.Account.Id;
            using (OwnerRepository repo = new OwnerRepository(factory.CreateDbContext()))
            {
                repo.Add(model);
                await repo.SaveAsync();
            }
            return model;
        }

        public async Task<Owner> Update(Owner model)
        {
            using (OwnerRepository repo = new OwnerRepository(factory.CreateDbContext()))
            {
                repo.Update(model);
                await repo.SaveAsync();
            }
            return model;
        }

        public async Task<Owner> CreateOrUpdate(Owner model)
        {
            if(model.Id == 0) { return await Create(model); }
            else { return await Update(model); }
        }

        public async Task Remove(Owner model)
        {
            using (OwnerRepository repo = new OwnerRepository(factory.CreateDbContext()))
            {
                repo.Remove(model);
                await repo.SaveAsync();
            }
        }
    }
}
