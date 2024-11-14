using Osbar.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using Osbar.Dto;

namespace Osbar.Repositories
{
    public class CarritoRepository
    {

        public int Registrar(CarritoDto oCarrito)
        {
            int respuesta = 0;
            using (SqlConnection oConexion = new SqlConnection(ConexionBD.CadenaSql))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("sp_InsertarCarrito", oConexion);
                    cmd.Parameters.AddWithValue("IdUsuario", oCarrito.oUsuario.IdUsuario);
                    cmd.Parameters.AddWithValue("IdProducto", oCarrito.oProducto.IdProducto);
                    cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oConexion.Open();
                    cmd.ExecuteNonQuery();
                    respuesta = Convert.ToInt32(cmd.Parameters["Resultado"].Value);

                }
                catch (Exception ex)
                {
                    respuesta = 0;
                }
            }
            return respuesta;
        }


        public int Cantidad(int idusuario)
        {
            int respuesta = 0;
            using (SqlConnection oConexion = new SqlConnection(ConexionBD.CadenaSql))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("select count(*) from carrito where idusuario = @idusuario", oConexion);
                    cmd.Parameters.AddWithValue("@idusuario", idusuario);
                    cmd.CommandType = CommandType.Text;

                    oConexion.Open();
                    respuesta = Convert.ToInt32(cmd.ExecuteScalar().ToString());

                }
                catch (Exception ex)
                {
                    respuesta = 0;
                }
            }
            return respuesta;
        }

        public List<CarritoDto> Obtener(int _idusuario)
        {
            List<CarritoDto> lst = new List<CarritoDto>();
            using (SqlConnection oConexion = new SqlConnection(ConexionBD.CadenaSql))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("sp_ObtenerCarrito", oConexion);
                    cmd.Parameters.AddWithValue("IdUsuario", _idusuario);
                    cmd.CommandType = CommandType.StoredProcedure;

                    oConexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lst.Add(new CarritoDto()
                            {

                                IdCarrito = Convert.ToInt32(dr["IdCarrito"].ToString()),
                                oProducto = new ProductoDto()
                                {
                                    IdProducto = Convert.ToInt32(dr["IdProducto"].ToString()),
                                    Nombre = dr["Nombre"].ToString(),
                                    Precio = Convert.ToDecimal(dr["Precio"].ToString(), new CultureInfo("es-PE")),
                                    RutaImagen = dr["RutaImagen"].ToString()
                                },

                            });
                        }
                    }

                }
                catch (Exception ex)
                {
                    lst = new List<CarritoDto>();
                }
            }
            return lst;
        }

        public bool Eliminar(string IdCarrito, string IdProducto)
        {

            bool respuesta = true;
            using (SqlConnection oConexion = new SqlConnection(ConexionBD.CadenaSql))
            {
                try
                {
                    StringBuilder query = new StringBuilder();
                    query.AppendLine("delete from carrito where idcarrito = @idcarrito");
                    query.AppendLine("update PRODUCTO set Stock = Stock + 1 where IdProducto = @idproducto");

                    SqlCommand cmd = new SqlCommand(query.ToString(), oConexion);
                    cmd.Parameters.AddWithValue("@idcarrito", IdCarrito);
                    cmd.Parameters.AddWithValue("@idproducto", IdProducto);
                    cmd.CommandType = CommandType.Text;

                    oConexion.Open();
                    cmd.ExecuteNonQuery();

                }
                catch (Exception ex)
                {
                    respuesta = false;
                }
            }
            return respuesta;
        }
    }
}