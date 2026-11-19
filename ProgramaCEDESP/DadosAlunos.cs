using System;
using System.Collections.Generic;

namespace ProgramaCEDESP
{
    public class RegistroFrequencia
    {
        public DateTime Data { get; set; }
        public char Status { get; set; } // 'P' para presente, 'F' para falta
    }

    public class RegistroNota
    {
        public string Tema { get; set; }
        public double Valor { get; set; } 
    }

    public class DadosAluno
    {
        public string Nome { get; set; } // Nome completo para uso na administração (Correto: é 'Nome')
        public string Usuario { get; set; } // Campo para o nome de usuário de login do aluno
        public string Senha { get; set; } // Campo para a senha de login do aluno
        public string Turno { get; set; }
        public List<RegistroFrequencia> Frequencia { get; set; } = new List<RegistroFrequencia>();
        public List<RegistroNota> Notas { get; set; } = new List<RegistroNota>();
    }
}