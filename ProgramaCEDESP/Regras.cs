using System.Windows.Forms; // Apenas o namespace mínimo necessário

namespace ProgramaCEDESP
{
    // O nome da classe deve ser 'Regras' para corresponder ao nome do arquivo Regras.cs
    public partial class Regras : Form
    {
        public Regras()
        {
            InitializeComponent();
            // NÃO HÁ CÓDIGO AQUI PARA CARREGAR REGRAS,
            // pois você já as colocou no RichTextBox pelo designer.
            // As propriedades StartPosition, MaximizeBox, etc.,
            // também já devem estar configuradas no designer.
        }

        // Se você adicionou um botão "Fechar" e o nomeou como 'btnFechar' no designer
        private void btnFechar_Click(object sender, System.EventArgs e)
        {
            this.Close(); // Fecha esta tela
        }
    }
}