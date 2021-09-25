using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ReltechApi.Models;

namespace ReltechApi.Services
{
    public interface IUsuariosService
    {
        List<Usuarios> GetAllUsuarios();

        Usuarios GetUsuario(int id);

        Usuarios AddUsuario(Usuarios usuario);

        void EditUsuario(Usuarios usuario);

        void DeleteUsuario(Usuarios usuario);

        bool ExisteCedula(string cedula);

        bool ExisteEmail(string email);

        bool ExisteTelefono(string telf);
    }
}
