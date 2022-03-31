using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Sistema_de_Prestamo.Models
{
    public class Prestamo
    {
        public int Id{ get; set; }
        [Required]
        public decimal Monto { get; set; }
        [Required]
        public int Interes { get; set; }
        [Required]
        public int NoCuotas { get; set; }

        //public List<string> FormaPago {
        //    get 
        //    { 
        //        return new List<string>()
        //        { "Diario", "Semanal", "Quincenal","Mensual","Anual" }; 
        //    } set {
        //    } }
        public DateTime FechaInicio { get; set; }
        public decimal MontoCuota { get; set; }
        public decimal TotalIntereses { get; set; }
        public decimal MontoPagar { get; set; }

        [ForeignKey("Cliente")]
        public int Cliente_Id { get; set; }
        public virtual Cliente Cliente { get; set; }

        [ForeignKey("Prestadore")]
        public int Prestadore_Id { get; set; }
        public virtual Prestadore Prestadore { get; set; }

    }
}