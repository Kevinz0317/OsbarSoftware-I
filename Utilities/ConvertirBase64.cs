using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Osbar.Utilities
{
    public class ConvertirBase64
    {
        public static string convertirBase64(string ruta)
        {
            byte[] bytes = File.ReadAllBytes(ruta);
            string archivo = Convert.ToBase64String(bytes);
            return archivo;
        }
    }
}