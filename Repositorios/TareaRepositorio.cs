using Kanban.Models;
using System.Data.SQLite;
using System.IO;
using System.Threading;
using TP10.ViewModels;

namespace Kanban.Repositorios
{
    public class TareaRepositorio : ITareaRepositorio
    {

        private readonly string _cadenaConexion;

        public TareaRepositorio(IConfiguration configuration)
        {
            _cadenaConexion = configuration.GetConnectionString("SqliteConexion");
        }

        public void Create(Tarea tarea)
        {
            try
            {
                var query = "INSERT INTO Tarea (Id_tablero, nombre, estado, descripcion, color, id_usuario_asignado) VALUES (@idTab, @nombre, @estado, @desc, @color, @idUsu)";
                using (SQLiteConnection connection = new SQLiteConnection(_cadenaConexion))
                {
                    connection.Open();
                    var command = new SQLiteCommand(query, connection);

                    command.Parameters.Add(new SQLiteParameter("@idTab", tarea.IdTablero));
                    command.Parameters.Add(new SQLiteParameter("@nombre", tarea.Nombre));
                    command.Parameters.Add(new SQLiteParameter("@estado", tarea.Estado));
                    command.Parameters.Add(new SQLiteParameter("@desc", tarea.Descripcion));
                    command.Parameters.Add(new SQLiteParameter("@color", tarea.Color));
                    command.Parameters.Add(new SQLiteParameter("@idUsu", tarea.IdUsuarioAsignado));

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected <= 0)
                    {
                        throw new InvalidOperationException("No se insertaron filas en la base de datos.");
                    }

                    connection.Close();
                }
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine($"Error SQLite al crear tarea: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error general al crear tarea: {ex.Message}");
                throw;
            }
        }

