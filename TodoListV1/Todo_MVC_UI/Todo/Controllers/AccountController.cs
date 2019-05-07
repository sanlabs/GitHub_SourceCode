using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using ToDo.Models;
using AutoMapper;
using System.Net.Http;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Todo;

namespace ToDo.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        [Route("Login")]
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [Route("Login")]
        [AllowAnonymous]       
        public  ActionResult Login(LoginViewModel loginViewModel)
        {
            UserViewModel userViewModel = null;
            if (ModelState.IsValid)
            {
                var request = Mapper.Map<LoginViewModel, LoginJsonRequest>(loginViewModel);
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Program.apiUrl);
                    var responseTask = client.PostAsJsonAsync("account/GetLoginUser", request);
                    responseTask.Wait();
                    var result = responseTask.Result;

                    if (result.IsSuccessStatusCode)
                    {
                        User UserDb = JsonConvert.DeserializeObject<User>(result.Content.ReadAsStringAsync().Result);
                        userViewModel = Mapper.Map<User, UserViewModel>(UserDb);

                        if (UserDb != null)
                        {
                            //Authenticate User
                            var claims = new List<Claim> { new Claim(ClaimTypes.Name, UserDb.UserId.ToString()), new Claim(ClaimTypes.GivenName, UserDb.UserName.ToString()) };
                            var userIdentity = new ClaimsIdentity(claims, "login");
                            ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);
                            HttpContext.SignInAsync(principal);

                            //return RedirectToAction("Index", "Events", new { UserId = UserDb.UserId.ToString() });
                            return RedirectToAction("Index", "Events");
                        }
                        else
                            TempData["error"] = "Invalid Login";

                    }
                    else
                        TempData["error"] = "Invalid Login";
                }
            }

            return View();
        }

        [HttpGet]
        [Route("Register")]
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [Route("Register")]
        [AllowAnonymous]
        public ActionResult Register(RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                var request = Mapper.Map<RegisterViewModel, RegisterJsonRequest>(registerViewModel);
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Program.apiUrl);
                    var responseTask = client.PostAsJsonAsync("account/Register", request);
                    responseTask.Wait();
                    var result = responseTask.Result;

                    if (result.IsSuccessStatusCode)
                    {
                        TempData["success"] = "Successfully Registered";
                        return RedirectToAction("Login", "Account");
                    }
                    else
                        TempData["error"] = "Failed to Register";
                }
            }

            return View();
        }

        [Route("Logout")]
        [AllowAnonymous]
        public ActionResult Logout()
        {        
            HttpContext.SignOutAsync();
            return RedirectToAction("Login");
        }

        public class LoginJsonRequest
        {
            public int UserId { get; set; }
            public string UserName { get; set; }
            public string Password { get; set; }
            public bool RememberMe { get; set; }
        }

        public class RegisterJsonRequest
        {
            public string UserName { get; set; }
            public string Password { get; set; }

        }


    }
}