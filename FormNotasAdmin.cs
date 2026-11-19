using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ProgramaCEDESP
{
    public partial class FormNotasAdmin : Form
    {
        private List<DadosAluno> _alunosExibidos;
        private string _turnoAtual;

        private static readonly string[] TemasAvaliacao = {
            "Sistema Operacional", "Word", "PowerPoint", "InDesign",
            "Apresentacao", "Revista", "Excel", "Modelagem 3D", "Participacao"
        };

        // Mapeamento tema -> nome da coluna (sem acentos/espaços)
        private static string ColName(string tema) =>
            tema.Replace(" ", "").Replace("ã", "a").Replace("ç", "c")
                .Replace("é", "e").Replace("ê", "e").Replace("á", "a")
                .Replace("í", "i").Replace("ó", "o").Replace("ú", "u")
                .Replace("Ã", "A").Replace("Ç", "C");

        public FormNotasAdmin(string turnoAdmin)
        {
            InitializeComponent();
            _turnoAtual = turnoAdmin;
            CarregarAlunosParaExibicao();
            ConfigurarDataGridView();
            CarregarNotasNaTabela();
            this.Text = $"Administração de Notas - Turno: {(_turnoAtual == "Não Definido" ? "Todos" : _turnoAtual)}";
        }

        public FormNotasAdmin()
        {
            InitializeComponent();
            _turnoAtual = "Não Definido";
            CarregarAlunosParaExibicao();
            ConfigurarDataGridView();
            CarregarNotasNaTabela();
            this.Text = "Administração de Notas - Todos os Turnos";
        }

        private void CarregarAlunosParaExibicao()
        {
            var todos = GerenciadorDados.CarregarAlunos();
            _alunosExibidos = (_turnoAtual == "Não Definido")
                ? todos
                : todos.Where(a => a.Turno != null &&
                      a.Turno.Equals(_turnoAtual, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        private void ConfigurarDataGridView()
        {
            dataGridView1.Columns.Clear();

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "Nome",
                HeaderText = "Nome do Aluno",
                ReadOnly = true,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            });

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "Turno",
                HeaderText = "Turno",
                ReadOnly = true,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            });

            foreach (string tema in TemasAvaliacao)
                dataGridView1.Columns.Add(new DataGridViewTextBoxColumn()
                {
                    Name = ColName(tema),
                    HeaderText = tema,
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
                });

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "Media",
                HeaderText = "Média",
                ReadOnly = true,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            });

            var colRemover = new DataGridViewButtonColumn()
            {
                Name = "Remover",
                HeaderText = "Remover",
                Text = "Remover",
                UseColumnTextForButtonValue = true,
                Width = 80
            };
            dataGridView1.Columns.Add(colRemover);

            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;

            dataGridView1.CellClick -= dataGridView1_CellClick;
            dataGridView1.CellClick += dataGridView1_CellClick;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (dataGridView1.Columns[e.ColumnIndex].Name != "Remover") return;

            string nome = dataGridView1.Rows[e.RowIndex].Cells["Nome"].Value?.ToString();
            string turno = dataGridView1.Rows[e.RowIndex].Cells["Turno"].Value?.ToString();
            if (string.IsNullOrEmpty(nome)) return;

            var resp = MessageBox.Show(
                $"Remover o aluno '{nome}' ({turno}) do sistema?\n\nIsso remove também frequência e notas.",
                "Confirmar Remoção", MessageBoxButtons.YesNo, MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2);
            if (resp != DialogResult.Yes) return;

            var todos = GerenciadorDados.CarregarAlunos();
            int antes = todos.Count;
            todos.RemoveAll(a =>
                a.Nome != null && a.Nome.Equals(nome, StringComparison.OrdinalIgnoreCase) &&
                a.Turno != null && a.Turno.Equals(turno, StringComparison.OrdinalIgnoreCase));

            if (todos.Count < antes)
            {
                GerenciadorDados.SalvarAlunos(todos);
                MessageBox.Show($"Aluno '{nome}' removido com sucesso!", "Removido",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                CarregarAlunosParaExibicao();
                ConfigurarDataGridView();
                CarregarNotasNaTabela();
            }
        }

        private void CarregarNotasNaTabela()
        {
            dataGridView1.Rows.Clear();
            var lista = (_turnoAtual == "Não Definido")
                ? _alunosExibidos.OrderBy(a => a.Turno == "Tarde" ? 0 : 1).ThenBy(a => a.Nome).ToList()
                : _alunosExibidos.OrderBy(a => a.Nome).ToList();

            foreach (var aluno in lista)
            {
                int idx = dataGridView1.Rows.Add();
                var row = dataGridView1.Rows[idx];
                if (dataGridView1.Columns.Contains("Nome")) row.Cells["Nome"].Value = aluno.Nome;
                if (dataGridView1.Columns.Contains("Turno")) row.Cells["Turno"].Value = aluno.Turno;

                foreach (string tema in TemasAvaliacao)
                {
                    string col = ColName(tema);
                    if (!dataGridView1.Columns.Contains(col)) continue;
                    // Busca nota pelo tema — compatível com nomes antigos e novos
                    var nota = aluno.Notas.FirstOrDefault(n =>
                        n.Tema != null && n.Tema.Equals(tema, StringComparison.OrdinalIgnoreCase));
                    row.Cells[col].Value = nota != null
                        ? nota.Valor.ToString("F1", System.Globalization.CultureInfo.InvariantCulture)
                        : "";
                }

                double media = CalculadoraEstatistica.CalcularMediaAluno(aluno);
                if (dataGridView1.Columns.Contains("Media"))
                    row.Cells["Media"].Value = media.ToString("F1", System.Globalization.CultureInfo.InvariantCulture);
            }
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            var todosAlunos = GerenciadorDados.CarregarAlunos();

            foreach (DataGridViewRow linha in dataGridView1.Rows)
            {
                if (linha.IsNewRow) continue;
                string nome = linha.Cells["Nome"]?.Value?.ToString();
                string turno = linha.Cells["Turno"]?.Value?.ToString();
                if (string.IsNullOrEmpty(nome) || string.IsNullOrEmpty(turno)) continue;

                var aluno = todosAlunos.FirstOrDefault(a =>
                    a.Nome != null && a.Nome.Equals(nome, StringComparison.OrdinalIgnoreCase) &&
                    a.Turno != null && a.Turno.Equals(turno, StringComparison.OrdinalIgnoreCase));
                if (aluno == null) continue;

                foreach (string tema in TemasAvaliacao)
                {
                    string col = ColName(tema);
                    if (!dataGridView1.Columns.Contains(col)) continue;
                    string notaTxt = linha.Cells[col]?.Value?.ToString();

                    if (string.IsNullOrWhiteSpace(notaTxt))
                    {
                        aluno.Notas.RemoveAll(n => n.Tema != null &&
                            n.Tema.Equals(tema, StringComparison.OrdinalIgnoreCase));
                        continue;
                    }
                    if (!double.TryParse(notaTxt,
                        System.Globalization.NumberStyles.Any,
                        System.Globalization.CultureInfo.InvariantCulture,
                        out double val))
                    {
                        MessageBox.Show($"Nota inválida para '{nome}' em '{tema}': '{notaTxt}'.\nUse números (ex: 7.5).",
                            "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    var notaExist = aluno.Notas.FirstOrDefault(n =>
                        n.Tema != null && n.Tema.Equals(tema, StringComparison.OrdinalIgnoreCase));
                    if (notaExist != null) notaExist.Valor = val;
                    else aluno.Notas.Add(new RegistroNota { Tema = tema, Valor = val });
                }

                double media = CalculadoraEstatistica.CalcularMediaAluno(aluno);
                if (dataGridView1.Columns.Contains("Media"))
                    linha.Cells["Media"].Value = media.ToString("F1", System.Globalization.CultureInfo.InvariantCulture);
            }

            GerenciadorDados.SalvarAlunos(todosAlunos);
            MessageBox.Show("Notas salvas com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            CarregarAlunosParaExibicao();
            ConfigurarDataGridView();
            CarregarNotasNaTabela();
        }

        private void btnVoltar1_Click(object sender, EventArgs e) => this.Close();
    }
}