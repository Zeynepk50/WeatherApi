using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace App.Repositories;
public class UnitOfWork(AppDbContext context) : IUnitOfWork //Yine lambadan implement ekledik. 
{

    public Task<int> SaveChangesAsync() => context.SaveChangesAsync();
                
        //public Task<int> SaveChangesAsync() 
        //{
        //    return context.SaveChangesAsync();
        //    //throw new NotImplementedException();
        //}
}

