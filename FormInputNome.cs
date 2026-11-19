using System;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Data;

namespace ProgramaCEDESP
{
    public partial class FormInputNome : Form
    {
        public string NomeCompleto { get; private set; }

        public FormInputNome()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string nomeDigitado = txtNome.Text.Trim();
            if (string.IsNullOrWhiteSpace(nomeDigitado))
            {
                MessageBox.Show("Por favor, digite o nome completo.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Aqui é feita a verificação com o nome completo.
            if (GerenciadorDados.VerificarNomeCompleto(nomeDigitado))
            {
                MessageBox.Show("Um aluno com este nome completo já está cadastrado. Por favor, verifique.", "Erro de Cadastro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            this.NomeCompleto = nomeDigitado;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void FormInputNome_Load(object sender, EventArgs e)
        {
        }
    }
}