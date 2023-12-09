using Kanban.Models;
using System.Data.SQLite;

namespace Kanban.Repositorios
{
    public class TableroRepositorio : ITableroRepositorio
    {
        private string cadenaConexion = "Data Source=DB/Taller2.db;Cache=Shared";

        public void Create(Tablero tablero)
        {
            var query = $"INSERT INTO Tablero (Id_usuario_propietario,nombre,descripcion) VALUES (@id_usu,@nombre,@desc)";
            using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
            {

                connection.Open();
                var command = new SQLiteCommand(query, connection);

                command.Parameters.Add(new SQLiteParameter("@id_usu", tablero.IdUsuarioPropietario));
                command.Parameters.Add(new SQLiteParameter("@nombre", tablero.Nombre));
                command.Parameters.Add(new SQLiteParameter("@desc", tablero.Descripcion));

                command.ExecuteNonQuery();

                connection.Close();
            }
        }

        public List<Tablero> GetAll()
        {
                var queryString = @"SELECT * FROM Tablero;";
                List<Tablero> tableros = new List<Tablero>();
                using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
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
                        var tablero = new Tablero(id,id_usu_propietario ,nombre ,descripcion);
                        
                        tableros.Add(tablero);
                        
                      }
                    }
                    connection.Close();
                }

                return tableros;
        }

        public Tablero GetById(int idTablero)
        {
            SQLiteConnection connection = new SQLiteConnection(cadenaConexion);
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM Tablero WHERE id = @idTab;";
            command.Parameters.Add(new SQLiteParameter("@idTab", idTablero));
            connection.Open();
            var tab = new Tablero();
            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var tablero = new Tablero(Convert.ToInt32(reader["Id"]), Convert.ToInt32(reader["Id_usuario_propietario"]), reader["nombre"].ToString(), reader["descripcion"].ToString());
                    tab = tablero;
                }
            }
            connection.Close();

            return (tab);
        }

        public List<Tablero> ListarPorUsuario(int idUsuario)
        {
            SQLiteConnection connection = new SQLiteConnection(cadenaConexion);
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = @"SELECT * FROM Tablero WHERE Id_usuario_propietario = @idUsu;";
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

            return (lista);

        }
        
        public void Remove(int id)
        {
            SQLiteConnection connection = new SQLiteConnection(cadenaConexion);
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = $"DELETE FROM Tablero WHERE id = '{id}';";
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }

        public void Update(Tablero tablero)
        {
            SQLiteConnection connection = new SQLiteConnection(cadenaConexion);
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = $"UPDATE Tablero SET Id = '{tablero.Id}', Id_usuario_propietario = '{tablero.IdUsuarioPropietario}',nombre='{tablero.Nombre}',descripcion='{tablero.Descripcion}' WHERE id = '{tablero.Id}';";
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }
    }
}
