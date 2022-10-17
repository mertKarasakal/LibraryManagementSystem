using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Security;
using LibraryManagementSystem.WebUI.Business.Abstract;
using LibraryManagementSystem.WebUI.Entity.Concrete;
using LibraryManagementSystem.WebUI.Utilities.Constants;
using LibraryManagementSystem.WebUI.Utilities.Logging;
using LibraryManagementSystem.WebUI.Utilities.Results;

namespace LibraryManagementSystem.WebUI.Business.Concrete {
    public class SecurityManager : ISecurityService {
        private readonly IUserService _userManager;

        public SecurityManager() {
            _userManager = new UserManager();
        }

        public IResult Login(User user) {
            try {
                var existedUser = _userManager.GetUserByCredentials(user.Username, user.Password).Data;
                if (existedUser != null) {
                    FormsAuthentication.SetAuthCookie(existedUser.Username, false);
                    return new SuccessResult( /*todo::message*/);
                }
                return new ErrorResult( Messages.UserNotFound);
            } catch (Exception exception) {
                Logger.Error(LoggerNames.Security, MethodBase.GetCurrentMethod(), exception, $"/*todo*/");
                return new ErrorResult(/*todo*/);
            }
        }

        public IResult Logout() {
            try {
                FormsAuthentication.SignOut();
                return new SuccessResult(/*todo::message*/);
            } catch (Exception exception) {
                Logger.Error(LoggerNames.Security, MethodBase.GetCurrentMethod(), exception, $"/*todo*/");
                return new ErrorResult(/*todo*/);
            }
        }
    }
}