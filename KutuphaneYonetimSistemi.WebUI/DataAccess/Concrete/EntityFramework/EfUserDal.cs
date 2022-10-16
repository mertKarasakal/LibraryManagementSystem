using LibraryManagementSystem.WebUI.DataAccess.Abstract;
using LibraryManagementSystem.WebUI.Entity.Concrete;
using LibraryManagementSystem.WebUI.Models.EntityFramework;

namespace LibraryManagementSystem.WebUI.DataAccess.Concrete.EntityFramework {
    public class EfUserDal : EfEntityRepositoryBase<User, LibraryContext>, IUserDal {
    }
}