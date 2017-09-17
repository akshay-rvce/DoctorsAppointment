using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc.Html;
using System.Web.Mvc;
using DoctorsAppointment.Models;
using System.Web.UI;
using System.IO;
using DoctorsAppointment.Controllers;
using System.Drawing;

namespace DoctorsAppointment.Controllers
{
    public class DefaultController : Controller
    {
        
        public ActionResult Index()
        {
            return View();
        }

        

        [HttpPost]
        public void Store(FormCollection collection)
        {

            DoctorEntities dd = new DoctorEntities();
            Patient p = new Patient();
            SendEmail se = new SendEmail();

            HttpPostedFileBase file = Request.Files["image"];
            if (file != null)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    file.InputStream.CopyTo(ms);
                    byte[] array = ms.GetBuffer();
                    p.image = array;
                }
            }


            p.email = collection["email"];
                p.name = collection["name"];
                p.password = new Encryption().Encrypt(collection["password"]);
                p.city = collection["city"];
                p.mobile = collection["mobile"];
            se.SendMail(p.email, p.name);
            dd.Patients.Add(p);
                dd.SaveChanges();
            string imageBase64Data = Convert.ToBase64String(p.image);
            string imageDataURL = string.Format("data:image/jpg;base64,{0}", imageBase64Data);


            
            Response.Write("<img src='" + @imageDataURL + "' height='200' width='200'/>");
              
                Response.Write("<h1>Welcome...!" + p.name+"</h1> We will soon redirect you tp log in page");
            for (int i = 0; i < 1000000; i++)
            { }
            Response.Redirect("~");
            }

        [HttpPost]
        public ActionResult PatientLogIn(FormCollection collection)
        {

            DoctorEntities dd = new DoctorEntities();
            try
            {
                Patient p = dd.Patients.Find(collection["email"]);
            string pass = new Encryption().Decrypt(p.password);
            
                if (collection["password"].Equals(pass))
                {
                    Session["email"] = collection["email"];

                    Response.Write("database password" + pass + "and given password is" + collection["password"]);

                    return RedirectToAction("Index", "Patient");



                }
                else
                {

                    return View();
                }
            }
            catch (Exception ee)
            {
                Response.Write("Invalid credentials....");
            }
            return View();
        

        }

        }
       
    }
