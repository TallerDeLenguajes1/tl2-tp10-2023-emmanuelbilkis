namespace Kanban.Models
{
    public class Tablero
    {
        public Tablero(int id, int idUsuarioPropietario, string nombre, string descripcion)
        {
            Id = id;
            IdUsuarioPropietario = idUsuarioPropietario;
            Nombre = nombre;
            Descripcion = descripcion;
        }

        public Tablero() { }
        public int Id { get; set; }
        public int IdUsuarioPropietario { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
    }
}
