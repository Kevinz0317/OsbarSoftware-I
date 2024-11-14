using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Osbar.Dto
{
    public class ProductoVentaDto
    {
        public int id_producto {  get; set; }
        public int id_venta {  get; set; }
        public int cantidad_producto { get; set; }
        public decimal total { get; set; }
    }
}