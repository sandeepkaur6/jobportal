using jobportal.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace jobportal.Controllers
{
    public class HomeController : Controller
    {
        jobportalEntities2 jd = new jobportalEntities2();
        public ActionResult Index()
        {
            var res = jd.reqs;
            return View(res.ToList());
         
        }
        [HttpPost]
        public ActionResult MainLogin(FormCollection s, admin a)
        {
            string si = s["type"].ToString();
            string uname = s["uname"].ToString();
            string pwd= s["pwd"].ToString() ;
            if (si == "recuiter")
            {
                Dictionary<string, string> admin = a.adminlist();
                var res = from t in admin
                          where t.Key == uname && t.Value == pwd
                          select t;
                if (res != null)
                {
                    Session["ruser"] = uname;
                    return RedirectToAction("managing", "Recruiter");
                }
            }
            else if (si == "candidate")
            {
                var res = jd.registers.Where(c => (c.username == uname && c.password == pwd)).FirstOrDefault();
                if (res != null)
                {
                    Session["Canduser"] = uname;
                    Session["Resumekey"] = res.resumekey;
                    //first parameter is action name second parameter is controller name
                    return RedirectToAction("ViewJob", "Candidate");
                }
            }
            else
            {
                var res = jd.Companies.Where(c => c.Username == uname && c.Password == pwd).FirstOrDefault();
                if (res != null)
                {
                    Session["Compuser"] = uname;
                    Session["Companyid"] = res.Companyid;
                    //first parameter is action name second parameter is controller name
                    return RedirectToAction("compreqdisplay", "Company");
                }
            }
            return View();
        }
   
        public ActionResult Logout()
        {
            Session.Clear();
            return View();
        }
        public FileResult GetResume(string id)
        {
            
                int resumeke1 = int.Parse(id.ToString());
                var res = jd.registers.Where(c => c.resumekey == resumeke1).FirstOrDefault();
                string filename = res.resumepath;
                //   string ReportURL = "C:/dotnetdemos/jobportal/jobportal/App_Data/uploads/" + filename;
                byte[] FileBytes = System.IO.File.ReadAllBytes(filename);
                return File(FileBytes, "application/pdf");
            
          

        }

    }
}