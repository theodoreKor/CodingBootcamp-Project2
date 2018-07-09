using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Project2.Models;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using Project2.Repositories;
using System.Web.Security;

namespace Project2.Controllers
{
    [Authorize(Roles = "Admin")]
    //[Authorize]
    public class ApplicationUsersController : Controller
    {
        // my code
        private ApplicationUserManager _userManager;

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

        public ApplicationUsersController()
        {

        }

        public ApplicationUsersController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }
        // end of my code

        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ApplicationUsers
        public async Task<ActionResult> Index(string sortOrder, string searchString)
        {
            var users = await db.Users.ToListAsync();

            // sorting parameters
            ViewBag.UsernameSorting = sortOrder == "Username" ? "UsernameDescending" : "Username";
            ViewBag.EmailSorting = sortOrder == "Email" ? "EmailDescending" : "Email";
            ViewBag.PhonenumberSorting = sortOrder == "Phonenumber" ? "PhoneNumberDescending" : "Phonenumber";
            ViewBag.RoleSorting = sortOrder == "Role" ? "RoleDescending" : "Role";

            // searching
            if (!String.IsNullOrEmpty(searchString))
            {
                users = users.Where(x => x.UserName.Contains(searchString) || x.UserName.Contains(searchString.ToLower()) 
                                 || x.UserName.ToLower().Contains(searchString) || x.UserName.ToLower().Contains(searchString.ToLower())
                                 || x.Email.Contains(searchString) || x.Email.Contains(searchString.ToLower())
                                 || x.Email.ToLower().Contains(searchString) || x.Email.ToLower().Contains(searchString.ToLower())).ToList();
            }

            // sorting
            switch (sortOrder)
            {
                case "Username":
                    users = users.OrderBy(x => x.UserName).ToList();
                    break;
                case "UsernameDescending":
                    users = users.OrderByDescending(x => x.UserName).ToList();
                    break;
                case "Email":
                    users = users.OrderBy(x => x.Email).ToList();
                    break;
                case "EmailDescending":
                    users = users.OrderByDescending(x => x.Email).ToList();
                    break;
                case "Phonenumber":
                    users = users.OrderBy(x => x.PhoneNumber).ToList();
                    break;
                case "PhonenumberDescending":
                    users = users.OrderByDescending(x => x.PhoneNumber).ToList();
                    break;
                case "Role":
                    users = users.OrderBy(x => x.Role).ToList();
                    break;
                case "RoleDescending":
                    users = users.OrderByDescending(x => x.Role).ToList();
                    break;
                default:
                    users = users.OrderBy(x => x.UserName).ToList();
                    break;
            }

            return View(users);
        }

