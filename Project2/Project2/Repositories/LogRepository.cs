using Project2.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Project2.Repositories
{
    public class LogRepository
    {
        public void Log(string logMessage, TextWriter w)
        {
            w.WriteLine($"--------------------------------------------------\r\n{DateTime.Now} -> {logMessage}\r\n");
        }

        public void TypeInLog(Message message, string path)
        {
            using (StreamWriter w = File.AppendText(path))
            {
                Log($"From : {message.Sender.UserName} To: {message.Receiver.UserName} : {message.Body}", w);
            }
        }

        public void DeleteLog(string path)
        {
            File.Delete(path);
        }
    }
}