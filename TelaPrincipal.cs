using System;
using IWshRuntimeLibrary; // Adicione esta linha
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO; // Certifique-se que esta linha está aqui para usar Directory
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProgramaCEDESP
{
    public partial class TelaPrincipal : Form
    {
        private string _nomeCompletoUsuario; // Alterado para evitar confusão com o nome da pasta
        private string _usuarioLogin;        // NOVO CAMPO para o nome de usuário de login
        private string _turno;

        // CONSTRUTOR MODIFICADO: Agora recebe o nome de usuário de login
        public TelaPrincipal(string nomeCompletoUsuario, string turno, string usuarioLogin)
        {
            InitializeComponent();
            _nomeCompletoUsuario = nomeCompletoUsuario;
            _turno = turno;
            _usuarioLogin = usuarioLogin; // Armazena o nome de usuário de login
            // Opcional: Você pode querer exibir o nome completo na tela, por exemplo:
            // lblNomeAluno.Text = "Olá, " + _nomeCompletoUsuario;

            // ***** ADICIONAR ESTA LINHA PARA CONECTAR O EVENTO FormClosing *****
            this.FormClosing += new FormClosingEventHandler(TelaPrincipal_FormClosing);
            // ***** FIM DA ADIÇÃO *****
        }

        private void imgMinhaPasta_Click(object sender, EventArgs e)
        {
            try
            {
                // Usa o _usuarioLogin para o nome da pasta, que é mais seguro
                string caminhoPastaUsuario = Path.Combine(@"\\server\", _turno, _usuarioLogin);

                // Cria a pasta se ela não existir
                if (!Directory.Exists(caminhoPastaUsuario))
                {
                    Directory.CreateDirectory(caminhoPastaUsuario);
                    MessageBox.Show("Pasta do usuário criada com sucesso!", "Informação", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                // ***** INÍCIO DA MODIFICAÇÃO PARA ATALHO NA REDE *****
                string nomeAtalho = $"Minha Pasta - {_usuarioLogin}"; // Nome do atalho para o usuário logado
                // Caminho para a pasta "Links" ou "NetHood" do usuário, onde atalhos de rede podem aparecer
                string caminhoAtalhosRede = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Microsoft", "Windows", "Network Shortcuts");

                // Se a pasta Network Shortcuts não existir, pode tentar "Links"
                if (!Directory.Exists(caminhoAtalhosRede))
                {
                    caminhoAtalhosRede = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "Links"); // Biblioteca "Links" no Win 7/8/10
                }

                string caminhoCompletoAtalho = Path.Combine(caminhoAtalhosRede, nomeAtalho + ".lnk");

                // Cria o atalho se ele não existir
                if (!System.IO.File.Exists(caminhoCompletoAtalho))
                {
                    WshShell shell = new WshShell();
                    IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(caminhoCompletoAtalho);
                    shortcut.TargetPath = caminhoPastaUsuario;
                    shortcut.Description = $"Pasta pessoal do aluno {_nomeCompletoUsuario}";
                    shortcut.Save();
                    MessageBox.Show($"Atalho para a pasta '{nomeAtalho}' criado em 'Rede' ou 'Links'.", "Atalho Criado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    // Se o atalho já existe, apenas abra a pasta
                    // MessageBox.Show($"O atalho para a pasta '{nomeAtalho}' já existe. Abrindo a pasta.", "Atalho Existente", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                // Abre a pasta no explorador de arquivos DEPOIS de tentar criar o atalho
                Process.Start("explorer.exe", caminhoPastaUsuario);
                // ***** FIM DA MODIFICAÇÃO *****
            }
            catch (Exception ex)
            {
                // Mensagem de erro mais detalhada para depuração
                MessageBox.Show($"Ocorreu um erro ao abrir/criar a pasta ou atalho: {ex.Message}\nVerifique o caminho de rede e as permissões: {Path.Combine(@"\\server\", _turno, _usuarioLogin)}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void imgMaterial_Click(object sender, EventArgs e)
        {
            try
            {
                string caminhoMaterial = @"\\server\Arquivos de curso"; // Verifique se o caminho do servidor está correto

                // Opcional: Criar a pasta de materiais se não existir (se for o caso, caso contrário, remova)
                if (!Directory.Exists(caminhoMaterial))
                {
                    Directory.CreateDirectory(caminhoMaterial);
                    MessageBox.Show("Pasta de materiais do curso criada com sucesso!", "Informação", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                if (Directory.Exists(caminhoMaterial))
                {
                    Process.Start(caminhoMaterial);
                }
                else
                {
                    MessageBox.Show("A pasta de materiais do curso não foi encontrada.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocorreu um erro: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Alterado: Agora abre a tela FreqAluno.cs passando o nome do usuário
        private void imgFreq_Click(object sender, EventArgs e)
        {
            // Verifica se o nome do usuário está disponível
            if (!string.IsNullOrEmpty(_nomeCompletoUsuario)) // Usando o nome completo, se FreqAluno esperar o nome completo
            {
                // Cria uma nova instância da tela de frequência do aluno, passando o nome do usuário
                FreqAluno formFrequencia = new FreqAluno(_nomeCompletoUsuario);
                formFrequencia.ShowDialog(); // Exibe a tela de frequência como uma janela modal
            }
            else
            {
                MessageBox.Show("Não foi possível identificar o aluno. Por favor, tente novamente.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // NOVO MÉTODO (ou modificado se já existia): Abre a tela NotaAluno.cs
        private void imgNota_Click(object sender, EventArgs e)
        {
            // Verifica se o nome do usuário está disponível
            if (!string.IsNullOrEmpty(_nomeCompletoUsuario)) // Usando o nome completo, se NotaAluno esperar o nome completo
            {
                // Cria uma nova instância da tela de notas do aluno, passando o nome do usuário
                NotaAluno formNotas = new NotaAluno(_nomeCompletoUsuario);
                formNotas.ShowDialog(); // Exibe a tela de notas como uma janela modal
            }
            else
            {
                MessageBox.Show("Não foi possível identificar o aluno. Por favor, tente novamente.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSair_Click(object sender, EventArgs e)
        {
            this.Hide(); // Esconde a tela principal
            Form1 formLogin = new Form1(); // Cria uma nova instância da tela de login (Form1)
            formLogin.Show(); // Exibe a tela de login
        }

        // ***** MÉTODO btnLive2_Click (ou modificado) para TelaPrincipal *****
        // Ajustado para usar os links Jitsi que você forneceu.
        private void btnLive2_Click(object sender, EventArgs e)
        {
            string url = "";
            if (_turno == "Tarde")
            {
                url = "https://meet.jit.si/cedesp-tarde";
            }
            else if (_turno == "Noite")
            {
                url = "https://meet.jit.si/cedesp-noite";
            }
            else
            {
                MessageBox.Show("Não foi possível determinar o turno para abrir a aula ao vivo.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                System.Diagnostics.Process.Start(url);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Não foi possível abrir o navegador: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void material_Click(object sender, EventArgs e)
        {
            // Este método parece estar vazio e pode ser removido se não for usado.
            // Se ele for o evento Click da imgMaterial, remova este método duplicado.
        }

        private void Imagens_MouseDown(object sender, MouseEventArgs e)
        {
            Control imagem = (Control)sender;
            imagem.Size = new Size(imagem.Width - 5, imagem.Height - 5);
            imagem.Location = new Point(imagem.Location.X + 2, imagem.Location.Y + 2);
        }

        private void Imagens_MouseUp(object sender, MouseEventArgs e)
        {
            Control imagem = (Control)sender;
            imagem.Size = new Size(imagem.Width + 5, imagem.Height + 5);
            imagem.Location = new Point(imagem.Location.X - 2, imagem.Location.Y - 2);
        }

        // ***** NOVO MÉTODO: Chamado quando a TelaPrincipal está prestes a fechar *****
        private void TelaPrincipal_FormClosing(object sender, FormClosingEventArgs e)
        {
            string nomeAtalho = $"Minha Pasta - {_usuarioLogin}";
            string caminhoAtalhosRede = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Microsoft", "Windows", "Network Shortcuts");

            // Tenta o caminho de Network Shortcuts primeiro
            if (!Directory.Exists(caminhoAtalhosRede))
            {
                // Se não existir, tenta o caminho de "Links" dentro da pasta pessoal
                caminhoAtalhosRede = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "Links");
            }

            string caminhoCompletoAtalho = Path.Combine(caminhoAtalhosRede, nomeAtalho + ".lnk");

            try
            {
                // Se o atalho existir, remova-o
                if (System.IO.File.Exists(caminhoCompletoAtalho))
                {
                    System.IO.File.Delete(caminhoCompletoAtalho);
                    // Opcional: MessageBox.Show("Atalho da pasta do aluno removido com sucesso.", "Limpeza", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                // Captura erros ao tentar remover o atalho (ex: atalho em uso)
                // Não mostramos MessageBox para não interromper o fechamento do app, mas logamos o erro se possível.
                Debug.WriteLine($"Erro ao remover atalho da pasta do aluno: {ex.Message}");
            }
        }
        // ***** FIM DO NOVO MÉTODO *****
    }
}