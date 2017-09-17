using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Net;
using System.Net.Mail;

namespace DoctorsAppointment
{
    public class SendEmail
    {

        public void SendMail(String email,String name)

        {

            MailMessage mail = new MailMessage();
            mail.To.Add(new MailAddress(email));

            mail.From = new MailAddress("askdoctor911@gmail.com");
            mail.Subject = "Welcome to Doctor appointment system";

            string Body = "Hi"+name+".....! Welcome to doctor appointment system... This email is sent to verify your credentials. If you didn't registered with us you can report this problem by replying to this mail.... Thank you...! <br/> <h1>Have a nice Health</h1><br/> <h3>Regards</h3><h4>Doctor appointment system</h4> ";
            mail.Body = Body;

            mail.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Port = 587;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.UseDefaultCredentials = false;
            smtp.Host = "smtp.gmail.com";
            smtp.Credentials = new System.Net.NetworkCredential
                 ("askdoctor911@gmail.com", "");

            smtp.EnableSsl = true;
            smtp.Timeout = 30000;
            smtp.Send(mail);




        }

    }
}
