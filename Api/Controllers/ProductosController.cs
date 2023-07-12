
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
    public class ProductosController : BaseApiController
    {
        public IProductoRepository _productoRepository { get; }
        
        public ProductosController(IProductoRepository productoRepository)
        {
            _productoRepository = productoRepository;
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<Producto>>> Get()
        {
            var productos = await _productoRepository.GetAllAsync();

            return Ok(productos);


        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Get(int id)
        {
            var producto = await _productoRepository.GetByIdAsync(id);

            return Ok(producto);


        }

    }
}
