using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using _036_MoviesMvcWissen.Contexts;
using _036_MoviesMvcWissen.Entities;
using _036_MoviesMvcWissen.Models;
using _036_MoviesMvcWissen.Models.ViewModel;

namespace _036_MoviesMvcWissen.Controllers
{
    public class DirectorsController : Controller
    {
        private MoviesContext db = new MoviesContext();

        // GET: Directors
        public ActionResult Index()
        {
            var model = new DirectorsIndexViewModel()
            {
                Directors = db.Directors.ToList()
            };

            return View(model);
        }

        // GET: Directors/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Director director = db.Directors.Find(id);
            if (director == null)
            {
                return HttpNotFound();
            }
            return View(director);
        }

        // GET: Directors/Create
        public ActionResult Create()
        {
            
            var movies = db.Movies.Select(e => new SelectListItem()
            {
                Value = e.Id.ToString(),
                Text = e.Name
            }).ToList();
            ViewBag.Movies = new MultiSelectList(movies, "Value", "Text");
            return View();
        }

        // POST: Directors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "Id,Name,Surname,Retired")] Director director , List<int> Movies)
        [ActionName("Create")]
        //public ActionResult CreateNew()
        public ActionResult CreateNew(FormCollection formCollection, List<int> Movies)
        {
            var director = new Director()
            {
                //Name = Request.Form["Name"],
                //Surname = Request.Form["Surname"],
                Name = formCollection["Name"],
                Surname = formCollection["Surname"],
            };
            //var retired = Request.Form["Retired"];
            var retired = formCollection["Retired"];
            var movieIds = formCollection["movieIds"].Split(',');
            director.Retired = true;
            if (retired.Equals("false"))
                director.Retired = false;
            if (String.IsNullOrWhiteSpace(director.Name))
                ModelState.AddModelError("Name", "Director Name is required!");
            if (String.IsNullOrWhiteSpace(director.Surname))
                ModelState.AddModelError("Surname", "Director Surname is required!");
            if (director.Name.Length > 100)
            {
                ModelState.AddModelError("Name", "Director Name must be maximum 100 characters!");
            }
            if (director.Surname.Length > 100)
            {
                ModelState.AddModelError("Surname", "Director Surname must be maximum 100 characters!");
            }
            if (ModelState.IsValid)
            {
                db.Directors.Add(director);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            if (ModelState.IsValid)
            {
                director.MovieDirectors = Movies.Select(e => new MovieDirector()
                {
                    MovieId = Convert.ToInt32(e),
                    DirectorId = director.Id
                }).ToList();
                db.Directors.Add(director);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(director);
        }

        // GET: Directors/Edit/5
        #region EditGet 1
        //public ActionResult Edit(int? id) //1
        //{

        //    var model = db.Directors.Find(id.Value);
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Director director = db.Directors.Find(id);
        //    if (director == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    var movies = db.Movies.Select(e => new MovieModel()
        //    {
        //        Id = e.Id,
        //        Name = e.Name
        //    }).ToList();
        //    var moviesIds = model.MovieDirectors.Select(e => e.MovieId).ToList();
        //    ViewBag.Movies = new MultiSelectList(movies, "Id", "Name", moviesIds);
        //    return View(model);
        //}

        #endregion

        public ActionResult Edit(int? id)
        {
            if (!id.HasValue)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var movies = db.Movies.Select(e => new SelectListItem()
            {
                Value= e.Id.ToString(),
                Text= e.Name
            }).ToList();
            var director = db.Directors.Find(id.Value);
            List<int> _movieIds = director.MovieDirectors.Select(e => e.MovieId).ToList();

            DirectorsEditViewModel model = new DirectorsEditViewModel();

            model.Director = director;
            model.movieIds = _movieIds;
            model.Movies = new MultiSelectList(movies,"Value","Text", model.movieIds);
           
            return View("EditNew",model);

        }

        // POST: Directors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        #region EditPost 1
        //public ActionResult Edit([Bind(Include = "Id,Name,Surname,Retired")] Director director, List<int> moviesIds)
        //{
        //    var dbDirector = db.Directors.Find(director.Id);

        //    dbDirector.Name = director.Name;
        //    dbDirector.Surname = director.Surname;
        //    dbDirector.Retired = director.Retired;
        //    dbDirector.MovieDirectors = new List<MovieDirector>();
        //    var movieDirectors = db.MovieDirectors.Where(e => e.DirectorId == director.Id).ToList();
        //    foreach (var movieDirector in movieDirectors)
        //    {
        //        db.MovieDirectors.Remove(movieDirector);
        //    }
        //    foreach (var movieId in moviesIds)
        //    {
        //        var movieDirector = new MovieDirector()
        //        {
        //            DirectorId = director.Id,
        //            MovieId = movieId
        //        };
        //        dbDirector.MovieDirectors.Add(movieDirector);
        //    }
        //    db.Entry(dbDirector).State = EntityState.Modified;
        //    db.SaveChanges();
        //    return RedirectToRoute(new { controller = "Directors", action = "Index" });
        //}
        #endregion
        public ActionResult Edit(DirectorsEditViewModel directorsEditViewModel)
        {
            if (ModelState.IsValid)
            {
                var director = db.Directors.Find(directorsEditViewModel.Director.Id);
                director.Name = directorsEditViewModel.Director.Name;
                director.Surname = directorsEditViewModel.Director.Surname;
                director.Retired = directorsEditViewModel.Director.Retired;
                var movieDirectors = db.MovieDirectors.Where(e => e.DirectorId == director.Id).ToList();
                foreach (var movieDirector in movieDirectors)
                {
                    db.MovieDirectors.Remove(movieDirector);
                }
                director.MovieDirectors = directorsEditViewModel.movieIds.Select(e => new MovieDirector()
                {
                    DirectorId = director.Id,
                    MovieId = e
                }).ToList();
                db.Entry(director).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(directorsEditViewModel);
        }

        // GET: Directors/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Director director = db.Directors.Find(id);
            if (director == null)
            {
                return HttpNotFound();
            }
            return View(director);
        }

        // POST: Directors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Director director = db.Directors.Find(id);
            db.Directors.Remove(director);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
