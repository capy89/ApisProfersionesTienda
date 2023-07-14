using Core.Entities;

namespace Api.Dtos
{
    public class ProductoDto
    {
        public string Nombre { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public DateTime FechaCreacion { get; set; }

        public int MarcaId { get; set; }


        public int CategoriaId { get; set; }

        public MarcaDto Marca { get; set; }
        public CategoriaDto Categoria { get; set; }
    }
}
