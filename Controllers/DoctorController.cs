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
    public class DoctorController : Controller
    {
        // GET: Doctor
        public ActionResult Index()
        {
            return View();
        }


        public void LogOut()
        {
            Session.RemoveAll();
            Session.Abandon();
            Response.Redirect("~");

        }

        [HttpPost]
        public ActionResult DoctorLogin(FormCollection collection)
        {
           
            DoctorEntities dd = new DoctorEntities();
            try
            {
                Doctor d = dd.Doctors.Find(collection["regid"]);
            
            string pass;
          
            pass = new Encryption().Decrypt(d.password);
            if (collection["password"].Equals(pass))
            {
                Session["regid"] = collection["regid"];

                

                return View();



            }
            else
            {

                return View();
            }
            }
            catch (Exception e)
            {
                Response.Write(e.Message);
                return View();
            }

        }

        [HttpPost]
        public void Register(FormCollection collection)
        {
            DoctorEntities dd = new DoctorEntities();
            Doctor doc = new Doctor();



            HttpPostedFileBase file = Request.Files["image"];
            if (file != null)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    file.InputStream.CopyTo(ms);
                    byte[] array = ms.GetBuffer();
                    doc.image = array;
                }
            }
            string[] st = collection["start_time"].Split(':');
            string[] et = collection["end_time"].Split(':');
            int st_hr = int.Parse(st[0]);
            int st_mn = int.Parse(st[1]);
            int en_hr = int.Parse(et[0]);
            int en_mn = int.Parse(et[1]);
            doc.latitude = float.Parse(collection["lat"]);
            doc.city = collection["city"];
            doc.email = collection["email"];
            doc.end_time = en_hr * 60 + en_mn;
            doc.interval = int.Parse(collection["interval"]);
            doc.longitude = float.Parse(collection["lng"]);
            doc.mobile = int.Parse(collection["mobile"]);
            doc.name = collection["name"];
            doc.password = new Encryption().Encrypt(collection["password"]);
            doc.regid = collection["regid"];
            doc.specialization = collection["special"];
            doc.start_time = st_hr * 60 + st_mn; ;
            dd.Doctors.Add(doc);
            dd.SaveChanges();
            string imageBase64Data = Convert.ToBase64String(doc.image);
            string imageDataURL = string.Format("data:image/jpg;base64,{0}", imageBase64Data);



            Response.Write("<img src='" + @imageDataURL + "' height='200' width='200'/>");

            Response.Write("<h1>Welcome...!" + doc.name + "</h1>");

        }

        [HttpPost]
        public void Publish(FormCollection collection)
        {
            
                DoctorEntities dd = new DoctorEntities();
                article a = new article();



                HttpPostedFileBase file = Request.Files["image"];
                if (file != null)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        file.InputStream.CopyTo(ms);
                        byte[] array = ms.GetBuffer();
                        a.picture = array;
                    }
                }
                a.title = collection["title"];
                a.genre = collection["genre"];
                a.context = collection["context"];
                a.author = collection["regid"];
                dd.articles.Add(a);


                dd.SaveChanges();
            
            Response.Redirect("DoctorLogin");


        }

        [HttpPost]
        public ActionResult prescribe(FormCollection collection)
        {
            if (Session["sid"] == null)
            {
                Session["sid"] = collection["sid"];
                Session["pat"] = collection["pat"];
                Session["doc"] = collection["doc"];
            }
            return View();
            
        }


        [HttpPost]

        public ActionResult medicate(FormCollection collection)
        {
            DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            DateTime localDateTime, univDateTime;
            localDateTime = DateTime.Now;
            univDateTime = localDateTime.ToUniversalTime();
            string temp = "" + (long)(univDateTime - UnixEpoch).TotalMilliseconds;
            

            DoctorEntities dd = new DoctorEntities();
            Prescription pr = new Prescription();
            pr.pid = temp.Substring(5);
            pr.sid = collection["sid"];
            pr.prescription_name = collection["medicine_name"];
            pr.random = int.Parse(collection["random"]);
            pr.quantity = int.Parse(collection["quantity"]);
            if (collection["times"].Contains("morning"))
                pr.morning = true;
            if (collection["times"].Contains("afternoon"))
                pr.afternoon = true;
            if (collection["times"].Contains("night"))
                pr.night = true;
            dd.Prescriptions.Add(pr);
            dd.SaveChanges();
            //Response.Redirect("prescribe");

            return View("prescribe");
        }

        [HttpGet]
        public ActionResult delete(int pid)
        {
            
            DoctorEntities dd = new DoctorEntities();
            //Response.Write(pid);
            Prescription pr = dd.Prescriptions.Find(pid.ToString());
            dd.Prescriptions.Remove(pr);
            dd.SaveChanges();
            return View("prescribe");
        }

        [HttpGet]
        public ActionResult print(string sid)
        {

            return View();
        }


        [HttpGet]
        public ActionResult home()
        {
            return View("DoctorLogin");
        }


        [HttpPost]
        public ActionResult comment(FormCollection collection)
        {
            DoctorEntities dd = new DoctorEntities();
            comment com = new Models.comment();
            com.regid = Session["regid"].ToString();
            com.message = collection["message"];
            com.aid = int.Parse(collection["aid"]);
            dd.comments.Add(com);
            dd.SaveChanges();




            return View("DoctorLogin");
        }

    }
}