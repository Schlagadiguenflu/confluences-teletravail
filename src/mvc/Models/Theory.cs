using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mvc.Models
{
    public partial class Theory
    {
        public Theory()
        {
            Exercices = new HashSet<Exercice>();
        }

        [Key]
        public int TheoryId { get; set; }
        [DisplayName("Date du cours")]
        public DateTime TheoryDate { get; set; }
        [DisplayName("Nom du cours")]
        public string TheoryName { get; set; }
        [DisplayName("Lien du cours")]
        public string TheoryLink { get; set; }
        [DisplayName("Lien audio")]
        public string AudioLink { get; set; }
        [DisplayName("Formateur-trice")]
        [Required]
        public string TeacherId { get; set; }
        [DisplayName("Concerne le devoir")]
        [Column("HomeworkV2Id")]
        public int HomeworkV2id { get; set; }
        [DisplayName("Concerne le devoir de")]
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
