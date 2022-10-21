using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using LibraryManagementSystem.WebUI.Business.Abstract;
using LibraryManagementSystem.WebUI.DataAccess.Abstract;
using LibraryManagementSystem.WebUI.DataAccess.Concrete.EntityFramework;
using LibraryManagementSystem.WebUI.Entity.Concrete;
using LibraryManagementSystem.WebUI.Utilities.Constants;
using LibraryManagementSystem.WebUI.Utilities.Logging;
using LibraryManagementSystem.WebUI.Utilities.Results;

namespace LibraryManagementSystem.WebUI.Business.Concrete {
    public class BookManager : IBookService {
        private readonly IBookDal _bookDal;

        public BookManager() {
            _bookDal = new EfBookDal();
        }

        public DataResult<List<Book>> GetList() {
            try {
                return new SuccessDataResult<List<Book>>(_bookDal.GetList(b=>b.RecordStatus == true));
            } catch (Exception exception) {
                Logger.Error(LoggerNames.Library, MethodBase.GetCurrentMethod(), exception, $"/*todo*/");
                return new ErrorDataResult<List<Book>>(/*todo*/);
            }
        }

        public DataResult<List<Book>> GetListByCategory(int categoryId) {
            try {
                return new SuccessDataResult<List<Book>>(_bookDal.GetList(b => b.CategoryId == categoryId && b.RecordStatus == true).ToList());
            } catch (Exception exception) {
                Logger.Error(LoggerNames.Library, MethodBase.GetCurrentMethod(), exception, $"/*todo*/");
                return new ErrorDataResult<List<Book>>(/*todo*/);
            }
        }

        public DataResult<List<Book>> GetListByUser(int userId) {
            try {
                return new SuccessDataResult<List<Book>>(_bookDal.GetList(b => b.UserId == userId && b.RecordStatus == true));
            } catch (Exception exception) {
                Logger.Error(LoggerNames.Library, MethodBase.GetCurrentMethod(), exception, $"/*todo*/");
                return new ErrorDataResult<List<Book>>(/*todo*/);
            }
        }

        public DataResult<Book> GetById(int bookId) {
            try {
                return new SuccessDataResult<Book>(_bookDal.Get(b => b.Id == bookId && b.RecordStatus == true));
            } catch (Exception exception) {
                Logger.Error(LoggerNames.Library, MethodBase.GetCurrentMethod(), exception, $"/*todo*/");
                return new ErrorDataResult<Book>(/*todo*/);
            }
        }
        public DataResult<Book> GetByIsbn(string isbn) {
            try {
                return new SuccessDataResult<Book>(_bookDal.Get(b => b.Isbn == isbn && b.RecordStatus == true));
            } catch (Exception exception) {
                Logger.Error(LoggerNames.Library, MethodBase.GetCurrentMethod(), exception, $"/*todo*/");
                return new ErrorDataResult<Book>(/*todo*/);
            }
        }

        public IResult Add(Book book) {
            try {
                _bookDal.Add(book);
                return new SuccessResult(Messages.BookAdded);
            } catch (Exception exception) {
                Logger.Error(LoggerNames.Library, MethodBase.GetCurrentMethod(), exception, $"/*todo*/");
                return new ErrorResult(/*todo*/);
            }
        }

        public IResult Update(Book book) {
            try {
                _bookDal.Update(book);
                return new SuccessResult(Messages.BookUpdated);
            } catch (Exception exception) {
                Logger.Error(LoggerNames.Library, MethodBase.GetCurrentMethod(), exception, $"/*todo*/");
                return new ErrorResult(/*todo*/);
            }
        }

        public IResult Delete(Book book) {
            try {
                _bookDal.Delete(book);
                return new SuccessResult(Messages.BookDeleted);
            } catch (Exception exception) {
                Logger.Error(LoggerNames.Library, MethodBase.GetCurrentMethod(), exception, $"/*todo*/");
                return new ErrorResult(/*todo*/);
            }
        }

        public IResult DeliverTheBook(Book book) {
            try {
                //todo::fix
                return new SuccessResult();
            } catch (Exception exception) {
                Logger.Error(LoggerNames.Library, MethodBase.GetCurrentMethod(), exception, $"/*todo*/");
                return new ErrorResult(/*todo*/);
            }
        }

        public IResult BorrowTheBook(Book book) {
            try {
                //todo::fix
                return new SuccessResult();
            } catch (Exception exception) {
                Logger.Error(LoggerNames.Library, MethodBase.GetCurrentMethod(), exception, $"/*todo*/");
                return new ErrorResult(/*todo*/);
            }
        }
    }
}