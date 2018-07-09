using PagedList;
using Project2.Models;
using Project2.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Project2.Controllers
{
    [Authorize(Roles = "God")]
    public class GodController : Controller
    {
        private MessageRepository msgRepo = new MessageRepository();

        // GET: God
        public async Task<ActionResult> Index(string searchString, string currentFilter, int? page)
        {
            //// god check
            //if (User.Identity.Name != "Goddess")
            //{
            //    TempData["GodDenied"] = "You are not GOD";
            //    return RedirectToAction("Index", "Home");
            //}

            // Get the messages
            GodViewModel viewmodel = new GodViewModel();
            var messages = await msgRepo.GodMessages();

            // paging and keeping current search filter
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            // searching
            if (!String.IsNullOrEmpty(searchString))
            {
                messages = messages.Where(x => x.Sender.UserName.Contains(searchString) || x.Sender.UserName.Contains(searchString.ToLower())
                                                                 || x.Sender.UserName.ToLower().Contains(searchString) || x.Sender.UserName.ToLower().Contains(searchString.ToLower())
                                                                 || x.Receiver.UserName.Contains(searchString) || x.Receiver.UserName.Contains(searchString.ToLower())
                                                                 || x.Receiver.UserName.ToLower().Contains(searchString) || x.Receiver.UserName.ToLower().Contains(searchString.ToLower())).ToList();
            }

            int pageSize = 9;
            int pageNumber = (page ?? 1);
            return View(messages.ToPagedList(pageNumber, pageSize));
        }
    }
}