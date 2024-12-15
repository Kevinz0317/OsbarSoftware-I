using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Osbar.Utilities
{
    public class EncriptarContraseña
    {
        public static string EncriptarMD5(string texto)
        {
            string hash = "Proyecto de Ingenieria de Software I - IPA - 2024";
            byte[] data = UTF8Encoding.UTF8.GetBytes(texto);

            MD5 md5 = MD5.Create();
            TripleDES tripleDES = TripleDES.Create();

            tripleDES.Key = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(hash));
            tripleDES.Mode = CipherMode.ECB;

            ICryptoTransform transform = tripleDES.CreateEncryptor();
            byte[] result = transform.TransformFinalBlock(data, 0, data.Length);

            return Convert.ToBase64String(result);
        }
    }
}