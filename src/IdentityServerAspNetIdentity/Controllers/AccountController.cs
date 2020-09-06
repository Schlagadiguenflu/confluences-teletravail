using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using IdentityServerAspNetIdentity.Models;
using Microsoft.Extensions.DependencyInjection;
using IdentityServerAspNetIdentity.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using IdentityModel;
using Serilog;
using IdentityServerAspNetIdentity.Services;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using Microsoft.Extensions.Configuration;
using IdentityServerAspNetIdentity.ViewModels;
using System.Text.Encodings.Web;
using Microsoft.Extensions.Options;
using Twilio.Rest.Verify.V2.Service;
using PhoneNumbers;
using IdentityServer4.Quickstart.UI;

namespace IdentityServerAspNetIdentity.Controllers
{
    public class AccountController : Controller
    {
        private readonly IEmailSender _emailSender;
        private readonly TwilioVerifySettings _settings;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private static PhoneNumberUtil _phoneUtil;
        private readonly IConfiguration _configuration;

        public AccountController(/*IEmailSender emailSender,*/
                                 IOptions<TwilioVerifySettings> settings,
                                 UserManager<ApplicationUser> userManager,
                                 ApplicationDbContext context,
                                 IConfiguration configuration)
        {
            //_emailSender = emailSender;
            _settings = settings.Value;
            _userManager = userManager;
            _context = context;
            _phoneUtil = PhoneNumberUtil.GetInstance();
            _configuration = configuration;
        }

        public async Task<IActionResult> SignUpAsync()
        {
            List<Gender> genders = await _context.Genders.ToListAsync();
            ViewData["genders"] = genders;

            return View();
        }

        [Route("SignUpSend", Name = "SignUpSend")]
        [HttpPost]
        public async Task<IActionResult> SignUpSend(RegisterInput input)
        {
            if (input.PasswordHash == null && input.ConfirmPassword == null)
            {
                input.PasswordHash = "0";
                input.ConfirmPassword = "0";
            }

            ApplicationUser user = new ApplicationUser();
            try
            {
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

                using (var serviceProvider = services.BuildServiceProvider())
                {
                    using (var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
                    {
                        var context = scope.ServiceProvider.GetService<ApplicationDbContext>();

                        var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

                        var checkUser = userMgr.FindByNameAsync(input.UserName).Result;
                        if (checkUser == null)
                        {
                            if(input.ConfirmPassword != input.PasswordHash)
                            {
                                throw new Exception("Les deux mots de passe ne correspondent pas");
                            }
                            else
                            {
                                user = input;
                            }

                            // Verify phone number
                            //PhoneNumber phoneNumber = _phoneUtil.Parse(input.PhoneNumber, input.CountryCode);

                            //if (!_phoneUtil.IsValidNumberForRegion(phoneNumber, input.CountryCode) && !phoneNumber.HasExtension)
                            //{
                            //    throw new Exception("Numéro invalide");
                            //}
                            //else
                            //{
                            //    input.PhoneNumber = "+" + phoneNumber.CountryCode.ToString() + phoneNumber.NationalNumber.ToString();
                            //}

                            // TODO: rajouter la confirmation de mail
                            user.EmailConfirmed = true;

                            checkUser = user;
                            var result = userMgr.CreateAsync(checkUser, user.PasswordHash).Result;
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
                          
                            Log.Debug($"{checkUser.UserName} created");
                        }
                        else
                        {
                            throw new Exception("Votre nom d'utilisateur-trice est déjà pris");
                        }
                    }
                }
                return Redirect(_configuration["URLClientMVC"] + "AspNetUsers");

                //return RedirectToAction("Login", "Account", new { returnUrl = _configuration["URLClientMVC"] });
            }
            catch (Exception e)
            {
                ViewData["error"] = e.Message;

                List<Gender> genders = await _context.Genders.ToListAsync();
                ViewData["genders"] = genders;

                return View("SignUp");
            }
             
        }

        public IActionResult Cancel()
        {
            return View("Login");
        }

        /*Validation par Email*/
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if(userId == null || code == null)
            {
                return RedirectToPage("/Account/Login");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if(user == null)
            {
                return NotFound($"Unable to load user with ID '{userId}'.");
            }

            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, code);
            // StatusMessage = result.Succeeded ? "Thank you for confirming your email." : "Error confirming you email";
            return View("ConfirmEmail");
        }

