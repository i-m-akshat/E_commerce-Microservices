
using Ecommerce.SharedLibrary.Responses;
using System.Linq.Expressions;

namespace Ecommerce.SharedLibrary.Interfaces
{
    public interface IGenericInterface<T> where T:class
    {
        /// <summary>
        /// To create the data
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<Response> CreateAsync(T entity);
        /// <summary>
        /// To Delete the data
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Response> DeleteAsync(int id);
        /// <summary>
        /// To Update the data
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<Response> UpdateAsync(T entity);
        /// <summary>
        /// To Get All The Data
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<T>> GetAllAsync();
        /// <summary>
        /// To Find the data by its id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<T> FindByIdAsync(int id);
        /// <summary>
        /// To get data by 
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task<T> GetByAsync(Expression<Func<T, bool>> predicate);
    }
}
