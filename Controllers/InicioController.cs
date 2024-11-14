using Osbar.Dto;
using Osbar.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Osbar.Repositories;
using Osbar.Utilities;
using System.Web.Security;

namespace Osbar.Controllers
{
    public class InicioController : Controller
    {
        // GET: Inicio
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(string Email, string Contraseña)
        {
            UsuarioDto usuario = UsuarioRepository.ValidacionUsuario(Email, EncriptarContraseña.EncriptarMD5(Contraseña));
            if (usuario != null)
            {
                Session["Usuario"] = usuario;
                if (!usuario.Confirmado)
                {
                    ViewBag.Mensaje = $"Su cuenta no se ha verificado. Revise su correo electrónico {Email}";
                }
                else if (usuario.Reestablecer)
                {
                    ViewBag.Mensaje = $"Se ha solicitado la restauración de su contraseña. Revise su correo electrónico {Email}";
                }
                else 
                {
                    return RedirectToAction("Index", "Home");
                }   
            }
            else
            {
                ViewBag.Mensaje = $"No se encontró coincidencias en las credenciales ingresadas";
            }
            return View();
        }

        public ActionResult Registro()
        {
            return View();
        }
        [HttpPost]
         public ActionResult Registro(UsuarioDto usuario)
        {
            if (usuario.Contraseña != usuario.Confirmar_contraseña)
            {
                ViewBag.Documento = usuario.noDocumento;
                ViewBag.Nombres = usuario.Nombres;
                ViewBag.Apellidos = usuario.Apellidos;
                ViewBag.Telefono = usuario.Telefono;
                ViewBag.Email = usuario.Email;
                ViewBag.Mensaje = "Las contraseñas que ingresó no coinciden.";
                return View();
            }

            if (UsuarioRepository.ObtenerUsuario(usuario.Email) == null)
            {
                usuario.Contraseña = EncriptarContraseña.EncriptarMD5(usuario.Contraseña);
                usuario.Token = GenerarToken.GenerarTokenMetodo();
                usuario.Reestablecer = false;
                usuario.Confirmado = false;
                bool resp = UsuarioRepository.Registro(usuario);

                if (resp)
                {
                    string ruta = HttpContext.Server.MapPath("~/Views/PlantillaCorreo/VerificarCorreo.html");
                    string contenido = System.IO.File.ReadAllText(ruta);
                    string url = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Headers["host"], "/Inicio/Verificacion?token=" + usuario.Token);

                    string htmlBody = string.Format(contenido, usuario.Nombres, url);

                    CorreoDto correoDto = new CorreoDto()
                    {
                        Destinatario = usuario.Email,
                        Asunto = "Verificación de Correo",
                        Contenido = htmlBody
                    };

                    bool enviado = CorreoService.EnviarCorreo(correoDto);
                    ViewBag.Creado = true;
                    ViewBag.Mensaje = $"Su cuenta ha sido creada satisfactoriamente. Se ha enviado un mensaje al correo proporcionado {usuario.Email} para la verificación de su cuenta.";
                }
                else
                {
                    ViewBag.Mensaje = "Ocurrió un error, no se pudo crear su cuenta";
                }
            }
            else
            {
                ViewBag.Mensaje = "El correo ya se encuentra registrado en la base de datos.";
            }



            return View();
        }

        public ActionResult Verificacion(string token)
        {
            ViewBag.Respuesta = UsuarioRepository.VerificarUsuario(token);
            return View();
        }

        public ActionResult Reestablecer()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Reestablecer(string Email)
        {
            UsuarioDto usuario = UsuarioRepository.ObtenerUsuario(Email);
            ViewBag.Email = Email;
            if (usuario != null)
            {
                bool respuesta = UsuarioRepository.ReestablecerUsuario(1, usuario.Contraseña, usuario.Token);

                if (respuesta)
                {
                    string ruta = HttpContext.Server.MapPath("~/Views/PlantillaCorreo/ReestablecerContraseña.html");
                    string contenido = System.IO.File.ReadAllText(ruta);
                    string url = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Headers["host"], "/Inicio/ActualizarContraseña?token=" + usuario.Token);

                    string htmlBody = string.Format(contenido, usuario.Nombres, url);

                    CorreoDto correoDto = new CorreoDto()
                    {
                        Destinatario = Email,
                        Asunto = "Reestablecimiento de cuenta",
                        Contenido = htmlBody
                    };

                    bool enviado = CorreoService.EnviarCorreo(correoDto);
                    ViewBag.Reestablecido = true;
                }
                else
                {
                    ViewBag.Mensaje = "No se pudo restablecer la cuenta";
                }
            }
            else
            {
                ViewBag.Mensaje = "No se encontró el correo que ingresó.";
            }
            return View();
        }

        public ActionResult ActualizarContraseña(string token)
        {
            ViewBag.Token = token;
            return View();
        }

        [HttpPost]
        public ActionResult ActualizarContraseña(string token, string contraseña, string confirmar_contraseña)
        {
            ViewBag.Token = token;

            if (contraseña != confirmar_contraseña)
            {
                ViewBag.Mensaje = "Las contraseñas que ingresó no coinciden.";
                return View();
            }

            bool respuesta = UsuarioRepository.ReestablecerUsuario(0, EncriptarContraseña.EncriptarMD5(contraseña), token);

            if (respuesta)
            {
                ViewBag.Reestablecido = true;
            }
            else
            {
                ViewBag.Mensaje = "No se puede actualizar su contraseña.";
            }
            return View();
        }

    }
}