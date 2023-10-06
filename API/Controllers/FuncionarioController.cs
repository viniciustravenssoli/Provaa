using API.Models;
using DepartmentManager.Data;
using Microsoft.AspNetCore.Mvc;

namespace API;

[ApiController]
[Route("api/funcionario")]
public class FuncionarioController : ControllerBase
{
    private readonly AppDbContext _context;

    public FuncionarioController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    [Route("cadastrar")]
    public async Task<IActionResult> Criar([FromBody] Funcionario funcionario)
    {
        try
        {
            _context.Funcionarios.Add(funcionario);
            _context.SaveChanges();

            return Created("Funcionario criado com sucesso", funcionario);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet]
    [Route("Listar")]
    public IActionResult Listar()
    {
        var funcionarios =
            _context.Funcionarios.ToList();

        if (funcionarios.Count == 0) return NotFound();

        return Ok(funcionarios);
    }
}

