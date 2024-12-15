using Osbar.Dto;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using Osbar.Utilities;

namespace Osbar.Repositories
{
    public class UsuarioRepository
    {
         public static bool Registro(UsuarioDto usuario)
        {
            bool respuesta = false;
            try
            {
                using (SqlConnection con = new SqlConnection(ConexionBD.CadenaSql))
                {
                    String consulta = "Insert into Usuario(noDocumento,idRol,idTipoIdentificacion,Nombres,Apellidos,Telefono,Email,Contrasenha,Reestablecer,Confirmado,Token)";
                    consulta += " values(@noDocumento,@idRol,@idTipoIdentificacion,@Nombres,@Apellidos,@Telefono,@Email,@Contrasenha,@Reestablecer,@Confirmado,@Token)";
                    SqlCommand cmd = new SqlCommand(consulta, con);
                    cmd.Parameters.AddWithValue("@noDocumento", usuario.noDocumento);
                    cmd.Parameters.AddWithValue("@idRol", 1);
                    cmd.Parameters.AddWithValue("@idTipoIdentificacion", usuario.idTipoIdentificacion);
                    cmd.Parameters.AddWithValue("@Nombres", usuario.Nombres);
                    cmd.Parameters.AddWithValue("@Apellidos", usuario.Apellidos);
                    cmd.Parameters.AddWithValue("@Telefono", usuario.Telefono);
                    cmd.Parameters.AddWithValue("@Email", usuario.Email);
                    cmd.Parameters.AddWithValue("@Contrasenha", usuario.Contraseña);
                    cmd.Parameters.AddWithValue("@Reestablecer", usuario.Reestablecer);
                    cmd.Parameters.AddWithValue("@Confirmado", usuario.Confirmado);
                    cmd.Parameters.AddWithValue("@Token", usuario.Token);
                    cmd.CommandType = CommandType.Text;

                    con.Open();

                    int columnasAfectadas = cmd.ExecuteNonQuery();
                    if (columnasAfectadas > 0) respuesta = true;
                    con.Close();
                }
                return respuesta;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static UsuarioDto ValidacionUsuario(string Email, string Contrasenha)
        {
            UsuarioDto usuario = null;
            try
            {
                using (SqlConnection con = new SqlConnection(ConexionBD.CadenaSql))
                {
                    String consulta = "SELECT noDocumento,Nombres,Apellidos,Telefono,Reestablecer,Confirmado from Usuario";
                    consulta += " where Email = @Email AND Contrasenha = @Contrasenha";

                    SqlCommand cmd = new SqlCommand(consulta, con);
                    cmd.Parameters.AddWithValue("@Email", Email);
                    cmd.Parameters.AddWithValue("@Contrasenha", Contrasenha);
                    cmd.CommandType = CommandType.Text;

                    con.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            usuario = new UsuarioDto()
                            {
                                noDocumento = (int)dr["noDocumento"],
                                Nombres = dr["Nombres"].ToString(),
                                Apellidos = dr["Apellidos"].ToString(),
                                Telefono = dr["Telefono"].ToString(),
                                Reestablecer = (bool)dr["Reestablecer"],
                                Confirmado = (bool)dr["Confirmado"],
                            };
                        }
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return usuario;
        }

        public static UsuarioDto ObtenerUsuario(string Email)
        {
            UsuarioDto usuario = null;
            try
            {
                using (SqlConnection con = new SqlConnection(ConexionBD.CadenaSql))
                {
                    String consulta = "SELECT noDocumento,Nombres,Apellidos,Telefono,Contrasenha,Reestablecer,Confirmado,Token from Usuario";
                    consulta += " where Email = @Email";

                    SqlCommand cmd = new SqlCommand(consulta, con);
                    cmd.Parameters.AddWithValue("@Email", Email);
                    cmd.CommandType = CommandType.Text;

                    con.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            usuario = new UsuarioDto()
                            {
                                noDocumento = (int)dr["noDocumento"],
                                Nombres = dr["Nombres"].ToString(),
                                Apellidos = dr["Apellidos"].ToString(),
                                Telefono = dr["Telefono"].ToString(),
                                Contraseña = dr["Contrasenha"].ToString(),
                                Reestablecer = (bool)dr["Reestablecer"],
                                Confirmado = (bool)dr["Confirmado"],
                                Token = dr["Token"].ToString()
                            };
                        }
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return usuario;
        }

        public static bool ReestablecerUsuario(int Reestablecer, string Contraseña, string Token)
        {
            bool respuesta = false;
            try
            {
                using (SqlConnection con = new SqlConnection(ConexionBD.CadenaSql))
                {
                    String consulta = @"update Usuario set " +
                        "Reestablecer = @Reestablecer, " +
                        "Contrasenha = @Contrasenha " +
                        "where Token = @Token";

                    SqlCommand cmd = new SqlCommand(consulta, con);
                    cmd.Parameters.AddWithValue("@Contrasenha", Contraseña);
                    cmd.Parameters.AddWithValue("@Reestablecer", Reestablecer);
                    cmd.Parameters.AddWithValue("@Token", Token);
                    cmd.CommandType = CommandType.Text;

                    con.Open();

                    int columnasAfectadas = cmd.ExecuteNonQuery();
                    if (columnasAfectadas > 0) respuesta = true;

                    con.Close();
                }

                return respuesta;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool VerificarUsuario(string Token)
        {
            bool respuesta = false;
            try
            {
                using (SqlConnection con = new SqlConnection(ConexionBD.CadenaSql))
                {
                    String consulta = @"update Usuario set " +
                        "Confirmado = 1 " +
                        "where Token = @Token";

                    SqlCommand cmd = new SqlCommand(consulta, con);
                    cmd.Parameters.AddWithValue("@Token", Token);
                    cmd.CommandType = CommandType.Text;

                    con.Open();

                    int columnasAfectadas = cmd.ExecuteNonQuery();
                    if (columnasAfectadas > 0) respuesta = true;

                    con.Close();
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