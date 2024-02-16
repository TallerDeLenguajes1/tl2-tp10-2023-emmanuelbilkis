using Kanban.Models;
using System.Data.SQLite;
using TP10.Models;
using TP10.ViewModels;

namespace Kanban.Repositorios
{
    public class TableroRepositorio : ITableroRepositorio
    {
        
        private readonly string _cadenaConexion;
     
        public TableroRepositorio(IConfiguration configuration) 
        {
            _cadenaConexion = configuration.GetConnectionString("SqliteConexion");
        }

        public void Create(Tablero tablero)
        {
            if (tablero is null)
            {
                throw new ArgumentNullException(nameof(tablero), "El tablero no puede ser nulo.");
            }

            var query = $"INSERT INTO Tablero (Id_usuario_propietario,nombre,descripcion) VALUES (@id_usu,@nombre,@desc)";
            using (SQLiteConnection connection = new SQLiteConnection(_cadenaConexion))
            {

                connection.Open();
                var command = new SQLiteCommand(query, connection);

                command.Parameters.Add(new SQLiteParameter("@id_usu", tablero.IdUsuarioPropietario));
                command.Parameters.Add(new SQLiteParameter("@nombre", tablero.Nombre));
                command.Parameters.Add(new SQLiteParameter("@desc", tablero.Descripcion));

                try
                {
                    int filasAfectadas = command.ExecuteNonQuery();

                    if (filasAfectadas <= 0)
                    {
                        throw new InvalidOperationException("No se insertaron filas en la base de datos.");
                    }
                }
                catch (SQLiteException ex)
                {
                    Console.WriteLine($"Error SQLite: {ex.Message}");
                    throw;
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public List<Tablero> GetAll()
        {
            var queryString = @"SELECT * FROM Tablero;";
            List<Tablero> tableros = new List<Tablero>();

            try
            {  
                using (SQLiteConnection connection = new SQLiteConnection(_cadenaConexion))
                {
                    SQLiteCommand command = new SQLiteCommand(queryString, connection);
                    connection.Open();

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var id = Convert.ToInt32(reader["Id"]);
                            var id_usu_propietario = Convert.ToInt32(reader["Id_usuario_propietario"]);
                            var nombre = reader["nombre"].ToString();
                            var descripcion = reader["descripcion"].ToString();
                            var tablero = new Tablero(id, id_usu_propietario, nombre, descripcion);

                            tableros.Add(tablero);

                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error al obtener la lista de tableros desde la base de datos.", ex);
            }

            return tableros;
        }

        public Tablero GetById(int idTablero)
        {
            SQLiteConnection connection = new SQLiteConnection(_cadenaConexion);
            Tablero tab = null;

            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM Tablero WHERE id = @idTab;";
            command.Parameters.Add(new SQLiteParameter("@idTab", idTablero));
            connection.Open();
            
            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var tablero = new Tablero(Convert.ToInt32(reader["Id"]), Convert.ToInt32(reader["Id_usuario_propietario"]), reader["nombre"].ToString(), reader["descripcion"].ToString());
                    tab = tablero;
                }
            }
            connection.Close();

            if (tab == null)
            {
                throw new InvalidOperationException($"No se encontró un tablero con el ID {idTablero}.");
            }

            return (tab);
        }

        public List<Tablero> ListarPorUsuario(int idUsuario)
        {
            SQLiteConnection connection = new SQLiteConnection(_cadenaConexion);
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = @"SELECT * FROM Tablero WHERE Id_usuario_propietario = @idUsu;"; // cambiarlo a innerjoin
            command.Parameters.Add(new SQLiteParameter("@idUsu", idUsuario));
            connection.Open();
            var lista = new List<Tablero>();    
            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var tablero = new Tablero(Convert.ToInt32(reader["Id"]), Convert.ToInt32(reader["Id_usuario_propietario"]), reader["nombre"].ToString(), reader["descripcion"].ToString());
                    lista.Add(tablero);
                }
            }
            connection.Close();

            if (lista is null || lista.Count==0)
            {
                throw new InvalidOperationException($"No se encontraron tableros con el id {idUsuario} solicitado");
            }

            return (lista);

        }
        
        public List<Tablero> ListarTablerosPropiosYConTareas(int idUsuario)
        {
            SQLiteConnection connection = new SQLiteConnection(_cadenaConexion);
            SQLiteCommand command = connection.CreateCommand();
                    command.CommandText = @"SELECT 
                    tab.Id,
                    tab.Id_usuario_propietario,
                    tab.nombre,
                    tab.descripcion 
                FROM 
                    Tarea tar
                    INNER JOIN Usuario u ON u.id = tar.id_usuario_asignado
                    INNER JOIN Tablero tab ON tab.Id = tar.Id_tablero 
                WHERE 
                    u.id = @idUsu

                UNION 

                SELECT 
                    * 
                FROM 
                    Tablero 
                WHERE 
                    Id_usuario_propietario = @idUsu;
                "; 
            command.Parameters.Add(new SQLiteParameter("@idUsu", idUsuario));
            connection.Open();
            var lista = new List<Tablero>();
            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var tablero = new Tablero(Convert.ToInt32(reader["Id"]), Convert.ToInt32(reader["Id_usuario_propietario"]), reader["nombre"].ToString(), reader["descripcion"].ToString());
                    lista.Add(tablero);
                }
            }
            connection.Close();

