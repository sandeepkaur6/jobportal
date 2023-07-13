using jobportal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace jobportal.Controllers
{
    public class CompanyController : Controller
    {
        jobportalEntities2 jd = new jobportalEntities2();
        // GET: Company
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CompRegister() {
            return View();
        }
        [HttpPost]
        public ActionResult CompRegister(Company c)
        {
            try
            {
                jd.Companies.Add(c);
                int i = jd.SaveChanges();
                TempData["in"] = i;
                var res = jd.Companies.Where(d => d.Username == c.Username).FirstOrDefault();
                Session["Compuser"] = c.Username;
                Session["Companyid"] = res.Companyid;
                return RedirectToAction("compreqdisplay");
            }
            catch (Exception e)
            {
                TempData["exc"] = e.Message;
            }
            return View();
        }
        public ActionResult comprequirement()
        {
            return View();
        }

        [HttpPost]
        public ActionResult comprequirement(req r)
        {
            try
            {
                int id = int.Parse(Session["Companyid"].ToString());
                r.companyid = id;
                jd.reqs.Add(r);
                int i = jd.SaveChanges();
                if (i > 0)
                {
                    TempData["jobreq"] = "Job Posted  successfully" + i;

                }

            }
            catch (Exception e)
            {
                TempData["error"] = e.Message;
            }

            return View();
        }
        [HttpPost]
        public ActionResult compreqdisplay(FormCollection s, req r)
        {
            string stext = s["txt1"].ToString();
            try
            {
                var res = from t in jd.reqs
                          where t.jobtitle.Contains(stext)
                          select t;
                if (res != null)
                {
                    return View(res.ToList());
                }
                else
                {
                    return RedirectToAction("compreqdisplay");
                }
            }
            catch
            {
                return RedirectToAction("compreqdisplay");
            }
        }
        public ActionResult Deletereq(int id)
        {
            try
            {
                int compid = int.Parse(Session["Companyid"].ToString());
                TempData["alertid"] = id;
                var res = (from tbl in jd.reqs
                           where tbl.reqid == id && tbl.companyid==compid
                           select tbl).FirstOrDefault();
                res.status = 2;
                jd.SaveChanges();
                return RedirectToAction("compreqdisplay");
            }
            catch
            {
                return RedirectToAction("deletereq");
            }
        }
        public ActionResult compreqdisplay()
        {
            int compid = int.Parse(Session["Companyid"].ToString());
            var res = jd.reqs.Where(c=>c.companyid==compid);
            return View(res.ToList());
        }
        public ActionResult Logout()
        {
            return RedirectToAction("Logout", "Home");
        }
        public ActionResult ViewAllCandidates()
        {
            int id = int.Parse(Session["Companyid"].ToString());
            var res = from t in jd.jobsubmissions
                      from t1 in jd.registers
                      from t2 in jd.reqs
                      from t3 in jd.Companies
                      where t.resumekey == t1.resumekey && t.reqid == t2.reqid && t2.companyid == t3.Companyid && t3.Companyid == id && t.selected == true
                      select new viewallcandidatemodel { js = t, regist = t1, requirement = t2, comp = t3 };

            return View(res.ToList());
        }
       
        public ActionResult viewselectedcand(string s)
        {
            int rid = int.Parse(s);
            int compid = Convert.ToInt32(Session["Companyid"]);
            var res = from t in jd.jobsubmissions
                      from t1 in jd.registers
                      from t2 in jd.reqs
                      from t3 in jd.Companies
                      where t.resumekey == t1.resumekey && t.reqid == t2.reqid && t2.companyid == t3.Companyid && t3.Companyid == compid && t.selected == true && t2.reqid == rid
                      select new viewallcandidatemodel { js = t, regist = t1, requirement = t2, comp = t3 };
            return View(res.ToList());
        }

    }
    public class viewallcandidatemodel {
        public register regist { get; set; }
        public req requirement { get; set; }

        public jobsubmission js { get; set; }


        public Company comp { get; set; }

    } 
}