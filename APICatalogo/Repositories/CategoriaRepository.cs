﻿using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Pagination;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace APICatalogo.Repositories;

public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
{
    public CategoriaRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IPagedList<Categoria>> GetCategoriasAsync(CategoriasParameters categoriasParams)
    {
        var categorias = await GetAllAsync();

        var categoriasOrdenadas = categorias.OrderBy(c => c.Nome).AsQueryable();

        //var resultado = IPagedList<Categoria>.ToPagedList(categoriasOrdenadas,
        //    categoriasParams.PageNumber, categoriasParams.PageSize);

        var resultado = await categoriasOrdenadas.ToPagedListAsync(categoriasParams.PageNumber,
                                                                    categoriasParams.PageSize);

        return resultado;
    }

    public async Task<IPagedList<Categoria>> GetCategoriasFiltroNomeAsync(
        CategoriasFiltroNome categoriasParams)
    {
        var categorias = await GetAllAsync();

        if (!string.IsNullOrEmpty(categoriasParams.Nome))
        {
            categorias = categorias.Where(c => c.Nome.ToLower().Contains(categoriasParams.Nome.ToLower()));
        }

        //var categoriasFiltradas = PagedList<Categoria>.ToPagedList(categorias.AsQueryable(),
        //    categoriasParams.PageNumber, categoriasParams.PageSize);

        var categoriasFiltradas = await categorias.ToPagedListAsync(categoriasParams.PageNumber, 
                                                                    categoriasParams.PageSize);

        return categoriasFiltradas;
    } 
}
