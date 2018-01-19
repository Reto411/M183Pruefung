using System;
using System.Collections.Generic;
using System.IO;
using System.Collections;
using System.Web.Mvc;
using System.Linq;

namespace Pruefung_Praktisch_Musterloesung.Controllers
{
    public class Lab1Controller : Controller
    {
        /**
         * File Data Access: 1-
         * File Inclusion
         * Bei der Detailansicht, kann man auch igrgendwelche andere Dateien abfragen, weil der dateipfad als parameter übergeben wird. So kann 
         * man dan verschiedenste Dateien auf dem system anschauen.
         * Directory Travel
         * Dasselbe gilt auch bei der Index. man kan ganze pfäde beim parameter "type" eingeben, weil der Server fügt dann die Type variable ungeprüft mit dem 
         * pfad zusammen.
         * 2. 
         * File Inclusion bei detail view:  localhost/lab1/detail?type=../../.ssh?file=id_rsa           // Versuchen des ssh private key zu stehlen
         * Directory Traversal bei index: localhost/lab1/index?type=../../
         * 3.
         * Bei Index wird der string einfach zusammengesetzt, was bewirkt, dass man auch navigiren kann. Einzige bedingung ist dass der pfad existiert.
         * Wenn man dann zumbeispiel ".." als type angibt, sieht man den inhalt des äusseren ordners und kann so dann navigieren.
         * Bei Details gilt das selbe, zudem kann man dann ein file den filename beim parameter "file" übergeben und so dieses herunterladen.
         * 4.
         * */


        public ActionResult Index()
        {
            var type = Request.QueryString["type"];

            if (string.IsNullOrEmpty(type))
            {
                type = "lions";                
            }

            if (type.Contains('.') || type.Contains('/'))
            {
                var fileurilist = new List<List<string>>();
                var errormsg = "Navigating around is permitted!";
                fileurilist.Add(new List<string>() { errormsg });
                return View(errormsg);
            }
            var path = "~/Content/images/" + type;

            List<List<string>> fileUriList = new List<List<string>>();

            if (Directory.Exists(Server.MapPath(path)))
            {
                var scheme = Request.Url.Scheme; 
                var host = Request.Url.Host; 
                var port = Request.Url.Port;
                
                string[] fileEntries = Directory.GetFiles(Server.MapPath(path));
                foreach (var filepath in fileEntries)
                {
                    var filename = Path.GetFileName(filepath);
                    var imageuri = scheme + "://" + host + ":" + port + path.Replace("~", "") + "/" + filename;

                    var urilistelement = new List<string>();
                    urilistelement.Add(filename);
                    urilistelement.Add(imageuri);
                    urilistelement.Add(type);

                    fileUriList.Add(urilistelement);
                }
            }
            
            return View(fileUriList);
        }

        public ActionResult Detail()
        {
            var file = Request.QueryString["file"];
            var type = Request.QueryString["type"];

            if (type.Contains('.') || type.Contains('/'))
            {
                var fileurilist = new List<List<string>>();
                var errormsg = "Navigating around is permitted!";
                fileurilist.Add(new List<string>() { errormsg });
                return View(errormsg);
            }

            if (string.IsNullOrEmpty(file))
            {
                file = "Lion1.jpg";
            }
            if (string.IsNullOrEmpty(type))
            {
                file = "lions";
            }

            var relpath = "~/Content/images/" + type + "/" + file;

            List<List<string>> fileUriItem = new List<List<string>>();
            var path = Server.MapPath(relpath);

            if (System.IO.File.Exists(path))
            {
                var scheme = Request.Url.Scheme;
                var host = Request.Url.Host;
                var port = Request.Url.Port;
                var absolutepath = Request.Url.AbsolutePath;

                var filename = Path.GetFileName(file);
                var imageuri = scheme + "://" + host + ":" + port + "/Content/images/" + type + "/" + filename;

                var urilistelement = new List<string>();
                urilistelement.Add(filename);
                urilistelement.Add(imageuri);
                urilistelement.Add(type);

                fileUriItem.Add(urilistelement);
            }
            
            return View(fileUriItem);
        }
    }
}