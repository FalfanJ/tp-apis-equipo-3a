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
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Articulos/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Articulos
        public HttpResponseMessage Post([FromBody]ArticulosDto art)
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

        // PUT: api/Articulos/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Articulos/5
        public void Delete(int id)
        {
        }
    }
}
