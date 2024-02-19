using Kanban.Models;
using TP10.ViewModels;

namespace Kanban.Repositorios
{
    public interface ITareaRepositorio
    {
        public void Create(Tarea tarea);
        public List<Tarea> GetAll();
        public Tarea GetById(int id);
        public void Remove(int id);
        public void Update(Tarea tarea);
        public List<Tarea> ListarPorUsuario(int idUsuario);
        public List<Tarea> ListarPorTablero(int idTablero);
        public void AsignarUsuario(int idUsuario,int idTarea);
        public void AsignarTablero(int idTab, int idTarea);
        public List<TareaViewModel> ObtenerTareasConUsuTablero();
    }
}
