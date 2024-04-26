using Osbar.Dto;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;

namespace Osbar.Repositories.Models
{
    public class UsuarioModel
    {
        private static String CadenaSql = @"Data Source=KEVIN;Initial Catalog=Osbar;User ID=kevinz;Password=Kevin19";

        public static bool Registro(UsuarioDto usuario)
        {
            bool respuesta = false;
            try
            {
                using (SqlConnection con = new SqlConnection(CadenaSql))
                {
                    String consulta = "Insert into Usuario(no_ident,nombre,apellido_m,apellido_p,dirección,teléfono,email,contraseña,restablecer,confirmado,token)";
                    consulta += " values(@no_ident,@nombre,@apellido_m,@apellido_p,@dirección,@teléfono,@email,@contraseña,@restablecer,@confirmado,@token)";
                    SqlCommand cmd = new SqlCommand(consulta, con);
                    cmd.Parameters.AddWithValue("@no_ident", usuario.no_ident);
                    cmd.Parameters.AddWithValue("@nombre", usuario.nombre);
                    cmd.Parameters.AddWithValue("@apellido_m", usuario.apellido_m);
                    cmd.Parameters.AddWithValue("@apellido_p", usuario.apellido_p);
                    cmd.Parameters.AddWithValue("@dirección", usuario.dirección);
                    cmd.Parameters.AddWithValue("@teléfono", usuario.teléfono);
                    cmd.Parameters.AddWithValue("@email", usuario.email);
                    cmd.Parameters.AddWithValue("@contraseña", usuario.contraseña);
                    cmd.Parameters.AddWithValue("@restablecer", usuario.restablecer);
                    cmd.Parameters.AddWithValue("@confirmado", usuario.confirmado);
                    cmd.Parameters.AddWithValue("@token", usuario.token);
                    cmd.CommandType = CommandType.Text;

                    con.Open();

                    int columnasAfectadas = cmd.ExecuteNonQuery();
                    if (columnasAfectadas > 0) respuesta = true;
                }

                return respuesta;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static UsuarioDto ValidacionUsuario(string email, string contraseña)
        {
            UsuarioDto usuario = null;
            try
            {
                using (SqlConnection con = new SqlConnection(CadenaSql))
                {
                    String consulta = "SELECT no_ident,nombre,apellido_m,apellido_p,dirección,teléfono,restablecer,confirmado from Usuario";
                    consulta += " where email = @email AND contraseña = @contraseña";

                    SqlCommand cmd = new SqlCommand(consulta, con);
                    cmd.Parameters.AddWithValue("@email", email);
                    cmd.Parameters.AddWithValue("@contraseña", contraseña);
                    cmd.CommandType = CommandType.Text;

                    con.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            usuario = new UsuarioDto()
                            {
                                no_ident = (int)dr["no_ident"],
                                nombre = dr["nombre"].ToString(),
                                apellido_m = dr["apellido_m"].ToString(),
                                apellido_p = dr["apellido_p"].ToString(),
                                dirección = dr["dirección"].ToString(),
                                teléfono = dr["teléfono"].ToString(),
                                restablecer = (bool)dr["restablecer"],
                                confirmado = (bool)dr["confirmado"],
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return usuario;
        }

        public static UsuarioDto ObtenerUsuario(string email)
        {
            UsuarioDto usuario = null;
            try
            {
                using (SqlConnection con = new SqlConnection(CadenaSql))
                {
                    String consulta = "SELECT no_ident,nombre,apellido_m,apellido_p,dirección,teléfono,contraseña,restablecer,confirmado,token from Usuario";
                    consulta += " where email = @email";

                    SqlCommand cmd = new SqlCommand(consulta, con);
                    cmd.Parameters.AddWithValue("@email", email);
                    cmd.CommandType = CommandType.Text;

                    con.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            usuario = new UsuarioDto()
                            {
                                no_ident = (int)dr["no_ident"],
                                nombre = dr["nombre"].ToString(),
                                apellido_m = dr["apellido_m"].ToString(),
                                apellido_p = dr["apellido_p"].ToString(),
                                dirección = dr["dirección"].ToString(),
                                teléfono = dr["teléfono"].ToString(),
                                contraseña = dr["contraseña"].ToString(),
                                restablecer = (bool)dr["restablecer"],
                                confirmado = (bool)dr["confirmado"],
                                token = dr["token"].ToString()
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return usuario;
        }

        public static bool RestablecerUsuario(int restablecer, string contraseña, string token)
        {
            bool respuesta = false;
            try
            {
                using (SqlConnection con = new SqlConnection(CadenaSql))
                {
                    String consulta = @"update Usuario set " +
                        "restablecer = @restablecer, " +
                        "contraseña = @contraseña " +
                        "where token = @token";

                    SqlCommand cmd = new SqlCommand(consulta, con);
                    cmd.Parameters.AddWithValue("@contraseña", contraseña);
                    cmd.Parameters.AddWithValue("@restablecer", restablecer);
                    cmd.Parameters.AddWithValue("@token", token);
                    cmd.CommandType = CommandType.Text;

                    con.Open();

                    int columnasAfectadas = cmd.ExecuteNonQuery();
                    if (columnasAfectadas > 0) respuesta = true;
                }

                return respuesta;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool VerificarUsuario(string token)
        {
            bool respuesta = false;
            try
            {
                using (SqlConnection con = new SqlConnection(CadenaSql))
                {
                    String consulta = @"update Usuario set " +
                        "confirmado = 1 " +
                        "where token = @token";

                    SqlCommand cmd = new SqlCommand(consulta, con);
                    cmd.Parameters.AddWithValue("@token", token);
                    cmd.CommandType = CommandType.Text;

                    con.Open();

                    int columnasAfectadas = cmd.ExecuteNonQuery();
                    if (columnasAfectadas > 0) respuesta = true;
                }

                return respuesta;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}