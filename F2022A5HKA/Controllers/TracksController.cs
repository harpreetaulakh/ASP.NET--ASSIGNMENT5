using F2022A5HKA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace F2022A5HKA.Controllers
{
    [Authorize(Roles = " Executive, Coordinator, Clerk")]
    public class TracksController : Controller
    {
        Manager m = new Manager();

        // GET: Tracks
        public ActionResult Index()
        {
            return View(m.TrackGetAll());
        }

        // GET: Tracks/Details/5
        public ActionResult Details(int? id)
        {
            var obj = m.TrackGetById(id.GetValueOrDefault());
            if (obj == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(obj);
            }

        }
    }
}