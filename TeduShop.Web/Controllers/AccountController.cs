using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TeduShop.Data;
using TeduShop.Model.Models;
using TeduShop.Web.App_Start;
using TeduShop.Web.Models;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;
using BotDetect.Web.Mvc;
using TeduShop.Common;
using Microsoft.Owin.Security;
using System.Security.Claims;

namespace TeduShop.Web.Controllers
{
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        // GET: Account
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginViewModel loginViewModel, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await UserManager.FindAsync(loginViewModel.UserName, loginViewModel.Password);
                if(user != null)
                {
                    IAuthenticationManager authenticationManager = HttpContext.GetOwinContext().Authentication;
                    authenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                    ClaimsIdentity identity = _userManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
                    AuthenticationProperties props = new AuthenticationProperties();
                    props.IsPersistent = loginViewModel.RememberMe;
                    authenticationManager.SignIn(props, identity);
                    if (Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
            }else
            {
                ModelState.AddModelError("", "Tài khoản hoặc mật khẩu không đúng.");
            }
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CaptchaValidation("CaptchaCode", "RegisterCaptcha", "Mã xác nhận không đúng!")]
        public async Task<ActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                var userByEmail = await _userManager.FindByEmailAsync(registerViewModel.Email);
                if(userByEmail != null)
                {
                    ModelState.AddModelError("Email","Email này đã tồn tại.");
                    return View(registerViewModel);
                }
                var userByUserName = await _userManager.FindByNameAsync(registerViewModel.UserName);
                if (userByUserName != null)
                {
                    ModelState.AddModelError("UserName", "Username này đã tồn tại.");
                    return View(registerViewModel);
                }
                var user = new ApplicationUser()
                {
                    UserName = registerViewModel.UserName,
                    Email = registerViewModel.Email,
                    EmailConfirmed = false,
                    FullName = registerViewModel.FullName,
                    PhoneNumber = registerViewModel.PhoneNumber,
                    Address = registerViewModel.Address
                };

                await _userManager.CreateAsync(user, registerViewModel.Password);

                var adminUser = await _userManager.FindByEmailAsync(registerViewModel.Email);
                if(adminUser != null)
                    await _userManager.AddToRolesAsync(adminUser.Id, new string[] {"User"});

                string mailContent = System.IO.File.ReadAllText(Server.MapPath("/Assets/client/templates/userTemplate.html"));
                mailContent = mailContent.Replace("{{Username}}", registerViewModel.UserName);
                mailContent = mailContent.Replace("{{Link}}", ConfigHelper.GetByKey("CurrentLink"));

                
                MailHelper.SendMail(adminUser.Email, "Đăng ký tài khoản tại TeduShop", mailContent);

                ViewData["SuccessMsg"] = "Đăng ký thành công.";
            }
            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOut()
        {
            IAuthenticationManager authenticationManager = HttpContext.GetOwinContext().Authentication;
            authenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}