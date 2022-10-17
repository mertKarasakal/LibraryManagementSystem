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
    public class UserManager : IUserService {
        private readonly IUserDal _userDal;

        public UserManager() {
            _userDal = new EfUserDal();
        }

        public DataResult<List<User>> GetList() {
            try {
                return new SuccessDataResult<List<User>>(_userDal.GetList());
            } catch (Exception exception) {
                Logger.Error(LoggerNames.Library, MethodBase.GetCurrentMethod(), exception, $"/*todo*/");
                return new ErrorDataResult<List<User>>(/*todo*/);
            }
        }
        
        public DataResult<User> GetUserByCredentials(string username, string password) {
            try {
                return new SuccessDataResult<User>(_userDal.Get(u => u.Username == username && u.Password == password));
            } catch (Exception exception) {
                Logger.Error(LoggerNames.Library, MethodBase.GetCurrentMethod(), exception, $"/*todo*/");
                return new ErrorDataResult<User>(/*todo*/);
            }
        }
        public DataResult<User> GetById(int userId) {
            try {
                return new SuccessDataResult<User>(_userDal.Get(u => u.Id == userId));
            } catch (Exception exception) {
                Logger.Error(LoggerNames.Library, MethodBase.GetCurrentMethod(), exception, $"/*todo*/");
                return new ErrorDataResult<User>(/*todo*/);
            }
        }

        public IResult Add(User user) {
            try {
                _userDal.Add(user);
                return new SuccessResult(Messages.UserAdded);
            } catch (Exception exception) {
                Logger.Error(LoggerNames.Library, MethodBase.GetCurrentMethod(), exception, $"/*todo*/");
                return new ErrorResult(/*todo*/);
            }
        }

        public IResult Update(User user) {
            try {
                _userDal.Update(user);
                return new SuccessResult(Messages.UserUpdated);
            } catch (Exception exception) {
                Logger.Error(LoggerNames.Library, MethodBase.GetCurrentMethod(), exception, $"/*todo*/");
                return new ErrorResult(/*todo*/);
            }
        }

        public IResult Delete(User user) {
            try {
                _userDal.Delete(user);
                return new SuccessResult(Messages.UserDeleted);
            } catch (Exception exception) {
                Logger.Error(LoggerNames.Library, MethodBase.GetCurrentMethod(), exception, $"/*todo*/");
                return new ErrorResult(/*todo*/);
            }
        }
    }
}