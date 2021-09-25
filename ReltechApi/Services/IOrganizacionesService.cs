using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ReltechApi.Models;

namespace ReltechApi.Services
{
    public interface IOrganizacionesService
    {
        bool ExisteOrganizacion(int id);
    }
}
