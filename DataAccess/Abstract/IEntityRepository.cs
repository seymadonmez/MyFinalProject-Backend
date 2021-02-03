using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq.Expressions;
using System.Text;
using Entities.Abstract;

namespace DataAccess.Abstract
{
    //generic constrait
    //where T:class -> referans tip olabilir anlamına gelir
    //IEntity : IEntity olabilir ya da IEntity implemente eden bir nesne olabilir
    // new() : new'lenebilir olmalı 
    public interface IEntityRepository<T> where T:class,IEntity,new() //T referans tip olmalı ve T ya IEntity olabilir ya da IEntity'yi implemente eden bir sınıf olabilir.
    {
        List<T> GetAll(Expression<Func<T, bool>>filter=null);
        T Get(Expression<Func<T, bool>> filter);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        //List<T> GetAllByCategory(int entity); //expression yazdığımız için gerek kalmadı.
    }
}
