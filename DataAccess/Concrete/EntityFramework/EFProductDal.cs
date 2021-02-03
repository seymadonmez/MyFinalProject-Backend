using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using DataAccess.Abstract;
using Entities.Concrete;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Concrete.EntityFramework
{
    public class EFProductDal : IProductDal
    {
        public void Add(Product entity)
        {
            // IDisposible pattern implementation of c# - using bittiği anda garbagecollectordan hızlıca temizlenmesini sağlar
            using (NorthwindContext context=new NorthwindContext())
            {
                var addedEntity = context.Entry(entity); //burada eşleştirdik veritabanı ile 
                addedEntity.State = EntityState.Added; // burada da veritabanında eklenmesi işlemini gerçekleştirdik.
                context.SaveChanges();
            }
        }

        public void Delete(Product entity)
        {
            using (NorthwindContext context = new NorthwindContext())
            {
                var deletedEntity = context.Entry(entity);
                deletedEntity.State = EntityState.Deleted; 
                context.SaveChanges();
            }
        }

        public Product Get(Expression<Func<Product, bool>> filter) // tabloyu liste şeklinde istedğim filtreye göre getirmeyi yarıyor.
        {
            using (NorthwindContext context=new NorthwindContext())
            {
                return context.Set<Product>().SingleOrDefault(filter);
            }
        }

        public List<Product> GetAll(Expression<Func<Product, bool>> filter = null) //null ise defaultta 
        {
            using (NorthwindContext context =new NorthwindContext())
            {
                return filter == null ? context.Set<Product>().ToList() : context.Set<Product>().Where(filter).ToList();
            }
        }

        public void Update(Product entity)
        {
            using (NorthwindContext context = new NorthwindContext())
            {
                var updatedEntity = context.Entry(entity);
                updatedEntity.State = EntityState.Modified;
                context.SaveChanges();
            }
        }
    }
}
