using Osbar.Dto;
using Osbar.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Osbar.Repositories.Models;
using Osbar.Utilities;

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
        public ActionResult Login(string email, string contraseña)
        {
            UsuarioDto usuario = UsuarioModel.ValidacionUsuario(email, EncriptarContraseña.ConvertirSHA256(contraseña));
            if (usuario != null)
            {
                if (!usuario.confirmado)
                {
                    ViewBag.Mensaje = $"Su cuenta no se ha verificado. Revise su correo electrónico {email}";
                }
                else if (usuario.restablecer)
                {
                    ViewBag.Mensaje = $"Se ha solicitado la restauración de su contraseña. Revise su correo electrónico {email}";
                }
                else
                {
                    return RedirectToAction("index", "Home");
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
            if (usuario.contraseña != usuario.confirmar_contraseña)
            {
                ViewBag.Documento = usuario.no_ident;
                ViewBag.Nombre = usuario.nombre;
                ViewBag.Apellido1 = usuario.apellido_m;
                ViewBag.Apellido2 = usuario.apellido_p;
                ViewBag.Dirección = usuario.dirección;
                ViewBag.Teléfono = usuario.teléfono;
                ViewBag.Email = usuario.email;
                ViewBag.Mensaje = "Las contraseñas que ingresó no coinciden.";
                return View();
            }

            if (UsuarioModel.ObtenerUsuario(usuario.email) == null)
            {
                usuario.contraseña = EncriptarContraseña.ConvertirSHA256(usuario.contraseña);
                usuario.token = EncriptarContraseña.GenerarToken();
                usuario.restablecer = false;
                usuario.confirmado = false;
                bool resp = UsuarioModel.Registro(usuario);

                if (resp)
                {
                    string ruta = HttpContext.Server.MapPath("~/Views/PlantillaCorreo/VerificarCorreo.html");
                    string contenido = System.IO.File.ReadAllText(ruta);
                    string url = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Headers["host"], "/Inicio/Verificacion?token=" + usuario.token);

                    string htmlBody = string.Format(contenido, usuario.nombre, url);

                    CorreoDto correoDto = new CorreoDto()
                    {
                        Destinatario = usuario.email,
                        Asunto = "Verificación de Correo",
                        Contenido = htmlBody
                    };

                    bool enviado = CorreoService.EnviarCorreo(correoDto);
                    ViewBag.Creado = true;
                    ViewBag.Mensaje = $"Su cuenta ha sido creada satisfactoriamente. Se ha enviado un mensaje al correo proporcionad {usuario.email} para la verificación de su cuenta.";
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
            ViewBag.Respuesta = UsuarioModel.VerificarUsuario(token);
            return View();
        }

        public ActionResult Reestablecer()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Reestablecer(string email)
        {
            UsuarioDto usuario = UsuarioModel.ObtenerUsuario(email);
            ViewBag.Email = email;
            if (usuario != null)
            {
                bool respuesta = UsuarioModel.RestablecerUsuario(1, usuario.contraseña, usuario.token);

                if (respuesta)
                {
                    string ruta = HttpContext.Server.MapPath("~/Views/PlantillaCorreo/ReestablecerContraseña.html");
                    string contenido = System.IO.File.ReadAllText(ruta);
                    string url = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Headers["host"], "/Inicio/ActualizarContraseña?token=" + usuario.token);

                    string htmlBody = string.Format(contenido, usuario.nombre, url);

                    CorreoDto correoDto = new CorreoDto()
                    {
                        Destinatario = email,
                        Asunto = "Reestablecimiento de cuenta",
                        Contenido = htmlBody
                    };

                    bool enviado = CorreoService.EnviarCorreo(correoDto);
                    ViewBag.Restablecido = true;
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

            bool respuesta = UsuarioModel.RestablecerUsuario(0, EncriptarContraseña.ConvertirSHA256(contraseña), token);

            if (respuesta)
            {
                ViewBag.Restablecido = true;
            }
            else
            {
                ViewBag.Mensaje = "No se puede actualizar su contraseña.";
            }
            return View();
        }

    }
}