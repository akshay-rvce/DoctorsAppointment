using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using DoctorsAppointment.Models;
using System.IO;

namespace DoctorsAppointment.Controllers
{
    public class PatientController : Controller
    {
       
        public ActionResult Index()
        {
            return View();
        }

        public void LogOut()
        {
            Session.RemoveAll();
            Session.Abandon();
            Session.Clear();
            Response.Redirect("~");
            
        }

        public ActionResult SearchDoctor(FormCollection collection)
        {
            Session["spdoctor"] = collection["special"];
            Session["date"] = collection["date"];
            Session["distance"] = collection["distance"];
            Session["lat"] = collection["lat"];
            Session["lng"] = collection["lng"];
            
            return View();
        }

        public void schedule(FormCollection collection)
        {
            DoctorEntities dd = new DoctorEntities();
           Schedule a = new Schedule();
            DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);


        a.date = Convert.ToDateTime(collection["date"]);
            a.regid = Request["regid"];
            a.email = Request["email"];
            //a.regid = collection["regid"];
            a.time = Convert.ToInt32(Request["time"]);
            DateTime localDateTime, univDateTime;
            localDateTime = DateTime.Now;
            univDateTime = localDateTime.ToUniversalTime();
            string temp=""+ (long)(univDateTime - UnixEpoch).TotalMilliseconds;
            a.sid = temp.Substring(5);
            a.date = Convert.ToDateTime(Request["date"]);
            Response.Write("reg=" + a.regid + "<br/>sid=" + a.sid);
            dd.Schedules.Add(a);
            Response.Write("Your appointment has been scheduled.. Soon we will redirect  you to home page");
            Session["email"] = Request["email"];
            System.Threading.Thread.Sleep(5000);



            try
            {
                dd.SaveChanges();
                Response.Redirect("Index");
            }
            catch (Exception e)
            {
                Response.Write(e.Message);
            }

        }

        [HttpGet]
        public ActionResult medicine(String sid)
        {

            Session["sid"] = sid;
            return View();
        }







        [HttpPost]
        public void comment(FormCollection collection)
        {
            DoctorEntities dd = new DoctorEntities();
            comment com = new Models.comment();
            com.pid = Session["email"].ToString();
            com.message = collection["message"];
            com.aid = int.Parse(collection["aid"]);
            dd.comments.Add(com);
            dd.SaveChanges();




            Response.Redirect("Index");
        }
    }
}