        // GET: ApplicationUsers/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = await db.Users.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            return View(applicationUser);
        }

        // GET: ApplicationUsers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ApplicationUsers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        // my code
        public async Task<ActionResult> Create(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Username, Email = model.Email, Avatar = Url.Content("~/Content/Images/Avatars/guest.jpg"), Role = UserRoles.User }; // changed model.Email to model.Username, added avatar url
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    // my code
                    await UserManager.AddToRoleAsync(user.Id, "User");
                    await WelcomeMessage(user);
                    // end of my code
                    //await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    return RedirectToAction("Index");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }
        // end of my code

        // Send Welcome Message
        public async Task WelcomeMessage(ApplicationUser user)
        {
            var sender = await db.Users.Where(x => x.UserName == "Admin").FirstOrDefaultAsync();
            var receiver = await db.Users.Where(x => x.UserName == user.UserName).FirstOrDefaultAsync();
            var welcomeMessage = new Message { Date = DateTime.Now, Read = false, Body = "Welcome to Condor Chat! Feel free to open your wings!", Sender = sender, Receiver = receiver };
            db.Messages.Add(welcomeMessage);
            await db.SaveChangesAsync();
        }
        // end of my code


        // GET: ApplicationUsers/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            // my code
            ApplicationUser applicationUser = await db.Users.Where(x => x.Id == id).FirstOrDefaultAsync();
            // end of my code
            if (applicationUser == null)
            {
                return HttpNotFound();
            }

            // check user role
            //var roles = await UserManager.GetRolesAsync(applicationUser.Id);
            //UserRoles currentRole =applicationUser.Role;
            //foreach (var role in roles)
            //{
            //    if (role == "User")
            //    {
            //        currentRole = UserRoles.User;
            //    }
            //    else
            //    {
            //        currentRole = UserRoles.Admin;
            //    }
            //}

            // my code
            // mappping to viewmode;
            EditViewModel user = new EditViewModel
            {
                Id = applicationUser.Id,
                UserName = applicationUser.UserName,
                Email = applicationUser.Email,
                PhoneNumber = applicationUser.PhoneNumber,
                Avatar = applicationUser.Avatar,
                Role = applicationUser.Role
               //LockoutEnabled = applicationUser.LockoutEnabled
            };
            //return View(applicationUser);
            return View(user);
            // end of my code
        }

        // POST: ApplicationUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Edit([Bind(Include = "Id,Email,PhoneNumber,LockoutEnabled,UserName")] ApplicationUser applicationUser)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(applicationUser).State = EntityState.Modified;
        //        await db.SaveChangesAsync();
        //        return RedirectToAction("Index");
        //    }
        //    return View(applicationUser);
        //}

        // my code
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(EditViewModel model)
        {
            var user = await db.Users.Where(x => x.Id == model.Id).FirstOrDefaultAsync();

            if (ModelState.IsValid)
            {
                var goodToGo = true;
                
                var users = await db.Users.ToListAsync();
                UserRoles newRole = UserRoles.User;

                // goodToGo checks
                foreach (var item in users)
                {
                    //if (item.Email == model.Email && item.Id != model.Id || item.UserName == model.UserName && item.Id != model.Id)
                    //    goodToGo = false;
                    if (item.Email == model.Email && item.Id != model.Id)
                    {
                        goodToGo = false;
                        TempData["EmailStatusMessage"] = "Email exists";
                    }
                    if (item.UserName == model.UserName && item.Id != model.Id)
                    {
                        goodToGo = false;
                        TempData["UserNameStatusMessage"] = "Username exists";
                    }
                    
                    
                }

                if (goodToGo)
                {
                    if (user.Role == UserRoles.Admin && model.Role == UserRoles.User)
                    {
                        await UserManager.RemoveFromRoleAsync(user.Id, "Admin");
                        await UserManager.AddToRoleAsync(user.Id, "User");
                        newRole = UserRoles.User;
                    }
                    else if (user.Role == UserRoles.User && model.Role == UserRoles.Admin)
                    {
                        await UserManager.RemoveFromRoleAsync(user.Id, "User");
                        await UserManager.AddToRoleAsync(user.Id, "Admin");
                        newRole = UserRoles.Admin;
                    }

                    // modify user
                    user.UserName = model.UserName;
                    user.Email = model.Email;
                    user.Role = newRole;
                    user.PhoneNumber = model.PhoneNumber;
                    db.Entry(user).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
               
            }
            model.UserName = user.UserName;
            model.Email = user.Email;
            model.Role = user.Role;
            model.PhoneNumber = user.PhoneNumber;
            //return View(model);
            return RedirectToAction("Edit", new { id = model.Id });
        }
        // end of my code

        // GET: ApplicationUsers/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            // my code
            ApplicationUser applicationUser = await db.Users.Where(x => x.Id == id).FirstOrDefaultAsync();
            // end of my code
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            return View(applicationUser);
        }

        // POST: ApplicationUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            // my code
            ApplicationUser applicationUser = await db.Users.Where(x => x.Id == id).FirstOrDefaultAsync();

            // delete the messages of this user
            var messages = await db.Messages.Where(x => x.Sender.Id == id || x.Receiver.Id == id).ToListAsync();
            foreach (var message in messages)
            {
                db.Messages.Remove(message);
            }
            // end of my code
            db.Users.Remove(applicationUser);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // my code
        public async Task<ActionResult> DeleteUser(string id)
        {
            var user = await db.Users.Where(x => x.Id == id).FirstOrDefaultAsync();
            // delete the messages of this user
            var messages = await db.Messages.Where(x => x.Sender.Id == id || x.Receiver.Id == id).ToListAsync();
            foreach (var message in messages)
            {
                db.Messages.Remove(message);
            }
            // end of my code
            db.Users.Remove(user);
            await db.SaveChangesAsync();
            ViewBag.UserDeletedSuccess = "User deleted successfully";
            return RedirectToAction("Index", "ApplicationUsers");


        }
        // end of my code

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();

                // my code
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }
                // end of my code
            }

            base.Dispose(disposing);
        }

        // my code
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
        // end of my code
    }
}
