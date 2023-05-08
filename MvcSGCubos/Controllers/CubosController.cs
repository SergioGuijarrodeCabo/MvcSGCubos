using Microsoft.AspNetCore.Mvc;
using MvcSGCubos.Filters;
using MvcSGCubos.Models;
using MvcSGCubos.Services;

namespace MvcSGCubos.Controllers
{
    public class CubosController : Controller
    {
        private ServiceApiCubos service;

        public CubosController(ServiceApiCubos service)
        {
            this.service = service;
        }



        public async Task<IActionResult> Index()
        {
            List<Cubo> cubos = await this.service.GetCubosAsync();

            return View(cubos);
        }

        [HttpPost]
        public async Task<IActionResult> Index(string marca)
        {
            List<Cubo> cubos = await this.service.FindCubosMarcaAsync(marca);

            return View(cubos);
        }

        [HttpPost]
        public async Task<IActionResult> InsertarUsuario(Usuario usuario)
        {
            await this.service.InsertUsuarioASync
             (usuario.Id_Usuario, usuario.Nombre, usuario.Email, usuario.Pass, usuario.Imagen);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult InsertarUsuario()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> InsertarCubo(Cubo cubo)
        {
            await this.service.InsertCuboAsync
             (cubo.Id_Cubo, cubo.Nombre, cubo.Imagen, cubo.Marca, cubo.Precio);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult InsertarCubo()
        {
            return View();
        }

        [AuthorizeUsuarios]
        public async Task<IActionResult> Perfil()
        {
            string token =
                HttpContext.Session.GetString("TOKEN");
            Usuario usuario = await
                this.service.PerfilUsuarioAsync(token);
            return View(usuario);
        }

        [AuthorizeUsuarios]
        public async Task<IActionResult> PedidosUsuario()
        {
            string token =
                HttpContext.Session.GetString("TOKEN");
            List<CompraCubos> pedidos = await
                this.service.GetPedidosUsuariosAsync(token);
            return View(pedidos);
        }



    }
}
