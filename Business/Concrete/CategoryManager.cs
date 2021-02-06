using System;
using System.Collections.Generic;
using System.Text;
using Business.Abstract;
using DataAccess.Abstract;
using Entities.Concrete;

namespace Business.Concrete
{
    public class CategoryManager : ICategoryService
    {
        private ICategoryDal _categoryDal;

        public CategoryManager(ICategoryDal categoryDal) //referans üzerinden bağımlıyım
        {
            _categoryDal = categoryDal;
        }
        public List<Category> GetAll()
        {
            //İş kodları
            return _categoryDal.GetAll();
        }

        public Category GetById(int categoryId)
        {
            // select * from Categories where categoryId = 3
            return _categoryDal.Get(c => c.CategoryId == categoryId);
        }
    }
}
