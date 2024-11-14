using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Osbar.Dto
{
    public class VentaDto
    {
        public int id_usuario {  get; set; }
        public string total_producto { get; set;}
        public decimal total_venta { get; set; }
        public string nombre { get; set; }
        public string id_ciudad {  get; set; }
        public string direccion {  get; set; }
        public string telefono { get; set; }
        public string fecha_venta { get; set; }
        public List<ProductoVentaDto> prodvent {  get; set; }
    }
}