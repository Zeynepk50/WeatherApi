using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace App.Repositories
{
    public class GenericRepository<T> (AppDbContext context ): IGenericRepository<T> where T : class //Soldaki lambaya basarak implement interface dedik. //primary construct oluşturarak AppDbContext i alabiliriz. Diğer container e geçtik. 
    {

        protected AppDbContext Context = context;

        //DbSet tanımlaması. Generic olarak T yi aldık yine
        private readonly DbSet<T> _dbSet = context.Set<T>();


        public IQueryable<T> GetAll() => _dbSet.AsQueryable().AsNoTracking(); //Tracker sistemine girmek istemediğimizden bunu yazdık. Dataları track etmesin diye. Çünkü sadece listeleyeceğiz.
        /*throw new NotImplementedException();*/


        public IQueryable<T> Where(Expression<Func<T, bool>> predicate) => _dbSet.Where(predicate).AsNoTracking();/*.AsQueryable();*/
            //throw new NotImplementedException();
        

        public ValueTask<T?> GetByIdAsync(int id) => _dbSet.FindAsync(id);

        //throw new NotImplementedException();


        public async ValueTask AddAsync(T entity) => await _dbSet.AddAsync(entity);

        //throw new NotImplementedException();


        public void Update(T entity) => _dbSet.Update(entity);
        
            //throw new NotImplementedException();
        

        public void Delete(T entity)=> _dbSet.Remove(entity);

    }

}    
