using Dominio;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public class MarcasNegocio
    {
        private string connectionString = "server=.; database=CATALOGO_P3_DB; integrated security=true;";

        // Listar todas las marcas
        public List<Marcas> Listar()
        {
            List<Marcas> lista = new List<Marcas>();
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.SetearConsulta("SELECT Id, Descripcion FROM MARCAS");
                datos.EjecutarLectura();
                while (datos.Lector.Read())
                {
                    Marcas aux = new Marcas();
                    aux.Id = (int)datos.Lector["Id"];
                    aux.Marca = (string)datos.Lector["Descripcion"];
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

        // Buscar marcas por nombre (contiene)
        public List<Dominio.Marcas> Buscar(string criterio)
        {
            List<Dominio.Marcas> lista = new List<Dominio.Marcas>();
            using (SqlConnection conexion = new SqlConnection(connectionString))
            {
                SqlCommand comando = new SqlCommand("SELECT Id, Descripcion FROM MARCAS WHERE Descripcion LIKE @criterio", conexion);
                comando.Parameters.AddWithValue("@criterio", "%" + criterio + "%");
                conexion.Open();
                SqlDataReader lector = comando.ExecuteReader();

                while (lector.Read())
                {
                    Dominio.Marcas marca = new Dominio.Marcas
                    {
                        Id = (int)lector["Id"],
                        Marca = (string)lector["Descripcion"]
                    };
                    lista.Add(marca);
                }
            }
            return lista;
        }

        // Agregar nueva marca
        public void Agregar(Dominio.Marcas nueva)
        {
            using (SqlConnection conexion = new SqlConnection(connectionString))
            {
                SqlCommand comando = new SqlCommand("INSERT INTO MARCAS (Descripcion) VALUES (@Descripcion)", conexion);
                comando.Parameters.AddWithValue("@Descripcion", nueva.Marca);
                conexion.Open();
                comando.ExecuteNonQuery();
            }
        }
        // Modificar marca existente
        public void Modificar(Dominio.Marcas marca)
        {
            using (SqlConnection conexion = new SqlConnection(connectionString))
            {
                SqlCommand comando = new SqlCommand("UPDATE MARCAS SET Descripcion = @Descripcion WHERE Id = @Id", conexion);
                comando.Parameters.AddWithValue("@Descripcion", marca.Marca);
                comando.Parameters.AddWithValue("@Id", marca.Id);
                conexion.Open();
                comando.ExecuteNonQuery();
            }
        }

        // Eliminar marca
        public void Eliminar(int id)
        {
            using (SqlConnection conexion = new SqlConnection(connectionString))
            {
                SqlCommand comando = new SqlCommand("DELETE FROM MARCAS WHERE Id = @Id", conexion);
                comando.Parameters.AddWithValue("@Id", id);
                conexion.Open();
                comando.ExecuteNonQuery();
            }
        }

        // Ver detalle de una marca por Id
        public Dominio.Marcas ObtenerPorId(int id)
        {
            Dominio.Marcas marca = null;
            using (SqlConnection conexion = new SqlConnection(connectionString))
            {
                SqlCommand comando = new SqlCommand("SELECT Id, Descripcion FROM MARCAS WHERE Id = @Id", conexion);
                comando.Parameters.AddWithValue("@Id", id);
                conexion.Open();
                SqlDataReader lector = comando.ExecuteReader();

                if (lector.Read())
                {
                    marca = new Dominio.Marcas
                    {
                        Id = (int)lector["Id"],
                        Marca = (string)lector["Descripcion"]
                    };
                }
            }

            return marca;
        }
    }
}
