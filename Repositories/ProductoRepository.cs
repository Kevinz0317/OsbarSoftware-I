using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Osbar.Utilities;
using System.Data;
using System.Data.SqlClient;
using Osbar.Dto;
using System.Runtime.Remoting.Messaging;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Osbar.Repositories
{
    public class ProductoRepository
    {
        public int AgregarProducto(ProductoDto producto)
        {
            int respuesta = 0;
            using (SqlConnection con = new SqlConnection(ConexionBD.CadenaSql))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("SP_AgregarProducto", con);
                    cmd.Parameters.AddWithValue("nombre", producto.nombre);
                    cmd.Parameters.AddWithValue("descripcion", producto.descripcion);
                    cmd.Parameters.AddWithValue("id_categoria", producto.id_categoria);
                    cmd.Parameters.AddWithValue("precio", producto.precio);
                    cmd.Parameters.AddWithValue("impuesto", producto.impuesto);
                    cmd.Parameters.AddWithValue("stock", producto.stock);
                    cmd.Parameters.AddWithValue("imagen", producto.imagen);
                    cmd.Parameters.Add("resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    con.Open();

                    cmd.ExecuteNonQuery();

                    respuesta = Convert.ToInt32(cmd.Parameters["resultado"].Value);
                }catch (Exception ex)
                {
                    respuesta = 0;
                    throw ex;
                } 
            }
            return respuesta;
        }

        public bool ActualizarProducto(ProductoDto producto)
        {
            bool respuesta = false;
            using (SqlConnection con = new SqlConnection(ConexionBD.CadenaSql))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("SP_ActualizarProductos", con);
                    cmd.Parameters.AddWithValue("id_prod", producto.id_prod);
                    cmd.Parameters.AddWithValue("nombre", producto.nombre);
                    cmd.Parameters.AddWithValue("descripcion", producto.descripcion);
                    cmd.Parameters.AddWithValue("id_categoria", producto.id_categoria);
                    cmd.Parameters.AddWithValue("precio", producto.precio);
                    cmd.Parameters.AddWithValue("impuesto", producto.impuesto);
                    cmd.Parameters.AddWithValue("stock", producto.stock);
                    cmd.Parameters.AddWithValue("imagen", producto.imagen);
                    cmd.Parameters.Add("resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    con.Open();

                    cmd.ExecuteNonQuery();

                    respuesta = Convert.ToBoolean(cmd.Parameters["resultado"].Value);
                }catch (Exception ex)
                {
                    respuesta = false;
                    throw ex;
                }
            }
            return respuesta;
        }

        public bool ActualizarCampoImagen(ProductoDto producto)
        {
            bool respuesta = true;
            using (SqlConnection con = new SqlConnection(ConexionBD.CadenaSql))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("SP_ActualizarCampoImagen", con);
                    cmd.Parameters.AddWithValue("id_prod", producto.id_prod);
                    cmd.Parameters.AddWithValue("imagen", producto.imagen);
                    cmd.CommandType = CommandType.StoredProcedure;

                    con.Open();

                    cmd.ExecuteNonQuery();
                }catch (Exception ex)
                {
                    respuesta = false;
                    throw ex;
                }
            }
            return respuesta;
        }

        public bool EliminarProducto(int id_prod)
        {
            bool respuesta = true;
            using (SqlConnection con = new SqlConnection(ConexionBD.CadenaSql))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("SP_EliminarProductos", con);
                    cmd.Parameters.AddWithValue("id_prod", id_prod);
                    cmd.CommandType = CommandType.StoredProcedure;

                    con.Open();

                    cmd.ExecuteNonQuery();

                    respuesta = true;
                }catch (Exception ex)
                {
                    respuesta = false;
                    throw ex;
                }
            }
            return respuesta;
        }
        
        public List<ProductoDto> ObtenerListaProductos()
        {
            List<ProductoDto> listProd = new List<ProductoDto>();
            using (SqlConnection con = new SqlConnection(ConexionBD.CadenaSql))
            {
                SqlCommand cmd = new SqlCommand("SP_ConsultarProductos", con);
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    while(dr.Read())
                    {
                        listProd.Add(new ProductoDto()
                        {
                            id_prod = Convert.ToInt32(dr["id_nombre"].ToString()),
                            nombre = dr["nombre"].ToString(),
                            descripcion = dr["descripcion"].ToString(),
                            id_categoria = Convert.ToInt32(dr["id_categoria"].ToString()),
                            precio = Convert.ToDecimal(dr["precio"].ToString()),
                            impuesto = Convert.ToDecimal(dr["impuesto"].ToString()),
                            stock = Convert.ToInt32(dr["stock"].ToString()),
                            imagen = dr["imagen"].ToString(),
                        });
                    }
                    dr.Close();

                    return listProd;

                }catch (Exception ex)
                {
                    listProd = null;
                    return listProd;

                    throw ex;
                }
            }
        }
    }
}