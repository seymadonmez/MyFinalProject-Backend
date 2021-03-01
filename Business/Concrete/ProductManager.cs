using System;
using System.Collections.Generic;
using System.Text;
using Business.Abstract;
using Business.BusinessAspects.Autofac;
using Business.CCS;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Validation;
using Core.CrossCuttingConcerns.Validation;
using Core.Utilities.Business;
using Core.Utilities.Results;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using DataAccess.Concrete.InMemory;
using Entities.Concrete;
using Entities.DTOs;
using FluentValidation;
using Microsoft.EntityFrameworkCore.Internal;

namespace Business.Concrete
{
    public class ProductManager:IProductService
    {
        IProductDal _productDal;
        ICategoryService _categoryService;
        

        public ProductManager(IProductDal productDal,ICategoryService categoryService)
        {
            _productDal = productDal;
            _categoryService = categoryService;

        }

        public IDataResult<List<Product>> GetAll()
        {
            // İş Kodları
            //InMemoryProductDal inMemoryProductDal=new InMemoryProductDal(); yazabiliriz referans eklediğimiz için.ama bunu böyle yazarsam direkt memory ile çalışır.
            // bir iş sınıfı başka sınıfları newlemez!!

            if (DateTime.Now.Hour == 1)
            {
                return new ErrorDataResult<List<Product>>(Messages.MaintenanceTime);
            }

            return new SuccessDataResult<List<Product>>(_productDal.GetAll(),Messages.ProductsListed);

            //businesste in memory yok, entity yok sadece Iproductdal olmalı
        }

        public IDataResult<List<Product>> GetAllByCategoryId(int id)
        {
            return new SuccessDataResult<List<Product>>(_productDal.GetAll(p => p.CategoryId == id)) ;
        }

        public IDataResult<List<Product>> GetByUnitPrice(decimal min, decimal max)
        {
            return  new SuccessDataResult<List<Product>>(_productDal.GetAll(p => p.UnitPrice >= min && p.UnitPrice <= max));
        }

        public IDataResult<List<ProductDetailDto>> GetProductDetails()
        {
            if (DateTime.Now.Hour == 00)
            {
                return new ErrorDataResult<List<ProductDetailDto>>(Messages.MaintenanceTime);
            }

            return new SuccessDataResult<List<ProductDetailDto>>(_productDal.GetProductDetails());
        }

        public IDataResult<Product> GetById(int productId)
        {
            return new SuccessDataResult<Product>(_productDal.Get(p => p.ProductId == productId));
        }

        [SecuredOperation("product.add,admin")]
        [ValidationAspect(typeof(ProductValidator))]
        public IResult Add(Product product)
        {
            //business codes
            //validation 
            //if (product.UnitPrice <= 0)
            //{
            //    return new ErrorResult(Messages.UnitPriceInvalid);
            //}

            //if (product.ProductName.Length < 2)
            //{
            //    //magic string
            //    return  new ErrorResult(Messages.ProductNameInvalid);
            //}
           

            //ValidationTool.Validate(new ProductValidator(), product);
            //loglama
            //cacheremove
            //performance
            //transaction
            //yetkilendirme

            //bir kategoride en fazla 10 ürün olabilir
            // aynı isimde ürün eklenemez
            // Eğer mevcut category sayısı 15'i geçtiyse sisteme yeni ürün eklenemez. mikroservis mimarilerine nasıl bakmamız gerektiğini öğreneceğiz
            //business codes

            IResult result = BusinessRules.Run(CheckIfProductCountOfCategoryCorrect(product.CategoryId),
                CheckIfProductNameExits(product.ProductName),CheckIfCategoryLimitExceded());
            if (result != null)
            {
                return result;
            }
            _productDal.Add(product);
            return new SuccessResult(Messages.ProductAdded);

        }
        [ValidationAspect(typeof(ProductValidator))]
        public IResult Update(Product product)
        {
            IResult result = BusinessRules.Run(CheckIfProductCountOfCategoryCorrect(product.CategoryId),
                CheckIfProductNameExits(product.ProductName));
            if (result!=null)
            {
                return result;
            }
            _productDal.Update(product);
            return new SuccessResult();

        }

        private IResult CheckIfProductCountOfCategoryCorrect(int categoryId) // Product product da diyebilirdik
        {
            //GetAll(p=>p.CategoryId==categoryId) burası Select Count(*) From Products where categoryId=1  bunu çalıştırır. Tüm datayı çekip sonra filtreleme yapmaz
            var result = _productDal.GetAll(p => p.CategoryId == categoryId).Count;
            if (result >= 15)
            {
                return new ErrorResult(Messages.ProductCountOfCategoryError);
            }
            return new SuccessResult();
        }
        private IResult CheckIfProductNameExits(string productName) // Product product da diyebilirdik
        {
            //GetAll(p=>p.CategoryId==categoryId) burası Select Count(*) From Products where categoryId=1  bunu çalıştırır. Tüm datayı çekip sonra filtreleme yapmaz
            var result = _productDal.GetAll(p => p.ProductName == productName).Any(); //any bu kurala uyan kayıt var mı demek
            if (result)
            {
                return new ErrorResult(Messages.ProductNameAlreadyExists);
            }

            //var result2 = _productDal.GetAll(p => p.ProductName == productName);
            //if (result!=null)
            //{
            //    return new ErrorResult(Messages.ProductNameAlreadyExists);
            //}
            return new SuccessResult();
        }
        private IResult CheckIfCategoryLimitExceded() // Product product da diyebilirdik
        {
            var result = _categoryService.GetAll();
            if (result.Data.Count > 15)
            {
                return new ErrorResult(Messages.CategoryLimitExceded);
            }
            return new SuccessResult();
        }
    }
}