            if (lista is null || lista.Count == 0)
            {
                throw new InvalidOperationException($"No se encontraron tableros con el id {idUsuario} solicitado, ni tableros con tareas asignadas a este usuario");
            }

            return (lista);
        }
        public void Remove(int id)
        {
            if (id < 0)
            {
                throw new ArgumentException("El ID del Tablero no es válido.", nameof(id));
            }

            SQLiteConnection connection = new SQLiteConnection(_cadenaConexion);
            SQLiteCommand command = connection.CreateCommand();
            try
            {
                command.CommandText = $"DELETE FROM Tablero WHERE id = '{id}';";
                connection.Open();
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw new InvalidOperationException($"No se puedo borrar el tablero | Información: {e.Data} + {e.StackTrace}");
            }
            finally 
            {
                connection.Close();
            }
        }

        public void Update(Tablero tablero)
        {
            if (tablero is null || tablero.Id < 0)
            {
                throw new InvalidOperationException("Tablero inválido");
            }

            SQLiteConnection connection = new SQLiteConnection(_cadenaConexion);
            SQLiteCommand command = connection.CreateCommand();

            try
            {
                command.CommandText = $"UPDATE Tablero SET Id = '{tablero.Id}', Id_usuario_propietario = '{tablero.IdUsuarioPropietario}',nombre='{tablero.Nombre}',descripcion='{tablero.Descripcion}' WHERE id = '{tablero.Id}';";
                connection.Open();
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw new InvalidOperationException($"No se puedo actualizar el tablero | Información: {e.Data } + {e.StackTrace}");
            }
            finally 
            {
                connection.Close();
            }
        }

        public List<TableroConUsuario> ObtenerTablerosConUsuario()
        {
            var queryString = @"SELECT tab.Id,tab.nombre,tab.descripcion,usu.nombre_de_usuario,usu.rol FROM Tablero as tab
                                inner join Usuario as usu ON tab.Id_usuario_propietario = usu.id
                                order by usu.nombre_de_usuario asc;";
            List<TableroConUsuario> tableros = new List<TableroConUsuario>();

            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(_cadenaConexion))
                {
                    SQLiteCommand command = new SQLiteCommand(queryString, connection);
                    connection.Open();

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var IDTablero = Convert.ToInt32(reader["Id"]);
                            var nombreTablero = reader["nombre"].ToString();
                            var descripcionTablero = reader["descripcion"].ToString();
                            var nombreUsu = reader["nombre_de_usuario"].ToString();
                            var rolUsuario = reader["rol"].ToString();
                            var tablero = new TableroConUsuario(IDTablero, nombreTablero, descripcionTablero, nombreUsu, rolUsuario);

                            tableros.Add(tablero);

                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception e)
            {

                throw new InvalidOperationException($"No se pudo obtener los tableros con usuarios | Información: {e.Data} + {e.StackTrace}");
            }
          
            return tableros;
        }

        public List<TableroConUsuario> ObtenerTablerosConUsuario(int idUsuario, int idTab)
        {
            var queryString = $@"SELECT tab.Id,tab.nombre,tab.descripcion,usu.nombre_de_usuario,usu.rol FROM Tablero as tab
                                inner join Usuario as usu ON tab.Id_usuario_propietario = usu.id 
                                WHERE usu.id = '{idUsuario}' AND tab.Id = '{idTab}'
                                order by usu.nombre_de_usuario asc;";
            List<TableroConUsuario> tableros = new List<TableroConUsuario>();

            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(_cadenaConexion))
                {
                    SQLiteCommand command = new SQLiteCommand(queryString, connection);
                    connection.Open();

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var IDTablero = Convert.ToInt32(reader["Id"]);
                            var nombreTablero = reader["nombre"].ToString();
                            var descripcionTablero = reader["descripcion"].ToString();
                            var nombreUsu = reader["nombre_de_usuario"].ToString();
                            var rolUsuario = reader["rol"].ToString();
                            var tablero = new TableroConUsuario(IDTablero, nombreTablero, descripcionTablero, nombreUsu, rolUsuario);

                            tableros.Add(tablero);

                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception e)
            {

                throw new InvalidOperationException($"No se pudo obtener los tableros con usuarios | Información: {e.Data} + {e.StackTrace}");
            }

            return tableros;
        }
    }
}
