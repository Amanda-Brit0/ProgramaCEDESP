using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using iTextSharp.text; // Adicionado para iTextSharp
using iTextSharp.text.pdf; // Adicionado para iTextSharp
using System.IO; // Adicionado para manipulação de arquivos

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
                // 1. Carregar todos os dados necessários
                List<DadosAluno> todosAlunos = GerenciadorDados.CarregarAlunos();

                // Obter a lista de todas as datas de aula únicas para calcular a frequência total
                var todasDatasAulas = todosAlunos
                    .SelectMany(aluno => aluno.Frequencia)
                    .Select(f => f.Data.Date)
                    .Distinct()
                    .ToList();

                int totalAulas = todasDatasAulas.Count;

                // 2. Criar o documento PDF
                string nomeArquivo = $"Relatorio_Alunos_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";
                string caminhoCompleto = Path.Combine(Application.StartupPath, nomeArquivo);

                Document doc = new Document(PageSize.A4, 25, 25, 25, 25);
                PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(caminhoCompleto, FileMode.Create));

                doc.Open();

                // 3. Adicionar título e data
                // CORREÇÃO: Usando o namespace completo para Font
                iTextSharp.text.Font fontTitulo = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 16, iTextSharp.text.Font.BOLD);
                Paragraph titulo = new Paragraph("Relatório de Notas e Frequência dos Alunos\n\n", fontTitulo);
                titulo.Alignment = Element.ALIGN_CENTER;
                doc.Add(titulo);

                // CORREÇÃO: Usando o namespace completo para Font
                iTextSharp.text.Font fontData = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 10);
                Paragraph data = new Paragraph($"Gerado em: {DateTime.Now:dd/MM/yyyy HH:mm}\n\n", fontData);
                data.Alignment = Element.ALIGN_RIGHT;
                doc.Add(data);

                // 4. Criar a tabela com 3 colunas
                PdfPTable tabela = new PdfPTable(3);
                tabela.WidthPercentage = 100;
                // CORREÇÃO: O erro CS1503 pode ter sido causado por tentar passar strings aqui,
                // mas a sintaxe com 'f' já está correta. Apenas para garantir.
                tabela.SetWidths(new float[] { 40f, 30f, 30f }); // Define as larguras das colunas

                // Adicionar cabeçalhos da tabela
                PdfPCell cell;
                // CORREÇÃO: Usando o namespace completo para Font
                iTextSharp.text.Font fontCabecalho = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 10, iTextSharp.text.Font.BOLD);

                cell = new PdfPCell(new Phrase("Nome do Aluno", fontCabecalho));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                tabela.AddCell(cell);

                cell = new PdfPCell(new Phrase("Frequência (%)", fontCabecalho));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                tabela.AddCell(cell);

                cell = new PdfPCell(new Phrase("Média", fontCabecalho));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                tabela.AddCell(cell);

                // 5. Preencher a tabela com os dados dos alunos
                // CORREÇÃO: Usando o namespace completo para Font
                iTextSharp.text.Font fontCorpo = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 10);
                var alunosOrdenados = todosAlunos.OrderBy(a => a.Nome).ToList();

                foreach (DadosAluno aluno in alunosOrdenados)
                {
                    // A coluna 1 (Nome)
                    tabela.AddCell(new PdfPCell(new Phrase(aluno.Nome, fontCorpo)));

                    // A coluna 2 (Frequência)
                    double frequencia = CalculadoraEstatistica.CalcularFrequenciaPercentual(aluno, totalAulas);
                    cell = new PdfPCell(new Phrase(frequencia.ToString("F1") + "%", fontCorpo));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    tabela.AddCell(cell);

                    // A coluna 3 (Média)
                    double media = CalculadoraEstatistica.CalcularMediaAluno(aluno);
                    cell = new PdfPCell(new Phrase(media.ToString("F1"), fontCorpo));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    tabela.AddCell(cell);
                }

                // 6. Adicionar a tabela ao documento e fechar
                doc.Add(tabela);
                doc.Close();

                MessageBox.Show($"Relatório gerado com sucesso!\nO arquivo foi salvo em: {caminhoCompleto}", "Relatório Gerado", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocorreu um erro ao gerar o relatório: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RelatorioAdmin_Load(object sender, EventArgs e)
        {

        }
    }
}