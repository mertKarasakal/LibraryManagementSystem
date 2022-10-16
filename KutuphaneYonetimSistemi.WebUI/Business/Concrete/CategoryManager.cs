using System;
using System.Collections.Generic;
using System.Reflection;
using LibraryManagementSystem.WebUI.Business.Abstract;
using LibraryManagementSystem.WebUI.DataAccess.Abstract;
using LibraryManagementSystem.WebUI.DataAccess.Concrete.EntityFramework;
using LibraryManagementSystem.WebUI.Entity.Concrete;
using LibraryManagementSystem.WebUI.Utilities.Constants;
using LibraryManagementSystem.WebUI.Utilities.Logging;
using LibraryManagementSystem.WebUI.Utilities.Results;

namespace LibraryManagementSystem.WebUI.Business.Concrete {
    public class CategoryManager : ICategoryService {
        private readonly ICategoryDal _categoryDal;

        public CategoryManager() {
            _categoryDal = new EfCategoryDal();
        }

        public DataResult<List<Category>> GetList() {
            try {
                return new SuccessDataResult<List<Category>>(_categoryDal.GetList());
            } catch (Exception exception) {
                Logger.Error(LoggerNames.Library, MethodBase.GetCurrentMethod(), exception, $"/*todo*/");
                return new ErrorDataResult<List<Category>>(/*todo*/);
            }
        }

        public DataResult<Category> GetById(int categoryId) {
            try {
                return new SuccessDataResult<Category>(_categoryDal.Get(c => c.Id == categoryId));
            } catch (Exception exception) {
                Logger.Error(LoggerNames.Library, MethodBase.GetCurrentMethod(), exception, $"/*todo*/");
                return new ErrorDataResult<Category>(/*todo*/);
            }
        }

        public IResult Add(Category category) {
            try {
                _categoryDal.Add(category);
                return new SuccessResult(Messages.CategoryAdded);
            } catch (Exception exception) {
                Logger.Error(LoggerNames.Library, MethodBase.GetCurrentMethod(), exception, $"/*todo*/");
                return new ErrorResult(/*todo*/);
            }
        }

        public IResult Update(Category category) {
            try {
                _categoryDal.Add(category);
                return new SuccessResult(Messages.CategoryUpdated);
            } catch (Exception exception) {
                Logger.Error(LoggerNames.Library, MethodBase.GetCurrentMethod(), exception, $"/*todo*/");
                return new ErrorResult(/*todo*/);
            }
        }

        public IResult Delete(Category category) {
            try {
                _categoryDal.Add(category);
                return new SuccessResult(Messages.CategoryDeleted);
            } catch (Exception exception) {
                Logger.Error(LoggerNames.Library, MethodBase.GetCurrentMethod(), exception, $"/*todo*/");
                return new ErrorResult(/*todo*/);
            }
        }
    }
}