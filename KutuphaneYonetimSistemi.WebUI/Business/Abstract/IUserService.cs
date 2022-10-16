using System.Collections.Generic;
using LibraryManagementSystem.WebUI.Entity.Concrete;
using LibraryManagementSystem.WebUI.Utilities.Results;

namespace LibraryManagementSystem.WebUI.Business.Abstract {
    public interface IUserService {
        DataResult<List<User>> GetList();
        DataResult<User> GetById(int userId);
        IResult Add(User user);
        IResult Update(User user);
        IResult Delete(User user);
    }
}