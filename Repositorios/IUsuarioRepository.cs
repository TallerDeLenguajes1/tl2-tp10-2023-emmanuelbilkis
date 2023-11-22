using Kanban.Models;
using System.IO;

namespace Kanban.Repositorios
{
    public interface IUsuarioRepository
    {
        public void Create(Usuario usuario);
        public List<Usuario> GetAll();
        public Usuario GetById(int id);
        public void Remove(int id);
        public void Update(Usuario usuario);
    }
}
