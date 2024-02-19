using Kanban.Models;
using System.IO;
using System.Data.SQLite;
using TP10.Models;
using System.Data;

namespace Kanban.Repositorios
{
    public class UsuarioRepositorio : IUsuarioRepositorio
    {

        private readonly string _cadenaConexion;

        public UsuarioRepositorio(IConfiguration configuration)
        {
            _cadenaConexion = configuration.GetConnectionString("SqliteConexion");
        }

        public void Create(Usuario usuario)
        {
            if (usuario is null)
            {
                throw new ArgumentNullException(nameof(usuario), "El usuario no puede ser nulo.");
            }

            if (usuario.Rol != "Admin" && usuario.Rol != "Operador")
            {
                throw new ArgumentException("El Rol del usuario debe ser Admin u Operador");
            }

            var query = "INSERT INTO Usuario (nombre_de_usuario, contrasenia, rol) VALUES (@nombre_de_usuario, @contra, @rol)";

            using (SQLiteConnection connection = new SQLiteConnection(_cadenaConexion))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.Add(new SQLiteParameter("@nombre_de_usuario", usuario.Nombre));
                    command.Parameters.Add(new SQLiteParameter("@contra", usuario.Contrasenia));
                    command.Parameters.Add(new SQLiteParameter("@rol", usuario.Rol));

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
        }


        public List<Usuario> GetAll()
        {
            var queryString = @"SELECT * FROM Usuario WHERE activo = 1;";
            List<Usuario> usuarios = new List<Usuario>();

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
            SQLiteConnection connection = new SQLiteConnection(_cadenaConexion);
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
            
            if (usuario.Rol != "Admin" && usuario.Rol != "Operador")
            {
                throw new ArgumentException("El Rol del usuario debe ser Admin u Operador");
            }

            SQLiteConnection connection = new SQLiteConnection(_cadenaConexion);

            if (usuario.Id < 0)
            {
                throw new ArgumentException("El ID del usuario no es válido: " + usuario.Id.ToString());
            }

            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = $"UPDATE Usuario SET nombre_de_usuario = '{usuario.Nombre}', contrasenia = '{usuario.Contrasenia}', rol='{usuario.Rol}' WHERE id = '{usuario.Id}';";

            try
            {
                connection.Open();
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw ;
            }
            finally
            {
                connection.Close();
            }
        }
        public void Remove(int id)
        {
            
            SQLiteConnection connection = new SQLiteConnection(_cadenaConexion);

            try
            {
                connection.Open();

                
                using (SQLiteCommand commandUsuario = connection.CreateCommand())
                {
                    commandUsuario.CommandText = $"UPDATE Usuario SET activo = 0 WHERE id = '{id}';";
                    commandUsuario.ExecuteNonQuery();
                }

                using (SQLiteCommand commandTableros = connection.CreateCommand())
                {
                    commandTableros.CommandText = $"UPDATE Tablero SET Id_usuario_propietario = null WHERE Id_usuario_propietario = '{id}';";
                    commandTableros.ExecuteNonQuery();
                }

                using (SQLiteCommand commandTableros = connection.CreateCommand())
                {
                    commandTableros.CommandText = $"UPDATE Tarea SET id_usuario_asignado = null WHERE id_usuario_asignado = '{id}';";
                    commandTableros.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
              
                throw new ApplicationException("Error al eliminar el usuario de la base de datos.", ex);
            }
            finally
            {
                if (connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
            }
        }

    }
}
