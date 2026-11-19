using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace ProgramaCEDESP
{
    public partial class RelatorioAdmin : Form
    {
        public RelatorioAdmin()
        {
            InitializeComponent();
        }

        private void imgRelatorio_Click(object sender, EventArgs e)
        {
            try
            {
                var todosAlunos = GerenciadorDados.CarregarAlunos();
                string nome = $"Relatorio_Alunos_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";
                string caminho = Path.Combine(Application.StartupPath, nome);

                Document doc = new Document(PageSize.A4, 25, 25, 30, 30);
                PdfWriter.GetInstance(doc, new FileStream(caminho, FileMode.Create));
                doc.Open();

                iTextSharp.text.Font fTitulo = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 16, iTextSharp.text.Font.BOLD);
                iTextSharp.text.Font fSubtitulo = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 13, iTextSharp.text.Font.BOLD);
                iTextSharp.text.Font fData = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 10);
                iTextSharp.text.Font fCabecalho = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 10, iTextSharp.text.Font.BOLD);
                iTextSharp.text.Font fCorpo = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 10);

                bool primeira = true;
                foreach (string turno in new[] { "Tarde", "Noite" })
                {
                    var alunos = todosAlunos
                        .Where(a => a.Turno != null && a.Turno.Equals(turno, StringComparison.OrdinalIgnoreCase))
                        .OrderBy(a => a.Nome).ToList();
                    if (!alunos.Any()) continue;

                    if (!primeira) doc.NewPage();
                    primeira = false;

                    int totalAulas = alunos
                        .SelectMany(a => a.Frequencia)
                        .Select(f => f.Data.Date).Distinct().Count();

                    var pTitulo = new Paragraph("Relatório de Notas e Frequência\n", fTitulo);
                    pTitulo.Alignment = Element.ALIGN_CENTER;
                    doc.Add(pTitulo);

                    var pSub = new Paragraph($"Turno: {turno}\n\n", fSubtitulo);
                    pSub.Alignment = Element.ALIGN_CENTER;
                    doc.Add(pSub);

                    var pData = new Paragraph($"Gerado em: {DateTime.Now:dd/MM/yyyy HH:mm}\n\n", fData);
                    pData.Alignment = Element.ALIGN_RIGHT;
                    doc.Add(pData);

                    PdfPTable tabela = new PdfPTable(3);
                    tabela.WidthPercentage = 100;
                    tabela.SetWidths(new float[] { 50f, 25f, 25f });

                    foreach (string cab in new[] { "Nome do Aluno", "Frequência (%)", "Média" })
                    {
                        var cell = new PdfPCell(new Phrase(cab, fCabecalho));
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                        tabela.AddCell(cell);
                    }

                    foreach (var aluno in alunos)
                    {
                        tabela.AddCell(new PdfPCell(new Phrase(aluno.Nome, fCorpo)));
                        double freq = CalculadoraEstatistica.CalcularFrequenciaPercentual(aluno, totalAulas);
                        var cFreq = new PdfPCell(new Phrase(freq.ToString("F1") + "%", fCorpo));
                        cFreq.HorizontalAlignment = Element.ALIGN_CENTER;
                        tabela.AddCell(cFreq);
                        double media = CalculadoraEstatistica.CalcularMediaAluno(aluno);
                        var cMedia = new PdfPCell(new Phrase(media.ToString("F1"), fCorpo));
                        cMedia.HorizontalAlignment = Element.ALIGN_CENTER;
                        tabela.AddCell(cMedia);
                    }
                    doc.Add(tabela);
                }

                doc.Close();
                MessageBox.Show($"Relatório gerado!\nSalvo em: {caminho}",
                    "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao gerar relatório: {ex.Message}",
                    "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RelatorioAdmin_Load(object sender, EventArgs e) { }
    }
}