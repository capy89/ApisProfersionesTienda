
using Api.Dtos;
using Api.Helpers;
using Api.Helpers.Errors;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
    [ApiVersion("1.0")]
    [ApiVersion("1.1")]
    [Authorize(Roles = "Administrador")]
    public class ProductosController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;

        public readonly IMapper _automaper;

        public ProductosController(IUnitOfWork unitOfWork, IMapper automaper)
        {
            _unitOfWork = unitOfWork;
            _automaper = automaper;
        }


        //[HttpGet]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //public async Task<ActionResult<IEnumerable<ProductoListDto>>> Get()
        //{
        //    var productos = await _unitOfWork.Productos.GetAllAsync();



        //    return _automaper.Map<List<ProductoListDto>>(productos);


        //}

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Pager<ProductoListDto>>> Get([FromQuery] Params productParams)
        {
            var result = await _unitOfWork.Productos.GetAllAsync(productParams.PageIndex,productParams.PageSize,productParams.Search);


            var listProducts = _automaper.Map<List<ProductoListDto>>(result.registros);

            //Esto es para que en el encabezado aparesca información de la paginación, se puede agregar más información
            Response.Headers.Add("X-InLineCount", result.totalRegistros.ToString());

            return new Pager<ProductoListDto>(listProducts,result.totalRegistros,
                productParams.PageIndex,productParams.PageSize,productParams.Search);
        }

        [HttpGet]
        [MapToApiVersion("1.1")]
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
                return NotFound(new ApiResponse(404,"El producto solicitado no existe") );
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
                return BadRequest(new ApiResponse(400));
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
                return NotFound(new ApiResponse(404, "El producto solicitado no existe"));

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
                return NotFound(new ApiResponse(404, "El producto solicitado no existe"));

            _unitOfWork.Productos.Remove(producto);
            await _unitOfWork.SaveAsync();


            return NoContent();

        }

    }
}
