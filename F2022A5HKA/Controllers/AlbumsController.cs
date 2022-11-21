using F2022A5HKA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace F2022A5HKA.Controllers
{
    [Authorize(Roles = " Executive, Coordinator, Clerk")]
    public class AlbumsController : Controller
    {
        Manager m = new Manager();

        // GET: Albums
        public ActionResult Index()
        {
            return View(m.AlbumGetAll());
        }

        // GET: Albums/Details/5
        public ActionResult Details(int? id)
        {
            var obj = m.AlbumGetById(id.GetValueOrDefault());
            if (obj == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(obj);
            }

        }

        //GET
        [Route("albums/{id}/addtrack")]
        public ActionResult AddTrack(int? id)
        {
            var album = m.AlbumGetById(id.GetValueOrDefault());

            if (album == null)
            {
                return HttpNotFound();
            }
            else
            {
                var trackAdd = new TrackAddFormViewModel();
                trackAdd.AlbumId = album.AlbumId;
                trackAdd.AlbumName = album.Name;
                trackAdd.TrackGenreList = new SelectList(m.GenreGetAll(), "Name", "Name");

                return View(trackAdd);
            }
        }

        //POST
        [Route("albums/{id}/addtrack")]
        [HttpPost]
        public ActionResult AddTrack(TrackAddViewModel track)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(track);
                }

                // TODO: Add insert logic here
                var addedTrack = m.TrackAdd(track);

                if (addedTrack == null)
                {
                    return View(track);
                }
                else
                {
                    return RedirectToAction("details", "tracks", new { id = addedTrack.TrackId });
                }
            }
            catch
            {
                return View();
            }
        }
    }

}