using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ProgramaCEDESP
{
    public static class GerenciadorDados
    {
        private static readonly string _caminhoArquivo = "dadosAlunos.json";

        // !!! ATENÇÃO CRÍTICA AQUI !!!
        private const string CaminhoBaseServidorPastasConviventes = @"\\server\"; // <--- AJUSTE ESTE CAMINHO!

        public static List<DadosAluno> CarregarAlunos()
        {
            if (File.Exists(_caminhoArquivo))
            {
                string json = File.ReadAllText(_caminhoArquivo);
                if (string.IsNullOrWhiteSpace(json) || json.Trim() == "[]")
                {
                    return new List<DadosAluno>();
                }
                try
                {
                    return JsonConvert.DeserializeObject<List<DadosAluno>>(json);
                }
                catch (JsonSerializationException ex)
                {
                    MessageBox.Show($"Erro ao carregar dados dos alunos: {ex.Message}\nO arquivo de dados pode estar corrompido. Será criada uma nova lista.", "Erro de Dados", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return new List<DadosAluno>();
                }
            }
            return new List<DadosAluno>();
        }

        public static void SalvarAlunos(List<DadosAluno> alunos)
        {
            string json = JsonConvert.SerializeObject(alunos, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(_caminhoArquivo, json);
        }

        /// <summary>
        /// Adiciona um novo aluno ao sistema, com validação para evitar nomes completos duplicados.
        /// </summary>
        /// <param name="novoAluno">O objeto DadosAluno do novo aluno a ser cadastrado.</param>
        /// <returns>True se o cadastro for bem-sucedido, False se o nome já existir.</returns>
        public static bool AdicionarNovoAluno(DadosAluno novoAluno)
        {
            List<DadosAluno> alunosExistentes = CarregarAlunos();

            // A validação abaixo verifica se a string completa do nome já existe.
            bool nomeCompletoJaExiste = alunosExistentes.Any(aluno =>
                aluno.Nome.Equals(novoAluno.Nome, StringComparison.OrdinalIgnoreCase));

            if (nomeCompletoJaExiste)
            {
                MessageBox.Show($"Já existe um aluno com o nome completo '{novoAluno.Nome}'. Não é possível cadastrar.", "Erro de Cadastro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            alunosExistentes.Add(novoAluno);
            SalvarAlunos(alunosExistentes);

            return true;
        }

        /// <summary>
        /// Verifica se um aluno com o nome completo já existe no sistema.
        /// </summary>
        /// <param name="nomeCompleto">O nome completo a ser verificado.</param>
        /// <returns>True se o nome já existir, False caso contrário.</returns>
        public static bool VerificarNomeCompleto(string nomeCompleto)
        {
            List<DadosAluno> alunosExistentes = CarregarAlunos();
            return alunosExistentes.Any(aluno => aluno.Nome.Equals(nomeCompleto, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Limpa todos os dados de notas e frequência de todos os alunos.
        /// Este método é chamado ao finalizar o semestre.
        /// </summary>
        public static void LimparDadosSemestre()
        {
            var alunos = CarregarAlunos();

            foreach (var aluno in alunos)
            {
                // Limpa a lista de notas
                aluno.Notas.Clear();

                // Limpa a lista de frequência
                aluno.Frequencia.Clear();
            }

            SalvarAlunos(alunos);
        }
    }
}