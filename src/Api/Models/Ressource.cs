using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Models
{
    [Table("ressources")]
    public partial class Ressource
    {
        public int? Id { get; set; }
        public string SessionNumber { get; set; }
        public DateTime? Date { get; set; }
        public string Name { get; set; }
        public string HomeworkTypeName { get; set; }
        public string Teacher { get; set; }
        public string RessourceType { get; set; }
    }
}
