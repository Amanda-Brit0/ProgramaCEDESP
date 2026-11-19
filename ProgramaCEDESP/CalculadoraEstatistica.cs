using System;
using System.Collections.Generic;
using System.Linq;

namespace ProgramaCEDESP
{
    public static class CalculadoraEstatistica
    {
        private static readonly string[] TemasAvaliacao = {
            "Sistema Operacional", "Word", "Excel", "PowerPoint", "InDesign", "Web", "Participacao"
        };

        /// <summary>
        /// Calcula a média das notas de um aluno para os temas definidos.
        /// </summary>
        public static double CalcularMediaAluno(DadosAluno aluno)
        {
            if (aluno.Notas == null || !aluno.Notas.Any())
            {
                return 0.0;
            }

            double somaDasNotas = 0;
            int contadorDeNotasValidas = 0;

            foreach (string tema in TemasAvaliacao)
            {
                RegistroNota nota = aluno.Notas.FirstOrDefault(n => n.Tema == tema);
                if (nota != null)
                {
                    somaDasNotas += nota.Valor;
                    contadorDeNotasValidas++;
                }
            }

            return contadorDeNotasValidas > 0 ? somaDasNotas / contadorDeNotasValidas : 0.0;
        }

        /// <summary>
        /// Calcula a frequência percentual de um aluno.
        /// </summary>
        public static double CalcularFrequenciaPercentual(DadosAluno aluno, int totalAulas)
        {
            if (aluno.Frequencia == null || totalAulas == 0)
            {
                return 0.0;
            }

            int totalPresencas = aluno.Frequencia.Count(f => f.Status == 'P');
            return (double)totalPresencas / totalAulas * 100;
        }
    }
}