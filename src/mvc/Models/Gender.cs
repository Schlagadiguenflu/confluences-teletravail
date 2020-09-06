using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mvc.Models
{
    public partial class Gender
    {
        public Gender()
        {
            AspNetUsers = new HashSet<AspNetUser>();
        }

        [Key]
        public short GenderId { get; set; }
        [Required]
        [StringLength(10)]
        public string GenderName { get; set; }

        [InverseProperty(nameof(AspNetUser.Gender))]
        public virtual ICollection<AspNetUser> AspNetUsers { get; set; }
    }
}
