using CrystalDecisions.CrystalReports.Engine;
using Sistema_de_Prestamo.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sistema_de_Prestamo.Controllers
{
    public class ReportesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult ClienteCuota()
        {
            ViewBag.Cliente_Id = new SelectList(db.Clientes, "Id", "Nombre");

            return View();
        }
        public ActionResult Moras()
        {
            return View(db.Moras.ToList());
        }
        public ActionResult CuotasPagoPorCliente(int? id)
        {
            var cuotas = db.Cuotas.Where(x => x.Prestamo.Cliente_Id == id).ToList();
            return View(cuotas);
        }

        public ActionResult Mensual()
        {
            ViewBag.Cliente_Id = new SelectList(db.Clientes.Append(new Cliente() { Nombre = "Todos los clientes", Id = 0 }), "Id", "Nombre",0);

            return View();
        }
        public ActionResult Anual()
        {
            ViewBag.Cliente_Id = new SelectList(db.Clientes.Append(new Cliente() { Nombre = "Todos los clientes", Id = 0 }), "Id", "Nombre", 0);

            return View();
        }
        public ActionResult PrestamoAnual(int id, string year)
        {
            ViewBag.Cliente_Id = new SelectList(db.Clientes.Append(new Cliente() { Nombre = "Todos los clientes", Id = 0 }), "Id", "Nombre", id);

            int year_parse = int.Parse(year);
            List<Prestamo> prestamos;
            if (id != 0 && year!="0")
                prestamos = db.Prestamo.Where(x => x.Cliente_Id == id && x.FechaInicio.Year == year_parse).ToList();
            else if (year != "0")
                prestamos = db.Prestamo.Where(x => x.FechaInicio.Year == year_parse).ToList();
            else if (year == "0" && id==0)
                prestamos = db.Prestamo.ToList();
            else
                prestamos = db.Prestamo.Where(x => x.Cliente_Id == id).ToList();

            return View(prestamos);
        }
        public ActionResult PrestamoMensual(int id, string mes)
        {
            ViewBag.Cliente_Id = new SelectList(db.Clientes.Append(new Cliente() { Nombre = "Todos los clientes", Id = 0 }), "Id", "Nombre",id);

            int mes_parse = int.Parse(mes);
            List<Prestamo> prestamos;
            if (id != 0 && mes != "0")
                prestamos = db.Prestamo.Where(x => x.Cliente_Id == id && x.FechaInicio.Month == mes_parse).ToList();
            else if (mes != "0")
                prestamos = db.Prestamo.Where(x => x.FechaInicio.Month == mes_parse).ToList();
            else if (mes == "0" && id == 0)
                prestamos = db.Prestamo.ToList();
            else
                prestamos = db.Prestamo.Where(x => x.Cliente_Id == id).ToList();
            return View(prestamos);
        }
        // POST: Reportes/Create
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

        // GET: Reportes/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Reportes/Edit/5
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

        // GET: Reportes/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Reportes/Delete/5
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
        public ActionResult ExportReport(string id)
        {
            ReportDocument rd = new ReportDocument();
            string name_report = "";
            object obj=null;

            switch (id)
            {
                case "Prestamo Mensual":
                    name_report = "ReporteMensual";
                    obj = db.Prestamo.Select(x=> new { Nombre=x.Cliente.Nombre,x.Cuotas,x.FechaInicio,x.FormaPago,x.Interes,x.Monto,x.MontoCuota,x.MontoPagar,x.NoCuotas,x.TotalIntereses}).ToList();
                    break;
                case "Prestamo Anual":
                    name_report = "ReporteAnual";
                    break;
                case "Cuotas de Pagos":
                    name_report = "CuotasPagosPorCliente";
                    break;
                default:
                    break;
            }
            rd.Load(Path.Combine(Server.MapPath("~/Report"), name_report + ".rpt"));
            rd.SetDataSource(obj);
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();
            try
            {
                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/pdf", id+".pdf");
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
