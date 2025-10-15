using Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public class CategoriasNegocio
    {
        // ---- Listar categorias
        public List<Categorias> listar()
        {
            List<Categorias> lista = new List<Categorias>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearConsulta("SELECT Id, Descripcion FROM CATEGORIAS");
                datos.EjecutarLectura();

                while (datos.Lector.Read())
                {
                    Categorias aux = new Categorias();
                    aux.Id = (int)datos.Lector["Id"];
                    aux.Descripcion = (string)datos.Lector["Descripcion"];

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

        // ---- Buscar Categoria por nombre
        public List<Categorias> buscar(string nombre)
        {
            List<Categorias> lista = new List<Categorias>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearConsulta($"SELECT Id, Descripcion FROM CATEGORIAS WHERE Descripcion LIKE '%{nombre}%'");
                datos.EjecutarLectura();

                while (datos.Lector.Read())
                {
                    Categorias aux = new Categorias();
                    aux.Id = (int)datos.Lector["Id"];
                    aux.Descripcion = (string)datos.Lector["Descripcion"];

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

        // ---- Agregar una nueva categoria
        public void agregar(Categorias categoria)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearConsulta($"INSERT INTO CATEGORIAS (Descripcion) VALUES ('{categoria.Descripcion}')");
                datos.EjecutarLectura();
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

        // ---- Editar categoria
        public void editar(Categorias categoria)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearConsulta($"UPDATE CATEGORIAS SET Descripcion = '{categoria.Descripcion}' WHERE Id = {categoria.Id}");
                datos.EjecutarLectura();
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

        // ---- Eliminar categoria
        public void eliminar(int id)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearConsulta($"DELETE FROM CATEGORIAS WHERE Id = {id}");
                datos.EjecutarLectura();
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

        // ---- Obtener por Id
        public Categorias obtenerPorId(int id)
        {
            AccesoDatos datos = new AccesoDatos();
            Categorias categoria = null;

            try
            {
                datos.SetearConsulta($"SELECT Id, Descripcion FROM CATEGORIAS WHERE Id = {id}");
                datos.EjecutarLectura();

                if (datos.Lector.Read())
                {
                    categoria = new Categorias();
                    categoria.Id = (int)datos.Lector["Id"];
                    categoria.Descripcion = (string)datos.Lector["Descripcion"];
                }

                return categoria;
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
