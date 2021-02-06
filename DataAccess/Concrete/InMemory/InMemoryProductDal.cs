using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;

namespace DataAccess.Concrete.InMemory
{
    public class InMemoryProductDal : IProductDal
    {
        List<Product> _products;

        public InMemoryProductDal()
        {
            //oracle,Sql server, postgres, MongoDb'den geliyormuş gibi 
            _products=new  List<Product>()
            {
                new Product(){ProductId = 1,CategoryId = 1,ProductName = "Bardak",UnitPrice = 15,UnitsInStock = 15},
                new Product(){ProductId = 2,CategoryId = 1,ProductName = "Kamera",UnitPrice = 500,UnitsInStock = 3},
                new Product(){ProductId = 3,CategoryId = 2,ProductName = "Telefon",UnitPrice = 1500,UnitsInStock = 2},
                new Product(){ProductId = 4,CategoryId = 2,ProductName = "Klavye",UnitPrice = 150,UnitsInStock = 65},
                new Product(){ProductId = 5,CategoryId = 3,ProductName = "Fare",UnitPrice = 85,UnitsInStock = 1},

            };
        }
        public void Add(Product product)
        {
            _products.Add(product);
        }

        public void Delete(Product product)
        {
            //Linq bilmeden böyle
            //Product productToDelete = null; //=new Product() yazsaydık gereksiz refesans vermiş olurduk. gereksiz memoryyi yormuş olurduk. 
            //foreach (var p in _products)
            //{
            //    if (product.ProductId == p.ProductId)
            //    {
            //        productToDelete = p;
            //    }
            //}

            //_products.Remove(productToDelete);

            //_products.Remove(product); // Bu çalışmaz! Tüm özelliklerini bile aynı versek silmez. Arayüzden newleyip gönderdiğimizde, referansları farklı olduğu için silmez

            //LINQ - Languafe Integrated Query
            Product productToDelete = _products.SingleOrDefault(p=> p.ProductId==product.ProductId); // tek bir eleman bulmaya yarar. p tek tek dolaşırken verdiğimiz takma isim. yukarıdaki foreach in yaptığı işi yapar. her p için pnin productid'si product'ın productid'sine eşit mi diye bakar
            _products.Remove(productToDelete);

        }

        public List<ProductDetailDto> GetProductDetails()
        {
            throw new NotImplementedException();
        }

        public Product Get(Expression<Func<Product, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public List<Product> GetAll()
        {
            return _products;
        }

        public List<Product> GetAll(Expression<Func<Product, bool>> filter = null)
        {
            throw new NotImplementedException();
        }

        public List<Product> GetAllByCategory(int categoryId)
        {
            return _products.Where(p => p.CategoryId == categoryId).ToList(); //sql'deki gibi. LINQ özelliği bu da
        }

        public void Update(Product product) 
        {
            //Gönderdiğim ürünid'sine sahip olan listedeki ürünid'sini yani ürünü bul 
            Product productToUpdate = _products.SingleOrDefault(p => p.ProductId == product.ProductId);
            productToUpdate.ProductName = product.ProductName;
            productToUpdate.CategoryId = product.CategoryId;
            productToUpdate.UnitPrice = product.UnitPrice;
            productToUpdate.UnitsInStock = product.UnitsInStock;
        }
    }
}
