using System.Collections.Generic;
using LibraryManagementSystem.WebUI.Entity.Concrete;
using LibraryManagementSystem.WebUI.Utilities.Results;

namespace LibraryManagementSystem.WebUI.Business.Abstract {
    public interface ICategoryService {
        DataResult<List<Category>> GetList();
        DataResult<Category> GetById(int categoryId);
        IResult Add(Category category);
        IResult Update(Category category);
        IResult Delete(Category category);
    }
}