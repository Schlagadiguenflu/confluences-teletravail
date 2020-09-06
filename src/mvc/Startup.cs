using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Mvc.Razor;
using System.Globalization;
using System.Collections.Generic;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.HttpOverrides;
using IdentityModel;
using Microsoft.AspNetCore.Authentication;

namespace mvc
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public static IConfiguration Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {  
            services.AddControllersWithViews();

            JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

            System.Console.WriteLine("Authority is : " + Configuration["URLIdentityServer4"]);

            // Besoin de l'utilisation des cookies pour gérer les authentification avec le protocole OpenIdConnect
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "Cookies";
                options.DefaultChallengeScheme = "oidc";
            })
                .AddCookie("Cookies")
             .AddOpenIdConnect("oidc", options =>
              {
                  options.SignInScheme = "Cookies";

                  options.Authority = Configuration["URLIdentityServer4"];
                  options.RequireHttpsMetadata = false;

                  options.ClientId = "mvc";
                  options.ClientSecret = "secret";
                  options.ResponseType = "code";

                  options.SaveTokens = true;
                  options.GetClaimsFromUserInfoEndpoint = true;

                  options.Scope.Add("api1");
                  options.Scope.Add("roles");
                  options.Scope.Add("offline_access");

                  // Fix for getting roles claims correctly :
                  options.ClaimActions.MapJsonKey("role", "role", "role");

                  options.TokenValidationParameters.NameClaimType = "name";
                  options.TokenValidationParameters.RoleClaimType = "role";
              });

            //services.AddSingleton<Microsoft.AspNetCore.Authentication.IClaimsTransformation, KarekeClaimsTransformer>();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Teacher", policy => policy.RequireClaim(JwtClaimTypes.Role, "Teacher"));
            });

            // Localisation
            services.AddLocalization(opts => { opts.ResourcesPath = "Resources"; });

            services.AddMvc()
                .AddViewLocalization(
                    LanguageViewLocationExpanderFormat.Suffix,
                    opts => { opts.ResourcesPath = "Resources"; })
                .AddDataAnnotationsLocalization();

            services.Configure<RequestLocalizationOptions>(
                opts =>
                {
                    var supportedCultures = new List<CultureInfo>
                    {
                        new CultureInfo("fr-FR"),
                        //new CultureInfo("en-US"),
                        //new CultureInfo("en"),
                        //new CultureInfo("en-GB"),
                        new CultureInfo("fr"),
                    };

                    opts.DefaultRequestCulture = new RequestCulture("fr-FR");
                    // Formatting numbers, dates, etc.
                    opts.SupportedCultures = supportedCultures;
                    // UI strings that we have localized.
                    opts.SupportedUICultures = supportedCultures;
                });
            //
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            var fordwardedHeaderOptions = new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            };
            fordwardedHeaderOptions.KnownNetworks.Clear();
            fordwardedHeaderOptions.KnownProxies.Clear();
            app.UseForwardedHeaders(fordwardedHeaderOptions);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //app.UseStatusCodePages(async context =>
            //{
            //    var response = context.HttpContext.Response;

            //    if (response.StatusCode == (int)HttpStatusCode.Unauthorized ||
            //        response.StatusCode == (int)HttpStatusCode.Forbidden)
            //        response.Redirect("/Account/Logout");
            //});

            //app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            // Localisation
            var options = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(options.Value);
            //

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}/{id2?}");
            });

        }
    }
}
