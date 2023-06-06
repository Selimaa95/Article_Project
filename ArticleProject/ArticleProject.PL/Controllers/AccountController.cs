using ArticleProject.BL.VModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Project;
using ArticleProject.BL.Helper;
using System.Runtime.InteropServices;
using ArticleProject.DAL.Entity;
using ArticleProject.BL.IRepository;

namespace ArticleProject.PL.Controllers
{
    public class AccountController : Controller
    {
        #region Prop
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly IBaseRepository<Author> repository;

        #endregion

        #region Ctor

        public AccountController( UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IBaseRepository<Author> repository)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.repository = repository;
        }

        #endregion

        #region Actions

        #region Registration

        public IActionResult Registration()
        {
            return View(); 
        }

        [HttpPost]
        public async Task<IActionResult> Registration(AccountVM model)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser() 
                { 
                    UserName = model.UserName,
                    Email = model.Email
                };
                var result = await userManager.CreateAsync(user, model.Password);
                //Add Author
                var userId = await userManager.GetUserIdAsync(user);  
                var author = new Author()
                {
                    UserId = userId,
                    UserName = user.UserName,
                };
                await repository.CreateAsync(author);

                //Add Claim For user.
                var claim = new System.Security.Claims.Claim("User","User");
                await userManager.AddClaimAsync(user,claim);
            
                if(result.Succeeded)
                {
                    return RedirectToAction("Login");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }

            return View(model);
        }
        #endregion

        #region Login

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(AccountVM model)
        {

            try
            {
                var userEmail = await userManager.FindByEmailAsync(model.UserName);
                var userName = await userManager.FindByNameAsync(model.UserName);

                if(userEmail is not null || userName is not null)
                {
                    dynamic result;

                    if (userEmail is not null)
                    {
                        result = await signInManager.PasswordSignInAsync(userEmail, model.Password, false, false);
                    }
                    else
                    {
                        result = await signInManager.PasswordSignInAsync(userName, model.Password, false, false);
                    }

                    if (result.Succeeded)
                    {
                        return RedirectToAction("index", "Home");
                    }
                    else
                    {
                        ViewData["error"] = "البريد الالكترونى او كلمه المرور خاطئه";
                    }
                }
                else
                {
                  ViewData["error"] = "البريد الالكترونى او كلمه المرور خاطئه";
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
            }
           
                

                return View(model);

        }
        #endregion

        #region LogOut

        [HttpPost]
        public async Task<IActionResult> LogOut()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index","Home");
        }

        #endregion

        #region ForgetPassword
        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgetPassword(AccountVM model)
        {
            var user = await userManager.FindByEmailAsync(model.Email);

            if (user != null)
            {
                var token = await userManager.GeneratePasswordResetTokenAsync(user);
                var link = Url.Action("ResetPassword","Account", new { Email = model.Email, Token = token }, Request.Scheme);

                //Sending link To Email.
                MailSender.SendMail(model, link);
                return RedirectToAction("ConfirmForgetPassword");

            }
            else
            {
                ViewData["error"] = "البريد الالكترونى غير مسجل";
            }

            return View(model);
        }
            
        public IActionResult ConfirmForgetPassword()
        {
            return View();
        }

        #endregion

        #region ResetPassword

        public IActionResult ResetPassword(string? Email, string? Token)
        {
            if(Email != null && Token != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(AccountVM model)
        {
                var user = await userManager.FindByEmailAsync(model.Email);

            if (user != null)
            {
                var result = await userManager.ResetPasswordAsync(user, model.Token, model.Password);

                if (result.Succeeded)
                {
                    return RedirectToAction("ConfirmResetPassword");
                }
                
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                ViewData["error"] = "البريد الالكترونى غير صحيح";
                return View(model);             
            }
            return RedirectToAction("ConfirmResetPassword");

        }

        [HttpGet]
        public IActionResult ConfirmResetPassword()
        {
            return View();
        }
        #endregion

        #endregion

    }
}
