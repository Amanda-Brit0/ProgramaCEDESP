using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace ProgramaCEDESP
{
    public partial class FormFrequenciaAdmin : Form
    {
        private List<DadosAluno> _alunos;
        private string _turnoAtual;

        private Label _lblAlunoSelecionado;
        private Label _lblInstrucao;
        private TextBox _txtDatas;
        private Button _btnConfirmarFaltas;
        private Label _lblFaltasAtuais;
        private ListBox _lstFaltasAtuais;
        private Button _btnRemoverFaltaSelecionada;
        private DataGridView _grid;
        private Button _btnVoltar;

        private DadosAluno _alunoSelecionado = null;

        public FormFrequenciaAdmin(string turnoAdmin)
        {
            InitializeComponent();
            _turnoAtual = turnoAdmin;
            CarregarAlunos();
            ConstruirInterface();
            AtualizarGrid();
            this.Text = $"Frequência - Turno: {(_turnoAtual == "Não Definido" ? "Todos" : _turnoAtual)}";
            this.Size = new Size(920, 660);
        }

        public FormFrequenciaAdmin()
        {
            InitializeComponent();
            _turnoAtual = "Não Definido";
            CarregarAlunos();
            ConstruirInterface();
            AtualizarGrid();
            this.Text = "Frequência - Todos os Turnos";
            this.Size = new Size(920, 660);
        }

        private void CarregarAlunos()
        {
            var todos = GerenciadorDados.CarregarAlunos();
            _alunos = (_turnoAtual == "Não Definido")
                ? todos
                : todos.Where(a => a.Turno == _turnoAtual).ToList();
        }

        private void ConstruirInterface()
        {
            this.Controls.Clear();

            var lblTitulo = new Label()
            {
                Text = "Registro de Faltas",
                Font = new Font("Segoe UI", 13, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(12, 10)
            };
            this.Controls.Add(lblTitulo);

            var lblDica = new Label()
            {
                Text = "Clique no nome de um aluno para registrar as faltas dele. Presenca é padrão — só lance quem faltou.",
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.DimGray,
                AutoSize = true,
                Location = new Point(12, 40)
            };
            this.Controls.Add(lblDica);

            // --- Grid de resumo (esquerda) ---
            _grid = new DataGridView()
            {
                Location = new Point(12, 68),
                Size = new Size(400, 520),
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White
            };
            _grid.Columns.Add("Nome", "Nome do Aluno");
            _grid.Columns.Add("Turno", "Turno");
            _grid.Columns.Add("Faltas", "Faltas");
            _grid.Columns["Faltas"].FillWeight = 35;
            _grid.CellClick += Grid_CellClick;
            this.Controls.Add(_grid);

            // --- Painel de lançamento (direita) ---
            var painel = new Panel()
            {
                Location = new Point(425, 68),
                Size = new Size(475, 520),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.White
            };
            this.Controls.Add(painel);

            _lblAlunoSelecionado = new Label()
            {
                Text = "Nenhum aluno selecionado",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.DarkBlue,
                AutoSize = false,
                Size = new Size(455, 28),
                Location = new Point(8, 10),
                TextAlign = ContentAlignment.MiddleLeft
            };
            painel.Controls.Add(_lblAlunoSelecionado);

            _lblInstrucao = new Label()
            {
                Text = "Digite as datas de falta separadas por virgula:\r\nExemplo: 14/03, 22/03, 05/04",
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.DimGray,
                AutoSize = false,
                Size = new Size(455, 40),
                Location = new Point(8, 44)
            };
            painel.Controls.Add(_lblInstrucao);

            _txtDatas = new TextBox()
            {
                Location = new Point(8, 90),
                Size = new Size(340, 24),
                Font = new Font("Segoe UI", 10),
                Text = ""
            };
            painel.Controls.Add(_txtDatas);

            _btnConfirmarFaltas = new Button()
            {
                Text = "Adicionar",
                Location = new Point(355, 89),
                Size = new Size(108, 26),
                BackColor = Color.SteelBlue,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };
            _btnConfirmarFaltas.Click += BtnConfirmarFaltas_Click;
            painel.Controls.Add(_btnConfirmarFaltas);

            _lblFaltasAtuais = new Label()
            {
                Text = "Faltas ja registradas para este aluno:",
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(8, 125)
            };
            painel.Controls.Add(_lblFaltasAtuais);

            _lstFaltasAtuais = new ListBox()
            {
                Location = new Point(8, 147),
                Size = new Size(455, 300),
                Font = new Font("Segoe UI", 10),
                SelectionMode = SelectionMode.MultiExtended
            };
            painel.Controls.Add(_lstFaltasAtuais);

            _btnRemoverFaltaSelecionada = new Button()
            {
                Text = "Remover data(s) selecionada(s)",
                Location = new Point(8, 455),
                Size = new Size(455, 30),
                BackColor = Color.Firebrick,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };
            _btnRemoverFaltaSelecionada.Click += BtnRemoverFaltaSelecionada_Click;
            painel.Controls.Add(_btnRemoverFaltaSelecionada);

            _btnVoltar = new Button()
            {
                Text = "Voltar",
                Location = new Point(12, 600),
                Size = new Size(90, 28),
                FlatStyle = FlatStyle.Flat
            };
            _btnVoltar.Click += (s, e) => this.Close();
            this.Controls.Add(_btnVoltar);
        }

        private void AtualizarGrid()
        {
            _grid.Rows.Clear();
            var lista = (_turnoAtual == "Não Definido")
                ? _alunos.OrderBy(a => a.Turno == "Tarde" ? 0 : 1).ThenBy(a => a.Nome).ToList()
                : _alunos.OrderBy(a => a.Nome).ToList();

            foreach (var aluno in lista)
            {
                int faltas = aluno.Frequencia?.Count(f => f.Status == 'F') ?? 0;
                int row = _grid.Rows.Add(aluno.Nome, aluno.Turno, faltas);
                if (faltas >= 5)
                    _grid.Rows[row].DefaultCellStyle.BackColor = Color.MistyRose;
                else if (faltas > 0)
                    _grid.Rows[row].DefaultCellStyle.BackColor = Color.LightYellow;
            }
        }

        private void Grid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            string nome = _grid.Rows[e.RowIndex].Cells["Nome"].Value?.ToString();
            string turno = _grid.Rows[e.RowIndex].Cells["Turno"].Value?.ToString();
            _alunoSelecionado = _alunos.FirstOrDefault(a => a.Nome == nome && a.Turno == turno);
            if (_alunoSelecionado == null) return;
            _lblAlunoSelecionado.Text = $"{_alunoSelecionado.Nome} ({_alunoSelecionado.Turno})";
            _txtDatas.Text = "";
            AtualizarListaFaltasDoAluno();
        }

        private void AtualizarListaFaltasDoAluno()
        {
            _lstFaltasAtuais.Items.Clear();
            if (_alunoSelecionado == null) return;
            var faltas = _alunoSelecionado.Frequencia?
                .Where(f => f.Status == 'F').OrderBy(f => f.Data).ToList();
            if (faltas == null || !faltas.Any())
            {
                _lstFaltasAtuais.Items.Add("(nenhuma falta registrada)");
                return;
            }
            foreach (var f in faltas)
                _lstFaltasAtuais.Items.Add(f.Data.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture));
        }

        private void BtnConfirmarFaltas_Click(object sender, EventArgs e)
        {
            if (_alunoSelecionado == null)
            {
                MessageBox.Show("Selecione um aluno na lista primeiro.", "Atenção",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string texto = _txtDatas.Text.Trim();
            if (string.IsNullOrWhiteSpace(texto))
            {
                MessageBox.Show("Digite ao menos uma data.", "Atenção",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var partes = texto.Split(new char[] { ',', ';', '|' }, StringSplitOptions.RemoveEmptyEntries);
            var datasValidas = new List<DateTime>();
            var datasInvalidas = new List<string>();
            int anoAtual = DateTime.Now.Year;

            foreach (string parte in partes)
            {
                string p = parte.Trim();
                DateTime dt;
                bool ok = DateTime.TryParseExact(p,
                    new[] { "dd/MM/yyyy", "d/M/yyyy", "dd/MM", "d/M" },
                    new CultureInfo("pt-BR"),
                    DateTimeStyles.None, out dt);
                if (ok)
                {
                    if (dt.Year <= 1) dt = new DateTime(anoAtual, dt.Month, dt.Day);
                    datasValidas.Add(dt.Date);
                }
                else datasInvalidas.Add(p);
            }

            if (datasInvalidas.Any())
                MessageBox.Show("Datas não reconhecidas (serão ignoradas):\n" + string.Join(", ", datasInvalidas) +
                    "\n\nUse o formato dd/MM ou dd/MM/yyyy.", "Datas invalidas",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);

            if (!datasValidas.Any()) return;

            var todosAlunos = GerenciadorDados.CarregarAlunos();
            var alunoArq = todosAlunos.FirstOrDefault(a =>
                a.Nome == _alunoSelecionado.Nome && a.Turno == _alunoSelecionado.Turno);
            if (alunoArq == null) return;

            int adicionadas = 0;
            foreach (var data in datasValidas)
            {
                var reg = alunoArq.Frequencia.FirstOrDefault(f => f.Data.Date == data);
                if (reg != null) { if (reg.Status != 'F') { reg.Status = 'F'; adicionadas++; } }
                else { alunoArq.Frequencia.Add(new RegistroFrequencia { Data = data, Status = 'F' }); adicionadas++; }
            }

            GerenciadorDados.SalvarAlunos(todosAlunos);
            _alunoSelecionado = alunoArq;
            CarregarAlunos();
            _txtDatas.Text = "";
            AtualizarListaFaltasDoAluno();
            AtualizarGrid();
            MessageBox.Show(adicionadas > 0
                ? $"{adicionadas} falta(s) registrada(s) para {_alunoSelecionado.Nome}."
                : "Nenhuma data nova (já estavam registradas).",
                "Faltas Registradas", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnRemoverFaltaSelecionada_Click(object sender, EventArgs e)
        {
            if (_alunoSelecionado == null || _lstFaltasAtuais.SelectedIndices.Count == 0)
            {
                MessageBox.Show("Selecione uma ou mais faltas na lista para remover.", "Atenção",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (_lstFaltasAtuais.Items[0].ToString().StartsWith("(")) return;

            var datasRemover = new List<DateTime>();
            foreach (int idx in _lstFaltasAtuais.SelectedIndices)
            {
                string item = _lstFaltasAtuais.Items[idx].ToString();
                if (DateTime.TryParseExact(item, "dd/MM/yyyy",
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt))
                    datasRemover.Add(dt.Date);
            }
            if (!datasRemover.Any()) return;

            var todosAlunos = GerenciadorDados.CarregarAlunos();
            var alunoArq = todosAlunos.FirstOrDefault(a =>
                a.Nome == _alunoSelecionado.Nome && a.Turno == _alunoSelecionado.Turno);
            if (alunoArq == null) return;

            alunoArq.Frequencia.RemoveAll(f => f.Status == 'F' && datasRemover.Contains(f.Data.Date));
            GerenciadorDados.SalvarAlunos(todosAlunos);
            _alunoSelecionado = alunoArq;
            CarregarAlunos();
            AtualizarListaFaltasDoAluno();
            AtualizarGrid();
            MessageBox.Show($"{datasRemover.Count} falta(s) removida(s).",
                "Removido", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void FormFrequenciaAdmin_Load(object sender, EventArgs e) { }
    }
}