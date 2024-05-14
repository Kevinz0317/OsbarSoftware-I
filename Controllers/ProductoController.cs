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

        [HttpPost]
        public JsonResult AgregarProducto(string producto, HttpPostedFileBase archivoImagen)
        {
            ResponseDto resp = new ResponseDto() { resultado = true, mensaje = "" };
            try
            {
                ProductoDto prod = new ProductoDto();
                prod = JsonConvert.DeserializeObject<ProductoDto>(producto);

                string rutaImagen = "~/Imagenes/ProductosImgs";
                string pathImagen = "~/Imagenes/ProductosImgs";

                if (!Directory.Exists(pathImagen))
                    Directory.CreateDirectory(pathImagen);

                if (prod.id_prod == 0)
                {
                    int id_prod = pr.AgregarProducto(prod);
                    prod.id_prod = id_prod;
                    resp.resultado = prod.id_prod == 0 ? false : true;
                }
                else
                {
                    resp.resultado = pr.ActualizarProducto(prod);
                }

                if (archivoImagen != null && prod.id_prod != 0)
                {
                    string ext = Path.GetExtension(archivoImagen.FileName);
                    rutaImagen = rutaImagen + prod.id_prod.ToString() + ext;
                    prod.imagen = rutaImagen;

                    archivoImagen.SaveAs(pathImagen + "/" + prod.id_prod.ToString() + ext);
                    resp.resultado = pr.ActualizarCampoImagen(prod);
                }
            }
            catch (Exception ex)
            {
                resp.resultado = false;
                resp.mensaje = ex.Message;
            }
            return Json(resp, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListaProductos()
        {
            List<ProductoDto> producto = new List<ProductoDto>();
            producto = pr.ObtenerListaProductos();
            producto = (from p in producto
                        select new ProductoDto()
                        {
                            id_prod = p.id_prod,
                            nombre = p.nombre,
                            descripcion = p.descripcion,
                            id_categoria = p.id_categoria,
                            precio = p.precio,
                            impuesto = p.impuesto,
                            stock = p.stock,
                            imagen = p.imagen,
                            imgBase64 = Utilities.ConvertirBase64.convertirBase64(Server.MapPath(p.imagen)),
                            extImg = Path.GetExtension(p.imagen).Replace(".", "")
                        }).ToList();
            return Json(new { data = producto }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EliminarProducto(int id_prod)
        {
            bool respuesta = false;
            respuesta = pr.EliminarProducto(id_prod);
            return Json(new { resultado = respuesta }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult ListarCategoria()
        {
            List<CategoriaDto> oLista = new List<CategoriaDto>();
            oLista = CategoriaRepository.Instance.Listar();
            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GuardarCategoria(CategoriaDto objeto)
        {
            bool respuesta = false;
            respuesta = (objeto.id_categoria == 0) ? CategoriaRepository.Instance.Registrar(objeto) : CategoriaRepository.Instance.Modificar(objeto);
            return Json(new { resultado = respuesta }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult EliminarCategoria(int id)
        {
            bool respuesta = false;
            respuesta = CategoriaRepository.Instance.Eliminar(id);
            return Json(new { resultado = respuesta }, JsonRequestBehavior.AllowGet);
        }
    }

    }