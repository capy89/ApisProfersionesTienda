
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
        [ApiVersion("1.0")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<ProductoListDto>>> Get()
        {
            var productos = await _unitOfWork.Productos.GetAllAsync();



            return _automaper.Map<List<ProductoListDto>>(productos);


        }

        [HttpGet]
        [ApiVersion("1.1")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<ProductoDto>>> Get11()
        {
            var productos = await _unitOfWork.Productos.GetAllAsync();



            return _automaper.Map<List<ProductoDto>>(productos);


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
        public async Task<ActionResult<Producto>> Post(ProductoAddUpdateDto productoDto)
        {

            var producto = _automaper.Map<Producto>(productoDto);

            _unitOfWork.Productos.Add(producto);
            await _unitOfWork.SaveAsync();

            if (producto == null)
            {
                return BadRequest();
            }

            productoDto.Id=producto.Id;
            return CreatedAtAction(nameof(Post),new { id = productoDto.Id }, productoDto);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ProductoAddUpdateDto>> Put(int id, [FromBody] ProductoAddUpdateDto productoDto)
        {

            if (productoDto == null)
                return NotFound();

            var producto = _automaper.Map<Producto>(productoDto);

            _unitOfWork.Productos.Update(producto);
            await _unitOfWork.SaveAsync();


            return productoDto;

        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Delete(int id)
        {

            var producto = await _unitOfWork.Productos.GetByIdAsync(id);

            if (producto == null)
                return NotFound();

            _unitOfWork.Productos.Remove(producto);
            await _unitOfWork.SaveAsync();


            return NoContent();

        }

    }
}
