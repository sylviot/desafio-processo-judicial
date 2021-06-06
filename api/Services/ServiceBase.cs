using api.Infra;
using api.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;
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

        public virtual async Task<bool> CreateAsync(T entity)
        {
            try
            {
                this.context.BeginTransaction();
                await this.context.AddAsync<T>(entity);
                await this.context.SendChanges();

                return true;
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

        public virtual async Task<bool> UpdateAsync(T entity)
        {
            try
            {
                this.context.BeginTransaction();
                this.context.Update<T>(entity);
                await this.context.SendChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public virtual async Task<bool> DeleteAsync(T entity)
        {
            try
            {
                var entry = this.context.Remove(entity);
                await this.context.SendChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
