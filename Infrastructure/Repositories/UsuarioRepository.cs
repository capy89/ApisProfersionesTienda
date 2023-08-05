using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class UsuarioRepository : GenericRepository<Usuario>, IUsuarioRepository
    {
        public UsuarioRepository(TiendaContext context) : base(context)
        {
            
        }

        public async Task<Usuario> GetByRefreshToken(string refreshToken)
        {
            return await _context.Usuarios
               .Include(u => u.Roles)
               .Include(u => u.RefreshToken)
               .FirstOrDefaultAsync(u => u.RefreshToken.Any(t=> t.Token==refreshToken));
        }

        public async Task<Usuario> GetByUserName(string userName)
        {
            return await _context.Usuarios
                .Include(u => u.Roles)
                .Include(u => u.RefreshToken)
                .FirstOrDefaultAsync(u => u.Username.ToLower()==userName.ToLower());
        }
    }
}
