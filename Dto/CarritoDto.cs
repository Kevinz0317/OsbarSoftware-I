using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Osbar.Dto
{
    public class CarritoDto
    {
        public ProductoDto producto { get; set; }
        public UsuarioDto usuario { get; set; }
    }
}