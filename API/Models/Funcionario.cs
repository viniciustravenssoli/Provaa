using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    //Data Annotations
    public class Funcionario
    {
        public Funcionario () => CriadoEm = DateTime.Now;
        public int Id { get; set; }

        public string Nome { get; set; }

        public string Cpf { get; set; }

        public double Salario { get; set; }
        public string Email { get; set; }
        public DateTime Nascimento { get; set; }
        public DateTime CriadoEm { get; set; }
    }
}