using Microsoft.AspNet.Identity;
using Project2.Models;
using Project2.Repositories;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Project2.Controllers
{
    public class MessageRepositoryController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private MessageRepository msgRepo = new MessageRepository();

        [HttpPost]
        public async Task<ActionResult> SendMessage(string receiverUsername, string body)
        {
            var senderUsername = User.Identity.GetUserName();
            //var success = await msgRepo.SendMessage(senderUsername, receiverUsername, body);
            //return Json(new { success });
            var messageId = await msgRepo.SendMessage(senderUsername, receiverUsername, body);
            return Json(new { messageId });
        }

        [HttpGet]
        public async Task<ActionResult> GetMessages(string username)
        {
            var userId = User.Identity.GetUserId();
            var messages = await msgRepo.GetMessages(userId, username);
            return Json(new { messages }, JsonRequestBehavior.AllowGet);
        }

        //[HttpGet]
        //public async Task<ActionResult> GetLastMessage()
        //{
        //    try
        //    {
        //        var userId = User.Identity.GetUserId();
        //        var user = await db.Users.Where(x => x.Id == userId).FirstOrDefaultAsync();
        //        var messagesGroupedByDate = await db.Messages.Where(x => x.Receiver.Id == user.Id).OrderBy(x => x.Date).ToListAsync();
        //        if (messagesGroupedByDate.Count == 0)
        //            return Json("No messages", JsonRequestBehavior.AllowGet);
        //        var lastMessage = messagesGroupedByDate[0];
        //        return Json(new { lastMessage }, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception)
        //    {
        //        return Json("Error", JsonRequestBehavior.AllowGet);
        //    }
        //}

        [HttpGet]
        public async Task<ActionResult> MessagesCount(string id)
        {
            //var userId = User.Identity.GetUserId();
            var allMessagesCount = await msgRepo.MessagesCount(id);
            return Json(allMessagesCount, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> UndreadMessagesCount(string senderUsername)
        {
            var userId = User.Identity.GetUserId();
            var unreadMessagesCount = await msgRepo.UnreadMessagesCount(userId, senderUsername);
            return Json(new { unreadMessagesCount }, JsonRequestBehavior.AllowGet);
        }

        //[HttpGet]
        //public async Task<ActionResult> MessagesCount()
        //{
        //    try
        //    {
        //        var userId = User.Identity.GetUserId();
        //        var user = await db.Users.Where(x => x.Id == userId).FirstOrDefaultAsync();
        //        //var user = db.Users.Find(User.Identity.GetUserId());
        //        var sentMessagesCount = await db.Messages.Where(x => x.Sender.Id == user.Id).CountAsync();
        //        var receivedMessagesCount = await db.Messages.Where(x => x.Receiver.Id == user.Id).CountAsync();
        //        var allMessagesCount = await db.Messages.CountAsync();
        //        return Json(new { sentMessagesCount, receivedMessagesCount, allMessagesCount }, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception)
        //    {
        //        return Json("Error", JsonRequestBehavior.AllowGet);
        //    }
        //}

        //[HttpGet]
        //public async Task<ActionResult> UnreadMessagesCount()
        //{
        //    try
        //    {
        //        var userId = User.Identity.GetUserId();
        //        var user = await db.Users.Where(x => x.Id == userId).FirstOrDefaultAsync();
        //        //var user = db.Users.Find(User.Identity.GetUserId());
        //        var unreadMessagesCount = db.Messages.Where(x => x.Receiver.Id == user.Id && x.Read == false).CountAsync();
        //        return Json(new { unreadMessagesCount }, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception)
        //    {
        //        return Json("Error");
        //    }

        //}

        [HttpGet]
        public async Task<ActionResult> GetPublicHistory()
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var user = await db.Users.Where(x => x.Id == userId).FirstOrDefaultAsync();

                //var user_messages = await db.Messages.Where(x => x.Sender.Id == user.Id && x.Receiver.UserName == "ChatBot").OrderByDescending(x => x.Date).ToListAsync();
                var user_messages = await db.Messages.Where(x => x.Sender.Id == user.Id && x.Receiver.UserName == "ChatBot").OrderBy(x => x.Date).ToListAsync();

                if (user_messages.Count == 0)
                {
                    return Json("No messages" , JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var older_date = user_messages[0].Date.AddMilliseconds(-1);
                    
                    var load_messages = await db.Messages.Where(x => x.Receiver.UserName == "ChatBot" && x.Date > older_date).Select(z => new { Sender = z.Sender.UserName, Body = z.Body, Date = z.Date, Avatar = z.Sender.Avatar , MessageId = z.Id, Received = false }).ToListAsync(); // works!!!

                    return Json(new { messages = load_messages }, JsonRequestBehavior.AllowGet);
                }
                
            }
            catch (Exception)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }


    }
}