using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServerAspNetIdentity.Models
{
    public class AnnouncePage
    {
        [Key]
        public int AnnouncePageId { get; set; }

        [Required]
        public string AnnouncePageName { get; set; }

        public bool IsActivated { get; set; }
    }
}
