using Osbar.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using Osbar.Dto;

namespace Osbar.Repositories
{
    public class EnvioRepository
    {

        public List<DepartamentoDto> ObtenerDepartamento()
        {
            List<DepartamentoDto> lst = new List<DepartamentoDto>();
            using (SqlConnection oConexion = new SqlConnection(ConexionBD.CadenaSql))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("SELECT idDepartamento, Descripcion FROM DEPARTAMENTO", oConexion);
                    cmd.CommandType = CommandType.Text;
                    oConexion.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lst.Add(new DepartamentoDto()
                            {
                                IdDepartamento = dr["IdDepartamento"].ToString(),
                                Descripcion = dr["Descripcion"].ToString()
                            });
                        }
                    }

                }
                catch (Exception ex)
                {
                    lst = new List<DepartamentoDto>();
                }
            }
            return lst;
        }

        public List<CiudadDto> ObtenerCiudad(string _iddepartamento)
        {
            List<CiudadDto> lst = new List<CiudadDto>();
            using (SqlConnection oConexion = new SqlConnection(ConexionBD.CadenaSql))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("SELECT idCiudad, Descripcion, idDepartamento WHERE IdDepartamento = @iddepartamento", oConexion);
                    cmd.Parameters.AddWithValue("@iddepartamento", _iddepartamento);
                    cmd.CommandType = CommandType.Text;
                    oConexion.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lst.Add(new CiudadDto()
                            {
                                IdCiudad = dr["IdCiudad"].ToString(),
                                Descripcion = dr["Descripcion"].ToString(),
                                IdDepartamento = dr["IdDepartamento"].ToString()
                            });
                        }
                    }

                }
                catch (Exception ex)
                {
                    lst = new List<CiudadDto>();
                }
            }
            return lst;
        }

    }
}