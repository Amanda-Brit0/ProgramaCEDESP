using System;
using System.IO; // Necessário para Directory
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics; // Necessário para Process.Start

namespace ProgramaCEDESP
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            // AQUI VOCÊ PODE CONFIGURAR O TEXTO DOS LINKS SE QUISER FAZER VIA CÓDIGO
            // this.linkLabel1.Text = "Visite nosso Instagram Principal";
            // this.linkCidadania.Text = "Visite nosso Instagram de Cidadania";
            // Os eventos Click para linkLabel1 e linkCidadania já são gerados automaticamente pelo designer.
        }

        private void button1_Click(object sender, EventArgs e) // Botão de Login
        {
            string usuarioDigitado = txtUsuario.Text;
            string senhaDigitada = txtSenha.Text;

            // Dados de login do administrador
            const string adminUsuario = "Dom@1415";
            const string adminSenha = "Dom@1415";

            // Garante que um turno seja selecionado ANTES de qualquer login.
            string turnoSelecionado = "";
            if (radioButton1.Checked)
            {
                turnoSelecionado = "Tarde";
            }
            else if (radioButton2.Checked)
            {
                turnoSelecionado = "Noite";
            }
            else
            {
                MessageBox.Show("Por favor, selecione um turno (Tarde ou Noite) para o login.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // Sai se nenhum turno for selecionado.
            }

            // ***** VERIFICA O LOGIN DO ADMINISTRADOR *****
            if (usuarioDigitado == adminUsuario && senhaDigitada == adminSenha)
            {
                // Se for o administrador, abre a TelaAdmin e PASSA O TURNO SELECIONADO
                TelaAdmin telaAdmin = new TelaAdmin(turnoSelecionado); // Passa o turno para TelaAdmin
                this.Hide();
                telaAdmin.Show();
                return;
            }

            // ***** LÓGICA PARA LOGIN DE ALUNO (SÓ É EXECUTADA SE NÃO FOR ADMIN) *****
            List<DadosAluno> todosAlunos = GerenciadorDados.CarregarAlunos();
            DadosAluno alunoLogado = todosAlunos.FirstOrDefault(a =>
                a != null &&
                a.Usuario != null && a.Usuario.Equals(usuarioDigitado, StringComparison.OrdinalIgnoreCase) &&
                a.Senha != null && a.Senha.Equals(senhaDigitada, StringComparison.OrdinalIgnoreCase) &&
                a.Turno != null && a.Turno.Equals(turnoSelecionado, StringComparison.OrdinalIgnoreCase));

            if (alunoLogado != null)
            {
                // Login de aluno bem-sucedido
                // Passa o nome completo, o turno E O NOME DE USUÁRIO DE LOGIN para a TelaPrincipal
                TelaPrincipal telaPrincipal = new TelaPrincipal(alunoLogado.Nome, alunoLogado.Turno, alunoLogado.Usuario);
                this.Hide();
                telaPrincipal.Show();
            }
            else
            {
                MessageBox.Show("Usuário, senha ou turno inválidos.", "Erro de Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e) // Botão de Cadastro
        {
            string novoUsuarioAluno = txtUsuario.Text.Trim();
            string novaSenhaAluno = txtSenha.Text.Trim();

            if (string.IsNullOrWhiteSpace(novoUsuarioAluno) || string.IsNullOrWhiteSpace(novaSenhaAluno))
            {
                MessageBox.Show("Por favor, preencha o Usuário e a Senha para o cadastro do aluno.", "Dados Incompletos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (FormInputNome formInput = new FormInputNome())
            {
                if (formInput.ShowDialog() == DialogResult.OK)
                {
                    string nomeCompletoAluno = formInput.NomeCompleto.Trim();

                    if (string.IsNullOrWhiteSpace(nomeCompletoAluno))
                    {
                        MessageBox.Show("O nome completo do aluno não pode ser vazio.", "Dados Incompletos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    string turnoSelecionado = "";
                    if (radioButton1.Checked)
                    {
                        turnoSelecionado = "Tarde";
                    }
                    else if (radioButton2.Checked)
                    {
                        turnoSelecionado = "Noite";
                    }
                    else
                    {
                        MessageBox.Show("Por favor, selecione um turno (Tarde ou Noite) para o cadastro do aluno.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    List<DadosAluno> todosAlunos = GerenciadorDados.CarregarAlunos();

                    // Verifica se o usuário (username) já existe para evitar duplicatas
                    bool usuarioJaExiste = todosAlunos.Any(a =>
                        a != null &&
                        a.Usuario != null && a.Usuario.Equals(novoUsuarioAluno, StringComparison.OrdinalIgnoreCase));

                    if (!usuarioJaExiste)
                    {
                        DadosAluno novoAluno = new DadosAluno
                        {
                            Nome = nomeCompletoAluno,
                            Usuario = novoUsuarioAluno,
                            Senha = novaSenhaAluno,
                            Turno = turnoSelecionado, // Aqui é usado
                            Frequencia = new List<RegistroFrequencia>(),
                            Notas = new List<RegistroNota>()
                        };

                        todosAlunos.Add(novoAluno);
                        GerenciadorDados.SalvarAlunos(todosAlunos);

                        MessageBox.Show($"Aluno '{nomeCompletoAluno}' (Usuário: '{novoUsuarioAluno}') do turno '{turnoSelecionado}' cadastrado com sucesso!", "Cadastro Concluído", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Mantido o caminho @"\\server\" direto, como estava no seu código original
                        try
                        {
                            string caminhoPastaAlunoNoServidor = Path.Combine(@"\\server\", turnoSelecionado, novoUsuarioAluno); // Aqui é usado

                            if (!Directory.Exists(caminhoPastaAlunoNoServidor))
                            {
                                Directory.CreateDirectory(caminhoPastaAlunoNoServidor);
                                MessageBox.Show($"Pasta '{novoUsuarioAluno}' criada com sucesso em '{Path.Combine(@"\\server\", turnoSelecionado)}' no servidor!", "Criação de Pasta", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                MessageBox.Show($"A pasta '{novoUsuarioAluno}' para o aluno '{nomeCompletoAluno}' já existia em '{Path.Combine(@"\\server\", turnoSelecionado)}' no servidor.", "Criação de Pasta", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Atenção: Não foi possível criar a pasta do aluno no servidor.\nErro: {ex.Message}\nVerifique as permissões de rede e o caminho: {Path.Combine(@"\\server\", turnoSelecionado, novoUsuarioAluno)}", "Erro na Criação da Pasta", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    else
                    {
                        MessageBox.Show($"O usuário '{novoUsuarioAluno}' já está cadastrado. Por favor, escolha outro usuário.", "Usuário Existente", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }

        private void txtUsuario_Enter(object sender, EventArgs e)
        {
            if (txtUsuario.Text == "Usuário")
            {
                txtUsuario.Text = "";
                txtUsuario.ForeColor = SystemColors.WindowText;
            }
        }

        private void txtUsuario_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsuario.Text))
            {
                txtUsuario.Text = "Usuário";
                txtUsuario.ForeColor = SystemColors.GrayText;
            }
        }

        private void txtSenha_Enter(object sender, EventArgs e)
        {
            if (txtSenha.Text == "Senha")
            {
                txtSenha.Text = "";
                txtSenha.ForeColor = SystemColors.WindowText;
                txtSenha.UseSystemPasswordChar = true;
            }
        }

        private void txtSenha_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSenha.Text))
            {
                txtSenha.Text = "Senha";
                txtSenha.ForeColor = SystemColors.GrayText;
                txtSenha.UseSystemPasswordChar = false;
            }
        }

        private void Botoes_MouseDown(object sender, MouseEventArgs e)
        {
            Control botao = (Control)sender;
            botao.Size = new Size(botao.Width - 5, botao.Height - 5);
            botao.Location = new Point(botao.Location.X + 2, botao.Location.Y + 2);
        }

        private void Botoes_MouseUp(object sender, MouseEventArgs e)
        {
            Control botao = (Control)sender;
            botao.Size = new Size(botao.Width + 5, botao.Height + 5);
            botao.Location = new Point(botao.Location.X - 2, botao.Location.Y - 2);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            txtUsuario_Leave(txtUsuario, EventArgs.Empty);
            txtSenha_Leave(txtSenha, EventArgs.Empty);
        }

        private void btnLive_Click(object sender, EventArgs e)
        {
            string url = "";
            string turnoSelecionado = "";

            if (radioButton1.Checked)
            {
                turnoSelecionado = "Tarde";
                url = "https://meet.jit.si/cedesp-tarde";
            }
            else if (radioButton2.Checked)
            {
                turnoSelecionado = "Noite";
                url = "https://meet.jit.si/cedesp-noite";
            }
            else
            {
                MessageBox.Show("Por favor, selecione um turno (Tarde ou Noite) para acessar a aula ao vivo.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                Process.Start(new ProcessStartInfo(url) { UseShellExecute = true }); // Usando ProcessStartInfo para segurança e compatibilidade
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Não foi possível abrir o navegador para a aula ao vivo.\nErro: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            // Lógica para quando o turno Noite é selecionado
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            // Lógica para quando o turno Tarde é selecionado
        }

        private void imgLogo_Click(object sender, EventArgs e)
        {
            try
            {
                Regras telaRegras = new Regras();
                telaRegras.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocorreu um erro ao abrir a tela de regras: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Este é o evento de clique do link que abre a página de cidadania.
        private void linkCidadania_Click(object sender, EventArgs e)
        {
            string instagramCidadaniaUrl = "https://www.instagram.com/cidadania.cedesp.dommacario/"; // O link do Instagram de Cidadania

            try
            {
                // Abre o link no navegador padrão do usuário
                Process.Start(new ProcessStartInfo(instagramCidadaniaUrl) { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Não foi possível abrir o Instagram Cidadania.\nErro: {ex.Message}", "Erro ao Abrir Link", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Este é o evento de clique do link que você quer que abra o InstaDoma.
        private void linkLabel2_Click(object sender, EventArgs e)
        {
            // O link para o InstaDoma principal
            string instagramUrl = "https://www.instagram.com/cedespdommacario/";

            try
            {
                // Abre o link no navegador padrão do usuário
                Process.Start(new ProcessStartInfo(instagramUrl) { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Não foi possível abrir o Instagram Doma.\nErro: {ex.Message}", "Erro ao Abrir Link", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}