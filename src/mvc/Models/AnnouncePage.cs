using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mvc.Models
{
    public partial class AnnouncePage
    {
        public AnnouncePage()
        {
            AnnouncePageLinks = new HashSet<AnnouncePageLink>();
        }

        [Key]
        public int AnnouncePageId { get; set; }
        [Required]
        public string AnnouncePageName { get; set; }
        public bool IsActivated { get; set; }

        [InverseProperty(nameof(AnnouncePageLink.AnnouncePage))]
        public virtual ICollection<AnnouncePageLink> AnnouncePageLinks { get; set; }
    }
}
