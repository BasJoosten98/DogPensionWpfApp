using DogPensionWpfApp.EntityFramework;
using DogPensionWpfApp.Models.Database;
using DogPensionWpfApp.Services.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DogPensionWpfApp.Services
{
    public class DogService : ICRUD<Dog>
    {
        private readonly DogPensionDbContextFactory factory;
        private readonly Authenticator authenticator;

        public DogService(DogPensionDbContextFactory factory, Authenticator authenticator)
        {
            this.factory = factory;
            this.authenticator = authenticator;
        }

        public async Task<Dog> Create(Dog model)
        {
            using (DogRepository repo = new DogRepository(factory.CreateDbContext()))
            {
                repo.Add(model);
                await repo.SaveAsync();
            }
            return model;
        }

        public async Task<Dog> CreateOrUpdate(Dog model)
        {
            if (model.Id == 0) { return await Create(model); }
            else { return await Update(model); }
        }

        public async Task<IEnumerable<Dog>> GetAllDogs()
        {
            using (DogRepository repo = new DogRepository(factory.CreateDbContext()))
            {

                return await repo.GetAllDogsAuthenticated(authenticator);
            }
        }
        public async Task<IEnumerable<Dog>> GetAllDogsIncludingProfilePicture()
        {
            using (DogRepository repo = new DogRepository(factory.CreateDbContext()))
            {
                return await repo.GetAllDogsAuthenticatedIncludingProfilePicture(authenticator);
            }
        }
        public async Task<IEnumerable<Dog>> GetAllDogsWithOwner(int ownerId)
        {
            using (DogRepository repo = new DogRepository(factory.CreateDbContext()))
            {
                return await repo.GetAllDogsWithOwnerId(ownerId);
            }
        }
        public async Task<IEnumerable<Dog>> GetAllDogsWithOwnerIncludingProfilePicture(int ownerId)
        {
            using (DogRepository repo = new DogRepository(factory.CreateDbContext()))
            {
                return await repo.GetAllDogsWithOwnerIdIncludingProfilePicture(ownerId);
            }
        }

        public async Task<Dog> GetDogById(int id)
        {
            using (DogRepository repo = new DogRepository(factory.CreateDbContext()))
            {
                return await repo.GetByIdAsync(id);
            }
        }

        public async Task<Dog> GetDogByIdIncludingProfilePicture(int id)
        {
            using (DogRepository repo = new DogRepository(factory.CreateDbContext()))
            {
                return await repo.GetByIdIncludingProfilePicture(id);
            }
        }

        public async Task Remove(Dog model)
        {
            using (DogRepository repo = new DogRepository(factory.CreateDbContext()))
            {
                repo.Remove(model);
                await repo.SaveAsync();
            }
        }

        public async Task<Dog> Update(Dog model)
        {
            using (DogRepository repo = new DogRepository(factory.CreateDbContext()))
            {
                repo.Update(model);
                await repo.SaveAsync();
            }
            return model;
        }
    }
}
