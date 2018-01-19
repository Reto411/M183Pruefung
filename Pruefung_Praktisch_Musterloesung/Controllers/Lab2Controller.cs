using System;
using System.Collections.Generic;
using System.IO;
using System.Collections;
using System.Web.Mvc;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text;
using System.Data.SqlClient;
using Pruefung_Praktisch_Musterloesung.Models;

namespace Pruefung_Praktisch_Musterloesung.Controllers
{
    public class Lab2Controller : Controller
    {

        /**
        * 1.
        *  Login: Sql Injections
        *  Index: Session Fixation
        *  
        * 2. SQL Injection Beschreibung:
        *    Der Angreifer gibt in das Passwort feld: 'OR' 1=1; ein. Welches bewirkt dass der Server dies beim Abfragen der datenbank immer als true war nimmt.
        *    So muss der Angreifer nur noch den nutzername kennen und kann sich dann einlogen. Er kann auch mit ein wenig mehr aufwand direkt alle NAmen abfragen.
        *    Weil dies nicht geprüft wird, kann der Angreifer direkt befehle auf dem Server ausfürhren und so theoretisch auch tabellen löschen.
        *    
         *   Session Fixation:
         *   Weil die Session in ein Cookie im browser schreibt, kann der Angreifer ganz einfach die SessionId im Cookie ändern und so eine Session
         *   eines anderen nutzers verwenden.
         *   
         * 
        *    
        * */

        public ActionResult Index() {

            var sessionid = Request.QueryString["sid"];

            if (string.IsNullOrEmpty(sessionid))
            {
                var hash = (new SHA1Managed()).ComputeHash(Encoding.UTF8.GetBytes(DateTime.Now.ToString()));
                sessionid = string.Join("", hash.Select(b => b.ToString("x2")).ToArray());
            }

            ViewBag.sessionid = sessionid;

            return View();
        }

        [HttpPost]
        public ActionResult Login()
        {
            var username = Request["username"];
            var password = Request["password"];
            var sessionid = Request.QueryString["sid"];

            // hints:
            //var used_browser = Request.Browser.Platform;
            //var ip = Request.UserHostAddress;

            Lab2Userlogin model = new Lab2Userlogin();

            if (model.checkCredentials(username, password))
            {
                model.storeSessionInfos(username, password, sessionid);

                HttpCookie c = new HttpCookie("sid");
                c.Expires = DateTime.Now.AddMonths(2);
                c.Value = sessionid;
                Response.Cookies.Add(c);

                return RedirectToAction("Backend", "Lab2");
            }
            else
            {
                ViewBag.message = "Wrong Credentials";
                return View();
            }
        }

        public ActionResult Backend()
        {
            var sessionid = "";

            if (Request.Cookies.AllKeys.Contains("sid"))
            {
                sessionid = Request.Cookies["sid"].Value.ToString();
            }           

            if (!string.IsNullOrEmpty(Request.QueryString["sid"]))
            {
                sessionid = Request.QueryString["sid"];
            }
            
            // hints:
            //var used_browser = Request.Browser.Platform;
            //var ip = Request.UserHostAddress;

            Lab2Userlogin model = new Lab2Userlogin();

            if (model.checkSessionInfos(sessionid))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Lab2");
            }              
        }
    }
}