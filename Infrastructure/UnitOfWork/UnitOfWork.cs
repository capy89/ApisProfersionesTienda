﻿using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        


        private TiendaContext _context { get; }

        private IProductoRepository _productos;
        private IMarcaRepository _marcas;
        private ICategoriaRepository _categorias;
        private IUsuarioRepository _usuarios;
        private IRolRepository _roles;

        public UnitOfWork(TiendaContext context)
        {
            _context = context;
        }

        public ICategoriaRepository Categorias
        {
            get
            {
                if(_categorias == null)
                {
                    _categorias = new CategoriaRepository(_context);
                }

                return _categorias;
            }
        }

        public IMarcaRepository Marcas
        {
            get
            {
                if (_marcas == null)
                {
                    _marcas = new MarcaRepository(_context);
                }

                return _marcas;
            }
        }

        public IProductoRepository Productos
        {
            get
            {
                if (_productos == null)
                {
                    _productos = new ProductoRepository(_context);
                }

                return _productos;
            }
        }

        public IUsuarioRepository Usuarios
        {
            get
            {
                if (_usuarios == null)
                {
                    _usuarios = new UsuarioRepository(_context);
                }

                return _usuarios;
            }
        }

        public IRolRepository Roles
        {
            get
            {
                if (_roles == null)
                {
                    _roles = new RolRepository(_context);
                }

                return _roles;
            }
        }

        public async Task<int> SaveAsync()
        {
            return await  _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
