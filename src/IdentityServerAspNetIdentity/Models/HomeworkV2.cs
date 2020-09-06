using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServerAspNetIdentity.Models
{
    public class HomeworkV2
    {
        [Key]
        public int HomeworkV2Id { get; set; }

        [Required]
        public DateTime HomeworkV2Date { get; set; }

        [Required]
        public string HomeworkV2Name { get; set; }

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

    }
}
