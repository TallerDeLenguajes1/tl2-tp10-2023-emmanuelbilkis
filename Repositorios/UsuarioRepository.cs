using Kanban.Models;
using System.IO;
using System.Data.SQLite;
using TP10.Models;

namespace Kanban.Repositorios
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private string cadenaConexion = "Data Source=DB/Taller2.db;Cache=Shared";

        public void Create(Usuario usuario)
        {
            if (usuario is null)
            {
                throw new ArgumentNullException(nameof(usuario), "El usuario no puede ser nulo.");
            }

            var query = "INSERT INTO Usuario (nombre_de_usuario, contrasenia, rol) VALUES (@nombre_de_usuario, @contra, @rol)";

            using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.Add(new SQLiteParameter("@nombre_de_usuario", usuario.Nombre));
                    command.Parameters.Add(new SQLiteParameter("@contra", usuario.Contrasenia));
                    command.Parameters.Add(new SQLiteParameter("@rol", usuario.Rol));

                    try
                    {
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected <= 0)
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
        }


        public List<Usuario> GetAll()
        {
            var queryString = @"SELECT * FROM Usuario;";
            List<Usuario> usuarios = new List<Usuario>();

            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
                {
                    SQLiteCommand command = new SQLiteCommand(queryString, connection);
                    connection.Open();

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var usuario = new Usuario();
                            usuario.Id = Convert.ToInt32(reader["id"]);
                            usuario.Nombre = reader["nombre_de_usuario"].ToString();
                            usuario.Contrasenia = reader["contrasenia"].ToString();
                            usuario.Rol = reader["rol"].ToString();

                            usuarios.Add(usuario);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error al obtener la lista de usuarios desde la base de datos.", ex);
            }

            return usuarios;
        }

        public Usuario GetById(int idUsuario)
        {
            SQLiteConnection connection = new SQLiteConnection(cadenaConexion);
            Usuario usuario = null;

            using (SQLiteCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM Usuario WHERE id = @idUsuario";
                command.Parameters.Add(new SQLiteParameter("@idUsuario", idUsuario));

                connection.Open();

                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        usuario = new Usuario
                        {
                            Id = Convert.ToInt32(reader["id"]),
                            Nombre = reader["nombre_de_usuario"].ToString(),
                            Contrasenia = reader["contrasenia"].ToString(),
                            Rol = reader["rol"].ToString()
                        };
                    }
                }
            }

            connection.Close();

            if (usuario == null)
            {
                throw new InvalidOperationException($"No se encontró un usuario con el ID {idUsuario}.");
            }

            return usuario;
        }


        public void Update(Usuario usuario)
        {
            if (usuario is null)
            {
                throw new ArgumentNullException(nameof(usuario), "El usuario no puede ser nulo.");
            }

            SQLiteConnection connection = new SQLiteConnection(cadenaConexion);

            if (usuario.Id <= 0)
            {
                throw new ArgumentException("El ID del usuario no es válido.", nameof(usuario));
            }

            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = $"UPDATE Usuario SET nombre_de_usuario = '{usuario.Nombre}', contrasenia = '{usuario.Contrasenia}', rol='{usuario.Rol}' WHERE id = '{usuario.Id}';";

            try
            {
                connection.Open();
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error al actualizar el usuario en la base de datos.", ex);
            }
            finally
            {
                connection.Close();
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
            command.CommandText = $"DELETE FROM Usuario WHERE id = '{id}';";

            try
            {
                connection.Open();
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error al eliminar el usuario de la base de datos.", ex);
            }
            finally
            {
                connection.Close();
            }
        }

    }
}
