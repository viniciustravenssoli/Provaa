using API.Models;

namespace API;

public class FolhaPagamento
{
    public int FuncionarioId { get; set; }
    public Funcionario Funcionario { get; set; }
    public int Id { get; set; }
    public double ValorHora { get; set; }
    public int QuantidadeHoras { get; set; }
    public double Bruto { get; set; }
    public double ImpostoRenda { get; set; }
    public double Inss { get; set; }
    public double Fgts { get; set; }
    public double SalarioLiquido { get; set; }
    public int Mes { get; set; }
    public int Ano { get; set; }
    public DateTime CriadoEm { get; set; } = DateTime.Now;
}
