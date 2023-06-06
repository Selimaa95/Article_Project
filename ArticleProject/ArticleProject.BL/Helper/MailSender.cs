using ArticleProject.BL.VModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ArticleProject.BL.Helper
{
    public class MailSender
    {
        public async static void SendMail(AccountVM model, string link)
        {
            var smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.EnableSsl = true;
            smtp.Credentials = new NetworkCredential("ahmed.selima95@gmail.com", "engjlcujratjyyks");
            string Body = $"Mail: {model.Email} \nLink To Reset Your Password: \n{link} ";
            await smtp.SendMailAsync(model.Email, model.Email, $"Email From {model.Email}", Body);

        }
    }   
}
