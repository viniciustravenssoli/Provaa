using DepartmentManager.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API;

[ApiController]
[Route("api/folha")]
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

            folhaPagamento.Bruto = CalcularBruto(folhaPagamento.ValorHora, folhaPagamento.QuantidadeHoras);
            folhaPagamento.ImpostoRenda = CalcularImpostoDeRenda(folhaPagamento.Bruto);
            folhaPagamento.Inss = CalcularInss(folhaPagamento.Bruto);
            folhaPagamento.Fgts = CalcularFGTS(folhaPagamento.Bruto);
            folhaPagamento.SalarioLiquido = CalcularLiquido(folhaPagamento.Bruto, folhaPagamento.ImpostoRenda, folhaPagamento.Inss);

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
    public IActionResult Buscar([FromRoute] string cpf, int mes, int ano)
    {
        return Ok (
            _context.Folhas
            .Include(f => f.Funcionario)
            .FirstOrDefault
            (f =>
                f.CriadoEm.Month == mes &&
                f.CriadoEm.Year == ano &&
                f.Funcionario.Cpf.Equals(cpf)));
    }

    public static double CalcularBruto(double valorHora, int qntdHoras)
    {
        return valorHora * qntdHoras;
    }

    public static double CalcularImpostoDeRenda(double bruto)
    {
        if (bruto <= 1903.98)
        {
            return 0;
        }
        else if(bruto <= 1903.99 && bruto == 2826.65)
        {
            return bruto * 0.075 - 142.80;
        }
        else if(bruto <= 2826.66 && bruto == 3751.05)
        {
            return bruto * 0.15 - 354.80;
        }
        else if(bruto <= 3751.06 && bruto == 4664.68)
        {
            return bruto * 0.225 - 636.13;
        }
        else
        {
            return bruto * 0.275 - 869.36;
        }
    }

    public static double CalcularInss(double bruto)
    {
        if (bruto <= 1693.72)
        {
            return bruto * 0.08;
        }
        else if(bruto >= 1693.73 && bruto <= 2822.90)
        {
            return  bruto * 0.09;
        }
        else if(bruto >= 2822.91 && bruto <= 5642.80)
        {
            return bruto * 0.11;
        }
        else
        {
            return 621.03;
        }
    }

    public static double CalcularFGTS(double bruto)
    {
        return bruto * 0.08;
    }

    public static double CalcularLiquido(double bruto, double impostoRenda, double inss)
    {
        return bruto - impostoRenda - inss;
    }



}
