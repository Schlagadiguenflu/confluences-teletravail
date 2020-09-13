using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mvc.Models
{
    public partial class HomeworkV2s
    {
        public HomeworkV2s()
        {
            Theories = new HashSet<Theory>();
        }

        [Key]
        [Column("HomeworkV2Id")]
        public int HomeworkV2id { get; set; }
        [DisplayName("Date")]
        [Column("HomeworkV2Date")]
        public DateTime HomeworkV2date { get; set; }
        [DisplayName("Nom")]
        [Required]
        [Column("HomeworkV2Name")]
        public string HomeworkV2name { get; set; }
        [DisplayName("Type")]
        public int HomeworkTypeId { get; set; }
        [DisplayName("Classe")]
        public int SessionId { get; set; }
        [DisplayName("Formateur-trice")]
        [Required]
        public string TeacherId { get; set; }
        [DisplayName("Afficher en tant que devoir futur")]
        public bool IsFutur { get; set; }
        [DisplayName("Type")]
        [ForeignKey(nameof(HomeworkTypeId))]
        [InverseProperty("HomeworkV2s")]
        public virtual HomeworkType HomeworkType { get; set; }
        [DisplayName("Classe")]
        [ForeignKey(nameof(SessionId))]
        [InverseProperty("HomeworkV2s")]
        public virtual Session Session { get; set; }
        [DisplayName("Formateur-trice")]
        [ForeignKey(nameof(TeacherId))]
        [InverseProperty(nameof(AspNetUser.HomeworkV2s))]
        public virtual AspNetUser Teacher { get; set; }
        [InverseProperty(nameof(ExercicesAlone.HomeworkV2))]
        public virtual ICollection<ExercicesAlone> ExercicesAlones { get; set; }
        [InverseProperty(nameof(Theory.HomeworkV2))]
        public virtual ICollection<Theory> Theories { get; set; }
    }
}
