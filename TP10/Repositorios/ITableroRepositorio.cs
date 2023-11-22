using Kanban.Models;

namespace Kanban.Repositorios
{
    public interface ITableroRepositorio
    {
        public void Create(Tablero tablero);
        public List<Tablero> GetAll();
        public Tablero GetById(int id);
        public void Remove(int id);
        public void Update(Tablero tablero);
        public List<Tablero> ListarPorUsuario(int idUsuario);
    }
}
