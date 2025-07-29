using APICatalogo.Models;
using Microsoft.EntityFrameworkCore.Update.Internal;

namespace APICatalogo.Repositories;

public interface IProdutoRepository : IRepository<Produto>
{
    IEnumerable<Produto> GetProdutosPorCategoria(int id);
}
