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
        CarritoRepository crto = new CarritoRepository();
        private static UsuarioDto oUsuario;
        EnvioRepository en = new EnvioRepository();
        public ActionResult Index()
        {
            if (Session["Usuario"] == null)
                return RedirectToAction("Login", "Inicio");
            else
                oUsuario = (UsuarioDto)Session["Usuario"];
            return View();
        }
        public ActionResult Producto(int idproducto = 0)
        {
            if (Session["Usuario"] == null)
                return RedirectToAction("Login", "Inicio");
            else
                oUsuario = (UsuarioDto)Session["Usuario"];
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

        public ActionResult Carrito()
        {
            if (Session["Usuario"] == null)
                return RedirectToAction("Login", "Inicio");
            else
                oUsuario = (UsuarioDto)Session["Usuario"];
            return View();
        }

        [HttpPost]
        public JsonResult InsertarCarrito(CarritoDto oCarrito)
        {
            oCarrito.oUsuario = new UsuarioDto() { IdUsuario = oUsuario.IdUsuario };
            int _respuesta = 0;
            _respuesta = crto.Registrar(oCarrito);
            return Json(new { respuesta = _respuesta }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult CantidadCarrito()
        {
            int _respuesta = 0;
            _respuesta = crto.Cantidad(oUsuario.IdUsuario);
            return Json(new { respuesta = _respuesta }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult ObtenerCarrito()
        {
            List<CarritoDto> oLista = new List<CarritoDto>();
            oLista = crto.Obtener(oUsuario.IdUsuario);

            if (oLista.Count != 0)
            {
                oLista = (from d in oLista
                          select new CarritoDto()
                          {
                              IdCarrito = d.IdCarrito,
                              oProducto = new ProductoDto()
                              {
                                  IdProducto = d.oProducto.IdProducto,
                                  Nombre = d.oProducto.Nombre,
                                  Precio = d.oProducto.Precio,
                                  RutaImagen = d.oProducto.RutaImagen,
                                  base64 = ConvertirBase64.convertirBase64(Server.MapPath(d.oProducto.RutaImagen)),
                                  extension = Path.GetExtension(d.oProducto.RutaImagen).Replace(".", ""),
                              }
                          }).ToList();
            }


            return Json(new { lista = oLista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EliminarCarrito(string IdCarrito, string IdProducto)
        {
            bool respuesta = false;
            respuesta = crto.Eliminar(IdCarrito, IdProducto);
            return Json(new { resultado = respuesta }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ObtenerDepartamento()
        {
            List<DepartamentoDto> oLista = new List<DepartamentoDto>();
            oLista = en.ObtenerDepartamento();
            return Json(new { lista = oLista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ObtenerCiudad(string _IdDepartamento)
        {
            List<CiudadDto> oLista = new List<CiudadDto>();
            oLista = en.ObtenerCiudad(_IdDepartamento);
            return Json(new { lista = oLista }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CerrarSesion()
        {
            Session["Usuario"] = null;
            Session.Abandon();
            return RedirectToAction("Login", "Inicio");
        }
    }

}