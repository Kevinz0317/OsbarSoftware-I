using Osbar.Dto;
using Osbar.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.EnterpriseServices;
using System.Linq;
using System.Web;

namespace Osbar.Repositories
{
    public class CategoriaRepository
    {

        public bool Agregar(CategoriaDto oCategoria)
        {
            bool respuesta = true;
            using (SqlConnection oConexion = new SqlConnection(ConexionBD.CadenaSql))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("SP_AgregarCategoria", oConexion);
                    cmd.Parameters.AddWithValue("Descripcion", oCategoria.Descripcion);
                    cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oConexion.Open();

                    cmd.ExecuteNonQuery();

                    respuesta = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);

                }
                catch (Exception ex)
                {
                    respuesta = false;
                    throw ex;
                }
            }
            return respuesta;
        }


        public List<CategoriaDto> Consultar()
        {

            List<CategoriaDto> rptListaCategoria = new List<CategoriaDto>();
            using (SqlConnection oConexion = new SqlConnection(ConexionBD.CadenaSql))
            {
                SqlCommand cmd = new SqlCommand("SP_ConsultarCategoria", oConexion);
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    oConexion.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        rptListaCategoria.Add(new CategoriaDto()
                        {
                            IdCategoria = Convert.ToInt32(dr["IdCategoria"].ToString()),
                            Descripcion = dr["Descripcion"].ToString(),
                        });
                    }
                    dr.Close();

                    return rptListaCategoria;

                }
                catch (Exception ex)
                {
                    rptListaCategoria = null;
                    return rptListaCategoria;
                    throw ex;
                }
            }
        }

        public bool Editar(CategoriaDto oCategoria)
        {
            bool respuesta = true;
            using (SqlConnection oConexion = new SqlConnection(ConexionBD.CadenaSql))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("sp_EditarCategoria", oConexion);
                    cmd.Parameters.AddWithValue("IdCategoria", oCategoria.IdCategoria);
                    cmd.Parameters.AddWithValue("Descripcion", oCategoria.Descripcion);
                    cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;

                    cmd.CommandType = CommandType.StoredProcedure;

                    oConexion.Open();

                    cmd.ExecuteNonQuery();

                    respuesta = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);

                }
                catch (Exception ex)
                {
                    respuesta = false;
                    throw ex;
                }
            }
            return respuesta;
        }

        public bool Eliminar(int id)
        {
            bool respuesta = true;
            using (SqlConnection oConexion = new SqlConnection(ConexionBD.CadenaSql))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("SP_EliminarCategoria", oConexion);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.CommandType = CommandType.StoredProcedure;

                    oConexion.Open();

                    cmd.ExecuteNonQuery();

                    respuesta = true;

                }
                catch (Exception ex)
                {
                    respuesta = false;
                    throw ex;
                }
            }
            return respuesta;
        }

    }
}