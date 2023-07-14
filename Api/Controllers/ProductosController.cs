
using Api.Dtos;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
    public class ProductosController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;

        public readonly IMapper _automaper;

        public ProductosController(IUnitOfWork unitOfWork, IMapper automaper)
        {
            _unitOfWork = unitOfWork;
            _automaper = automaper;
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<Producto>>> Get()
        {
            var productos = await _unitOfWork.Productos.GetAllAsync();

            return Ok(productos);


        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductoDto>> Get(int id)
        {
            var producto = await _unitOfWork.Productos.GetByIdAsync(id);

            if(producto==null)
            {
                return NotFound();
            }

            return _automaper.Map<ProductoDto>(producto);


        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Producto>> Post(Producto producto)
        {
            _unitOfWork.Productos.Add(producto);
            _unitOfWork.Save();

            if (producto == null)
            {
                return BadRequest();
            }

            return CreatedAtAction(nameof(Post),new { id = producto.Id }, producto);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Producto>> Put(int id, [FromBody]Producto producto)
        {

            if(producto == null)
                return NotFound();

            _unitOfWork.Productos.Update(producto);
            _unitOfWork.Save();


            return producto;

        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Delete(int id)
        {

            var producto=await _unitOfWork.Productos.GetByIdAsync(id);

            if (producto == null)
                return NotFound();

            _unitOfWork.Productos.Remove(producto);
            _unitOfWork.Save();


            return NoContent();

        }

    }
}
