using EmployeeManagement.Data;
using EmployeeManagement.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EmployeeManagement.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IWebHostEnvironment Environment;
        MdDataContext Context = new MdDataContext();

        public HomeController(IWebHostEnvironment _environment)
        {
            Environment = _environment;
        }
        [AllowAnonymous]
        public IActionResult Index()
        {
            var result=Context.Employees.ToList();
            return View(result);
        }
        public IActionResult AddNew()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddNew(Employee model, IFormFile ImgUrl)
        {
            string file = "";
            if (ImgUrl != null)
            {
                 file = "/Image/";
                file += Guid.NewGuid().ToString() + "_" + ImgUrl.FileName;
                string rootname = Environment.WebRootPath;
                string fullpath = rootname + file;
                ImgUrl.CopyToAsync(new FileStream(fullpath, FileMode.Create));
                
            }
            if (model.Id == 0)
            {
                model.ImgUrl = file;
                Context.Employees.Add(model);
                Context.SaveChanges();
                return RedirectToAction("GetList");
            }
            else
            {
                if (ImgUrl != null)
                {
                    string rootname = Environment.WebRootPath + model.ImgUrl;
                    System.IO.File.Delete(rootname);
                }
                model.ImgUrl = file;
                Context.Employees.Update(model);
                Context.SaveChanges();
                return RedirectToAction("GetList");
            }
            
        }
        [AllowAnonymous]
        public IActionResult GetDetail(int id)
        {
            var get = Context.Employees.Where(m => m.Id == id).ToList();
            return View(get);
        }
        public IActionResult GetList()
        {
            var get = Context.Employees.ToList();
            return View(get);
        }
        public IActionResult Edit(int id)
        {
            var get = Context.Employees.Find(id);
            return View("AddNew", get);
        }
        public IActionResult Delete(int id)
        {

            var get = Context.Employees.Find(id);
            string rootname = Environment.WebRootPath + get.ImgUrl;
            System.IO.File.Delete(rootname);
            Context.Employees.Remove(get);
            Context.SaveChanges();
            return RedirectToAction("GetList");
        }
        [AllowAnonymous]
        public IActionResult LogIn( )
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public IActionResult LogIn(UserLogin model)
        {
            var getuser = Context.UserLogins.Where(m => m.Email == model.Email).FirstOrDefault();
            if (getuser == null)
            {
                TempData["wrongemail"] = "You have enter wrong E-Mail";
                return View();
            }
            else
            {
                if (getuser.Email==model.Email&&getuser.Password==model.Password)
                {
                    var claims = new[] { new Claim(ClaimTypes.Name, getuser.Name), new Claim(ClaimTypes.Email, getuser.Email) };
                    var identy = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var auth = new AuthenticationProperties { IsPersistent=true };
                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identy), auth);

                    HttpContext.Session.SetString("Name", getuser.Name);
                    return RedirectToAction("AdminHome");
                }
                else
                {
                    TempData["wrongpass"] = "You have enter wrong Password";
                    return View();
                }
            }
        }
        public IActionResult LogOut()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("LogIn");
        }
        public IActionResult AdminHome()
        {
            return View();
        }
        [AllowAnonymous]
        public IActionResult ResetPass()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public IActionResult ResetPass(UserLogin model)
        {
            Context.UserLogins.Update(model);
            Context.SaveChanges();
            return RedirectToAction("LogIn");
        }
        [AllowAnonymous]
        public IActionResult GetResetDetail()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public IActionResult GetResetDetail(UserLogin model)
        {
            var getphone = Context.UserLogins.Where(m => m.Phone == model.Phone).FirstOrDefault();
            if (getphone == null)
            {
                TempData["wrong"] = "Not match found! please try again";
            }
            else
            {
                return View("ResetPass", getphone);
            }
            return View();
            
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
