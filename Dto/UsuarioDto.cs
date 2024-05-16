using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Web;

namespace Osbar.Dto
{
    public class UsuarioDto
    {
        public int IdUsuario { get; set; }
        public int noDocumento { get; set; }
        public int idRol {  get; set; }
        public int idTipoIdentificacion { get; set; }
        public String Nombres { get; set; }
        public String Apellidos { get; set; }
        public String Telefono { get; set; }
        public String Email { get; set; }
        public String Contraseña { get; set; }
        public String Confirmar_contraseña { get; set; }
        public bool Reestablecer { get; set; }
        public bool Confirmado { get; set; }
        public String Token { get; set; }

     }
}