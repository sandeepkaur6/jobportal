using jobportal.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace jobportal.Controllers
{
    public class CandidateController : Controller
    {
        jobportalEntities2 jd=new jobportalEntities2();
        // GET: Candidate
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult candRegister() {
            return View();
        }

        [HttpPost]
        public ActionResult candRegister(register r)
        {
            if(ModelState.IsValid)
            { 
                jd.registers.Add(r);
                int i = jd.SaveChanges();
                if (i > 0)
                {
                    TempData["register"] = "candidate register successfully" + i;
                    var res1 = jd.registers.Where(c => c.username == r.username && c.phonenumber == r.phonenumber && c.firstname == r.firstname).FirstOrDefault();
                    Session["Canduser"] = res1.username;
                    Session["Resumekey"] = res1.resumekey;

                    return RedirectToAction("resumeupload");
                }
                else
                {
                    return View();
                }
            }
            return View();

        }

        public ActionResult resumeupload()
        {
            return View();
        }
        [HttpPost]
        public ActionResult resumeupload(HttpPostedFileBase file)
        {
            int resumek = int.Parse(Session["Resumekey"].ToString());
            var res = jd.registers.Where(c => c.resumekey == resumek).FirstOrDefault();
            string pnum = res.phonenumber;
            string un = res.firstname;
            if (res.resumepath == null)
            {
                if (file != null && un != null)
                {// extract only the filename
                    var fileName = Path.GetFileName(file.FileName);
                    if (fileName.Contains(".pdf"))
                    {
                        var newname = un + pnum + ".pdf";
                        var path = Path.Combine(Server.MapPath("~/App_Data/uploads"), newname);
                        file.SaveAs(path);
                        TempData["up"] = "uploaded successfully";
                        // updating the resume path in register table
                        if (res != null)
                        {
                            res.resumepath = "C:/dotnetdemos/jobportal/jobportal/App_Data/uploads/" + newname;
                            int j = jd.SaveChanges();
                            TempData["re"] = "resume path set successfully" + j;
                        }
                        else
                        { TempData["fe"] = "please upload only pdf files"; }
                    }
                    else
                    { TempData["up"] = "please upload only pdf files"; }
                }
                else
                { TempData["up"] = "not uploaded"; }
                return RedirectToAction("Qualificationview");
            }
            else
            {
                if (file != null && un != null)
                {// extract only the filename
                    var fileName = Path.GetFileName(file.FileName);
                    if (fileName.Contains(".pdf"))
                    {
                        var newname = un + pnum + ".pdf";
                        var path = Path.Combine(Server.MapPath("~/App_Data/uploads"), newname);
                        file.SaveAs(path);
                        TempData["up"] = "uploaded successfully";
                        // updating the resume path in register table
                        if (res != null)
                        {
                            res.resumepath = "C:/dotnetdemos/jobportal/jobportal/App_Data/uploads/" + newname;
                            int j = jd.SaveChanges();
                            TempData["re"] = "resume path set successfully" + j;
                        }
                        else
                        { TempData["fe"] = "please upload only pdf files"; }
                    }
                    else
                    { TempData["up"] = "please upload only pdf files"; }
                }
                else
                { TempData["up"] = "not uploaded"; }
                return RedirectToAction("viewprofile", res);
            }
        }

        public ActionResult Qualificationview()
        { 
            return View();
        }
        [HttpPost]
        public ActionResult Qualificationview(qualification q)
        {
          
            if (Session["Canduser"] != null && Session["Resumekey"] != null)
            {
                int resumek = int.Parse(Session["Resumekey"].ToString());
                var res = jd.registers.Where(c => c.resumekey == resumek).FirstOrDefault();
                string pnum = res.phonenumber;
                string un = res.firstname;
                if (res != null)
                {
                    q.resumekey = res.resumekey;
                    jd.qualifications.Add(q);
                    int i = jd.SaveChanges();
                    TempData["qual"] = "Qualification saved successfully";
                    return RedirectToAction("Experienceview");
                }
                else {
                    TempData["qual"] = "data not updated";
                    return View();
                }
            }
            else
            { return View(); }
         
        }

        public ActionResult Experienceview()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Experienceview(experience e)
        {
            if (Session["Canduser"] != null && Session["Resumekey"] != null)
            {
                int resumek = int.Parse(Session["Resumekey"].ToString());
                var res = jd.registers.Where(c => c.resumekey == resumek).FirstOrDefault();
                string pnum = res.phonenumber;
                string un = res.firstname; 
                if (res != null)
                {
                    e.resumekey = res.resumekey;
                    jd.experiences.Add(e);
                    int i = jd.SaveChanges();
                    TempData["exp"] = "Your data has saved successfully";
                }
                else
                {
                    TempData["exp"] = "data not updated";
                }
            }
            return View();
        }
        public ActionResult ViewJob()
        {
            var res = jd.reqs.Where(c => c.approvalstatus == 1);
            return View(res.ToList());
        }
        [HttpPost]
        public ActionResult ViewJob(FormCollection s, req r)
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
                    return RedirectToAction("ViewJob");
                }
            }
            catch
            {
                return RedirectToAction("ViewJob");
            }
        }

        public ActionResult Apply(string reqid)
        {
            int id = int.Parse(reqid);
            int resumek = int.Parse(Session["Resumekey"].ToString());
            var res1 = jd.registers.Where(c => c.resumekey == resumek).FirstOrDefault();
            string pnum = res1.phonenumber;
            string un = res1.firstname;
            var res = jd.reqs.Where(d => d.reqid == id).FirstOrDefault();
            var resj = jd.jobsubmissions.Where(e => e.resumekey == res1.resumekey && e.reqid == id).FirstOrDefault();
            var resr = jd.reqs.Where(c => c.approvalstatus == 1);
            if (resj!=null)
            {
                TempData["submission"] = "Cannot reapply for the job";
            }
            else
            {
                if (res != null && un != null && pnum != null)
                {
                    jobsubmission j = new jobsubmission();
                    j.resumekey = res1.resumekey;
                    j.reqid = res.reqid;
                    j.appliedon = System.DateTime.Now;
                    jd.jobsubmissions.Add(j);
                    int k = jd.SaveChanges();
                    TempData["submission"] = "Applied successfully" + k;
                    return RedirectToAction("ViewJob", resr.ToList());
                }
                else
                {
                    TempData["submission"] = "no req avaiable to apply for the position";
                }
            }
            return RedirectToAction("ViewJob", resr.ToList());
        }

        public ActionResult AppliedJob()
        {
            int resumek = int.Parse(Session["Resumekey"].ToString());
            var res = from t in jd.jobsubmissions.ToList()
                      from t1 in jd.reqs.ToList()
                      where t.reqid == t1.reqid && t.resumekey == resumek
                      select new jobreqmodel { jobdes = t, re = t1 };
            return View(res.ToList());
        }

        public ActionResult Withdraw(string reqid,string resumekey)
        {
            int resumek = int.Parse(Session["Resumekey"].ToString());

            var res = from t in jd.jobsubmissions.ToList()
                      from t1 in jd.reqs.ToList()
                      where t.reqid == t1.reqid && t.resumekey == resumek
                      select new jobreqmodel { jobdes = t, re = t1 };

            int req = int.Parse(reqid);
            int resume=int.Parse(resumekey);
            var res1 = jd.jobsubmissions.Where(c => c.resumekey == resume && c.reqid == req).FirstOrDefault();
            if (res1 != null)
            {
                jd.jobsubmissions.Remove(res1);
                int i = jd.SaveChanges();
                TempData["with"] = "Job Submission removed for the requested req" + req;
            }
            else {
                TempData["with"] = "No record found";
            }

            return View("AppliedJob",res.ToList());
        }
        public ActionResult CandidateAssessment()
        {
            int resumek = int.Parse(Session["Resumekey"].ToString());
            var res = jd.jobsubmissions.Where(c => c.resumekey == resumek && c.assessmentstatus==false).ToList();
            if (res.Count() > 0)
            {
                return View();
            }
            else
            {
                TempData["assess"] = "No Pending Aseesment";
                return RedirectToAction("AssessmentStatus");
            }
        }
        [HttpPost]
        public ActionResult CandidateAssessment(FormCollection s)
        {
            int score = 0;
            string q1 = s["q1"].ToString();
            string q2 = s["q2"].ToString();
            string q3 = s["q3"].ToString();
            string q4 = s["q4"].ToString();
            string q5 = s["q5"].ToString();
            if (q1 == "public static void main(String[] args)")
            {
                score++;
            }
            if (q2 == "Boolean")
            {
                score++;
            }
            if (q3 == "Depend upon the type of variable")
            {
                score++;
            }
            if (q4 == "8 bit")
            {
                score++;
            }
            if (q5 == "A class is a blue print from which individual objects are created. A class can contain fields and methods to describe the behavior of an object")
            {
                score++;
            }
            int resumek = int.Parse(Session["Resumekey"].ToString());
            var res = jd.jobsubmissions.Where(c => c.resumekey == resumek && c.assessmentstatus == false).FirstOrDefault();
            if (res != null)
            {
                res.score = score;
                res.assessmentstatus = true;
                jd.SaveChanges();
                TempData["assess"] = "Assessment submitted successfully";
                return RedirectToAction("AssessmentStatus");

            }
            else
            {
                TempData["assess"] = "assessment not submitted";
                return View();
            }
        }

        public ActionResult AssessmentStatus()
        {
            return View();
        }


        public ActionResult Logout()
        {
            return RedirectToAction("Logout", "Home");
        }

        public ActionResult viewprofile()
        {
            int resumeke = int.Parse(Session["resumekey"].ToString());
            var res = jd.registers.Where(c => c.resumekey == resumeke).FirstOrDefault();
            return View(res);
        }
        [HttpPost]
        public ActionResult viewprofile(register r)
        {
            int resumeke = int.Parse(Session["resumekey"].ToString());
            var res = jd.registers.Where(c => c.resumekey == resumeke).FirstOrDefault();
            if (res != null)
            {
                res.firstname = r.firstname;
                res.lastname = r.lastname;
                res.email = r.email;
                res.phonenumber = r.phonenumber;
                int i = jd.SaveChanges();
                if (i > 0)
                {
                    TempData["profileupdate"] = "Profile Data Updated";
                }
                else
                {
                    TempData["profileupdate"] = "No changes performed";
                }

            }
            else
            {
                TempData["profileupdate"] = "No changes performed";
            }
            return View(res);
        }
        public FileResult GetReport(register r)
        {
            int resumeke1 = int.Parse(Session["resumekey"].ToString());
            var res = jd.registers.Where(c => c.resumekey == resumeke1).FirstOrDefault();
            string filename = res.resumepath;
         //   string ReportURL = "C:/dotnetdemos/jobportal/jobportal/App_Data/uploads/" + filename;
            byte[] FileBytes = System.IO.File.ReadAllBytes(filename);
            return File(FileBytes, "application/pdf");
        }
    }
    public class jobreqmodel { 
        public jobsubmission jobdes { get; set; }
        public req re { get; set; }
    }
}