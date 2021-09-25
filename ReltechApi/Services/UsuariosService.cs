using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ReltechApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ReltechApi.Services
{
    public class UsuariosService : IUsuariosService
    {
        private ReltechCrudContext _dbContext;
        private ILogger<UsuariosService> _logger;
        public UsuariosService(ReltechCrudContext dbContext, ILogger<UsuariosService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public Usuarios AddUsuario(Usuarios usuario)
        {
            try
            {
                _dbContext.Usuarios.Add(usuario);
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "{Fecha}: Error al insertar un usuario: {Usuario}",
                    DateTime.Now, usuario);
                throw;
            }

            return usuario;
        }

        public void DeleteUsuario(Usuarios usuario)
        {
            try
            {
                _dbContext.Usuarios.Remove(usuario);
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "{Fecha}: Error al eliminar el usuario: {Usuario}",
                    DateTime.Now, usuario);
                throw;
            }
        }

        public void EditUsuario(Usuarios usuario)
        {
            var usuarioExistente = GetUsuario(usuario.Id);

            usuarioExistente.NombreCompleto = usuario.NombreCompleto;
            usuarioExistente.Cedula = usuario.Cedula;
            usuarioExistente.FechaNacimiento = usuario.FechaNacimiento;
            usuarioExistente.CorreoElectronico = usuario.CorreoElectronico;
            usuarioExistente.Telefono = usuario.Telefono;
            usuarioExistente.OrganizacionId = usuario.OrganizacionId;
            
            try
            {
                _dbContext.Usuarios.Update(usuarioExistente);
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "{Fecha}: Error al editar el usuario: {Usuario}",
                    DateTime.Now, usuario);
                throw;
            }
        }

        public Usuarios GetUsuario(int id)
        {
            Usuarios usuario = null;

            try
            {
                usuario = _dbContext.Usuarios.SingleOrDefault(u => u.Id == id);
            } 
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "{Fecha}: Error al obtener los datos para el usuario {Id}",
                    DateTime.Now, id);
                throw;
            }

            return usuario;
        }

        public List<Usuarios> GetAllUsuarios()
        {
            List<Usuarios> usuarios = null;

            try
            {
                usuarios = _dbContext.Usuarios.ToList(); ;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "{Fecha}: Error al obtener todos los usuarios",
                    DateTime.Now);
                throw;
            }

            return usuarios;
        }

        public bool ExisteCedula(string cedula)
        {
            bool existe = true;

            try
            {
                var usuario = _dbContext.Usuarios.Where(u => u.Cedula == cedula).SingleOrDefault();
                if (usuario == null) existe = false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "{Fecha}: Error al buscar cedula \"{Cedula}\"",
                    DateTime.Now, cedula);
                throw;
            }

            return existe;
        }

        public bool ExisteEmail(string email)
        {
            bool existe = true;

            try
            {
                var usuario = _dbContext.Usuarios.Where(u => u.CorreoElectronico == email).SingleOrDefault();
                if (usuario == null) existe = false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "{Fecha}: Error al buscar email \"{Email}\"",
                    DateTime.Now, email);
                throw;
            }

            return existe;
        }

        public bool ExisteTelefono(string telf)
        {
            bool existe = true;

            try
            {
                var usuario = _dbContext.Usuarios.Where(u => u.Telefono == telf).SingleOrDefault();
                if (usuario == null) existe = false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "{Fecha}: Error al buscar telefono \"{Telefono}\"",
                    DateTime.Now, telf);
                throw;
            }

            return existe;
        }
    }
}
