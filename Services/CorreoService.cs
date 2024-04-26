using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Osbar.Dto;
using MailKit.Security;
using MailKit.Net.Smtp;
using MimeKit.Text;
using MimeKit;


namespace Osbar.Services
{
    public static class CorreoService
    {
        private static string Host = "smtp.gmail.com";
        private static int Puerto = 587;
        private static string Nombre = "Osbar";
        private static string Correo = "osbar0317@gmail.com";
        private static string Contraseña = "sioiovqtezwzyssc";

        public static bool EnviarCorreo(CorreoDto correo)
        {
            try
            {
                var email = new MimeMessage();

                email.From.Add(new MailboxAddress(Nombre, Correo));
                email.To.Add(MailboxAddress.Parse(correo.Destinatario));
                email.Subject = correo.Asunto;
                email.Body = new TextPart(TextFormat.Html)
                {
                    Text = correo.Contenido
                };

                var smtp = new SmtpClient();
                smtp.Connect(Host, Puerto, SecureSocketOptions.StartTls);

                smtp.Authenticate(Correo, Contraseña);
                smtp.Send(email);
                smtp.Disconnect(true);

                return true;

            }
            catch
            {
                return false;
            }
        }
    }
}