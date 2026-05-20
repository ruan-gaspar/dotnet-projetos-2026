// Controllers/AuditoriaController.cs
using CadastroProdutos.Services;
using Microsoft.AspNetCore.Authorization;using Microsoft.AspNetCore.Mvc;
namespace CadastroProdutos.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]public class AuditoriaController : ControllerBase{
    private readonly IAuditoriaService _audit;
    public AuditoriaController(IAuditoriaService audit) => _audit = audit;

    [HttpGet]
    public async Task<IActionResult> Listar() => Ok(await _audit.ListarAsync());
}