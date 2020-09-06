using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mvc.Models
{
    public partial class HomeworkType
    {
        public HomeworkType()
        {
            Homework = new HashSet<Homework>();
            HomeworkV2s = new HashSet<HomeworkV2s>();
        }

        [Key]
        public int HomeworkTypeId { get; set; }
        [Required]
        public string HomeworkTypeName { get; set; }

        public int HomeworkOrder { get; set; }

        [InverseProperty("HomeworkType")]
        public virtual ICollection<Homework> Homework { get; set; }
        [InverseProperty("HomeworkType")]
        public virtual ICollection<HomeworkV2s> HomeworkV2s { get; set; }
    }
}
