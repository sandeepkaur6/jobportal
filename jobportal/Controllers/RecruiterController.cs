using jobportal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Web;
using System.Web.Mvc;

namespace jobportal.Controllers
{
    public class RecruiterController : Controller
    {
        jobportalEntities2 jd = new jobportalEntities2();
        // GET: Recruiter
        public ActionResult Companydetails()
        {

            var res = from t in jd.Companies
                      select t;
            return View(res.ToList());
        }

        public ActionResult candidateinfo()
        {
            var res = from t in jd.jobsubmissions.ToList()
                      from t1 in jd.registers.ToList()
                      from t2 in jd.reqs.ToList()
                      from t3 in jd.Companies.ToList()
                      where t.resumekey == t1.resumekey && t.reqid == t2.reqid && t2.companyid == t3.Companyid
                      select new reqjobmodel { jobdes = t, re = t1, requi = t2, comp1 = t3 };
            return View(res.ToList());
        }
        public ActionResult selectto(string reqid, string resumekey, string ty)
        {
            try
            {
                int hg = int.Parse(reqid);
                int hg1 = int.Parse(resumekey);
                if (ty == "a")
                {
                    var res1 = jd.jobsubmissions.Where(c => (c.reqid == hg && c.resumekey == hg1)).FirstOrDefault();
                    res1.selected = true;
                    int i = jd.SaveChanges();
                    TempData["Selectedstatus"] = "Candidate is selected for the job";
                    return RedirectToAction("candidateinfo");
                }
                else
                {
                    var res1 = jd.jobsubmissions.Where(c => (c.reqid == hg && c.resumekey == hg1)).FirstOrDefault();
                    res1.selected = false;
                    int i = jd.SaveChanges();
                    TempData["Selectedstatus"] = "Rejected for the job";
                    return RedirectToAction("candidateinfo");
                }
            }
            catch(Exception ex)
            {
                TempData["error"] = ex.Message;
                return View();
            }
        }
        
        public ActionResult managing()
        {
            var res = from t in jd.reqs
                      select t;
            return View(res.ToList());

        }
        [HttpPost]
        public ActionResult managing(FormCollection s, req r)
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
                    return RedirectToAction("managing");
                }
            }
            catch
            {
                return RedirectToAction("managing");
            }
        }
        public ActionResult reqstatusupdate(string reqid, string ty)
        {
            int id = int.Parse(reqid);
            if (ty == "a")
            {
                var res1 = jd.reqs.Where(c => (c.reqid == id)).FirstOrDefault();
                res1.approvalstatus = 1;
                int i = jd.SaveChanges();
                TempData["ReqStatus"] = "Req Approved successfully";
                return RedirectToAction("managing");
            }
            else
            {
                var res1 = jd.reqs.Where(c => (c.reqid == id)).FirstOrDefault();
                res1.approvalstatus = 0;
                int i = jd.SaveChanges();
                TempData["ReqStatus"] = "Req is rejected";
                return RedirectToAction("managing");
            }
        }

        public ActionResult sendassessment(string reqid, string resumekey)
        {
            int req = int.Parse(reqid);
            int resu = int.Parse(resumekey);
            var res1 = jd.jobsubmissions.Where(c => c.reqid == req && c.resumekey == resu).FirstOrDefault();
            if (res1 != null)
            {
                res1.assessmentstatus = false;
                jd.SaveChanges();
                TempData["assessmentstatus"] = "Assessment sent successfully";
            }
            else
            {
                TempData["assessmentstatus"] = "Assessment not sent successfully";
            }

            return RedirectToAction("candidateinfo");
        }

        public ActionResult reqview(string reqid)
        {
            int re = int.Parse(reqid);
            var res = jd.reqs.Where(c => c.reqid == re);
            return View(res.ToList());
        }
        public ActionResult compview(string compid)
        {
            int id = int.Parse(compid);
            var res = jd.Companies.Where(c => c.Companyid == id);
            return View(res.ToList());
        }

        public ActionResult reqcandview(string reqid)
        {
            int re = int.Parse(reqid);
            var res = from t in jd.jobsubmissions.ToList()
                      from t1 in jd.registers.ToList()
                      from t2 in jd.reqs.ToList()
                      from t3 in jd.Companies.ToList()
                      where t.resumekey == t1.resumekey && t.reqid == t2.reqid && t2.companyid == t3.Companyid && t2.reqid == re
                      select new reqjobmodel { jobdes = t, re = t1, requi = t2, comp1 = t3 };
            return View(res.ToList());

        }
        public ActionResult candidateview(string resumekey)
        {
            int id=int.Parse(resumekey);
            var res = from t in jd.registers.ToList()
                      from t1 in jd.qualifications.ToList()
                      from t2 in jd.experiences.ToList()
                      where t.resumekey == t1.resumekey && t.resumekey == t2.resumekey && t.resumekey == id
                      select new candidateVmodel { reg = t, experience=t2, qualification=t1 } ;
            return View(res.ToList());
        }

        public ActionResult ScoreReport()
        {
            var res = from t in jd.jobsubmissions.ToList()
                      from t1 in jd.registers.ToList()
                      where t.resumekey == t1.resumekey
                      orderby t.score descending
                      select new candidatescore { jobdes = t, re = t1 };
            return View(res.ToList());
        }

        public ActionResult Logout()
        {
            return RedirectToAction("Logout", "Home");
        }


    }
    public class reqjobmodel
    {
        public jobsubmission jobdes { get; set; }
        public register re { get; set; }
        public req requi { get; set; }
        public Company comp1 { get; set; }
    }
    public class candidatescore
    {
        public jobsubmission jobdes { get; set; }
        public register re { get; set; }
    }

    public class candidateVmodel { 
        public register reg { get; set; }
        public qualification qualification { get; set; }
        public experience experience { get; set; }
    }
}
