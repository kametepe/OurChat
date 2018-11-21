using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Claims;
using System.Threading.Tasks;
using OurChat.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

using OurChat.Entities;
using OurChat.Services.Interfaces;
using Newtonsoft.Json;

namespace OurChat.Controllers
{
    public class AuthController : Controller
    {
        private readonly IMemberService _memberService;
        private readonly IConfiguration _configuration;

        public AuthController(IMemberService memberService,
            IConfiguration configuration)
        {
            _memberService = memberService;
            _configuration = configuration;
        }
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }
        // GET: Login
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            bool isValid = false;
            if (ModelState.IsValid)
            {
                isValid = true;
                string username = model.login;
                if (!string.IsNullOrEmpty(username))
                {
                    username = username.Replace("'", "''");
                }
                string currentPassword = HttpUtility.UrlEncode(model.CPassword);
                string newPassword = HttpUtility.UrlEncode(model.NewPassword);
                string passwordConfirmation = HttpUtility.UrlEncode(model.ConfirmPassword);

                if (currentPassword == newPassword)
                {

                    ModelState.AddModelError("NewPassword", "Veuillez Entrer un mot de passe différent de l'ancien.");
                    isValid = false;
                }

                if (passwordConfirmation != newPassword)
                {

                    ModelState.AddModelError("ConfirmPassword", "Veuillez Entrer un mot de passe différent de l'ancien.");
                    isValid = false;
                }

                isValid = await _memberService.UpdateMemberCredentials(username, currentPassword, newPassword);

                if (isValid == true)
                {
                    return View("login");
                }


            }

            return View();


        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([FromForm] AuthViewModel model)
        {



            if (ModelState.IsValid)
            {
                var (isValid, member) = await _memberService.ValidateMemberCredentialsAsync(model.login, model.password);
                if (!isValid)
                {
                    ModelState.AddModelError("password", "Veuillez vérifier votre login et/ou votre mot de passe.");
                }

                if (member != null)
                {

                    //if (member.CreationDate == member.LastLoginDate)
                    //{
                    //    ChangePasswordViewModel model2 = new ChangePasswordViewModel();
                    //    model2.login = model.login;
                    //    return View("ChangePassword", model2);

                    //}
                }
                /*
                 if CreationDate == LastLoginDate
                 Redirect to Change Password
                 hidden email

                 OldPassword 
                 NewPassord -- strength
                 ConfirmPasword 
                  

                 */
                if (isValid)
                {
                    await LoginAsync(member);
                    if (IsUrlValid(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    JsonSerializerSettings settings = new JsonSerializerSettings
                    {
                        PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                        Formatting = Formatting.Indented
                    };

                    string json = JsonConvert.SerializeObject(member, settings);

                    HttpContext.Session.SetString(Common.Constants.MemberSessionKey, json);

                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("InvalidCredentials", "Invalid credentials.");
            }
            return View("Login");
        }

        public async Task<IActionResult> LogOff()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home", null);
        }
        private IEnumerable<Claim> GetUserClaims()
        {
            List<Claim> claims = new List<Claim>();

            claims.Add(new Claim(ClaimTypes.NameIdentifier, "200"));
            claims.Add(new Claim(ClaimTypes.Name, "Manager"));
            claims.Add(new Claim(ClaimTypes.Email, "manager@pca.ma"));
            return claims;
        }

        private static bool IsUrlValid(string returnUrl)
        {
            return !string.IsNullOrWhiteSpace(returnUrl)
                   && Uri.IsWellFormedUriString(returnUrl, UriKind.Relative);
        }

        private async Task LoginAsync(Member member)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, member.ID.ToString()),
                new Claim(ClaimTypes.Name, member.Email),
                new Claim(ClaimTypes.GivenName, member.Fname),
                new Claim(ClaimTypes.Surname, member.Lname),
                new Claim(ClaimTypes.Email, member.Email, ClaimValueTypes.Email),
                new Claim(ClaimTypes.Thumbprint, member.PicturePath),
                new Claim("Position", member.Position)
            };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(principal);
        }

    }
}