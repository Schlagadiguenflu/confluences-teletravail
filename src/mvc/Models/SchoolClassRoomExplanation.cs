using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mvc.Models
{
    public partial class SchoolClassRoomExplanation
    {
        [Key]
        public int SchoolClassRoomExplanationId { get; set; }
        [DisplayName("Langue")]
        [Required]
        public string LanguageName { get; set; }
        [DisplayName("Lien audio")]
        [Required]
        public string AudioLink { get; set; }
        [DisplayName("Classe")]
        public int SchoolClassRoomId { get; set; }
        [DisplayName("Classe")]
        [ForeignKey(nameof(SchoolClassRoomId))]
        [InverseProperty("SchoolClassRoomExplanations")]
        public virtual SchoolClassRoom SchoolClassRoom { get; set; }
    }
}
