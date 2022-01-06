using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using UploadPlayVideoFile.Models;

namespace UploadPlayVideoFile.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


        [HttpGet]
        public ActionResult MediaFiles()
        {
            List<GetMediaFiles_Result> listMediaFiles = new List<GetMediaFiles_Result>();
            TestingDBEntities testingDBEntities = new TestingDBEntities();
            listMediaFiles = testingDBEntities.GetMediaFiles().ToList();
            
            return View(listMediaFiles);
        }


        [HttpPost]
        public ActionResult MediaFiles(HttpPostedFileBase httpPostedFileBase)
        {
            if (httpPostedFileBase != null)
            {
                string fileName = MediaFileName(Path.GetFileNameWithoutExtension(httpPostedFileBase.FileName));
                string fileExtension = Path.GetExtension(httpPostedFileBase.FileName);
                int contentLength = httpPostedFileBase.ContentLength;
                int fileSize = contentLength / 1000;
                httpPostedFileBase.SaveAs(Server.MapPath("~/UploadMediaFiles/" + fileName + fileExtension));
                TestingDBEntities testingDBEntities = new TestingDBEntities();
                MediaFile mediaFile = new MediaFile
                {
                    filename = Path.GetFileNameWithoutExtension(httpPostedFileBase.FileName),
                    filesize = fileSize,
                    filepath = "~/UploadMediaFiles/" + fileName + fileExtension
                };
                testingDBEntities.MediaFiles.Add(mediaFile);
                testingDBEntities.SaveChanges();
            }
            return RedirectToAction("MediaFiles");
        }

        public static string MediaFileName(string mediaFile)
        {
            string encodedString = (mediaFile ?? "").ToLower();
            encodedString = Regex.Replace(encodedString, @"\&+", "and");
            encodedString = encodedString.Replace("'", "");
            encodedString = Regex.Replace(encodedString, @"[^a-z0-9]", "-");
            encodedString = Regex.Replace(encodedString, @"-+", "-");
            encodedString = encodedString.Trim('-');
            return encodedString;
        }
    }
}