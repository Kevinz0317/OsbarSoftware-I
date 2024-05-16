using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Osbar.Dto
{
    public class CarritoDto
    {
        public int IdCarrito { get; set; }
        public ProductoDto oProducto { get; set; }
        public UsuarioDto oUsuario { get; set; }
    }
}