        [Route("ForgotPassword", Name = "ForgotPassword")]
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordInput input)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(input.UserName);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please 
                // visit https://go.microsoft.com/fwlink/?LinkID=532713
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                
                var callbackUrl = Url.Action("ResetPassword", "Account", new { code = code }, Request.Scheme);

                await _emailSender.SendEmailAsync(
                    user.Email,
                    "Reset Password",
                    $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                return View("ForgotPasswordConfirmation");
            }

            return View();
        }

        [Route("ForgotPassword", Name = "ForgotPassword")]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [Route("ForgotSend", Name = "ForgotSend")]
        [HttpPost]
        public async Task<IActionResult> ForgotSend(ForgotPasswordInput Input)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(Input.UserName);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please 
                // visit https://go.microsoft.com/fwlink/?LinkID=532713
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Action("ResetPassword", "Account", new { code = code }, Request.Scheme);

                await _emailSender.SendEmailAsync(
                    user.Email,
                    "Reset Password",
                    $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                return View("ForgotPasswordConfirmation");
            }

            return View("ResetPasswordFail");
        }

        public IActionResult ResetPassword(string code = null)
        {
            if (code == null)
            {
                return View("ResetPasswordFail");
            }
            else
            {
                ResetPasswordInput test = new ResetPasswordInput
                {
                    Code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code))
                };
                return View("ResetPassword");
            }
        }

        [Route("ChangePassword", Name = "ChangePassword")]
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ResetPasswordInput input)
        {
            if (!ModelState.IsValid)
            {
                return View("ResetPasswordFail");
            }

            var user = await _userManager.FindByNameAsync(input.UserName);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return View("ResetPasswordFail");
            }
            string code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(input.Code));
            var result = await _userManager.ResetPasswordAsync(user, code, input.Password);
            if (!result.Succeeded)
            {
                return View("ResetPasswordFail");
            }

            return View("ResetPasswordConfirmation");
        }

        public async Task<IActionResult> VerifyPhoneAsync()
        {
            await LoadPhoneNumber();
            SMSVerification model = new SMSVerification();
            model.PhoneNumber = PhoneNumber;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> PostVerifyPhoneModelAsync() 
        {
            await LoadPhoneNumber();
            SMSVerification model = new SMSVerification();
            try
            {
                VerificationResource verification = await VerificationResource.CreateAsync(
                    to: PhoneNumber,
                    channel: "sms",
                    pathServiceSid: _settings.VerificationServiceSID
                );

                if (verification.Status == "pending") 
                {
                    model.PhoneNumber = PhoneNumber;
                    return View("ConfirmPhone", model);
                }

                ModelState.AddModelError("", $"Your verification is not pending, please constact admin");
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "There was an error sending the verification code, please contact admin");
            }
            return View("VerifyPhone", model);
        }

        [HttpPost]
        public async Task<IActionResult> PostVerifyPhoneCodeAsync(SMSVerification input) {
            await LoadPhoneNumber();
            if (!ModelState.IsValid) 
            { 
                return View("ConfirmPhone", input); 
            }

            try
            {
                VerificationCheckResource verification = await VerificationCheckResource.CreateAsync(
                    to: PhoneNumber,
                    code: input.VerificationCode,
                    pathServiceSid: _settings.VerificationServiceSID
                );
                if (verification.Status == "approved")
                {
                    var identityUser = await _userManager.GetUserAsync(User);
                    identityUser.PhoneNumberConfirmed = true;
                    var updateResult = await _userManager.UpdateAsync(identityUser);

                    if (updateResult.Succeeded)
                    {
                        return View("ConfirmPhoneSuccess");
                    }
                    else
                    {
                        ModelState.AddModelError("", "There was an error confirming the verification code, please try again");
                    }
                }
                else
                {
                    ModelState.AddModelError("", $"There was an error confirming the verification code: {verification.Status}");
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError("",
                    "There was an error confirming the code, please check the verification code is correct and try again");
            }

            return View("ConfirmPhone", input);
        }

        public string PhoneNumber { get; set; }
        private async Task LoadPhoneNumber ()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new Exception($"Unable to load user with ID");
            }
            PhoneNumber = user.PhoneNumber;
        }
    }
}