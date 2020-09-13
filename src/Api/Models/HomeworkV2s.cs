using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Models
{
    public partial class HomeworkV2s
    {
        public HomeworkV2s()
        {
            ExercicesAlones = new HashSet<ExercicesAlone>();
            Theories = new HashSet<Theory>();
        }

        [Key]
        [Column("HomeworkV2Id")]
        public int HomeworkV2id { get; set; }
        [Column("HomeworkV2Date")]
        public DateTime HomeworkV2date { get; set; }
        [Required]
        [Column("HomeworkV2Name")]
        public string HomeworkV2name { get; set; }
        public int HomeworkTypeId { get; set; }
        public int SessionId { get; set; }
        [Required]
        public string TeacherId { get; set; }
        public bool IsFutur { get; set; }

        [ForeignKey(nameof(HomeworkTypeId))]
        [InverseProperty("HomeworkV2s")]
        public virtual HomeworkType HomeworkType { get; set; }
        [ForeignKey(nameof(SessionId))]
        [InverseProperty("HomeworkV2s")]
        public virtual Session Session { get; set; }
        [ForeignKey(nameof(TeacherId))]
        [InverseProperty(nameof(AspNetUser.HomeworkV2s))]
        public virtual AspNetUser Teacher { get; set; }
        [InverseProperty(nameof(ExercicesAlone.HomeworkV2))]
        public virtual ICollection<ExercicesAlone> ExercicesAlones { get; set; }
        [InverseProperty(nameof(Theory.HomeworkV2))]
        public virtual ICollection<Theory> Theories { get; set; }
    }
}
