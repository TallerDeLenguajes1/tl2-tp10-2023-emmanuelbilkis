using Kanban.Models;

namespace TP10.Models
{
    public class TableroConUsuario
    {
        public int IdTablero { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string UsuarioNombre { get; set; }

        public TableroConUsuario(int IdTablero,string nombreTablero, string descripcionTablero, string nombreUsu)
        {
            this.IdTablero = IdTablero;
            Nombre = nombreTablero;
            Descripcion = descripcionTablero;
            UsuarioNombre = nombreUsu;
        }

        public TableroConUsuario(int IDTablero, string nombreTablero,string descripcionTablero, string nombreUsu,string rol,int id_usu) 
        {
            IdTablero = IDTablero;
            Nombre = nombreTablero;
            Descripcion = descripcionTablero;
            UsuarioNombre = nombreUsu;
        }

        public TableroConUsuario() { }
    }
}
