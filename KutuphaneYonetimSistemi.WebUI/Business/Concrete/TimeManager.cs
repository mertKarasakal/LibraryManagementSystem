using LibraryManagementSystem.WebUI.Business.Abstract;
using LibraryManagementSystem.WebUI.DataAccess.Abstract;
using LibraryManagementSystem.WebUI.DataAccess.Concrete.EntityFramework;

namespace LibraryManagementSystem.WebUI.Business.Concrete {
    public class TimeManager : ITimeService {
        private readonly ITimeDal _timeDal;

        public TimeManager() {
            _timeDal = new EfTimeDal();
        }
    }
}