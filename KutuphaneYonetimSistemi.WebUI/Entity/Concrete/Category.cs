using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LibraryManagementSystem.WebUI.Entity.Abstract;

namespace LibraryManagementSystem.WebUI.Entity.Concrete {
    [Table("Category")]
    public class Category : IEntity {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        public string UpdatingUserCode { get; set; }
        public string UpdatingTranCode { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool RecordStatus { get; set; }
    }
}