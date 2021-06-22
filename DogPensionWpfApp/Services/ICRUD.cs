using DogPensionWpfApp.Models.Wrappers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DogPensionWpfApp.Services
{
    public interface ICRUD<T> where T : WrapperModelBase
    {
        Task<T> Create(T model);
        Task<T> Update(T model);
        Task<T> CreateOrUpdate(T model);
        Task Remove(T model);
    }
}
