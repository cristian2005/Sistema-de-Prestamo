using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Sistema_de_Prestamo.Models;

namespace Sistema_de_Prestamo.Controllers
{
    [Authorize]
    public class PrestamoController : Controller
    {

        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Prestamo
        public ActionResult Index()
        {
            var prestamo = db.Prestamo.Include(p => p.Cliente).Include(p => p.Prestadore);
            return View(prestamo.ToList());
        }

        // GET: Prestamo/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Prestamo prestamo = db.Prestamo.Find(id);
            if (prestamo == null)
            {
                return HttpNotFound();
            }
            return View(prestamo);
        }

        // GET: Prestamo/Create
        public ActionResult Create()
        {
            ViewBag.Cliente_Id = new SelectList(db.Clientes, "Id", "Nombre");
            ViewBag.Prestadore_Id = new SelectList(db.Prestadores, "Id", "Nombre");
            return View();
        }

        // POST: Prestamo/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        
        public ActionResult Create(Prestamo prestamo)
        {
            if (ModelState.IsValid)
            {
                db.Prestamo.Add(prestamo);
                var result= db.SaveChanges();
                return Json(new {State=result });
            }

            ViewBag.Cliente_Id = new SelectList(db.Clientes, "Id", "Nombre", prestamo.Cliente_Id);
            ViewBag.Prestadore_Id = new SelectList(db.Prestadores, "Id", "Nombre", prestamo.Prestadore_Id);
            return View(prestamo);
        }

        // GET: Prestamo/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Prestamo prestamo = db.Prestamo.Find(id);
            if (prestamo == null)
            {
                return HttpNotFound();
            }
            ViewBag.Cliente_Id = new SelectList(db.Clientes, "Id", "Nombre", prestamo.Cliente_Id);
            ViewBag.Prestadore_Id = new SelectList(db.Prestadores, "Id", "Nombre", prestamo.Prestadore_Id);
            return View(prestamo);
        }

        // POST: Prestamo/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Monto,Interes,NoCuotas,FechaInicio,MontoCuota,TotalIntereses,MontoPagar,Cliente_Id,Prestadore_Id")] Prestamo prestamo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(prestamo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Cliente_Id = new SelectList(db.Clientes, "Id", "Nombre", prestamo.Cliente_Id);
            ViewBag.Prestadore_Id = new SelectList(db.Prestadores, "Id", "Nombre", prestamo.Prestadore_Id);
            return View(prestamo);
        }

        // GET: Prestamo/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Prestamo prestamo = db.Prestamo.Find(id);
            if (prestamo == null)
            {
                return HttpNotFound();
            }
            return View(prestamo);
        }

        // POST: Prestamo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Prestamo prestamo = db.Prestamo.Find(id);
            db.Prestamo.Remove(prestamo);
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
