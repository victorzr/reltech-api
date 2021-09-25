using Microsoft.EntityFrameworkCore;
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
        public UsuariosService(ReltechCrudContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Usuarios AddUsuario(Usuarios usuario)
        {
            _dbContext.Usuarios.Add(usuario);
            _dbContext.SaveChanges();
            return usuario;
        }

        public void DeleteUsuario(Usuarios usuario)
        {
            _dbContext.Usuarios.Remove(usuario);
            _dbContext.SaveChanges();
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
            
            _dbContext.Usuarios.Update(usuarioExistente);
            _dbContext.SaveChanges();
        }

        public Usuarios GetUsuario(int id)
        {
            return _dbContext.Usuarios.SingleOrDefault(u => u.Id == id);
        }

        public List<Usuarios> GetAllUsuarios()
        {
            return _dbContext.Usuarios.ToList();
        }

        public bool ExisteCedula(string cedula)
        {
            var usuario = _dbContext.Usuarios.Where(u => u.Cedula == cedula).SingleOrDefault();
            if (usuario == null) return false;
            else return true;
        }

        public bool ExisteEmail(string email)
        {
            var usuario = _dbContext.Usuarios.Where(u => u.CorreoElectronico == email).SingleOrDefault();
            if (usuario == null) return false;
            else return true;
        }

        public bool ExisteTelefono(string telf)
        {
            var usuario = _dbContext.Usuarios.Where(u => u.Telefono == telf).SingleOrDefault();
            if (usuario == null) return false;
            else return true;
        }
    }
}
