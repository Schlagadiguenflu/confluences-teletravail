using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServerAspNetIdentity.Models
{
    public class Homework
    {
        [Key]
        public int HomeworkId { get; set; }

        [Required]
        public DateTime HomeworkDate { get; set; }

        public string ClassName { get; set; }
        public string ClassLink { get; set; }

        public string ExerciceName { get; set; }
        public string ExerciceLink { get; set; }

        [Required]
        public int HomeworkTypeId { get; set; }
        [ForeignKey(nameof(HomeworkTypeId))]
        public virtual HomeworkType HomeworkType { get; set; }

        [Required]
        public int SessionId { get; set; }
        [ForeignKey(nameof(SessionId))]
        public virtual Session Session { get; set; }

        [Required]
        public string TeacherId { get; set; }
        [ForeignKey(nameof(TeacherId))]
        public virtual ApplicationUser Teacher { get; set; }

        [DisplayName("Est un devoir supplémentaire")]
        public bool IsHomeworkAdditionnal { get; set; }
    }
}
