using Api.Models;
using Api.ViewModel;
using AutoMapper;

namespace Api.Utility
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<Entrepris, Entreprise>(); // means you want to map from Entrepris to Entreprise
        }
    }
}
