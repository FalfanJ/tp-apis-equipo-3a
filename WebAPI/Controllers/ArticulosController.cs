using Dominio;
using Negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

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
                negocio.Modificar(articulo); // 🔹 no devuelve nada
                return Ok("Artículo actualizado correctamente.");
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
