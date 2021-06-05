using api.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Services
{
    public class ServiceBase<T> where T : class
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
    }
}
