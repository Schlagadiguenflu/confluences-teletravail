using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using IdentityServerAspNetIdentity.Data;
using IdentityServerAspNetIdentity.Models;
using IdentityServerAspNetIdentity.ModelsGestionStagiaire;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityServerAspNetIdentity.Controllers
{
    public class migrationController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _contextConfluences;

        private readonly IConfiguration _configuration;
        public migrationController(
                                 UserManager<ApplicationUser> userManager,
                                 ApplicationDbContext contextConfluences,
                                 IConfiguration configuration)
        {
            _userManager = userManager;
            _contextConfluences = contextConfluences;
            _configuration = configuration;
        }
        // GET: migrationController
        public ActionResult Index()
        {
            confluencesContext _contextGestionStagiaire = new confluencesContext();

            List<Eleve> eleves = _contextGestionStagiaire.Eleves.AsNoTracking().ToList();
            // supprimer les espaces
            foreach (var eleve in eleves)
            {
                eleve.Prenomeleve = eleve.Prenomeleve.Trim();
                eleve.NomStagiaire = eleve.NomStagiaire.Trim();
                _contextGestionStagiaire.Eleves.Update(eleve);
                _contextGestionStagiaire.SaveChanges();
            }
            var usersConfluences = _contextConfluences.Users.AsNoTracking().ToList();
            foreach (var userConfluences in usersConfluences)
            {
                userConfluences.Firstname = userConfluences.Firstname.Trim();
                userConfluences.LastName = userConfluences.LastName.Trim();
                _contextConfluences.Update(userConfluences);
                _contextConfluences.SaveChanges();
            }

            string connectionString = Startup.Configuration.GetConnectionString("DefaultConnection");
            var services = new ServiceCollection();
            services.AddLogging();
            services.AddDbContext<ApplicationDbContext>(options =>
               options.UseNpgsql(connectionString));

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                // Basic built in validations
                options.Password.RequiredLength = 1;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = false;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();


            int doublon = 1;
            int cpt = 1;
            int sum = eleves.Count();
            foreach (var eleve in eleves)
            {
                Console.WriteLine("Eleve : " + eleve.Prenomeleve + " " + eleve.NomStagiaire);

                var user = _contextConfluences.Users.Where(u => u.Firstname == eleve.Prenomeleve && u.LastName == eleve.NomStagiaire).ToList();
                //var user = contextConfluences.AspNetUsers.Where(u =>
                //(u.Firstname == eleve.Prenomeleve || u.Firstname == eleve.Prenomeleve + " ") &&
                //(u.LastName == eleve.NomStagiaire || u.LastName == eleve.NomStagiaire + " ")
                //).ToList();
                //var user = contextConfluences.AspNetUsers.Where(u =>
                //    EF.Functions.ILike(u.Firstname, eleve.Prenomeleve) && 
                //    EF.Functions.ILike(u.LastName, eleve.NomStagiaire)
                //).ToList();

                if (user.Count() > 0)
                {
                    Console.WriteLine("\tUtilisateur trouvé ! " + user.Count);
                }
                else
                {
                    Console.WriteLine("\tUtilisateur à créer !");
                    var affiliation = _contextConfluences.TypeAffiliations.Where(c => c.Code == eleve.Codeaffilisation).AsNoTracking().SingleOrDefault();
                    int? typeAffiliationId = null;
                    if (affiliation != null)
                    {
                        typeAffiliationId = affiliation.TypeAffiliationId;
                    }

                    // Enlever accent s'il y en a
                    string userName = eleve.NomStagiaire.Substring(0, 3).ToUpper() + eleve.Prenomeleve.Substring(0, 2).ToUpper();
                    string text = userName.Normalize(NormalizationForm.FormD);
                    var chars = text.Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark).ToArray();
                    userName = new string(chars).Normalize(NormalizationForm.FormC);
                    userName = userName.Replace(" ", "X");
                    if (userName == "TESTE")
                    {
                        userName += doublon;
                        doublon++;
                    }
                    ApplicationUser newUser = new ApplicationUser()
                    {
                        UserName = userName,
                        Firstname = eleve.Prenomeleve,
                        LastName = eleve.NomStagiaire,
                        GenderId = 1,
                        TypeAffiliationId = typeAffiliationId
                    };
                    using (var serviceProvider = services.BuildServiceProvider())
                    {
                        using (var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
                        {
                            var context = scope.ServiceProvider.GetService<ApplicationDbContext>();

                            var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

                            var checkUser = userMgr.FindByNameAsync(newUser.UserName).Result;
                            if (checkUser == null)
                            {

                                newUser.EmailConfirmed = true;

                                checkUser = newUser;
                                var result = userMgr.CreateAsync(checkUser, "0").Result;
                                if (!result.Succeeded)
                                {
                                    throw new Exception(result.Errors.First().Description);
                                }
                                else
                                {
                                    //_logger.LogInformation("User created a new account with password.");

                                    // This is for send an email
                                    //var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                                    //code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                                    //var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, Request.Scheme);

                                    //string message = "Salut mon pote comment ca va ? si tu veux confirmer ton inscription c'est par <a href='" + callbackUrl + "'>ici</a>";
                                    //await _emailSender.SendEmailAsync(user.Email, "Confirmer votre Email", message);

                                }

                            }
                            else
                            {
                                throw new Exception("Votre nom d'utilisateur-trice est déjà pris");
                            }
                        }
                    }
                }
            }

            using (var serviceProvider = services.BuildServiceProvider())
            {
                using (var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
                {
                    var context = scope.ServiceProvider.GetService<ApplicationDbContext>();

                    var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

                    ApplicationUser newUser2 = new ApplicationUser()
                    {
                        UserName = "LG",
                        Firstname = "Line",
                        LastName = "G.",
                        GenderId = 2
                    };
                    var result = userMgr.CreateAsync(newUser2, "0").Result;

                    ApplicationUser newUser3 = new ApplicationUser()
                    {
                        UserName = "PT",
                        Firstname = "PT",
                        LastName = "PT",
                        GenderId = 1
                    };
                    var result2 = userMgr.CreateAsync(newUser3, "0").Result;

                    ApplicationUser newUser4 = new ApplicationUser()
                    {
                        UserName = "OM",
                        Firstname = "OM",
                        LastName = "OM",
                        GenderId = 1
                    };
                    var result3 = userMgr.CreateAsync(newUser4, "0").Result;
                }
            }
                   
            return Ok();
    }

}
}
