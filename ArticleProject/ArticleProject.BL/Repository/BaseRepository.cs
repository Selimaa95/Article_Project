using ArticleProject.BL.IRepository;
using ArticleProject.DAL.DataBase;
using ArticleProject.DAL.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ArticleProject.BL.Repository
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        #region Prop
        private readonly ApplicationDbContext db;

        #endregion

        #region Ctor

        public BaseRepository(ApplicationDbContext db)
        {
            this.db = db;
        }

        #endregion

        #region Methods

        public async Task<T> GetByIdAsync(int id)
        {
            return await db.Set<T>().FindAsync(id);
        }
        public async Task<IEnumerable<T>> GetByUserAsync(string userId, Expression<Func<T, bool>> filter)
        {
            return await db.Set<T>().Where(filter).ToListAsync();
        }
        public async Task<T> GetByIdAsync(Expression<Func<T, bool>> filter)
        {
            if (filter is not null)
            {
                return await db.Set<T>().Where(filter).FirstOrDefaultAsync();
            }
            return await db.Set<T>().FirstOrDefaultAsync();
        }
        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter)
        {
            if (filter is not null)
            {
                return await db.Set<T>().Where(filter).ToListAsync();
            }

            return await db.Set<T>().ToListAsync();

        }
        public async Task<IEnumerable<T>> FindAllAsync(string[] includes, Expression<Func<T, bool>> filter)
        {
            IQueryable<T> query = db.Set<T>();

            if(includes is not null)
                foreach (var include in includes)
                     query = query.Where(filter).Include(include);

            return await query.Where(filter).ToListAsync();
        }
        public async Task<IEnumerable<T>> FindAllAsync(string[] includes)
        {
            IQueryable<T> query = db.Set<T>();

            if (includes is not null)
                foreach (var include in includes)
                    query = query.Include(include);

            return await query.ToListAsync();
        }
        
        public async Task<T> CreateAsync(T entity)
        {
            await db.Set<T>().AddAsync(entity);
            await db.SaveChangesAsync();

            return entity;
        }

        public async Task<T> UpdateAsync(T entity)
        {
            db.Entry(entity).State = EntityState.Modified;
            await db.SaveChangesAsync();
           
            return entity;
        }

        public async Task<T> DeleteAsync(T entity)
        {
            db.Remove(entity);
            await db.SaveChangesAsync();  
            
            return entity;
        }

     
        #endregion
    }
}
 