using System.Collections.Generic;
using LibraryManagementSystem.WebUI.Entity.Concrete;

namespace LibraryManagementSystem.WebUI.Utilities.Results {
    public class SuccessDataResult<T> : DataResult<T> {
        public SuccessDataResult(T data, string message) : base(data, true, message) {
        }
        public SuccessDataResult(T data) : base(data, true) {
        }
        public SuccessDataResult(string message) : base(default, true, message) {
        }
        public SuccessDataResult(IList<User> getList) : base(default, true) {
        }
    }
}