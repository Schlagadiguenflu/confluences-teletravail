using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Models
{
    public partial class SchoolClassRoomExplanation
    {
        [Key]
        public int SchoolClassRoomExplanationId { get; set; }
        [Required]
        public string LanguageName { get; set; }
        [Required]
        public string AudioLink { get; set; }
        public int SchoolClassRoomId { get; set; }

        [ForeignKey(nameof(SchoolClassRoomId))]
        [InverseProperty("SchoolClassRoomExplanations")]
        public virtual SchoolClassRoom SchoolClassRoom { get; set; }
    }
}
