using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq; // Necessário para usar .FirstOrDefault()
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProgramaCEDESP
{
    public partial class NotaAluno : Form
    {
        private string _nomeAluno; // Variável para armazenar o nome do aluno

        public NotaAluno(string nomeAluno)
        {
            InitializeComponent();
            _nomeAluno = nomeAluno; // Armazena o nome do aluno passado
            this.Text = $"CEDESP - Notas de {_nomeAluno}"; // O título da janela será definido aqui
            CarregarNotasDoAluno(); // Chama o método para carregar os dados
        }

        private void CarregarNotasDoAluno()
        {
            // 1. Carrega todos os alunos
            List<DadosAluno> todosAlunos = GerenciadorDados.CarregarAlunos();

            // 2. Encontra o aluno específico pelo nome
            DadosAluno alunoAtual = todosAlunos.FirstOrDefault(a => a.Nome == _nomeAluno);

            if (alunoAtual != null)
            {
                // 3. Configura o DataGridView para mostrar as notas
                dataGridViewNota.Columns.Clear(); // Limpa colunas existentes
                dataGridViewNota.Rows.Clear();   // Limpa linhas existentes

                dataGridViewNota.Columns.Add("Tema", "Tema");
                dataGridViewNota.Columns.Add("Valor", "Nota");

                // Configura o estilo da coluna de nota para centralizar (opcional, pode ser removido se não quiser)
                dataGridViewNota.Columns["Valor"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridViewNota.Columns["Valor"].Width = 80; // Largura da coluna

                // Opcional: Define a largura das colunas
                dataGridViewNota.Columns["Tema"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridViewNota.Columns["Valor"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;

                // 4. Preenche o DataGridView com os dados de notas do aluno
                if (alunoAtual.Notas != null && alunoAtual.Notas.Any())
                {
                    foreach (var nota in alunoAtual.Notas)
                    {
                        dataGridViewNota.Rows.Add(nota.Tema, nota.Valor);
                    }
                }
                else
                {
                    MessageBox.Show("Não há notas registradas para este aluno.", "Informação", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    // Opcional: Adiciona uma linha informativa no grid se não houver notas
                    // dataGridViewNota.Rows.Add("Nenhuma nota registrada até o momento.", "");
                }
            }
            else
            {
                MessageBox.Show("Aluno não encontrado.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                // Pode fechar a tela ou voltar para a anterior se o aluno não for encontrado
                this.Close();
            }
        }

        // Adicione um botão de Voltar/Fechar se necessário (ex: btnVoltar), e seu evento de clique
        private void btnVoltar_Click(object sender, EventArgs e)
        {
            this.Close(); // Fecha a tela atual
        }

        private void NotaAluno_Load(object sender, EventArgs e)
        {
            // Lógica adicional quando o formulário é carregado, se houver.
            // Nenhum código específico para Label ou PictureBox aqui.
        }
    }
}