﻿using System;
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
        public ActionResult Ganancias()
        {
            var cuotas = db.Cuotas.ToList();
            return View(cuotas);
        }
        public ActionResult Moras()
        {
            var today = DateTime.Now.Date;
            var cuotas = db.Cuotas.Where(x=>x.noCuota>0 && x.pagado==false && x.Moras.Where(m=>m.IdCuota==x.Id).Count()==0);
            List<Cuotas> cuotas1 = new List<Cuotas>();
            foreach (var item in cuotas)
            {
                if (today>Convert.ToDateTime(item.fecha_pago))
                {
                    //Almaceno en esta variable los dias atrasados solo es temporalmente
                    item.Prestamo_Id = int.Parse((today-Convert.ToDateTime(item.fecha_pago) ).TotalDays.ToString());
                    cuotas1.Add(item);
                }
            }
            return View(cuotas1);
        }
        public ActionResult Cuotas()
        {
            ViewBag.ClientesId = new SelectList(db.Clientes, "Id", "Nombre");

            var cliente = db.Clientes.First();
            return View(cliente);
        }
        public ActionResult PagarMora(int id, decimal montoMora)
        {
            db.Moras.Add(new Models.Moras { FechaPago = DateTime.Now, MontoPagado = montoMora, IdCuota = id, Pagado = true });
            db.SaveChanges();
            return RedirectToAction("Moras");
        }
        
        public ActionResult Pagar(int id, int admin)
        {
            var cuota = db.Cuotas.Find(id);
            if (admin == 0)
                cuota.autorizado = true;
            else
                cuota.pagado = true;
            db.Cuotas.Add(cuota);
            db.Entry(cuota).State = EntityState.Modified;
             db.SaveChanges();
            ViewBag.ClientesId = new SelectList(db.Clientes, "Id", "Nombre");

            return RedirectToAction("ViewCuota", new { id=cuota.Prestamo.Cliente_Id });
        }
        public ActionResult ViewCuota(int? id)
        {
            ViewBag.ClientesId = new SelectList(db.Clientes, "Id", "Nombre");

            var cuotas = db.Cuotas.Where(x => x.Prestamo.Cliente_Id == id).ToList();
            return View(cuotas);
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
                int result;
                var cliente = db.Clientes.Find(prestamo.Cliente_Id);
                var monto_cuota_puede_pagar = cliente.Salario * 0.30m;
                if (monto_cuota_puede_pagar<prestamo.MontoCuota)
                {
                     result = -2;
                }
                else
                {
                    db.Prestamo.Add(prestamo);
                    result = db.SaveChanges();
                }
            
                return Json(new {Code=result});
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
