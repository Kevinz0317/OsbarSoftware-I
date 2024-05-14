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
        private static CategoriaRepository instancia = null;

        public CategoriaRepository() { }
        public static CategoriaRepository Instance
        {
            get
            {
                if(instancia == null)
                {
                    instancia = new CategoriaRepository();
                }
                return instancia;
            }
        }

        public List<CategoriaDto> Listar()
        {

            List<CategoriaDto> rptListaCategoria = new List<CategoriaDto>();
            using (SqlConnection oConexion = new SqlConnection(ConexionBD.CadenaSql))
            {
                SqlCommand cmd = new SqlCommand("sp_obtenerCategoria", oConexion);
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    oConexion.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        rptListaCategoria.Add(new CategoriaDto()
                        {
                            id_categoria = Convert.ToInt32(dr["id_categoria"].ToString()),
                            nombre = dr["nombre"].ToString(),
                        });
                    }
                    dr.Close();

                    return rptListaCategoria;

                }
                catch (Exception ex)
                {
                    rptListaCategoria = null;
                    return rptListaCategoria;
                }
            }
        }


        public bool Registrar(CategoriaDto oCategoria)
        {
            bool respuesta = true;
            using (SqlConnection oConexion = new SqlConnection(ConexionBD.CadenaSql))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("sp_RegistrarCategoria", oConexion);
                    cmd.Parameters.AddWithValue("Descripcion", oCategoria.nombre);
                    cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oConexion.Open();

                    cmd.ExecuteNonQuery();

                    respuesta = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);

                }
                catch (Exception ex)
                {
                    respuesta = false;
                }
            }
            return respuesta;
        }

        public bool Modificar(CategoriaDto oCategoria)
        {
            bool respuesta = true;
            using (SqlConnection oConexion = new SqlConnection(ConexionBD.CadenaSql))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("sp_ModificarCategoria", oConexion);
                    cmd.Parameters.AddWithValue("IdCategoria", oCategoria.id_categoria);
                    cmd.Parameters.AddWithValue("Descripcion", oCategoria.nombre);
                    cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;

                    cmd.CommandType = CommandType.StoredProcedure;

                    oConexion.Open();

                    cmd.ExecuteNonQuery();

                    respuesta = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);

                }
                catch (Exception ex)
                {
                    respuesta = false;
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
                    SqlCommand cmd = new SqlCommand("delete from CATEGORIA where idcategoria = @id", oConexion);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.CommandType = CommandType.Text;

                    oConexion.Open();

                    cmd.ExecuteNonQuery();

                    respuesta = true;

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