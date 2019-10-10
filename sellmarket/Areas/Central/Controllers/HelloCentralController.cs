using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Central.Controllers
{
    public class HelloCentralController : Controller
    {
        // GET: HelloCentral
        public ActionResult Index()
        {
            return View();
        }

        // GET: HelloCentral/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: HelloCentral/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: HelloCentral/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: HelloCentral/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: HelloCentral/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: HelloCentral/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: HelloCentral/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
