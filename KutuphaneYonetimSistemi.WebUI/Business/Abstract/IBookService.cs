using System.Collections.Generic;
using LibraryManagementSystem.WebUI.Entity.Concrete;
using LibraryManagementSystem.WebUI.Utilities.Results;

namespace LibraryManagementSystem.WebUI.Business.Abstract {
    public interface IBookService {
        DataResult<List<Book>> GetList();
        DataResult<List<Book>> GetListByCategory(int categoryId);
        DataResult<List<Book>> GetListByUser(int userId);
        DataResult<Book> GetById(int bookId);
        IResult Add(Book book);
        IResult Update(Book book);
        IResult Delete(Book book);
        IResult DeliverTheBook(Book book);
        IResult BorrowTheBook(Book book);
    }
}