using api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Services.Interfaces
{
    public interface IServiceBase<T> where T : ModelBase
    {
        Task<T> CreateAsync(T entity);
        IQueryable<T> Read();
        Task<bool> UpdateAsync(T entity);
        Task<bool> DeleteAsync(T entity);
    }
}
