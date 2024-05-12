using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Osbar.Utilities
{
    public class ConexionBD
    {
        private static String CadenaSql = @"Data Source=KEVIN;Initial Catalog=Osbar;User ID=kevinz;Password=Kevin19";

        public static IDbConnection Conexion()
        {
            return new SqlConnection(CadenaSql);
        }
    }
}