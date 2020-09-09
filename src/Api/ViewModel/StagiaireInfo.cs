using Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.ViewModel
{
    public class StagiaireInfo
    {
        public string StagiaireId { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public int TypeAffiliationId { get; set; }
        public TypeAffiliation TypeAffiliation { get; set; }
        public ICollection<Stage> StageStagiaires { get; set; }
    }
}
