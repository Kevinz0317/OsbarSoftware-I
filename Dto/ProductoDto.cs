using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Osbar.Dto
{
    public class ProductoDto
    {
        public int IdProducto { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public CategoriaDto oCategoria { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public string RutaImagen { get; set; }
        public string base64 { get; set; }
        public string extension { get; set; }
    }
}