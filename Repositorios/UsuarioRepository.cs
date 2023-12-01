﻿using Kanban.Models;
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
            var query = $"INSERT INTO Usuario (nombre_de_usuario,contrasenia,id_rol) VALUES (@nombre_de_usuario,@contra,@id_rol)";
            using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
            {
                connection.Open();
                var command = new SQLiteCommand(query, connection);

                command.Parameters.Add(new SQLiteParameter("@nombre_de_usuario", usuario.Nombre));
                command.Parameters.Add(new SQLiteParameter("@contra", usuario.Contrasenia));
                command.Parameters.Add(new SQLiteParameter("@id_rol", usuario.IdRol));

                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public List<Usuario> GetAll()
        {
                var queryString = @"SELECT * FROM Usuario;";
                List<Usuario> usuarios = new List<Usuario>();
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
                            var v = reader["id_rol"].ToString();
                            usuario.IdRol = int.Parse(v); 


                        usuarios.Add(usuario);
                        }
                    }
                    connection.Close();
                }

                return usuarios;
        }

        public Usuario GetById(int idUsuario)
        {
            SQLiteConnection connection = new SQLiteConnection(cadenaConexion);
            var usuario = new Usuario();
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM Usuario WHERE id = @idUsuario";
            command.Parameters.Add(new SQLiteParameter("@idUsuario", idUsuario));
            connection.Open();

            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    usuario.Id = Convert.ToInt32(reader["id"]);
                    usuario.Nombre = reader["nombre_de_usuario"].ToString();
                }
            }
            connection.Close();

            return (usuario);
        }

        public void Update(Usuario usuario)
        {
            SQLiteConnection connection = new SQLiteConnection(cadenaConexion);
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = $"UPDATE Usuario SET nombre_de_usuario = '{usuario.Nombre}',contrasenia = '{usuario.Contrasenia}',id_rol='{usuario.IdRol}' WHERE id = '{usuario.Id}';";
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }

        public void Remove(int id)
        {
            SQLiteConnection connection = new SQLiteConnection(cadenaConexion);
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = $"DELETE FROM Usuario WHERE id = '{id}';";
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }

        Rol IUsuarioRepository.GetRolById(int id)
        {
            SQLiteConnection connection = new SQLiteConnection(cadenaConexion);
            var rol = new Rol();
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM Roles WHERE Id = @idRol";
            command.Parameters.Add(new SQLiteParameter("@idRol", id));
            connection.Open();

            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    rol.Id = Convert.ToInt32(reader["Id"]);
                    rol.NombreRol = reader["Rol"].ToString();
                }
            }
            connection.Close();

            return (rol);
        }
    }
}
