using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServerAspNetIdentity.Models
{
    public class Theory
    {
        [Key]
        public int TheoryId { get; set; }

        [Required]
        public DateTime TheoryDate { get; set; }
        public string TheoryName { get; set; }
        public string TheoryLink { get; set; }

        public string AudioLink { get; set; }

        [Required]
        public string TeacherId { get; set; }
        [ForeignKey(nameof(TeacherId))]
        public virtual ApplicationUser Teacher { get; set; }

        [Required]
        public int HomeworkV2Id { get; set; }
        [ForeignKey(nameof(HomeworkV2Id))]
        public virtual HomeworkV2 HomeworkV2 { get; set; }
    }
}
