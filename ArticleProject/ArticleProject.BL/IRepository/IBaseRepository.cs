using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ArticleProject.BL.IRepository
{
    public interface IBaseRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter = null);
        Task<T> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetByUserAsync(string userId, Expression<Func<T, bool>> filter);
        Task<T> GetByIdAsync(Expression<Func<T, bool>> filter = null);
        Task<T> CreateAsync(T entity);

        Task<T> UpdateAsync(T entity);

        Task<T> DeleteAsync(T entity);

        Task<IEnumerable<T>> FindAllAsync(string[] includes, Expression<Func<T, bool>> filter);
        Task<IEnumerable<T>> FindAllAsync(string[] includes);




    }
}