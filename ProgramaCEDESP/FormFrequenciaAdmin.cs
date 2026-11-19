using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ProgramaCEDESP
{
    public partial class FormFrequenciaAdmin : Form
    {
        private List<DadosAluno> _alunos;
        private List<DateTime> _datasAulas;
        private string _turnoAtual;

        public FormFrequenciaAdmin(string turnoAdmin)
        {
            InitializeComponent();
            _turnoAtual = turnoAdmin;

            if (_turnoAtual == "Não Definido")
            {
                _alunos = GerenciadorDados.CarregarAlunos();
            }
            else
            {
                _alunos = GerenciadorDados.CarregarAlunos()
                                             .Where(a => a.Turno == _turnoAtual)
                                             .ToList();
            }

            CarregarDatasAulasExistentes();
            ConfigurarDataGridView();
            CarregarFrequenciaNaTabela();
            this.Text = $"Administração de Frequência - Turno: {(_turnoAtual == "Não Definido" ? "Todos" : _turnoAtual)}";
        }

        public FormFrequenciaAdmin()
        {
            InitializeComponent();
            _turnoAtual = "Não Definido";
            _alunos = GerenciadorDados.CarregarAlunos();
            CarregarDatasAulasExistentes();
            ConfigurarDataGridView();
            CarregarFrequenciaNaTabela();
            this.Text = "Administração de Frequência - Todos os Turnos";
        }

        private void CarregarDatasAulasExistentes()
        {
            _datasAulas = _alunos
                .SelectMany(aluno => aluno.Frequencia)
                .Select(f => f.Data.Date)
                .Distinct()
                .OrderBy(d => d)
                .ToList();
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

            foreach (DateTime dataAula in _datasAulas)
            {
                dataGridView1.Columns.Add(new DataGridViewTextBoxColumn()
                {
                    Name = dataAula.ToString("yyyyMMdd"),
                    HeaderText = dataAula.ToString("dd/MM"),
                    Width = 70,
                    MaxInputLength = 1
                });
            }

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "TotalAulas",
                HeaderText = "Total Aulas",
                ReadOnly = true,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "FrequenciaPercentual",
                HeaderText = "Frequência (%)",
                ReadOnly = true,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            });

            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;

            foreach (DataGridViewColumn col in dataGridView1.Columns)
            {
                if (col.Name.StartsWith("20"))
                {
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
            }
        }

        private void CarregarFrequenciaNaTabela()
        {
            dataGridView1.Rows.Clear();

            var alunosOrdenados = (_turnoAtual == "Não Definido") ?
                _alunos.OrderBy(a => a.Turno == "Tarde" ? 0 : (a.Turno == "Noite" ? 1 : 2)).ThenBy(a => a.Nome).ToList() :
                _alunos.OrderBy(a => a.Nome).ToList();

            foreach (var aluno in alunosOrdenados)
            {
                int rowIndex = dataGridView1.Rows.Add();
                DataGridViewRow row = dataGridView1.Rows[rowIndex];

                row.Cells["Nome"].Value = aluno.Nome;
                row.Cells["Turno"].Value = aluno.Turno;

                foreach (DateTime dataAtual in _datasAulas)
                {
                    RegistroFrequencia freqExistente = aluno.Frequencia.FirstOrDefault(f => f.Data.Date == dataAtual.Date);
                    string columnName = dataAtual.ToString("yyyyMMdd");

                    if (dataGridView1.Columns.Contains(columnName))
                    {
                        row.Cells[columnName].Value = freqExistente != null ? freqExistente.Status.ToString().ToUpper() : "";
                    }
                }

                int totalAulasConsideradas = _datasAulas.Count;

                if (dataGridView1.Columns.Contains("TotalAulas"))
                {
                    row.Cells["TotalAulas"].Value = totalAulasConsideradas;
                }

                if (dataGridView1.Columns.Contains("FrequenciaPercentual"))
                {
                    // Usa o novo método da classe utilitária
                    double frequenciaPercentual = CalculadoraEstatistica.CalcularFrequenciaPercentual(aluno, totalAulasConsideradas);
                    row.Cells["FrequenciaPercentual"].Value = frequenciaPercentual.ToString("F1") + "%";
                }
            }
        }

        private void btnAddAula_Click(object sender, EventArgs e)
        {
            dtpNovaAula.Visible = !dtpNovaAula.Visible;
            dtpNovaAula.BringToFront();
        }

        private void dtpNovaAula_ValueChanged(object sender, EventArgs e)
        {
            DateTime novaDataAula = dtpNovaAula.Value.Date;

            if (_datasAulas.Contains(novaDataAula))
            {
                MessageBox.Show($"A data {novaDataAula.ToString("dd/MM/yyyy")} já foi adicionada como data de aula.", "Data Já Existe", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                _datasAulas.Add(novaDataAula);
                _datasAulas = _datasAulas.OrderBy(d => d).ToList();

                foreach (var aluno in _alunos)
                {
                    if (!aluno.Frequencia.Any(f => f.Data.Date == novaDataAula.Date))
                    {
                        aluno.Frequencia.Add(new RegistroFrequencia { Data = novaDataAula, Status = 'F' });
                    }
                }

                List<DadosAluno> todosAlunosAtuais = GerenciadorDados.CarregarAlunos();
                foreach (var alunoModificado in _alunos)
                {
                    var alunoOriginal = todosAlunosAtuais.FirstOrDefault(a => a.Usuario == alunoModificado.Usuario);
                    if (alunoOriginal != null)
                    {
                        alunoOriginal.Frequencia = alunoModificado.Frequencia;
                    }
                }
                GerenciadorDados.SalvarAlunos(todosAlunosAtuais);

                ConfigurarDataGridView();
                CarregarFrequenciaNaTabela();

                MessageBox.Show($"Data {novaDataAula.ToString("dd/MM/yyyy")} adicionada com sucesso! Você pode agora preencher a frequência para esta data.", "Data Adicionada", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            dtpNovaAula.Visible = false;
        }

        private void btnAddAula_MouseDown(object sender, MouseEventArgs e)
        {
            btnAddAula.Size = new System.Drawing.Size(63, 61);
            btnAddAula.Location = new System.Drawing.Point(285, 5);
        }

        private void btnAddAula_MouseUp(object sender, MouseEventArgs e)
        {
            btnAddAula.Size = new System.Drawing.Size(65, 63);
            btnAddAula.Location = new System.Drawing.Point(284, 4);
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            List<DadosAluno> todosAlunosParaSalvar = GerenciadorDados.CarregarAlunos();

            foreach (DataGridViewRow linha in dataGridView1.Rows)
            {
                if (linha.IsNewRow) continue;

                string nomeAluno = linha.Cells["Nome"].Value?.ToString();
                string turnoAlunoLinha = linha.Cells["Turno"].Value?.ToString();

                DadosAluno aluno = todosAlunosParaSalvar.FirstOrDefault(a => a.Nome == nomeAluno && a.Turno == turnoAlunoLinha);

                if (aluno == null) continue;

                foreach (DateTime dataDaColuna in _datasAulas)
                {
                    string columnName = dataDaColuna.ToString("yyyyMMdd");

                    if (!dataGridView1.Columns.Contains(columnName)) continue;

                    string statusTexto = linha.Cells[columnName].Value?.ToString()?.ToUpper();

                    if (string.IsNullOrWhiteSpace(statusTexto))
                    {
                        aluno.Frequencia.RemoveAll(f => f.Data.Date == dataDaColuna.Date);
                        continue;
                    }

                    char statusFrequencia;
                    if (statusTexto == "P")
                    {
                        statusFrequencia = 'P';
                    }
                    else if (statusTexto == "F")
                    {
                        statusFrequencia = 'F';
                    }
                    else
                    {
                        MessageBox.Show($"Valor de frequência inválido para o aluno '{nomeAluno}' na data '{dataDaColuna.ToString("dd/MM")}': '{statusTexto}'. Por favor, digite 'P' ou 'F'.", "Erro de Entrada", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        continue;
                    }

                    RegistroFrequencia freqExistente = aluno.Frequencia.FirstOrDefault(f => f.Data.Date == dataDaColuna.Date);

                    if (freqExistente != null)
                    {
                        if (freqExistente.Status != statusFrequencia)
                        {
                            freqExistente.Status = statusFrequencia;
                        }
                    }
                    else
                    {
                        aluno.Frequencia.Add(new RegistroFrequencia { Data = dataDaColuna, Status = statusFrequencia });
                    }
                }
            }

            GerenciadorDados.SalvarAlunos(todosAlunosParaSalvar);
            MessageBox.Show("Frequência registrada e atualizada com sucesso!");

            if (_turnoAtual == "Não Definido")
            {
                _alunos = GerenciadorDados.CarregarAlunos();
            }
            else
            {
                _alunos = GerenciadorDados.CarregarAlunos()
                                             .Where(a => a.Turno == _turnoAtual)
                                             .ToList();
            }

            CarregarDatasAulasExistentes();
            ConfigurarDataGridView();
            CarregarFrequenciaNaTabela();
        }

        private void btnVoltar1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FormFrequenciaAdmin_Load(object sender, EventArgs e)
        {
        }
    }
}