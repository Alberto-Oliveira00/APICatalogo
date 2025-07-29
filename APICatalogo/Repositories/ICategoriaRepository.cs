using APICatalogo.Models;
using System.Collections;
using System.Runtime.InteropServices;

namespace APICatalogo.Repositories
{
    public interface ICategoriaRepository
    {
        IEnumerable<Categoria> GetCategoria();

        Categoria GetCategoria(int id);
        Categoria Create (Categoria categoria);
        Categoria Update(Categoria categoria);
        Categoria Delete(int id);
    }
}
