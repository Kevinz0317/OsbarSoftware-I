using Osbar.Dto;
using Osbar.Repositories;
using Osbar.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Osbar.Controllers
{ 
    public class CatalogoController : Controller
    {
        CategoriaRepository cr = new CategoriaRepository();
        ProductoRepository pr = new ProductoRepository();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Producto(int idproducto = 0)
        {
            
            ProductoDto oProducto = new ProductoDto();
            List<ProductoDto> oLista = new List<ProductoDto>();

            oLista = pr.Listar();
            oProducto = (from o in oLista
                         where o.IdProducto == idproducto
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
                         }).FirstOrDefault();

            return View(oProducto);
        }


        [HttpPost]
        public JsonResult ListarProducto(int idcategoria = 0)
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

            if (idcategoria != 0)
            {
                oLista = oLista.Where(x => x.oCategoria.IdCategoria == idcategoria).ToList();
            }

            var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();

            var json = Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = 500000000;
            return json;
        }


        [HttpGet]
        public JsonResult ListarCategoria()
        {
            List<CategoriaDto> oLista = new List<CategoriaDto>();
            oLista = cr.Consultar();
            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }

    }
}