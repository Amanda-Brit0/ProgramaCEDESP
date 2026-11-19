using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq; // Essencial para usar .FirstOrDefault(), .Count(), .Where()
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProgramaCEDESP
{
    public partial class FreqAluno : Form
    {
        private string _nomeAluno; // Variável para armazenar o nome do aluno

        public FreqAluno(string nomeAluno)
        {
            InitializeComponent();
            _nomeAluno = nomeAluno; // Armazena o nome do aluno passado
            this.Text = $"CEDESP - Frequência de {_nomeAluno}"; // Opcional: define o título da janela
            CarregarFrequenciaDoAluno(); // Chama o método para carregar os dados
        }

        private void CarregarFrequenciaDoAluno()
        {
            // 1. Carrega todos os alunos
            List<DadosAluno> todosAlunos = GerenciadorDados.CarregarAlunos();

            // 2. Encontra o aluno específico pelo nome
            // REMOVIDO "Data." DAQUI, ESSA É A LINHA QUE CAUSAVA O ERRO!
            DadosAluno alunoAtual = todosAlunos.FirstOrDefault(a => a.Nome == _nomeAluno);

            if (alunoAtual != null)
            {
                // 3. Configura o DataGridView para mostrar apenas as Faltas
                dataGridViewFrequencia.Columns.Clear(); // Limpa colunas existentes
                dataGridViewFrequencia.Rows.Clear();    // Limpa linhas existentes

                dataGridViewFrequencia.Columns.Add("DataDaFalta", "Data da Falta");

                // Opcional: Define a largura das colunas
                dataGridViewFrequencia.Columns["DataDaFalta"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                // Variáveis para cálculo da porcentagem e total de aulas
                int totalAulasDadas = 0;
                int totalFaltas = 0;

                // 4. Preenche o DataGridView com as datas de falta e calcula totais
                if (alunoAtual.Frequencia != null && alunoAtual.Frequencia.Any())
                {
                    // O total de aulas dadas é o número de registros de frequência para o aluno
                    // Isso inclui tanto 'P' quanto 'F', representando todas as aulas que o aluno deveria ter frequência registrada.
                    totalAulasDadas = alunoAtual.Frequencia.Count;

                    // Filtra apenas os registros de falta
                    var faltasDoAluno = alunoAtual.Frequencia.Where(r => r.Status == 'F').OrderBy(r => r.Data).ToList();
                    totalFaltas = faltasDoAluno.Count;

                    if (faltasDoAluno.Any())
                    {
                        foreach (var falta in faltasDoAluno)
                        {
                            dataGridViewFrequencia.Rows.Add(falta.Data.ToShortDateString());
                        }
                    }
                    else
                    {
                        // Se não houver faltas, exibe uma mensagem no DataGridView
                        dataGridViewFrequencia.Rows.Add("Nenhuma falta registrada até o momento.");
                    }
                }
                else
                {
                    // Caso não haja nenhum registro de frequência para o aluno
                    dataGridViewFrequencia.Rows.Add("Nenhum registro de frequência encontrado para este aluno.");
                }

                // 5. Calcula e exibe a porcentagem de frequência
                double porcentagemFrequencia = 0;
                if (totalAulasDadas > 0)
                {
                    porcentagemFrequencia = ((double)(totalAulasDadas - totalFaltas) / totalAulasDadas) * 100;
                }

                // Atualiza os Labels com as informações
                lblPorcentagemFrequencia.Text = $"Frequência: {porcentagemFrequencia:F2}%"; // Formata para 2 casas decimais
                lblPorcentagemFrequencia.Font = new Font(lblPorcentagemFrequencia.Font, FontStyle.Bold); // Deixa em negrito
                lblPorcentagemFrequencia.ForeColor = Color.DarkGreen;

                // NOVO: Atualiza o label do Total de Aulas (Usando o nome 'totalAulas' que você definiu)
                totalAulas.Text = $"Total de Aulas: {totalAulasDadas}";
                totalAulas.Font = new Font(totalAulas.Font, FontStyle.Bold); // Deixa em negrito
                totalAulas.ForeColor = Color.DarkBlue; // Cor diferente para destaque (opcional)
            }
            else
            {
                MessageBox.Show("Aluno não encontrado.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        // Adicione um botão de Voltar/Fechar se necessário
        private void btnVoltar_Click(object sender, EventArgs e)
        {
            this.Close(); // Fecha a tela atual
        }

        private void FreqAluno_Load(object sender, EventArgs e)
        {
            // Se precisar de alguma lógica que dependa do formulário já estar totalmente carregado, coloque aqui.
        }

        private void lblPorcentagemFrequencia_Click(object sender, EventArgs e)
        {
            // Este método está vazio, é um evento de clique no label de frequência,
            // pode ser deixado assim se não houver funcionalidade para ele.
        }
    }
}