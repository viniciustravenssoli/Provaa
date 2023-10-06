using DepartmentManager.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API;

public class FolhaPagamentoController : ControllerBase
{
    private readonly AppDbContext _context;
    public FolhaPagamentoController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    [Route("create")]
    public async Task<IActionResult> Criar([FromBody] FolhaPagamento folhaPagamento)
    {
        try
        {    
            var funcionario = _context.Funcionarios.FirstOrDefault(x => x.Id == folhaPagamento.FuncionarioId);

            folhaPagamento.Funcionario = funcionario;
            _context.Folhas.Add(folhaPagamento);
            _context.SaveChanges();

            return Created("Funcionario criado com sucesso", folhaPagamento);
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
        var folhas =
            _context.Folhas.Include(x => x.Funcionario).ToList();

        if (folhas.Count == 0) return NotFound();

        return Ok(folhas);
    }

    [HttpGet]
        [Route("buscar/{cpf}/{mes}/{ano}")]
        public IActionResult Buscar([FromRoute]string cpf, int mes, int ano)
        {            
            return Ok(
                _context.Folhas
                .Include(f => f.Funcionario)
                .FirstOrDefault
                (f => 
                    f.CriadoEm.Month == mes && 
                    f.CriadoEm.Year == ano &&
                    f.Funcionario.Cpf.Equals(cpf)));
        } 
    


}
