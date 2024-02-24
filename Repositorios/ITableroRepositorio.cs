using Kanban.Models;
using TP10.Models;
using TP10.ViewModels;

namespace Kanban.Repositorios
{
    public interface ITableroRepositorio
    {
        public void Create(Tablero tablero);
        public List<Tablero> GetAll();
        public Tablero GetById(int id);
        public void Remove(int id);
        public void Update(Tablero tablero);
        public List<TableroConUsuario> ListarPorUsuario(int idUsuario);
        public List<TableroConUsuario> ObtenerTablerosConUsuario();
        public List<TableroConUsuario> ObtenerTablerosConUsuario(int idTab);
        public List<TableroConUsuario> ObtenerTablerosConUsuario(int idUsuario,int idTab);
        public List<Tablero> ListarTablerosPropiosYConTareas(int idUsuario);
    }
}
