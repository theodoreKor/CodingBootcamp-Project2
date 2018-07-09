using Microsoft.AspNet.Identity;
using Project2.Models;
using Project2.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Project2.Controllers
{
    public class UserRepositoryController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserRepository userRepo = new UserRepository();

        [HttpGet]
        public async Task<ActionResult> GetCurrentUser()
        {
            var userId = User.Identity.GetUserId();
            var user = await userRepo.GetUserById(userId);
            if (user == null)
                return Json("UserNotFound", JsonRequestBehavior.AllowGet);
            else
                return Json(new { user }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> GetUserById(string id)
        {
            var user = await userRepo.GetUserById(id);
            if (user == null)
                return Json("UserNotFound", JsonRequestBehavior.AllowGet);
            else
                return Json(new { user }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> GetUserByUsername(string username)
        {
            var user = await userRepo.GetUserByUsername(username);
            if (user == null)
                return Json("UserNotFound", JsonRequestBehavior.AllowGet);
            else
                return Json(new { user }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> GetAllUsers()
        {
            var users = await userRepo.GetAllusers();
            if (users == null || users.Count == 0)
                return Json("NoUsersFound", JsonRequestBehavior.AllowGet);
            else
                return Json(new { users }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> FavoriteUser()
        {
            var userId = User.Identity.GetUserId();
            var favouriteUsername = await userRepo.FavoriteUser(userId);
            return Json(favouriteUsername, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> GetAvatar()
        {
            var userId = User.Identity.GetUserId();
            var avatarUrl = await userRepo.GetAvatar(userId);
            if (avatarUrl != null)
            {
                return Json(new { avatarUrl }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                avatarUrl = Url.Content("~/Content/Images/Avatars/guest.jpg");
                return Json(new { avatarUrl }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetAvatarByUsername(string username)
        {
            
            var avatarUrl = await userRepo.GetAvatarByUsername(username);
            if (avatarUrl != null)
            {
                return Json(new { avatarUrl }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                avatarUrl = Url.Content("~/Content/Images/Avatars/guest.jpg");
                return Json(new { avatarUrl }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetTopUsers()
        {
            var userId = User.Identity.GetUserId();
            var topThreeFavoriteUsers = await userRepo.TopUsersSent(userId);
            return Json(new { topUsers = topThreeFavoriteUsers } , JsonRequestBehavior.AllowGet );
        }
       
    }
}