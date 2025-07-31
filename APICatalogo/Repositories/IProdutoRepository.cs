using APICatalogo.Models;
using APICatalogo.Pagination;
using Microsoft.EntityFrameworkCore.Update.Internal;

namespace APICatalogo.Repositories;

public interface IProdutoRepository : IRepository<Produto>
{
    // IEnumerable<Produto> GetProdutos(ProdutosParameters produtosParams);

    PagedList<Produto> GetProdutos(ProdutosParameters produtosParams);
    IEnumerable<Produto> GetProdutosPorCategoria(int id);
}
