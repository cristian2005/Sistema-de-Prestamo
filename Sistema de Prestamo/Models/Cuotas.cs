using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Sistema_de_Prestamo.Models
{
    public class Cuotas
    {
        public int Id { get; set; }
        public int noCuota { get; set; }
        public string fecha_pago { get; set; }
        public decimal interes { get; set; }
        public decimal capital { get; set; }
        public decimal restante { get; set; }
        [DefaultValue(false)]
        public bool pagado { get; set; }

        [ForeignKey("Prestamo")]
        public int Prestamo_Id { get; set; }
        public virtual Prestamo Prestamo { get; set; }
    }
}