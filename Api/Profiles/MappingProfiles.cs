using Api.Dtos;
using AutoMapper;
using Core.Entities;

namespace Api.Profiles
{
    public class MappingProfiles :Profile
    {
        public MappingProfiles()
        {

            CreateMap<Producto, ProductoDto>()
                .ReverseMap();


            CreateMap<Categoria, CategoriaDto>()
                .ReverseMap();


            CreateMap<Marca, MarcaDto>()
                .ReverseMap();
        }
    }
}
