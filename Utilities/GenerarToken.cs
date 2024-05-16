using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Osbar.Utilities
{
    public class GenerarToken
    {
        public static string GenerarTokenMetodo()
        {
            string token = Guid.NewGuid().ToString("N");
            return token;
        }
    }
}