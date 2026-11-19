using System;
using System.Collections.Generic;
using System.Linq;

namespace ProgramaCEDESP
{
    public static class CalculadoraEstatistica
    {
        private static readonly string[] TemasAvaliacao = {
            "Sistema Operacional", "Word", "PowerPoint", "InDesign",
            "Apresentacao", "Revista", "Excel", "Modelagem 3D", "Participacao"
        };

        public static double CalcularMediaAluno(DadosAluno aluno)
        {
            if (aluno.Notas == null || !aluno.Notas.Any()) return 0.0;
            double soma = 0; int count = 0;
            foreach (string tema in TemasAvaliacao)
            {
                var nota = aluno.Notas.FirstOrDefault(n =>
                    n.Tema != null && n.Tema.Equals(tema, StringComparison.OrdinalIgnoreCase));
                if (nota != null) { soma += nota.Valor; count++; }
            }
            return count > 0 ? soma / count : 0.0;
        }

        public static double CalcularFrequenciaPercentual(DadosAluno aluno, int totalAulas)
        {
            if (totalAulas == 0) return 100.0;
            int faltas = aluno.Frequencia?.Count(f => f.Status == 'F') ?? 0;
            int presencas = Math.Max(0, totalAulas - faltas);
            return (double)presencas / totalAulas * 100.0;
        }
    }
}