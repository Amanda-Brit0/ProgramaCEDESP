using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ProgramaCEDESP
{
    public partial class FreqAluno : Form
    {
        private string _nomeAluno;

        public FreqAluno(string nomeAluno)
        {
            InitializeComponent();
            _nomeAluno = nomeAluno;
            this.Text = $"CEDESP - Frequência de {_nomeAluno}";
            CarregarFrequenciaDoAluno();
        }

        private void CarregarFrequenciaDoAluno()
        {
            var todosAlunos = GerenciadorDados.CarregarAlunos();
            var aluno = todosAlunos.FirstOrDefault(a => a.Nome == _nomeAluno);

            if (aluno != null)
            {
                dataGridViewFrequencia.Columns.Clear();
                dataGridViewFrequencia.Rows.Clear();
                dataGridViewFrequencia.Columns.Add("DataDaFalta", "Data da Falta");
                dataGridViewFrequencia.Columns["DataDaFalta"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                var faltas = aluno.Frequencia?
                    .Where(r => r.Status == 'F').OrderBy(r => r.Data).ToList();
                int totalFaltas = faltas?.Count ?? 0;

                if (faltas != null && faltas.Any())
                    foreach (var f in faltas)
                        dataGridViewFrequencia.Rows.Add(f.Data.ToString("dd/MM/yyyy"));
                else
                    dataGridViewFrequencia.Rows.Add("Nenhuma falta registrada ate o momento.");

                lblPorcentagemFrequencia.Text = $"Total de Faltas: {totalFaltas}";
                lblPorcentagemFrequencia.Font = new Font(lblPorcentagemFrequencia.Font, FontStyle.Bold);
                lblPorcentagemFrequencia.ForeColor = totalFaltas > 0 ? Color.DarkRed : Color.DarkGreen;

                totalAulas.Text = "(somente faltas lancadas pelo professor sao exibidas)";
                totalAulas.ForeColor = Color.DimGray;
            }
            else
            {
                MessageBox.Show("Aluno não encontrado.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        private void btnVoltar_Click(object sender, EventArgs e) => this.Close();
        private void FreqAluno_Load(object sender, EventArgs e) { }
        private void lblPorcentagemFrequencia_Click(object sender, EventArgs e) { }
    }
}