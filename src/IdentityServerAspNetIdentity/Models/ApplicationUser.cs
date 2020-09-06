using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace IdentityServerAspNetIdentity.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        /* Donnée supplémentaire pour la base de donnée */
        [DisplayName("Prénom")]
        [Required]
        public string Firstname { get; set; }

        [DisplayName("Nom")]
        [Required]
        public string LastName { get; set; }

        [DisplayName("Date de naissance")]
        public DateTime Birthday { get; set; }

        [DisplayName("Genre")]
        public short GenderId { get; set; }

        [DisplayName("Genre")]
        [ForeignKey(nameof(GenderId))]
        public virtual Gender Gender { get; set; }

        [DisplayName("Image de profil")]
        public string PathImage { get; set; }

        [DisplayName("Veut des devoirs supplémentaires")]
        public bool WantsMoreHomeworks { get; set; }

        [DisplayName("A vu la video")]
        public bool HasSeenHelpVideo { get; set; }

        [DisplayName("Affiliation")]
        public int? TypeAffiliationId { get; set; }
        [DisplayName("Affiliation")]
        [ForeignKey(nameof(TypeAffiliationId))]
        public virtual TypeAffiliation TypeAffiliation { get; set; }

    }
}
