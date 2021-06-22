using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DogPensionWpfApp.Services.Repositories
{
    interface IGenericRepository<T>
    {
        Task<T> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task SaveAsync();
        bool HasChanges();
        void Add(T model);
        void Remove(T model);
        void Update(T model);
    }
}
