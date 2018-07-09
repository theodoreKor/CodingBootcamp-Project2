﻿using Project2.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Project2.Repositories
{
    public class UserRepository
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public async Task<ApplicationUser> GetUserById(string id)
        {
            try
            {
                var user = await db.Users.Where(x => x.Id == id).FirstOrDefaultAsync();
                return user;
            }
            catch (Exception)
            {
                return null;
            }
            
        }

        public async Task<ApplicationUser> GetUserByUsername(string username)
        {
            try
            {
                var user = await db.Users.Where(x => x.UserName == username).FirstOrDefaultAsync();
                return user;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<List<ApplicationUser>> GetAllusers()
        {
            try
            {
                var users = await db.Users.ToListAsync();
                return users;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<string> FavoriteUser(string id)
        {
            try
            {
                var user = await db.Users.Where(x => x.Id == id).FirstOrDefaultAsync();
                var sentMessages = await db.Messages.Where(x => x.Sender.Id == user.Id).GroupBy(x => x.Receiver.UserName).Select(x => new { Username = x.Key, Count = x.Count() }).OrderByDescending(x => x.Count).ToListAsync();
                var favouriteUsername = sentMessages[0].Username;
                return (favouriteUsername);
            }
            catch (Exception)
            {
                return null;
            }
        }

        //public async Task<List<List<string>>> TopUsers(string id)
        //{
        //    try
        //    {
        //        var user = await db.Users.Where(x => x.Id == id).FirstOrDefaultAsync();
        //        var topSent = await db.Messages.Where(x => x.Sender.Id == user.Id).GroupBy(x => x.Receiver.UserName).Select(x => new { Username = x.Key, Count = x.Count() }).OrderByDescending(x => x.Count).Select(y => y.Username).ToListAsync();
        //        var topReceived = await db.Messages.Where(x => x.Receiver.Id == user.Id).GroupBy(x => x.Sender.UserName).Select(x => new { Username = x.Key, Count = x.Count() }).OrderByDescending(x => x.Count).Select(y => y.Username).ToListAsync();
        //        List<string> topUsersSent = new List<string>();
        //        List<string> topUsersReceived = new List<string>();
        //        List<List<string>> topUsers = new List<List<string>>();
        //        foreach (var item in topSent)
        //        {
        //            topUsersSent.Add(item);
        //        }
        //        foreach (var item in topReceived)
        //        {
        //            topUsersReceived.Add(item);
        //        }
        //        topUsers.Add(topUsersSent);
        //        topUsers.Add(topUsersReceived);
        //        return (topUsers);
        //    }
        //    catch (Exception)
        //    {
        //        return null;
        //    }
        //}

        // for stats
        // sent
        public async Task<List<string>> TopUsersSent(string id)
        {
            try
            {
                var user = await db.Users.Where(x => x.Id == id).FirstOrDefaultAsync();
                var messagesUsername = await db.Messages.Where(x => x.Sender.Id == user.Id).GroupBy(x => x.Receiver.UserName).Select(x => new { Username = x.Key, Count = x.Count() }).OrderByDescending(x => x.Count).Select(y=> y.Username).ToListAsync();
                
                List<string> topUsers = new List<string>();
               
                foreach (var username in messagesUsername)
                {
                    topUsers.Add(username);
                }
               
                return (topUsers);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<List<int>> TopCountsSent(string id)
        {
            try
            {
                var user = await db.Users.Where(x => x.Id == id).FirstOrDefaultAsync();
                var messagesCount = await db.Messages.Where(x => x.Sender.Id == user.Id).GroupBy(x => x.Receiver.UserName).Select(x => new { Username = x.Key, Count = x.Count() }).OrderByDescending(x => x.Count).Select(y => y.Count).ToListAsync();

                List<int> topCount = new List<int>();

                foreach (var count in messagesCount)
                {
                    topCount.Add(count);
                }
                return (topCount);
            }
            catch (Exception)
            {
                return null;
            }
        }

        //received

        public async Task<List<string>> TopUsersReceived(string id)
        {
            try
            {
                var user = await db.Users.Where(x => x.Id == id).FirstOrDefaultAsync();
                var messagesUsername = await db.Messages.Where(x => x.Receiver.Id == user.Id).GroupBy(x => x.Sender.UserName).Select(x => new { Username = x.Key, Count = x.Count() }).OrderByDescending(x => x.Count).Select(y => y.Username).ToListAsync();

                List<string> topUsers = new List<string>();

                foreach (var username in messagesUsername)
                {
                    topUsers.Add(username);
                }

                return (topUsers);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<List<int>> TopCountsReceived(string id)
        {
            try
            {
                var user = await db.Users.Where(x => x.Id == id).FirstOrDefaultAsync();
                var messagesCount = await db.Messages.Where(x => x.Receiver.Id == user.Id).GroupBy(x => x.Sender.UserName).Select(x => new { Username = x.Key, Count = x.Count() }).OrderByDescending(x => x.Count).Select(y => y.Count).ToListAsync();

                List<int> topCount = new List<int>();

                foreach (var count in messagesCount)
                {
                    topCount.Add(count);
                }
                return (topCount);
            }
            catch (Exception)
            {
                return null;
            }
        }
        // received


        public async Task<bool> AddAvatar(string id, string url)
        {
            try
            {
                var user = await db.Users.Where(x => x.Id == id).FirstOrDefaultAsync();
                user.Avatar = url;
                db.Entry(user).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<string> GetAvatar(string id)
        {
            try
            {
                string avatarUrl = null;
                var user = await db.Users.Where(x => x.Id == id).FirstOrDefaultAsync();
                if (user.Avatar != null)
                {
                    avatarUrl = user.Avatar;
                    return avatarUrl;
                }
                else
                {
                    return null;
                }
                
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<string> GetAvatarByUsername(string username)
        {
            try
            {
                string avatarUrl = null;
                var user = await db.Users.Where(x => x.UserName == username).FirstOrDefaultAsync();
                if (user.Avatar != null)
                {
                    avatarUrl = user.Avatar;
                    return avatarUrl;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception)
            {
                return null;
            }
        }

    }
}