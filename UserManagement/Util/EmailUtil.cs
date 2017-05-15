using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Mail;

namespace UserManagement.Util
{
    public class EmailUtil
    {
        public static void SendMail(string from, string to, string title, string body)
        {
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential("stephenajoses@gmail.com", "Holliness9368"),
                EnableSsl = true
            };
            client.UseDefaultCredentials = true;
            client.Send(from, to, title, body);
        }
    }
}