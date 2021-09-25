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
        [Route("api/[controller]")]
        public IActionResult GetAllUsuarios()
        {
            var lstUsuarios = _usuariosService.GetAllUsuarios();
            return Ok(lstUsuarios);
        }

        [HttpGet]
        [Route("api/[controller]/{id}")]
        public IActionResult GetUsuario([FromRoute] int id)
        {
            var usuario = _usuariosService.GetUsuario(id);
            if(usuario != null)
            {
                return Ok(usuario);
            }

            return Problem("Usuario no encontrado");
        }

        [HttpPost]
        [Route("api/[controller]")]
        public IActionResult AddUsuario([FromBody] Usuarios usuario)
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

        [HttpPatch]
        [Route("api/[controller]/{id}")]
        public IActionResult EditUsuario([FromRoute] int id, [FromBody] Usuarios usuarioEditado)
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

        [HttpDelete]
        [Route("api/[controller]/{id}")]
        public IActionResult DeleteUsuario([FromRoute] int id)
        {
            var usuario = _usuariosService.GetUsuario(id);
            if (usuario != null)
            {
                _usuariosService.DeleteUsuario(usuario);
                return Ok("Usuario eliminado con éxito");
            }

            return Problem("Usuario no encontrado");
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
    }
}
