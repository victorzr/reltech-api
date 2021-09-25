using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ReltechApi.Services;
using System.Text.Json;
using ReltechApi.Models;
using ReltechApi.CustomValidation;

namespace ReltechApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private IUsuariosService _usuariosService;
        private IOrganizacionesService _organizacionesService;

        public UsuariosController(IUsuariosService usuariosService, IOrganizacionesService organizacionesService)
        {
            _usuariosService = usuariosService;
            _organizacionesService = organizacionesService;
        }

        [HttpGet]
        public IActionResult GetAllUsuarios()
        {
            try
            {
                var lstUsuarios = _usuariosService.GetAllUsuarios();
                return Ok(lstUsuarios);
            }
            catch
            {
                return ErrorInterno();
            }
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult GetUsuario([FromRoute] int id)
        {
            try
            {
                var usuario = _usuariosService.GetUsuario(id);
                if (usuario != null)
                {
                    return Ok(usuario);
                }

                return Problem("Usuario no encontrado");
            }
            catch
            {
                return ErrorInterno();
            }
        }

        [HttpPost]
        public IActionResult AddUsuario([FromBody] Usuarios usuario)
        {
            try
            {
                if (!ModelState.IsValid) return ValidationProblem();
                else if (!_organizacionesService.ExisteOrganizacion((int)usuario.OrganizacionId)) return Problem("Organización no encontrada");
                else
                {
                    string validationResult = ValidarDatosUnicos(usuario);
                    if (validationResult != null) return ValidationProblem(validationResult);
                }

                _usuariosService.AddUsuario(usuario);
                return Created(String.Format("{0}://{1}{2}/{3}",
                        HttpContext.Request.Scheme,
                        HttpContext.Request.Host,
                        HttpContext.Request.Path,
                        usuario.Id
                    ), usuario);
            }
            catch
            {
                return ErrorInterno();
            }
        }

        [HttpPatch]
        [Route("{id}")]
        public IActionResult EditUsuario([FromRoute] int id, [FromBody] Usuarios usuarioEditado)
        {
            try
            {
                if (!ModelState.IsValid) return ValidationProblem();
                else if (!_organizacionesService.ExisteOrganizacion((int)usuarioEditado.OrganizacionId)) return Problem("Organización no encontrada");

                var usuarioExistente = _usuariosService.GetUsuario(id);
                if (usuarioExistente != null)
                {
                    string validationResult = ValidarDatosUnicos(usuarioExistente, usuarioEditado);
                    if (validationResult != null) return ValidationProblem(validationResult);

                    usuarioEditado.Id = usuarioExistente.Id;
                    _usuariosService.EditUsuario(usuarioEditado);
                    return Ok("Usuario editado con éxito");
                }

                return Problem("Usuario no encontrado");
            }
            catch
            {
                return ErrorInterno();
            }
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult DeleteUsuario([FromRoute] int id)
        {
            try
            {
                var usuario = _usuariosService.GetUsuario(id);
                if (usuario != null)
                {
                    _usuariosService.DeleteUsuario(usuario);
                    return Ok("Usuario eliminado con éxito");
                }

                return Problem("Usuario no encontrado");
            }
            catch
            {
                return ErrorInterno();
            }
        }

        private string ValidarDatosUnicos(Usuarios usuario)
        {
            string datoRepetido = null;

            if (_usuariosService.ExisteCedula(usuario.Cedula)) datoRepetido = "Número de cédula";
            else if (_usuariosService.ExisteEmail(usuario.CorreoElectronico)) datoRepetido = "Correo eletrónico";
            else if (_usuariosService.ExisteTelefono(usuario.Telefono)) datoRepetido = "Número de teléfono";

            if (datoRepetido != null)
            {
                datoRepetido = String.Format("{0} ya existente", datoRepetido);
            }

            return datoRepetido;
        }

        private string ValidarDatosUnicos(Usuarios usuarioExistente, Usuarios usuarioEditado)
        {
            string datoRepetido = null;

            if (usuarioExistente.Cedula != usuarioEditado.Cedula)
            {
                if (_usuariosService.ExisteCedula(usuarioEditado.Cedula)) datoRepetido = "Número de cédula";
            }
            else if (usuarioExistente.CorreoElectronico != usuarioEditado.CorreoElectronico)
            {
                if (_usuariosService.ExisteEmail(usuarioEditado.CorreoElectronico)) datoRepetido = "Correo eletrónico";
            }
            else if (usuarioExistente.Telefono != usuarioEditado.Telefono)
            {
                if (_usuariosService.ExisteTelefono(usuarioEditado.Telefono)) datoRepetido = "Número de teléfono";
            }

            if (datoRepetido != null)
            {
                datoRepetido = String.Format("{0} ya existente", datoRepetido);
            }

            return datoRepetido;
        }

        private ObjectResult ErrorInterno()
        {
            return Problem("Ha ocurrido un error al procesar la solicitud");
        }
    }
}
