﻿using System;
using System.Web.Mvc;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text;
using Pruefung_Praktisch_Musterloesung.Models;

namespace Pruefung_Praktisch_Musterloesung.Controllers
{
    public class Lab4Controller : Controller
    {

        /**
        * 1. 
        * 1. Save points for versioning
        * 
        * */

        public ActionResult Index() {

            Lab4IntrusionLog model = new Lab4IntrusionLog();
            return View(model.getAllData());   
        }

        [HttpPost]
        public ActionResult Login()
        {
            var username = Request["username"];

            

            var password = Request["password"];

            bool intrusion_detected = false;
        
            if(!username.Contains("@") || !username.Contains('.') || username.Any(c => char.IsUpper(c)))
            {
                intrusion_detected = true;
            }

            // Hints
            // Request.Browser.Platform;
            // Request.UserHostAddress;

            Lab4IntrusionLog model = new Lab4IntrusionLog();

            // Hint:
            //model.logIntrusion();

            if (intrusion_detected)
            {
                return RedirectToAction("Index", "Lab4");
            }
            else
            {
                // check username and password
                // this does not have to be implemented!
                return RedirectToAction("Index", "Lab4");
            }
        }
    }
}