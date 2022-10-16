using System;
using System.Collections.Generic;
using System.Reflection;
using LibraryManagementSystem.WebUI.Business.Abstract;
using LibraryManagementSystem.WebUI.Entity.Concrete;
using LibraryManagementSystem.WebUI.Utilities.Constants;
using LibraryManagementSystem.WebUI.Utilities.Logging;
using LibraryManagementSystem.WebUI.Utilities.Results;

namespace LibraryManagementSystem.WebUI.Business.Concrete {
    public class SecurityManager : ISecurityService {

        public SecurityManager() {
        }

        public IResult Login(User user) {
            try {

            } catch (Exception exception) {
                Logger.Error(LoggerNames.Security, MethodBase.GetCurrentMethod(), exception, $"/*todo*/");
                return new ErrorResult(/*todo*/);
            }
        }

        public IResult Logout(User user) {
            try {

            } catch (Exception exception) {
                Logger.Error(LoggerNames.Security, MethodBase.GetCurrentMethod(), exception, $"/*todo*/");
                return new ErrorResult(/*todo*/);
            }
        }
    }
}