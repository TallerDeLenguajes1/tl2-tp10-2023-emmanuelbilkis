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
            var query = $"INSERT INTO Tarea (Id_tablero, nombre,estado,descripcion,color,id_usuario_asignado) VALUES (@idTab, @nombre,@estado,@desc,@color,@idUsu)";
            using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
            {
                connection.Open();
                var command = new SQLiteCommand(query, connection);

                command.Parameters.Add(new SQLiteParameter("@idTab", tarea.IdTablero));
                command.Parameters.Add(new SQLiteParameter("@nombre",tarea.Nombre));
                command.Parameters.Add(new SQLiteParameter("@estado", tarea.Estado));
                command.Parameters.Add(new SQLiteParameter("@desc", tarea.Descripcion));
                command.Parameters.Add(new SQLiteParameter("@color", tarea.Color));
                command.Parameters.Add(new SQLiteParameter("@idUsu", tarea.IdUsuarioAsignado));
                command.ExecuteNonQuery();

                connection.Close();
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
                        var tarea = new Tarea(Convert.ToInt32((reader["Id"])), Convert.ToInt32((reader["Id_tablero"])), reader["nombre"].ToString(), reader["descripcion"].ToString(), reader["color"].ToString(), Convert.ToInt32(reader["estado"]), Convert.ToInt32(reader["id_usuario_asignado"]));                       
                        tareas.Add(tarea);
                    }
                }
                connection.Close();
            }
            return tareas;
        }

        public Tarea GetById(int id)
        {
            SQLiteConnection connection = new SQLiteConnection(cadenaConexion);
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM Tarea WHERE id = @idTarea;";
            command.Parameters.Add(new SQLiteParameter("@idTarea", id));
            connection.Open();
            var tar = new Tarea();
            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var tarea = new Tarea(Convert.ToInt32((reader["Id"])), Convert.ToInt32((reader["Id_tablero"])), reader["nombre"].ToString(), reader["descripcion"].ToString(), reader["color"].ToString(), Convert.ToInt32(reader["estado"]), Convert.ToInt32(reader["id_usuario_asignado"]));
                    tar = tarea;
                }
            }
            connection.Close();

            return (tar);
        }

        public List<Tarea> ListarPorTablero(int idTablero)
        {
            SQLiteConnection connection = new SQLiteConnection(cadenaConexion);
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = @"SELECT * FROM Tarea WHERE Id_tablero = @id;";
            command.Parameters.Add(new SQLiteParameter("@id", idTablero));
            connection.Open();
            var lista = new List<Tarea>();
            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var tarea = new Tarea(Convert.ToInt32((reader["Id"])), Convert.ToInt32((reader["Id_tablero"])), reader["nombre"].ToString(), reader["descripcion"].ToString(), reader["color"].ToString(), Convert.ToInt32(reader["estado"]), Convert.ToInt32(reader["id_usuario_asignado"]));
                    lista.Add(tarea);
                }
            }
            connection.Close();

            return (lista);
        }

        public List<Tarea> ListarPorUsuario(int idUsuario)
        {
            SQLiteConnection connection = new SQLiteConnection(cadenaConexion);
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = @"SELECT * FROM Tarea WHERE id_usuario_asignado = @id;";
            command.Parameters.Add(new SQLiteParameter("@id", idUsuario));
            connection.Open();
            var lista = new List<Tarea>();
            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var tarea = new Tarea(Convert.ToInt32((reader["Id"])), Convert.ToInt32((reader["Id_tablero"])), reader["nombre"].ToString(), reader["descripcion"].ToString(), reader["color"].ToString(), Convert.ToInt32(reader["estado"]), Convert.ToInt32(reader["id_usuario_asignado"]));
                    lista.Add(tarea);
                }
            }
            connection.Close();

            return (lista);
        }

        public void Remove(int id)
        {
            SQLiteConnection connection = new SQLiteConnection(cadenaConexion);
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = $"DELETE FROM Tarea WHERE id = '{id}';";
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }

        public void Update(Tarea tarea)
        {
            SQLiteConnection connection = new SQLiteConnection(cadenaConexion);
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = $"UPDATE Tarea SET id = '{tarea.Id}', Id_tablero = '{tarea.IdTablero}', nombre = '{tarea.Nombre}', estado = '{tarea.Estado}', descripcion = '{tarea.Descripcion}', color = '{tarea.Color}', id_usuario_asignado = '{tarea.IdUsuarioAsignado}' WHERE id = '{tarea.Id}';";
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }

        public void Asignar(int idUsuario, int idTarea)
        {
            throw new NotImplementedException();
        }
    }
}
