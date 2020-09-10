// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServerAspNetIdentity.Data;
using IdentityServerAspNetIdentity.Models;
using IdentityServerAspNetIdentity.Utility;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace IdentityServerAspNetIdentity
{
    public class Startup
    {
        public IWebHostEnvironment Environment { get; }
        public static IConfiguration Configuration { get; private set; }

        private const string XForwardedPathBase = "X-Forwarded-PathBase";
        private const string XForwardedProto = "X-Forwarded-Proto";

        public Startup(IWebHostEnvironment environment, IConfiguration configuration)
        {
            Environment = environment;
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                // this defines a CORS policy called "default"
                options.AddPolicy("default", policy =>
                {
                    policy.WithOrigins(Configuration["URLVueJsGestionStagiaire"].Remove(Configuration["URLVueJsGestionStagiaire"].Length - 1))
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            services.AddControllersWithViews();

            // configures IIS out-of-proc settings (see https://github.com/aspnet/AspNetCore/issues/14882)
            services.Configure<IISOptions>(iis =>
            {
                iis.AuthenticationDisplayName = "Windows";
                iis.AutomaticAuthentication = false;
            });

            // configures IIS in-proc settings
            services.Configure<IISServerOptions>(iis =>
            {
                iis.AuthenticationDisplayName = "Windows";
                iis.AutomaticAuthentication = false;
            });

            //services.AddDbContext<ApplicationDbContext>(options =>
            //    options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            var test = Configuration.GetConnectionString("DefaultConnection");
            services.AddEntityFrameworkNpgsql().AddDbContext<ApplicationDbContext>(
                opt => opt.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders()
                // pour les roles
                .AddClaimsPrincipalFactory<ClaimsFactory>();


            var builder = services.AddIdentityServer(options =>
                {
                    //options.PublicOrigin = Configuration["URLIdentityServer4"];
                    options.Events.RaiseErrorEvents = true;
                    options.Events.RaiseInformationEvents = true;
                    options.Events.RaiseFailureEvents = true;
                    options.Events.RaiseSuccessEvents = true;
                })
                .AddInMemoryIdentityResources(Config.Ids)
                .AddInMemoryApiResources(Config.Apis)
                .AddInMemoryClients(Config.Clients)
                .AddAspNetIdentity<ApplicationUser>();

            builder.AddDeveloperSigningCredential();
            //// Ajout d'un certificat pour Identity Server 4
            //if (Environment.IsDevelopment())
            //{
            //    // SEULEMENT POUR ENV DEVELOPPEMENT
            //    builder.AddDeveloperSigningCredential();
            //}
            //else
            //{
            //    X509Certificate2 cert = null;
            //    using (X509Store certStore = new X509Store(StoreName.My, StoreLocation.CurrentUser))
            //    {
            //        certStore.Open(OpenFlags.ReadOnly);
            //        X509Certificate2Collection certCollection = certStore.Certificates.Find(
            //            X509FindType.FindByThumbprint,
            //            // Replace below with your cert's thumbprint
            //            Configuration["CertificationThumbprint"],
            //            false);
            //        // Get the first cert with the thumbprint
            //        if (certCollection.Count > 0)
            //        {
            //            cert = certCollection[0];
            //            Log.Logger.Information($"Successfully loaded cert from registry: {cert.Thumbprint}");
            //        }
            //    }

            //    // Fallback to DeveloperSigningCredential
            //    if (cert == null)
            //    {
            //        Log.Logger.Error($"Certification not found, going to use DeveloperSigningCredential NOT GOOD");
            //        builder.AddDeveloperSigningCredential();
            //    }
            //    else
            //    {
            //        builder.AddSigningCredential(cert);
            //    }
            //}

            services.AddAuthentication()
                .AddGoogle(options =>
                {
                    // register your IdentityServer with Google at https://console.developers.google.com
                    // enable the Google+ API
                    // set the redirect URI to http://localhost:5000/signin-google
                    options.ClientId = "copy client ID from Google here";
                    options.ClientSecret = "copy client secret from Google here";
                });


            //SeedData.EnsureSeedData(Configuration.GetConnectionString("DefaultConnection"));

            // requires
            // using Microsoft.AspNetCore.Identity.UI.Services;
            // using WebPWrecover.Services;
            //services.AddTransient<IEmailSender, EmailSender>();
            //services.Configure<AuthMessageSenderOptions>(Configuration);

            //var accountSid = Configuration["Twilio:AccountSID"]; 
            //var authToken = Configuration["Twilio:AuthToken"]; 
            //TwilioClient.Init(accountSid, authToken);
            //services.Configure<TwilioVerifySettings>(Configuration.GetSection("Twilio"));

            // Pour les roles
            //services.AddTransient<IResourceOwnerPasswordValidator, ResourceOwnerPasswordValidator>();
            //services.AddTransient<IProfileService, AspNetIdentityProfileService>();

        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseCors("default");

            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }

            app.UseStaticFiles();

            app.UseRouting();

            var fordwardedHeaderOptions = new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            };
            fordwardedHeaderOptions.KnownNetworks.Clear();
            fordwardedHeaderOptions.KnownProxies.Clear();
            app.UseForwardedHeaders(fordwardedHeaderOptions);

            app.UseIdentityServer();

            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}