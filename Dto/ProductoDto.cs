using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Osbar.Dto
{
    public class ProductoDto
    {
        public int id_prod {  get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set;}
        public int id_categoria { get; set; }
        public decimal precio { get; set; }
        public decimal impuesto { get; set; }
        public int stock { get; set; }
        public string imagen {  get; set; }
        public string imgBase64 { get; set; }
        public string extImg { get; set; }
    }
}