using System;

namespace LibraryManagementSystem.WebUI.Entity.Abstract {
    public interface IEntity {
        int Id { get; set; }
        string UpdatingUserCode { get; set; }
        string UpdatingTranCode { get; set; }
        DateTime? UpdateDate { get; set; }
        bool RecordStatus { get; set; }
    }
}