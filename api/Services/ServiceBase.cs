using api.Infra;
using api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Services
{
    public class ServiceBase<T> where T : ModelBase
    {
        protected readonly Context context;

        public ServiceBase(Context _context)
        {
            this.context = _context;
        }

        public async Task<bool> CreateAsync(T entity)
        {
            try
            {
                this.context.BeginTransaction();
                await this.context.AddAsync<T>(entity);
                return await this.context.SaveChangesAsync() > 0;
            }
            catch
            {
                return false;
            }
        }

        public IQueryable<T> Read()
        {
            return this.context.Set<T>().AsQueryable();
        }

        public bool Update(T entity)
        {
            try
            {
                this.context.BeginTransaction();
                var entry = this.context.Update<T>(entity);

                return entry.State == Microsoft.EntityFrameworkCore.EntityState.Modified;
            }
            catch
            {
                return false;
            }
        }

        public bool Delete(T entity)
        {
            try
            {
                var entry = this.context.Remove(entity);

                return entry.State == Microsoft.EntityFrameworkCore.EntityState.Deleted;
            }
            catch
            {
                return false;
            }
        }
    }
}
