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
                //datos.SetearConsulta("SELECT a.id, a.Codigo, a.Nombre, a.Descripcion, b.Descripcion AS 'Marca', c.Descripcion AS 'Categoria' , a.Precio FROM ARTICULOS A LEFT JOIN MARCAS B on a.IdMarca = b.Id LEFT JOIN CATEGORIAS C on a.IdCategoria = c.Id");
                //datos.SetearConsulta("SELECT a.id, a.Codigo, a.Nombre, a.Descripcion, ISNULL(m.Descripcion, 'Marca NO encontrada') AS 'Marca', ISNULL(c.Descripcion, 'Dato NO encontrado') AS 'Categoria' , a.Precio FROM ARTICULOS A LEFT JOIN MARCAS M on a.IdMarca = m.Id LEFT JOIN CATEGORIAS C on a.IdCategoria = c.Id");
                datos.SetearConsulta("SELECT a.id, a.Codigo, a.Nombre, a.Descripcion, b.Descripcion AS 'Marca', c.Descripcion AS 'Categoria' , a.Precio, b.Id AS 'IdMarca' , c.Id AS 'IdCategoria'  FROM ARTICULOS A LEFT JOIN MARCAS B on a.IdMarca = b.Id LEFT JOIN CATEGORIAS C on a.IdCategoria = c.Id");
                datos.EjecutarLectura();
                while (datos.Lector.Read())
                {
                    Articulos aux = new Articulos();
                    aux.Id = (int)datos.Lector["Id"];
                    aux.Codigo = (string)datos.Lector["Codigo"];
                    aux.Nombre = (string)datos.Lector["Nombre"];
                    aux.Descripcion = (string)datos.Lector["Descripcion"];
                    aux.Precio = (decimal)datos.Lector["Precio"];

                    if (!(datos.Lector["Marca"] is DBNull))
                    {
                        aux.Marca = new Marcas();
                        aux.Marca.Marca = (string)datos.Lector["Marca"];
                        aux.Marca.Id = (int)datos.Lector["IdMarca"];
                    }
                    if (!(datos.Lector["Categoria"] is DBNull))
                    {
                        aux.Categoria = new Categorias();
                        aux.Categoria.Descripcion = (string)datos.Lector["Categoria"];
                        aux.Categoria.Id = (int)datos.Lector["IdCategoria"];
                    }

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

        public void Agregar(Articulos nuevo)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                //datos.SetearConsulta("INSERT INTO ARTICULOS (Codigo, Nombre, Descripcion, Precio, IdMarca, IdCategoria) VALUES ('" + nuevo.Codigo+"', '"+nuevo.Nombre+"', '"+nuevo.Descripcion+"', "+nuevo.Precio+", "+nuevo.Marca.Id+", "+nuevo.Categoria.Id+")");
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
                datos.SetearConsulta("UPDATE ARTICULOS set Codigo = @codigo, Nombre = @nombre, Descripcion = @descripcion, IdMarca = @idmarca, IdCategoria =  @idcategoria, Precio = @precio WHERE Id = @id");
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
                datos.SetearConsulta("SELECT Id from ARTICULOS WHERE Codigo = @codigo");
                datos.SetearParametro("@codigo", codArticulo);
                datos.EjecutarLectura();
                datos.Lector.Read();
                return (int)datos.Lector["Id"]; ;

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
