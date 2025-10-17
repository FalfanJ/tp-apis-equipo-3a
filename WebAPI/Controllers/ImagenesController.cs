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
    public class ImagenesController : ApiController
    {
        // GET: api/Imagenes
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Imagenes/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Imagenes/5
        public HttpResponseMessage Post(int id, [FromBody] List<string> urlList)
        {
            ImagenesNegocio imgNeg = new ImagenesNegocio();
            if (urlList.Count == 0)
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Vacio...");
            try
            {
                foreach (string item in urlList)
                {
                    Imagenes aux = new Imagenes();
                    aux.ImagenURL = item;
                    aux.IdArticulo = id;
                    imgNeg.Agregar(aux);
                }
                return Request.CreateResponse(HttpStatusCode.OK, "Imagen/es agregadas.");

            }
            catch (Exception)
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, "Error interno");
            }
        }

        // PUT: api/Imagenes/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/Imagenes/5
        public void Delete(int id)
        {
        }
    }
}
