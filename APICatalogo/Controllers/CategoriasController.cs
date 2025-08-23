using APICatalogo.Context;
using APICatalogo.DTOs;
using APICatalogo.Models;
using APICatalogo.Pagination;
using APICatalogo.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using X.PagedList;

namespace APICatalogo.Controllers;

[Route("[controller]")]
[ApiController]
[EnableRateLimiting("fixedwindow")]
public class CategoriasController : ControllerBase
{
    private readonly IUnitOfWork _uof;
    private readonly IMapper _mapper;

    private readonly IMemoryCache _cache;
    private const string CacheCategoriasKey = "CacheCategorias";

    public CategoriasController(IUnitOfWork uof, IMapper mapper, IMemoryCache cache)
    {
        _uof = uof;
        _mapper = mapper;
        _cache = cache;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoriaDTO>>> GetAsync()
    {
        if(!_cache.TryGetValue(CacheCategoriasKey, out IEnumerable<CategoriaDTO>? categoriasDto))
        {
            var categorias = await _uof.CategoriaRepository.GetAllAsync();

            if (categorias is null)
                return NotFound();

            categoriasDto = _mapper.Map<IEnumerable<CategoriaDTO>>(categorias);

            var cacheOptions = new MemoryCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30),
                SlidingExpiration = TimeSpan.FromSeconds(15),
                Priority = CacheItemPriority.High
            };
            _cache.Set(CacheCategoriasKey, categoriasDto, cacheOptions);
        }
        
        return Ok(categoriasDto);
    }

    [HttpGet("Pagination")]
    public async Task<ActionResult<IEnumerable<CategoriaDTO>>> Get([FromQuery]
                        CategoriasParameters categoriasParameters)
    {
        var categorias = await _uof.CategoriaRepository.GetCategoriasAsync(categoriasParameters);
        return ObterCategorias(categorias);
    }

    private ActionResult<IEnumerable<CategoriaDTO>> ObterCategorias(IPagedList<Categoria> categorias)
    {
        var metadata = new
        {
            categorias.Count,
            categorias.PageSize,
            categorias.PageCount,
            categorias.TotalItemCount,
            categorias.HasNextPage,
            categorias.HasPreviousPage
        };

        Response.Headers.Append("X-Pagination",
            JsonConvert.SerializeObject(metadata));

        var categoriasDTO = _mapper.Map<IEnumerable<CategoriaDTO>>(categorias);

        return Ok(categoriasDTO);
    }

    [HttpGet("filter/nome/pagination")]
    public async Task<ActionResult<IEnumerable<CategoriaDTO>>> GetCategoriasFiltradasAsync(
                                    [FromQuery] CategoriasFiltroNome categoriasFiltro)
    {
        var categoriasFiltradas = await _uof.CategoriaRepository.GetCategoriasFiltroNomeAsync(categoriasFiltro);

        return ObterCategorias(categoriasFiltradas);
    }

    [HttpGet("{id:int}", Name = "ObterCategoria")]
    public async Task<ActionResult<CategoriaDTO>> GetAsync(int id)
    {
        var categoria = await _uof.CategoriaRepository.GetAsync(c => c.CategoriaId == id);

        if (categoria is null)
        {
            return NotFound($"Categoria com id= {id} não encontrada...");
        }

        var categoriaDto = _mapper.Map<CategoriaDTO>(categoria);

        return Ok(categoriaDto);
    }

    [HttpPost]
    public async Task<ActionResult<CategoriaDTO>> Post(CategoriaDTO categoriaDto)
    {
        if (categoriaDto is null)
            return BadRequest("Categoria inválida...");

        var categoria = _mapper.Map<Categoria>(categoriaDto);

        var categoriaCriada = _uof.CategoriaRepository.Create(categoria);
        await _uof.CommitAsync();

        var novoProdutoDto = _mapper.Map<CategoriaDTO>(categoriaCriada);

        return new CreatedAtRouteResult("ObterCategoria",
            new { id = categoriaCriada.CategoriaId }, novoProdutoDto);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<CategoriaDTO>> Put(int id, CategoriaDTO categoriaDto)
    {
        if (id != categoriaDto.CategoriaId)
            return BadRequest($"Categoria com id= {id} não encontrada...");
        
        var categoria = _mapper.Map<Categoria>(categoriaDto);

        var categoriaAtualizada = _uof.CategoriaRepository.Update(categoria);
        await _uof.CommitAsync();

        var categoriaAtualizadaDto = _mapper.Map<CategoriaDTO>(categoriaAtualizada);

        return Ok(categoriaAtualizadaDto);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Policy ="AdminOnly")]
    public async Task<ActionResult<CategoriaDTO>> Delete(int id)
    {
        var categoria = await _uof.CategoriaRepository.GetAsync(c => c.CategoriaId == id);

        if (categoria is null)
        {
            return NotFound($"Categoria com id= {id} não encontrada...");
        }

        var categoriaExcluida = _uof.CategoriaRepository.Delete(categoria);
        await _uof.CommitAsync();

        var categoriaDeletadaDto = _mapper.Map<CategoriaDTO>(categoriaExcluida);

        return Ok(categoriaDeletadaDto);
    }
}
