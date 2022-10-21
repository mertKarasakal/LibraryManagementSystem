using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LibraryManagementSystem.WebUI.Entity.Abstract;

namespace LibraryManagementSystem.WebUI.Entity.Concrete {
    [Table("Book")]
    public  class Book : IEntity {
        public int Id { get; set; }

        public int? UserId { get; set; }

        public int? CategoryId { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }


        [StringLength(13)]
        public string Isbn { get; set; }

        [StringLength(50)]
        public string Author { get; set; }

        public string Description { get; set; }

        public int? NumberOfPages { get; set; }

        public virtual Category Category { get; set; }

        public virtual User User { get; set; }

        public string ImagePath { get; set; }

        public string Status { get; set; }

        public DateTime? DeliveryTime { get; set; }
        public string UpdatingUserCode { get; set; }
        public string UpdatingTranCode { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool RecordStatus { get; set; }
    }
}
