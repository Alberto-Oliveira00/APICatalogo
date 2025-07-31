using APICatalogo.Models;
using AutoMapper;

namespace APICatalogo.DTOs.Mappings;

public class CategoriaDTOMappingProfile : Profile
{
    public CategoriaDTOMappingProfile()
    {
        CreateMap<Categoria, CategoriaDTO>().ReverseMap();
    }
}
