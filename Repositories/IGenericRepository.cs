using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace App.Repositories
{
    public interface IGenericRepository<T> where T:class  //Generiğe fazla yüklenmek iyi değildir. Bu kadar yeter mesela.
    {
        IQueryable<T> GetAll();
        IQueryable<T> Where(Expression <Func <T, bool >> predicate);
        ValueTask<T?> GetByIdAsync(int id);

        ValueTask AddAsync(T entity); //<T> yaparsak dinamik olur. Yazmazsak asenkron (async) olur
        void Update(T entity);
        void Delete(T entity);




    } 
}
