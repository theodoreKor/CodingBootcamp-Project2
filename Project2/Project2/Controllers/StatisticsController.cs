using Project2.Models;
using Project2.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Net;

namespace Project2.Controllers
{
    [Authorize(Roles = "User, Admin, God")]
    public class StatisticsController : Controller
    {

        private UserRepository userRepo = new UserRepository();
        private MessageRepository msgRepo = new MessageRepository();
        private LogRepository logRepo = new LogRepository();

        // GET: Statistics
        public async Task<ActionResult> Index(string id)
        {
            // get the data
            var user = await userRepo.GetUserById(id);
            var topUsersSent = await userRepo.TopUsersSent(id);
            var topCountsSent = await userRepo.TopCountsSent(id);
            var topUsersReceived = await userRepo.TopUsersReceived(id);
            var topCountsReceived = await userRepo.TopCountsReceived(id);
            var messagesCount = await msgRepo.MessagesCountForStatistics(id);

            // apply the data
            StatsViewModel model = new StatsViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                Avatar = user.Avatar,
                TopUsersSent = topUsersSent,
                TopCountsSent = topCountsSent,
                TopUsersReceived = topUsersReceived,
                TopCountsReceived = topCountsReceived,
                MessagesCount = messagesCount
            };
            return View(model);
        }

        public async Task<ActionResult> DownloadMessages(string id)
        {
            var user = await userRepo.GetUserById(id);
            var messages = await msgRepo.GetMyMessages(id);

            // map the path
            var path = Server.MapPath($"~/Content/{User.Identity.Name}.txt");
            var path2 = Server.MapPath("~/Content");

            // create the txt
            foreach (var message in messages)
            {
                logRepo.TypeInLog(message, path);
            }

            // download the txt
            string filename = $"{User.Identity.Name}.txt";
            Response.ContentType = "application/octet-stream";
            Response.AppendHeader("Content-Disposition", "attachment;filename=" + filename);
            string aaa = Server.MapPath("~/Content/" + filename);
            Response.TransmitFile(Server.MapPath("~/Content/" + filename));
            Response.End();
            
            // delete the txt
            logRepo.DeleteLog(path);
            return RedirectToAction("Index", new { id = id});
        }
    }
}