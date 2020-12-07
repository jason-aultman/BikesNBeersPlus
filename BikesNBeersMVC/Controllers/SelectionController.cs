using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BikesNBeersMVC.Controllers
{
    public class SelectionController : Controller
    {
        // GET: SelectionController
        public ActionResult Index()
        {
            return View();
        }
        //GET: SelectionController/Details/result
        public ActionResult Details(Result result)
        {
            return View(result);
        }
        // GET: SelectionController/Details/5
     //   public ActionResult Details(int id)
     //   {
     //       return View();
    //    }

        // GET: SelectionController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SelectionController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: SelectionController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: SelectionController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: SelectionController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: SelectionController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
