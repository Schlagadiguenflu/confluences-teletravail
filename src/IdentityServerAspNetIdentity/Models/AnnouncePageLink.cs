using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServerAspNetIdentity.Models
{
    public class AnnouncePageLink
    {
        [Key]
        public int AnnouncePageLinkId { get; set; }

        [Required]
        public string AnnouncePageLinkName { get; set; }

        [Required]
        public string AnnouncePageLinkLanguage { get; set; }

        public int AnnouncePageId { get; set; }
        [ForeignKey(nameof(AnnouncePageId))]
        public virtual AnnouncePage AnnouncePage { get; set; }
    }
}
