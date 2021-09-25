using ReltechApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReltechApi.Services
{
    public class OrganizacionesService : IOrganizacionesService
    {
        private ReltechCrudContext _dbContext;
        public OrganizacionesService(ReltechCrudContext dbContext)
        {
            _dbContext = dbContext;
        }

        public bool ExisteOrganizacion(int id)
        {
            var organizacion = _dbContext.Organizaciones.Where(o => o.Id == id).SingleOrDefault();
            if (organizacion == null) return false;
            else return true;
        }
    }
}
