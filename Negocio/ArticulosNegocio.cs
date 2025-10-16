using Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public class ArticulosNegocio
    {
        public List<Articulos> Listar()
        {
            List<Articulos> lista = new List<Articulos>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearConsulta(@"
                    SELECT a.Id, a.Codigo, a.Nombre, a.Descripcion, a.Precio,
                           m.Id AS IdMarca, m.Descripcion AS Marca,
                           c.Id AS IdCategoria, c.Descripcion AS Categoria
                    FROM ARTICULOS a
                    LEFT JOIN MARCAS m ON a.IdMarca = m.Id
                    LEFT JOIN CATEGORIAS c ON a.IdCategoria = c.Id");

                datos.EjecutarLectura();

                while (datos.Lector.Read())
                {
                    Articulos aux = new Articulos();
                    aux.Id = (int)datos.Lector["Id"];
                    aux.Codigo = (string)datos.Lector["Codigo"];
                    aux.Nombre = (string)datos.Lector["Nombre"];
                    aux.Descripcion = (string)datos.Lector["Descripcion"];
                    aux.Precio = (decimal)datos.Lector["Precio"];

                    // --- Marca ---
                    if (!(datos.Lector["Marca"] is DBNull))
                    {
                        aux.Marca = new Marcas();
                        aux.Marca.Marca = (string)datos.Lector["Marca"];
                        aux.Marca.Id = (int)datos.Lector["IdMarca"];
                    }

                    // --- Categoría ---
                    if (!(datos.Lector["Categoria"] is DBNull))
                    {
                        aux.Categoria = new Categorias();
                        aux.Categoria.Descripcion = (string)datos.Lector["Categoria"];
                        aux.Categoria.Id = (int)datos.Lector["IdCategoria"];
                    }

                    // --- Imágenes ---
                    aux.Imagenes = ObtenerImagenes(aux.Id);

                    lista.Add(aux);
                }

                return lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        // carga todas las imágenes de un artículo
        private List<Imagenes> ObtenerImagenes(int idArticulo)
        {
            List<Imagenes> lista = new List<Imagenes>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearConsulta("SELECT Id, ImagenUrl FROM IMAGENES WHERE IdArticulo = @id");
                datos.SetearParametro("@id", idArticulo);
                datos.EjecutarLectura();

                while (datos.Lector.Read())
                {
                    Imagenes img = new Imagenes();
                    img.Id = (int)datos.Lector["Id"];
                    img.ImagenURL = (string)datos.Lector["ImagenUrl"];
                    lista.Add(img);
                }

                return lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.CerrarConexion();
            }
        }
        public void Agregar(Articulos nuevo)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearConsulta("INSERT INTO ARTICULOS (Codigo, Nombre, Descripcion, Precio, IdMarca, IdCategoria) VALUES (@codigo, @nombre, @descripcion, @precio, @idMarca, @idCategoria)");
                datos.SetearParametro("@codigo", nuevo.Codigo);
                datos.SetearParametro("@nombre", nuevo.Nombre);
                datos.SetearParametro("@descripcion", nuevo.Descripcion);
                datos.SetearParametro("@precio", nuevo.Precio);
                datos.SetearParametro("@idMarca", nuevo.Marca.Id);
                datos.SetearParametro("@idCategoria", nuevo.Categoria.Id);
                datos.EjecutarAccion();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        public void Modificar(Articulos modificar)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearConsulta("UPDATE ARTICULOS SET Codigo = @codigo, Nombre = @nombre, Descripcion = @descripcion, IdMarca = @idmarca, IdCategoria =  @idcategoria, Precio = @precio WHERE Id = @id");
                datos.SetearParametro("@id", modificar.Id);
                datos.SetearParametro("@codigo", modificar.Codigo);
                datos.SetearParametro("@nombre", modificar.Nombre);
                datos.SetearParametro("@descripcion", modificar.Descripcion);
                datos.SetearParametro("@precio", modificar.Precio);
                datos.SetearParametro("@idmarca", modificar.Marca.Id);
                datos.SetearParametro("@idcategoria", modificar.Categoria.Id);
                datos.EjecutarAccion();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        public int IdArticulo(string codArticulo)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearConsulta("SELECT Id FROM ARTICULOS WHERE Codigo = @codigo");
                datos.SetearParametro("@codigo", codArticulo);
                datos.EjecutarLectura();
                datos.Lector.Read();
                return (int)datos.Lector["Id"];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        public void EliminarFisico(int id)
        {
            // ---- Eliminamos la imagen
            AccesoDatos datosImagenes = new AccesoDatos();
            try
            {
                datosImagenes.SetearConsulta("DELETE FROM IMAGENES WHERE IdArticulo = @id");
                datosImagenes.SetearParametro("@id", id);
                datosImagenes.EjecutarAccion();
            }
            finally
            {
                datosImagenes.CerrarConexion();
            }

            // Luego de cerrar la conexion eliminamos el articulo
            AccesoDatos datosArticulo = new AccesoDatos();
            try
            {
                datosArticulo.SetearConsulta("DELETE FROM ARTICULOS WHERE Id = @id");
                datosArticulo.SetearParametro("@id", id);
                datosArticulo.EjecutarAccion();
            }
            finally
            {
                datosArticulo.CerrarConexion();
            }
        }
        // ---- Buscar producto por codigo
        public Articulos BuscarPorCodigo(string codigo)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearConsulta(@"
            SELECT a.Id, a.Codigo, a.Nombre, a.Descripcion, a.Precio,
                   m.Id AS IdMarca, m.Descripcion AS Marca,
                   c.Id AS IdCategoria, c.Descripcion AS Categoria
            FROM ARTICULOS a
            LEFT JOIN MARCAS m ON a.IdMarca = m.Id
            LEFT JOIN CATEGORIAS c ON a.IdCategoria = c.Id
            WHERE a.Codigo = @codigo");

                datos.SetearParametro("@codigo", codigo);
                datos.EjecutarLectura();

                if (datos.Lector.Read())
                {
                    // ---- Construimos el articulo
                    Articulos aux = new Articulos();
                    aux.Id = (int)datos.Lector["Id"];
                    aux.Codigo = (string)datos.Lector["Codigo"];
                    aux.Nombre = (string)datos.Lector["Nombre"];
                    aux.Descripcion = (string)datos.Lector["Descripcion"];
                    aux.Precio = (decimal)datos.Lector["Precio"];

                    // --- Marca
                    if (!(datos.Lector["Marca"] is DBNull))
                    {
                        aux.Marca = new Marcas();
                        aux.Marca.Id = (int)datos.Lector["IdMarca"];
                        aux.Marca.Marca = (string)datos.Lector["Marca"];
                    }

                    // --- Categoria
                    if (!(datos.Lector["Categoria"] is DBNull))
                    {
                        aux.Categoria = new Categorias();
                        aux.Categoria.Id = (int)datos.Lector["IdCategoria"];
                        aux.Categoria.Descripcion = (string)datos.Lector["Categoria"];
                    }

                    // ---- Imag
                    aux.Imagenes = ObtenerImagenes(aux.Id);

                    return aux;
                }

                // ---- Retornamos null si no encontro nada

                return null; 
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

    }


}