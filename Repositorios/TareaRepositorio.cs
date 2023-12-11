using Kanban.Models;
using System.Data.SQLite;
using System.IO;

namespace Kanban.Repositorios
{
    public class TareaRepositorio : ITareaRepositorio
    {
        private string cadenaConexion = "Data Source=DB/Taller2.db;Cache=Shared";

        public void Create(Tarea tarea)
        {
            try
            {
                var query = "INSERT INTO Tarea (Id_tablero, nombre, estado, descripcion, color, id_usuario_asignado) VALUES (@idTab, @nombre, @estado, @desc, @color, @idUsu)";
                using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
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
            using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
            {
                SQLiteCommand command = new SQLiteCommand(queryString, connection);
                connection.Open();

                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var id = Convert.ToInt32((reader["Id"]));
                        var id_tablero = Convert.ToInt32((reader["Id_tablero"]));
                        var nombre = reader["nombre"].ToString();
                        var descripcion = reader["descripcion"].ToString();
                        var color = reader["color"].ToString();
                        var estado = Convert.ToInt32(reader["estado"]);
                        var id_usu_asignado = Convert.ToInt32(reader["id_usuario_asignado"]);
                        
                        var tarea = new Tarea(id,id_tablero ,nombre ,descripcion ,color ,estado , id_usu_asignado);   
                        
                        tareas.Add(tarea);
                    }
                }
                connection.Close();
            }
            return tareas;
        }

        public Tarea GetById(int id)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
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
                            var id_usu_asignado = Convert.ToInt32(reader["id_usuario_asignado"]);

                            tarea = new Tarea(id, id_tablero, nombre, descripcion, color, estado, id_usu_asignado);
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
                using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
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
                                Convert.ToInt32(reader["id_usuario_asignado"])
                            );
                            lista.Add(tarea);
                        }
                    }

                    
                    if (lista.Count == 0)
                    {
                        throw new InvalidOperationException("La consulta no devolvió resultados para el tablero especificado.");
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
                using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
                {
                    SQLiteCommand command = connection.CreateCommand();
                    command.CommandText = @"SELECT * FROM Tarea WHERE id_usuario_asignado = @id;";
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

                    if (lista.Count == 0)
                    {
                        throw new InvalidOperationException($"La consulta no devolvió tareas para el usuario con ID {idUsuario}.");
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
            if (id <= 0)
            {
                throw new ArgumentException("El ID del usuario no es válido.", nameof(id));
            }

            SQLiteConnection connection = new SQLiteConnection(cadenaConexion);
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
            if (tarea is null)
            {
                throw new ArgumentNullException(nameof(tarea), "La tarea no puede ser nulo.");
            }

            SQLiteConnection connection = new SQLiteConnection(cadenaConexion);

            if (tarea.Id <= 0)
            {
                throw new ArgumentException("El ID del usuario no es válido.", nameof(tarea));
            }

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
                throw new ApplicationException("Error al actualizar la tarea en la base de datos.", ex);
            }
            finally
            {
                connection.Close();
            }
        }


        public void Asignar(int idUsuario, int idTarea)
        {
            throw new NotImplementedException();
        }
    }
}
