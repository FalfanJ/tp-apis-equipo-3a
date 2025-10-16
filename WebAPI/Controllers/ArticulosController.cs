using Dominio;
using Negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class ArticulosController : ApiController
    {
        // GET: api/Articulos
        [HttpGet]
        public IEnumerable<Articulos> Get()
        {
            ArticulosNegocio negocio = new ArticulosNegocio();
            return negocio.Listar();  // Devuelve todos los artículos con marca, categoría e imágenes
        }

        // GET: api/Articulos/5
        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            ArticulosNegocio negocio = new ArticulosNegocio();
            var articulos = negocio.Listar();
            var articulo = articulos.Find(a => a.Id == id);

            if (articulo == null)
                return NotFound();

            return Ok(articulo);
        }

        // PUT: api/Articulos/5
        [HttpPut]
        public IHttpActionResult Put(int id, [FromBody] Articulos articulo)
        {
            if (articulo == null)
                return BadRequest("El cuerpo de la solicitud está vacío.");

            articulo.Id = id;
            ArticulosNegocio negocio = new ArticulosNegocio();

            try
            {
                negocio.Modificar(articulo); 
                return Ok("Artículo actualizado correctamente.");
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        // POST: api/Articulos
        public HttpResponseMessage Post([FromBody] ArticulosDto art)
        {
            Articulos nuevo = new Articulos();
            ArticulosNegocio artNeg = new ArticulosNegocio();
            nuevo.Marca = new Marcas();
            nuevo.Categoria = new Categorias();

            if (string.IsNullOrEmpty(art.Codigo))
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Sin ingreso de codigo prodcuto.");

            nuevo.Codigo = art.Codigo;
            nuevo.Nombre = art.Nombre;
            nuevo.Descripcion = art.Descripcion;
            nuevo.Precio = art.Precio;
            nuevo.Marca.Id = (int)art.IdMarca;
            nuevo.Categoria.Id = (int)art.IdCategoria;

            artNeg.Agregar(nuevo);
            return Request.CreateResponse(HttpStatusCode.OK, "Producto agregado.");
        }

        // DELETE api/Articulos/5
        public IHttpActionResult Delete(int id)
        {
            ArticulosNegocio negocio = new ArticulosNegocio();

            try
            {
                negocio.EliminarFisico(id);
                return Ok("Articulo eliminado correctamente.");
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // GET: api/Articulos/buscarPorCodigo/XXXX
        [HttpGet]
        [Route("api/Articulos/buscarPorCodigo/{codigo}")]
        public IHttpActionResult BuscarPorCodigo(string codigo)
        {
            // ---- Verificamos que no venga vacio
            if (string.IsNullOrEmpty(codigo))
                return BadRequest("Debe ingresar un código de producto.");

            try
            {
                ArticulosNegocio negocio = new ArticulosNegocio();
                var articulo = negocio.BuscarPorCodigo(codigo);
                // ---- Verificamos si el articulo existe en la base
                if (articulo == null)
                    return BadRequest("Articulo inexistente.");

                return Ok(articulo);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
