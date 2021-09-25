using Microsoft.Extensions.Logging;
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
        private ILogger<Organizaciones> _logger;
        public OrganizacionesService(ReltechCrudContext dbContext, ILogger<Organizaciones> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public bool ExisteOrganizacion(int id)
        {
            bool existe = true;

            try
            {
                var organizacion = _dbContext.Organizaciones.Where(o => o.Id == id).SingleOrDefault();
                if (organizacion == null) existe = false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "{Fecha}: Error al buscar organización \"{Organizacion}\"",
                    DateTime.Now, id);
                throw;
            }

            return existe;
        }
    }
}