        public List<Tarea> GetAll()
        {
            var queryString = @"SELECT * FROM Tarea;";
            List<Tarea> tareas = new List<Tarea>();
            using (SQLiteConnection connection = new SQLiteConnection(_cadenaConexion))
            {
                SQLiteCommand command = new SQLiteCommand(queryString, connection);
                connection.Open();

                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var id = Convert.ToInt32((reader["Id"]));
                        var id_tablero = reader["Id_tablero"] != DBNull.Value && Convert.ToInt32(reader["Id_tablero"]) != 0 ? Convert.ToInt32(reader["Id_tablero"]) : 0;
                        var nombre = reader["nombre"].ToString();
                        var descripcion = reader["descripcion"].ToString();
                        var color = reader["color"].ToString();
                        var estado = Convert.ToInt32(reader["estado"]);
                        var id_usu_propietario = reader["id_usuario_asignado"] != DBNull.Value && Convert.ToInt32(reader["id_usuario_asignado"]) != 0 ? Convert.ToInt32(reader["id_usuario_asignado"]) : 0; 
                        
                        var tarea = new Tarea(id,id_tablero ,nombre ,descripcion ,color ,estado , id_usu_propietario);   
                        
                        tareas.Add(tarea);
                    }
                }
                connection.Close();
            }
            return tareas;
        }

        public List<TareaViewModel> ObtenerTareasConUsuTablero()
        {
            List<TareaViewModel> tareas = new List<TareaViewModel>();

            try
            {
                var queryString = @"SELECT tar.Id, tar.color, tar.descripcion, tar.nombre, tar.estado,
                            tab.nombre as nombreTab,
                            tab.descripcion as descTab,
                            usu.nombre_de_usuario,
                            usu.rol
                            FROM Tarea tar
                            INNER JOIN Tablero tab on tab.Id = tar.Id_tablero
                            INNER JOIN Usuario usu on usu.id = tar.id_usuario_asignado";    

                using (SQLiteConnection connection = new SQLiteConnection(_cadenaConexion))
                {
                    SQLiteCommand command = new SQLiteCommand(queryString, connection);
                    connection.Open();

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var tareaViewModel = new TareaViewModel
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Nombre = reader["nombre"].ToString(),
                                Descripcion = reader["descripcion"].ToString(),
                                Color = reader["color"].ToString(),
                                Estado = (TP10.Models.EstadoTarea)Convert.ToInt32(reader["estado"]),
                                UsuarioAsignado = reader["nombre_de_usuario"].ToString(),
                                //RolUsu = reader["rol"].ToString(),
                                TableroAsignado = reader["nombreTab"].ToString(),
                                //TableroDesc = reader["descTab"].ToString()
                            };

                            tareas.Add(tareaViewModel);
                        }
                    }

                    connection.Close();
                }
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine($"Error SQLite al obtener tareas : {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error general al obtener tareas : {ex.Message}");
                throw;
            }


            return tareas;
        }


        public Tarea GetById(int id)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(_cadenaConexion))
                {
                    SQLiteCommand command = connection.CreateCommand();
                    command.CommandText = "SELECT * FROM Tarea WHERE id = @idTarea;";
                    command.Parameters.Add(new SQLiteParameter("@idTarea", id));
                    connection.Open();

                    Tarea tarea = null;

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            var id_tablero = Convert.ToInt32(reader["Id_tablero"]);
                            var nombre = reader["nombre"].ToString();
                            var descripcion = reader["descripcion"].ToString();
                            var color = reader["color"].ToString();
                            var estado = Convert.ToInt32(reader["estado"]);
                            var id_usu_propietario = reader["id_usuario_asignado"] != DBNull.Value && Convert.ToInt32(reader["id_usuario_asignado"]) != 0 ? Convert.ToInt32(reader["id_usuario_asignado"]) : 0;

                            tarea = new Tarea(id, id_tablero, nombre, descripcion, color, estado, id_usu_propietario);
                        }
                    }

                    connection.Close();

                    if (tarea == null)
                    {
                        throw new InvalidOperationException($"No se encontró una tarea con el ID {id}.");
                    }

                    return tarea;
                }
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine($"Error SQLite al obtener tarea por ID: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error general al obtener tarea por ID: {ex.Message}");
                throw;
            }
        }

        public List<Tarea> ListarPorTablero(int idTablero)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(_cadenaConexion))
                {
                    SQLiteCommand command = connection.CreateCommand();
                    command.CommandText = @"SELECT * FROM Tarea WHERE Id_tablero = @id;";
                    command.Parameters.Add(new SQLiteParameter("@id", idTablero));
                    connection.Open();

                    List<Tarea> lista = new List<Tarea>();

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var tarea = new Tarea(
                                Convert.ToInt32(reader["Id"]),
                                Convert.ToInt32(reader["Id_tablero"]),
                                reader["nombre"].ToString(),
                                reader["descripcion"].ToString(),
                                reader["color"].ToString(),
                                Convert.ToInt32(reader["estado"]),
                                reader["id_usuario_asignado"] != DBNull.Value && Convert.ToInt32(reader["id_usuario_asignado"]) != 0 ? Convert.ToInt32(reader["id_usuario_asignado"]) : 0
                            );
                            lista.Add(tarea);
                        }
                    }

                    return lista;
                }
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine($"Error SQLite al obtener tareas por tablero: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error general al obtener tareas por tablero: {ex.Message}");
                throw;
            }
        }


        public List<Tarea> ListarPorUsuario(int idUsuario)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(_cadenaConexion))
                {
                    SQLiteCommand command = connection.CreateCommand();
                    command.CommandText = @"select * from Tarea tar
                        inner join Usuario u on u.id=tar.id_usuario_asignado
                        where u.id = @id";
                    command.Parameters.Add(new SQLiteParameter("@id", idUsuario));
                    connection.Open();

                    List<Tarea> lista = new List<Tarea>();

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var tarea = new Tarea(
                                Convert.ToInt32(reader["Id"]),
                                Convert.ToInt32(reader["Id_tablero"]),
                                reader["nombre"].ToString(),
                                reader["descripcion"].ToString(),
                                reader["color"].ToString(),
                                Convert.ToInt32(reader["estado"]),
                                Convert.ToInt32(reader["id_usuario_asignado"])
                            );
                            lista.Add(tarea);
                        }
                    }

                    return lista;
                }
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine($"Error SQLite al obtener tareas por usuario: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error general al obtener tareas por usuario: {ex.Message}");
                throw;
            }
        }

        public void Remove(int id)
        {
            
            SQLiteConnection connection = new SQLiteConnection(_cadenaConexion);
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = $"DELETE FROM Tarea WHERE id = '{id}';";
            try
            {
                connection.Open();
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error al eliminar la tarea de la base de datos.", ex);
            }
            finally
            {
                connection.Close();
            }
        }

        public void Update(Tarea tarea)
        {
           
            SQLiteConnection connection = new SQLiteConnection(_cadenaConexion);

            SQLiteCommand command = connection.CreateCommand();

            int estadoInt = (int)tarea.Estado;

            command.CommandText = $"UPDATE Tarea SET id = '{tarea.Id}', Id_tablero = '{tarea.IdTablero}', nombre = '{tarea.Nombre}', estado = '{estadoInt}', descripcion = '{tarea.Descripcion}', color = '{tarea.Color}', id_usuario_asignado = '{tarea.IdUsuarioAsignado}' WHERE id = '{tarea.Id}';";

            try
            {
                connection.Open();
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ;
            }
            finally
            {
                connection.Close();
            }
        }

        public void AsignarUsuario(int idUsuario, int idTarea)
        {
            using (SQLiteConnection connection = new SQLiteConnection(_cadenaConexion))
            {
                string query = "UPDATE Tarea SET id_usuario_asignado = @idUsuario WHERE Id = @idTarea;";

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@idUsuario", idUsuario);
                    command.Parameters.AddWithValue("@idTarea", idTarea);

                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        throw new ApplicationException("Error al actualizar la tarea en la base de datos.", ex);
                    }
                }
            }
        }

        public void AsignarTablero(int idTab, int idTarea)
        {
            using (SQLiteConnection connection = new SQLiteConnection(_cadenaConexion))
            {
                string query = "UPDATE Tarea SET Id_tablero = @Id_tablero WHERE Id = @idTarea;";

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id_tablero", idTab);
                    command.Parameters.AddWithValue("@idTarea", idTarea);

                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        throw new ApplicationException("Error al actualizar la tarea en la base de datos.", ex);
                    }
                }
            }
        }


    }
}
