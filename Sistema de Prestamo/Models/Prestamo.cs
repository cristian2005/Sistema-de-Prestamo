using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Sistema_de_Prestamo.Models
{
    [Table("Prestamos")]
    public class Prestamo
    {
        public int Id{ get; set; }
        [Required]
        [DataType(DataType.Currency)]
        public decimal Monto { get; set; }
        [Required]
        [DataType(DataType.Currency)]
        public int Interes { get; set; }
        [Required]
        [DataType(DataType.Currency)]
        public int NoCuotas { get; set; }
        [Required]
        public string FormaPago { get; set; }
        [DataType(DataType.Date)]
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
        public List<Cuotas> Cuotas { get; set; }
    }
}