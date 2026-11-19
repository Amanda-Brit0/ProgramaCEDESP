using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ProgramaCEDESP
{
    public partial class FormNotasAdmin : Form
    {
        // _alunos agora será a lista de ALUNOS FILTRADOS que está sendo exibida na grid.
        // A lista COMPLETA de todos os alunos será carregada e manipulada no btnRegistrar_Click
        // para garantir que todas as alterações sejam salvas corretamente.
        private List<DadosAluno> _alunosExibidos;
        private string _turnoAtual; // Armazena o turno que o admin selecionou (Tarde, Noite, ou "Não Definido" para todos)

        // Definimos os temas das avaliações como constantes para fácil manutenção
        private static readonly string[] TemasAvaliacao = {
            "Sistema Operacional", "Word", "Excel", "PowerPoint", "InDesign", "Web", "Participacao"
        };

        // **** NOVO CONSTRUTOR PARA RECEBER O TURNO ****
        public FormNotasAdmin(string turnoAdmin)
        {
            InitializeComponent();
            _turnoAtual = turnoAdmin; // Armazena o turno que o admin selecionou

            CarregarAlunosParaExibicao(); // Carrega e filtra os alunos para exibir na grid
            ConfigurarDataGridView(); // Configura as colunas do DataGridView
            CarregarNotasNaTabela(); // Carrega os dados na tabela

            this.Text = $"Administração de Notas - Turno: {(_turnoAtual == "Não Definido" ? "Todos" : _turnoAtual)}";
        }

        // Construtor original (mantido para compatibilidade, se necessário, mas o novo é preferível)
        public FormNotasAdmin()
        {
            InitializeComponent();
            _turnoAtual = "Não Definido"; // Valor padrão se o turno não for passado
            CarregarAlunosParaExibicao();
            ConfigurarDataGridView();
            CarregarNotasNaTabela();
            this.Text = "Administração de Notas - Todos os Turnos";
        }

        // Método para carregar e filtrar os alunos para exibição na grid
        private void CarregarAlunosParaExibicao()
        {
            List<DadosAluno> todosAlunos = GerenciadorDados.CarregarAlunos();

            if (_turnoAtual == "Não Definido") // "Não Definido" indica que o admin quer ver todos
            {
                _alunosExibidos = todosAlunos; // Exibe todos
            }
            else
            {
                // Filtra os alunos pelo turno selecionado
                _alunosExibidos = todosAlunos
                                        .Where(a => a.Turno != null && a.Turno.Equals(_turnoAtual, StringComparison.OrdinalIgnoreCase))
                                        .ToList();
            }
        }

        // Configura as colunas do DataGridView
        private void ConfigurarDataGridView()
        {
            dataGridView1.Columns.Clear(); // Limpa quaisquer colunas existentes

            // Adiciona a coluna "Nome do Aluno" (apenas leitura)
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "Nome", // Nome da coluna programático
                HeaderText = "Nome do Aluno", // Texto do cabeçalho
                ReadOnly = true,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            });

            // **** ADICIONAR COLUNA DE TURNO ****
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "Turno",
                HeaderText = "Turno",
                ReadOnly = true,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            });

            // Adiciona as colunas para cada Tema de Avaliação (editáveis)
            foreach (string tema in TemasAvaliacao)
            {
                dataGridView1.Columns.Add(new DataGridViewTextBoxColumn()
                {
                    Name = tema.Replace(" ", ""), // Remove espaços para o nome da coluna (ex: "SistemaOperacional")
                    HeaderText = tema,
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
                });
            }

            // Adiciona a coluna "Média" (apenas leitura)
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "Media",
                HeaderText = "Média",
                ReadOnly = true,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            });

            // Opcional: Impedir que o usuário adicione ou remova linhas diretamente
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
        }

        // Carrega as notas dos alunos na tabela
        private void CarregarNotasNaTabela()
        {
            dataGridView1.Rows.Clear(); // Limpa as linhas existentes

            // Ordena os alunos para exibição: primeiro por turno (se estiver exibindo todos), depois por nome
            var alunosOrdenados = (_turnoAtual == "Não Definido") ?
                _alunosExibidos.OrderBy(a => a.Turno == "Tarde" ? 0 : (a.Turno == "Noite" ? 1 : 2)).ThenBy(a => a.Nome).ToList() :
                _alunosExibidos.OrderBy(a => a.Nome).ToList();

            foreach (var aluno in alunosOrdenados)
            {
                // Cria uma nova linha no DataGridView
                int rowIndex = dataGridView1.Rows.Add(); // Adiciona uma nova linha e obtém seu índice
                DataGridViewRow row = dataGridView1.Rows[rowIndex]; // Obtém a referência da nova linha

                // Define o nome do aluno e o turno
                // Acessamos as células pelo nome da coluna, o que é mais seguro
                if (dataGridView1.Columns.Contains("Nome"))
                    row.Cells["Nome"].Value = aluno.Nome;

                if (dataGridView1.Columns.Contains("Turno"))
                    row.Cells["Turno"].Value = aluno.Turno;

                // Preenche as notas para cada tema
                for (int i = 0; i < TemasAvaliacao.Length; i++)
                {
                    string temaAtual = TemasAvaliacao[i];
                    RegistroNota notaExistente = aluno.Notas.FirstOrDefault(n => n.Tema == temaAtual);

                    // Acessa a célula da nota pelo nome da coluna, que é o nome do tema sem espaços
                    string columnName = temaAtual.Replace(" ", "");
                    if (dataGridView1.Columns.Contains(columnName))
                    {
                        if (notaExistente != null)
                        {
                            row.Cells[columnName].Value = notaExistente.Valor.ToString("F1", System.Globalization.CultureInfo.InvariantCulture); // Formata para 1 casa decimal
                        }
                        else
                        {
                            row.Cells[columnName].Value = ""; // Deixa vazio se não houver nota para o tema
                        }
                    }
                }

                // Calcula e define a média usando o novo método da classe utilitária
                double media = CalculadoraEstatistica.CalcularMediaAluno(aluno);
                // A coluna da média é a última, acessada pelo nome
                if (dataGridView1.Columns.Contains("Media"))
                {
                    row.Cells["Media"].Value = media.ToString("F1", System.Globalization.CultureInfo.InvariantCulture); // Formata para 1 casa decimal
                }
            }
        }

        // NOVO MÉTODO - CHAMANDO A CLASSE UTITLITÁRIA
        // Este método não é mais usado para o cálculo em si, mas serve para chamar o método estático.
        private double CalcularMediaAluno(DadosAluno aluno)
        {
            return CalculadoraEstatistica.CalcularMediaAluno(aluno);
        }

        // Evento de clique do botão Registrar
        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            // Carrega a lista COMPLETA de alunos para garantir que todas as modificações sejam persistidas
            // Esta é a lista master que será atualizada e salva.
            List<DadosAluno> todosAlunosDoSistema = GerenciadorDados.CarregarAlunos();

            foreach (DataGridViewRow linha in dataGridView1.Rows)
            {
                if (linha.IsNewRow) continue; // Ignora a linha de adição de novo registro, se houver

                string nomeAluno = linha.Cells["Nome"]?.Value?.ToString();
                string turnoAlunoLinha = linha.Cells["Turno"]?.Value?.ToString();

                if (string.IsNullOrEmpty(nomeAluno) || string.IsNullOrEmpty(turnoAlunoLinha))
                {
                    continue;
                }

                DadosAluno alunoParaAtualizar = todosAlunosDoSistema.FirstOrDefault(a =>
                    a.Nome != null && a.Nome.Equals(nomeAluno, StringComparison.OrdinalIgnoreCase) &&
                    a.Turno != null && a.Turno.Equals(turnoAlunoLinha, StringComparison.OrdinalIgnoreCase)
                );

                if (alunoParaAtualizar == null)
                {
                    MessageBox.Show($"Aluno '{nomeAluno}' do turno '{turnoAlunoLinha}' não encontrado na lista de alunos. As notas não foram salvas para este aluno.", "Erro de Sincronização", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    continue;
                }

                bool notasAtualizadas = false;

                foreach (string temaAtual in TemasAvaliacao)
                {
                    string columnName = temaAtual.Replace(" ", "");
                    if (!dataGridView1.Columns.Contains(columnName)) continue;

                    string notaTexto = linha.Cells[columnName]?.Value?.ToString();

                    if (string.IsNullOrWhiteSpace(notaTexto))
                    {
                        alunoParaAtualizar.Notas.RemoveAll(n => n.Tema == temaAtual);
                        notasAtualizadas = true;
                        continue;
                    }

                    if (!double.TryParse(notaTexto, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out double notaValor))
                    {
                        MessageBox.Show($"Valor de nota inválido para o aluno '{nomeAluno}' no tema '{temaAtual}': '{notaTexto}'. Por favor, digite um número válido (ex: 7.5).", "Erro de Entrada", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    RegistroNota notaExistente = alunoParaAtualizar.Notas.FirstOrDefault(n => n.Tema == temaAtual);

                    if (notaExistente != null)
                    {
                        if (Math.Abs(notaExistente.Valor - notaValor) > 0.001)
                        {
                            notaExistente.Valor = notaValor;
                            notasAtualizadas = true;
                        }
                    }
                    else
                    {
                        alunoParaAtualizar.Notas.Add(new RegistroNota { Tema = temaAtual, Valor = notaValor });
                        notasAtualizadas = true;
                    }
                }

                if (notasAtualizadas)
                {
                    double novaMedia = CalculadoraEstatistica.CalcularMediaAluno(alunoParaAtualizar);
                    if (dataGridView1.Columns.Contains("Media"))
                    {
                        linha.Cells["Media"].Value = novaMedia.ToString("F1", System.Globalization.CultureInfo.InvariantCulture);
                    }
                }
            }

            GerenciadorDados.SalvarAlunos(todosAlunosDoSistema);
            MessageBox.Show("Notas registradas e atualizadas com sucesso!");

            CarregarAlunosParaExibicao();
            ConfigurarDataGridView();
            CarregarNotasNaTabela();
        }

        private void btnVoltar1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}