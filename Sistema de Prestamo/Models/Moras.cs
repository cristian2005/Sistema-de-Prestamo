using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Sistema_de_Prestamo.Models
{
    public class Moras
    {
        public int Id{ get; set; }
        public decimal MontoPagado { get; set; }
        public DateTime FechaPago { get; set; }
        [DefaultValue(false)]
        public bool Pagado { get; set; }
        [ForeignKey("Cuotas")]
        public int IdCuota { get; set; }
        public virtual Cuotas Cuotas { get; set; }
    }
}