using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class ProductoRepository : GenericRepository<Producto>, IProductoRepository
    {
        public ProductoRepository(TiendaContext context) :base(context)
        {
            
        }

        public async Task<IEnumerable<Producto>> GetProductosMasCaros(int cantidad) =>
            await _context.Productos
            .OrderByDescending(p => p.Precio)
            .Take(cantidad)
            .ToListAsync();
    }
}
