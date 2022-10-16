using System.Web;
using LibraryManagementSystem.WebUI.Utilities.Results;

namespace LibraryManagementSystem.WebUI.Business.Abstract {
    public interface IImageProcessingService {
        DataResult<string> ProcessImage(HttpPostedFileBase uploadFile);
    }
}