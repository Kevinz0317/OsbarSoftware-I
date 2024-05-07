using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Web;

namespace Osbar.Dto
{
    public class UsuarioDto
    {
        public int no_ident { get; set; }
        public int id_rol {  get; set; }
        public int id_tipo_ident { get; set; }
        public String nombre { get; set; }
        public String apellido_m { get; set; }
        public String apellido_p { get; set; }
        public int id_ciudad {  get; set; }
        public String dirección { get; set; }
        public String teléfono { get; set; }
        public String email { get; set; }
        public String contraseña { get; set; }
        public String confirmar_contraseña { get; set; }
        public bool restablecer { get; set; }
        public bool confirmado { get; set; }
        public String token { get; set; }
    }
}