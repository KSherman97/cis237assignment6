using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using cis237Assignment6.Models;

namespace cis237Assignment6.Controllers
{
    // authorizes requires that any user is logged in to access the page
    [Authorize]
    public class BeveragesController : Controller
    {
        private BeverageKShermanEntities db = new BeverageKShermanEntities();

        // GET: Beverages
        public ActionResult Index()
        {
            //return View(db.Beverages.ToList());

            // return View(db.Cars.ToList());

            // set up a variable to hold the cars data
            DbSet<Beverage> BeveragesToFilter = db.Beverages;

            // setup some strings to hold the data that might be in the session.
            // If there is nothing in the session we can still use these variables

            //Filter fields should include Name, Pack, Min Price, and Max Price
            // as a default value
            string filterName = "";
            string filterPack = "";
            string filterMinPrice = "";
            string filterMaxPrice = "";

            // define a min and max integer for the cylinders
            int min = 0;
            int max = 9999;

            if (Session["name"] != null && !String.IsNullOrWhiteSpace((string)Session["name"]))
            {
                filterName = (string)Session["name"];

            }
            if (Session["pack"] != null && !String.IsNullOrWhiteSpace((string)Session["pack"]))
            {
                filterPack = (string)Session["pack"];
            }
            if (Session["min"] != null && !String.IsNullOrWhiteSpace((string)Session["min"]))
            {
                filterMinPrice = (string)Session["min"];
                min = Int32.Parse(filterMinPrice);
            }
            if (Session["max"] != null && !String.IsNullOrWhiteSpace((string)Session["max"]))
            {
                filterMaxPrice = (string)Session["max"];
                max = Int32.Parse(filterMaxPrice);
            }

            // do the filter on the carsToFilter dataset. Use the where that we used before
            // when doing the last inclass, only this time send in more lamda expressions 
            // to narrow it down further. Since we setup the default values for each of the filter
            // paramaters, min, max, and filterMake, we can count on this always running with no 
            // errors
            IEnumerable<Beverage> filtered = BeveragesToFilter.Where(Beverage => Beverage.price >= min &&
                                                                                 Beverage.price <= max &&
                                                                                 Beverage.name.Contains(filterName) && 
                                                                                 Beverage.pack.Contains(filterPack));

            // convert the dataset to a list now. that the query work is done on it.
            // the view is expecting a list, so we conver the database set to a list.
            IEnumerable<Beverage> finalFiltered = filtered.ToList();

            // place the string representation of the values that are in the session into 
            // the viewbag so that they can be retrieved and displayed on the view
            ViewBag.filterName = filterName;
            ViewBag.filterPack = filterPack;
            ViewBag.filterMinPrice = filterMinPrice;
            ViewBag.filterMaxPrice = filterMaxPrice;

            // return the view with the filtered selection of cars
            return View(finalFiltered);

            // this is the original
            // return View(db.Cars.ToList());
        }

        // GET: Beverages/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Beverage beverage = db.Beverages.Find(id);
            if (beverage == null)
            {
                return HttpNotFound();
            }
            return View(beverage);
        }

        // GET: Beverages/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Beverages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,name,pack,price,active")] Beverage beverage)
        {
            if (ModelState.IsValid)
            {
                db.Beverages.Add(beverage);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(beverage);
        }

        // GET: Beverages/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Beverage beverage = db.Beverages.Find(id);
            if (beverage == null)
            {
                return HttpNotFound();
            }
            return View(beverage);
        }

        // POST: Beverages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,name,pack,price,active")] Beverage beverage)
        {
            if (ModelState.IsValid)
            {
                db.Entry(beverage).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(beverage);
        }

        // GET: Beverages/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Beverage beverage = db.Beverages.Find(id);
            if (beverage == null)
            {
                return HttpNotFound();
            }
            return View(beverage);
        }

        // POST: Beverages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Beverage beverage = db.Beverages.Find(id);
            db.Beverages.Remove(beverage);
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

        // Mark the method as post since it is reached from a form submit
        [HttpPost]
        [ValidateAntiForgeryToken]
        // This is the Filter method
        public ActionResult Filter()
        {
                //User hit the Submit for Approval button, handle accordingly
                // get the form data that we sent out of
                // the request object
                // the string that is used as a key to get
                string filterName = Request.Form.Get("name");
                string filterPack = Request.Form.Get("pack");
                string filterMinPrice = Request.Form.Get("min");
                string filterMaxPrice = Request.Form.Get("max");

                // now that we have the data pulled out from the request object
                // let's put it into the session so that 
                // other methods can have access to it
                Session["name"] = filterName;
                Session["pack"] = filterPack;
                Session["min"] = filterMinPrice;
                Session["max"] = filterMaxPrice;
            

            // Redirect to the index page
            return RedirectToAction("Index");
        }

        public ActionResult Reset()
        {
            string filterName = "";
            string filterPack = "";
            string filterMinPrice = "";
            string filterMaxPrice = "";

            Session["name"] = filterName;
            Session["pack"] = filterPack;
            Session["min"] = filterMinPrice;
            Session["max"] = filterMaxPrice;

            ViewBag.filterName = filterName;
            ViewBag.filterPack = filterPack;
            ViewBag.filterMinPrice = filterMinPrice;
            ViewBag.filterMaxPrice = filterMaxPrice;

            return RedirectToAction("Index");
        }

        // dump data to json
        // handles the conversion of data to json
        // JsonRequestBehavior.AllowGet gives clients permission
        // to obtain the json data
        public ActionResult Json()
        {
            return Json(db.Beverages.ToList(), JsonRequestBehavior.AllowGet);
        }
    }
}
