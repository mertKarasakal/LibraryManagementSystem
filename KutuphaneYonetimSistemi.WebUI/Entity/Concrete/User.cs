using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LibraryManagementSystem.WebUI.Entity.Abstract;

namespace LibraryManagementSystem.WebUI.Entity.Concrete {
    [Table("User")]
    public class User : IEntity {
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string Name { get; set; }

        [Required]
        [StringLength(20)]
        public string Surname { get; set; }

        public int Role { get; set; }

        [Required]
        [StringLength(20)]
        public string Username { get; set; }

        [Required]
        [StringLength(20)]
        public string Password { get; set; }
        public string UpdatingUserCode { get; set; }
        public string UpdatingTranCode { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool RecordStatus { get; set; }
    }
}
