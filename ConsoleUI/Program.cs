using System;
using Business.Concrete;
using DataAccess.Concrete.EntityFramework;
using DataAccess.Concrete.InMemory;

namespace ConsoleUI
{
    //SOLID 
    // Open Closed Principal- yeni bir özellij ekleniyorsa mevcuttaki hiçbir koda dokunmamalısın
    class Program
    {
        static void Main(string[] args)
        {
            ProductManager productManager=new ProductManager(new EFProductDal());

            foreach (var product in productManager.GetByUnitPrice(40,100))
            {
                Console.WriteLine(product.ProductName);
            }

        }
    }
}
