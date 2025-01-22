using ConcertBooking.Application.Common;
using ConcertBooking.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ConcertBooking.Infrastructure.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {

        private readonly ApplicationDbContext _context;
        internal DbSet<T> dbSet { get; set; }

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
            dbSet = _context.Set<T>(); // _context.artists = dbset
        }

        public void Add(T entity)
        {
            dbSet.Add(entity);
        }

        public async Task<T> AddAsync(T entity)
        {
            await dbSet.AddAsync(entity);
            return entity;
        }

        public void Delete(T entity)
        {
            if (_context.Entry(entity).State == EntityState.Detached)
            {
                dbSet.Attach(entity);
            }
            dbSet.Remove(entity);
        }

        public async Task<T> DeleteAsync(T entity)
        {
            if (_context.Entry(entity).State == EntityState.Detached)
            {
                dbSet.Attach(entity);
            }
            dbSet.Remove(entity);
            return entity;
        }

        public void DeleteRange(List<T> entities)
        {
            dbSet.RemoveRange(entities);
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>> filter = null, 
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null, 
            bool disableTracking = true)
        {
            IQueryable<T> query = dbSet;
            if (disableTracking)
            {
                query = query.AsNoTracking();
            }
            if(filter != null)
            {
                query = query.Where(filter);
            }
            if(include != null)
            {
                query = include(query);
            }
            if(orderBy != null)
            {
                return orderBy(query).ToList();

            }
            else
            {
                return query.ToList();
            }
        }

        public T GetById(object id)
        {
            return dbSet.Find(id);
        }

        public T GetByIdAsync(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null, bool disableTracking = true)
        {
            IQueryable<T> query = dbSet;
            if (disableTracking)
            {
                query = query.AsNoTracking();
            }
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (include != null)
            {
                query = include(query);
            }
            if (orderBy != null)
            {
                return orderBy(query).FirstOrDefault();

            }
            else
            {
                return query.FirstOrDefault();
            }
        }
    }
}
