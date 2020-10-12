using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Models
{
    public partial class Theory
    {
        public Theory()
        {
            Exercices = new HashSet<Exercice>();
        }

        [Key]
        public int TheoryId { get; set; }
        public DateTime TheoryDate { get; set; }
        public string TheoryName { get; set; }
        public string TheoryLink { get; set; }
        [Required]
        public string TeacherId { get; set; }
        [Column("HomeworkV2Id")]
        public int HomeworkV2id { get; set; }
        public string AudioLink { get; set; }
        public string VideoLink { get; set; }

        [ForeignKey(nameof(HomeworkV2id))]
        [InverseProperty(nameof(HomeworkV2s.Theories))]
        public virtual HomeworkV2s HomeworkV2 { get; set; }
        [ForeignKey(nameof(TeacherId))]
        [InverseProperty(nameof(AspNetUser.Theories))]
        public virtual AspNetUser Teacher { get; set; }
        [InverseProperty(nameof(Exercice.Theory))]
        public virtual ICollection<Exercice> Exercices { get; set; }
    }
}
