namespace TP10.ViewModels
{
    public class CrearTareaViewModel
    {
        public CrearTareaViewModel(int id, int idTablero, string nombre, string descripcion, string color, int estado, int idUsuarioAsignado)
        {
            Id = id;
            IdTablero = idTablero;
            Nombre = nombre;
            Descripcion = descripcion;
            Color = color;
            Estado = estado;
            IdUsuarioAsignado = idUsuarioAsignado;
        }

        public CrearTareaViewModel()
        {
           
        }

        public int Id { get; set; }
        public int IdTablero { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Color { get; set; }
        public int Estado { get; set; }
        public int IdUsuarioAsignado { get; set; }

    }
}
