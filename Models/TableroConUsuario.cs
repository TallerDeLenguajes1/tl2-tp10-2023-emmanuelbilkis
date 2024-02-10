namespace TP10.Models
{
    public class TableroConUsuario
    {
        public int IdTablero { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string UsuarioNombre { get; set; }
        public string UsuarioRol { get; set; }

        public TableroConUsuario(int IdTablero,string nombreTablero, string descripcionTablero, string nombreUsu, string rolUsu)
        {
            this.IdTablero = IdTablero;
            Nombre = nombreTablero;
            Descripcion = descripcionTablero;
            UsuarioNombre = nombreUsu;
            UsuarioRol = rolUsu;
        }

        public TableroConUsuario() { }
    }
}
