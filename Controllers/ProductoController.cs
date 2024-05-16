using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Osbar.Dto;
using Osbar.Repositories;
using Osbar.Utilities;

namespace Osbar.Controllers
{
    public class ProductoController : Controller
    {
        ProductoRepository pr = new ProductoRepository();
        CategoriaRepository cr = new CategoriaRepository();

        // GET: Producto
        public ActionResult Producto()
        {
            return View();
        }

        public ActionResult Categoria()
        {
            return View();
        }

        [HttpGet]
        public JsonResult ListarCategoria()
        {
            List<CategoriaDto> oLista = new List<CategoriaDto>();
            oLista = cr.Consultar();
            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarCategoria(CategoriaDto objeto)
        {
            bool respuesta = false;
            respuesta = (objeto.IdCategoria == 0) ? cr.Agregar(objeto) : cr.Editar(objeto);
            return Json(new { resultado = respuesta }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult EliminarCategoria(int id)
        {
            bool respuesta = false;
            respuesta = cr.Eliminar(id);
            return Json(new { resultado = respuesta }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListarProducto()
        {
            List<ProductoDto> oLista = new List<ProductoDto>();

            oLista = pr.Listar();
            oLista = (from o in oLista
                      select new ProductoDto()
                      {
                          IdProducto = o.IdProducto,
                          Nombre = o.Nombre,
                          Descripcion = o.Descripcion,
                          oCategoria = o.oCategoria,
                          Precio = o.Precio,
                          Stock = o.Stock,
                          RutaImagen = o.RutaImagen,
                          base64 = ConvertirBase64.convertirBase64(Server.MapPath(o.RutaImagen)),
                          extension = Path.GetExtension(o.RutaImagen).Replace(".", ""),
                      }).ToList();
            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarProducto(string objeto, HttpPostedFileBase imagenArchivo)
        {

            ResponseDto oresponse = new ResponseDto() { resultado = true, mensaje = "" };

            try
            {
                ProductoDto oProducto = new ProductoDto();
                oProducto = JsonConvert.DeserializeObject<ProductoDto>(objeto);

                string GuardarEnRuta = "~/Imagenes/ProductosOsbar/";
                string physicalPath = Server.MapPath("~/Imagenes/ProductosOsbar");

                if (!Directory.Exists(physicalPath))
                    Directory.CreateDirectory(physicalPath);

                if (oProducto.IdProducto == 0)
                {
                    int id = pr.Registrar(oProducto);
                    oProducto.IdProducto = id;
                    oresponse.resultado = oProducto.IdProducto == 0 ? false : true;

                }
                else
                {
                    oresponse.resultado = pr.Modificar(oProducto);
                }


                if (imagenArchivo != null && oProducto.IdProducto != 0)
                {
                    string extension = Path.GetExtension(imagenArchivo.FileName);
                    GuardarEnRuta = GuardarEnRuta + oProducto.IdProducto.ToString() + extension;
                    oProducto.RutaImagen = GuardarEnRuta;

                    imagenArchivo.SaveAs(physicalPath + "/" + oProducto.IdProducto.ToString() + extension);

                    oresponse.resultado = pr.ActualizarRutaImagen(oProducto);
                }

            }
            catch (Exception e)
            {
                oresponse.resultado = false;
                oresponse.mensaje = e.Message;
            }

            return Json(oresponse, JsonRequestBehavior.AllowGet);
        }
    

    [HttpPost]
        public JsonResult EliminarProducto(int id)
        {
            bool respuesta = false;
            respuesta = pr.Eliminar(id);
            return Json(new { resultado = respuesta }, JsonRequestBehavior.AllowGet);
        }
    }
}