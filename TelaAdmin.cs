using System;
using IWshRuntimeLibrary;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProgramaCEDESP
{
    public partial class TelaAdmin : Form
    {
        private string _turnoSelecionadoAdmin;

        // Define o caminho para a pasta do Admin no servidor
        // CaminhoBaseServidor: Ex: \\server\
        // NomePastaAdmin: Ex: DomaAdmin
        // Resulta em: \\server\Tarde\DomaAdmin ou \\server\Noite\DomaAdmin
        private const string CaminhoPastaBaseServidor = @"\\server\"; // Este caminho está correto para \\server\tarde e \\server\noite
        private const string NomePastaAdmin = "DomaAdmin"; // Nome fixo da pasta do admin

        // Construtor principal modificado para receber o turno
        public TelaAdmin(string turno)
        {
            InitializeComponent();
            _turnoSelecionadoAdmin = turno; // Armazena o turno que o admin selecionou no login
            this.FormClosing += new FormClosingEventHandler(TelaAdmin_FormClosing);

            // Opcional: Exibir o turno do admin em um Label, se você tiver um.
            // Ex: lblTurnoAdmin.Text = $"Admin do Turno: {_turnoSelecionadoAdmin}";
        }

        // Se você NUNCA chamar TelaAdmin sem um turno (ex: TelaAdmin adminForm = new TelaAdmin();)
        // você pode remover o construtor abaixo. Se houver alguma chance, mantenha-o, mas considere o comportamento padrão.
        /*
        public TelaAdmin()
        {
            InitializeComponent();
            _turnoSelecionadoAdmin = "Não Definido"; // Valor padrão se o turno não for passado
            this.FormClosing += new FormClosingEventHandler(TelaAdmin_FormClosing);
        }
        */

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

        private void imgFreq_Click(object sender, EventArgs e)
        {
            // ***** CORREÇÃO AQUI: PASSANDO O TURNO *****
            // Verifica se o _turnoSelecionadoAdmin é "Todos os Turnos" ou similar,
            // e ajusta para "Não Definido" para FormFrequenciaAdmin
            string turnoParaFreq = _turnoSelecionadoAdmin;
            // Se TelaAdmin permitisse seleção "Todos", isso converteria. Como você tem só Tarde/Noite,
            // o "Não Definido" provavelmente não será alcançado aqui, mas a lógica está correta.
            if (turnoParaFreq == "Todos os Turnos")
            {
                turnoParaFreq = "Não Definido"; // FormFrequenciaAdmin interpreta isso como "todos"
            }

            FormFrequenciaAdmin formFrequencia = new FormFrequenciaAdmin(turnoParaFreq);
            formFrequencia.ShowDialog();
        }

        private void imgNota_Click(object sender, EventArgs e)
        {
            // ***** CORREÇÃO AQUI: PASSANDO O TURNO *****
            // Verifica se o _turnoSelecionadoAdmin é "Todos os Turnos" ou similar,
            // e ajusta para "Não Definido" para FormNotasAdmin
            string turnoParaNotas = _turnoSelecionadoAdmin;
            // Se TelaAdmin permitisse seleção "Todos", isso converteria. Como você tem só Tarde/Noite,
            // o "Não Definido" provavelmente não será alcançado aqui, mas a lógica está correta.
            if (turnoParaNotas == "Todos os Turnos")
            {
                turnoParaNotas = "Não Definido"; // FormNotasAdmin interpreta isso como "todos"
            }

            FormNotasAdmin formNotas = new FormNotasAdmin(turnoParaNotas);
            formNotas.ShowDialog();
        }

        private void imgMaterial_Click(object sender, EventArgs e)
        {
            try
            {
                // Este caminho deve ser ajustado se "Arquivos de curso" não estiver diretamente em "\\server\"
                string caminhoMaterial = @"\\server\Arquivos de curso";
                if (Directory.Exists(caminhoMaterial))
                {
                    Process.Start(new ProcessStartInfo(caminhoMaterial) { UseShellExecute = true });
                }
                else
                {
                    MessageBox.Show("A pasta de materiais do curso não foi encontrada. Verifique o caminho de rede.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocorreu um erro ao tentar abrir a pasta de materiais:\n{ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Método de clique para abrir a pasta do Admin
        private void imgMinhaPasta_Click(object sender, EventArgs e)
        {
            try
            {
                // Constrói o caminho completo da pasta do Admin usando o turno
                // Ex: \\server\Tarde\DomaAdmin
                string caminhoPastaAdminCompleto = Path.Combine(CaminhoPastaBaseServidor, _turnoSelecionadoAdmin, NomePastaAdmin);

                // Verifica se o diretório existe, se não, tenta criar
                if (!Directory.Exists(caminhoPastaAdminCompleto))
                {
                    Directory.CreateDirectory(caminhoPastaAdminCompleto);
                    MessageBox.Show($"A pasta '{caminhoPastaAdminCompleto}' foi criada com sucesso!", "Pasta Criada", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                string nomeAtalhoAdmin = $"Minha Pasta - Administrador ({_turnoSelecionadoAdmin})"; // Nome do atalho para o admin
                string caminhoAtalhosRede = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Microsoft", "Windows", "Network Shortcuts");

                // Tenta o caminho de Network Shortcuts primeiro
                if (!Directory.Exists(caminhoAtalhosRede))
                {
                    // Se não existir, tenta o caminho de "Links" dentro da pasta pessoal (usado em algumas versões do Windows)
                    caminhoAtalhosRede = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "Links");
                }

                string caminhoCompletoAtalhoAdmin = Path.Combine(caminhoAtalhosRede, nomeAtalhoAdmin + ".lnk");

                // Cria o atalho se ele não existir
                if (!System.IO.File.Exists(caminhoCompletoAtalhoAdmin))
                {
                    WshShell shell = new WshShell();
                    IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(caminhoCompletoAtalhoAdmin);
                    shortcut.TargetPath = caminhoPastaAdminCompleto;
                    shortcut.Description = $"Pasta pessoal do Administrador do Turno {_turnoSelecionadoAdmin}";
                    shortcut.Save();
                    MessageBox.Show($"Atalho para a pasta '{nomeAtalhoAdmin}' criado em 'Rede' ou 'Links'.", "Atalho Criado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                // else { MessageBox.Show($"O atalho para a pasta '{nomeAtalhoAdmin}' já existe. Abrindo a pasta.", "Atalho Existente", MessageBoxButtons.OK, MessageBoxIcon.Information); }

                // Abre a pasta no explorador de arquivos DEPOIS de tentar criar o atalho
                Process.Start(new ProcessStartInfo("explorer.exe", caminhoPastaAdminCompleto) { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocorreu um erro ao tentar abrir ou criar sua pasta pessoal ou atalho:\n{ex.Message}\n\nVerifique as permissões de rede e se o caminho do servidor está correto.", "Erro ao Abrir Pasta", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        // Evento de clique para o botão Sair
        private void btnSair_Click(object sender, EventArgs e)
        {
            this.Hide(); // Esconde a tela atual (TelaAdmin)
            Form1 telaLogin = new Form1(); // Cria uma nova instância da tela de login
            telaLogin.Show(); // Mostra a tela de login
        }


        private void TelaAdmin_Load(object sender, EventArgs e)
        {
            // Seu código existente para o evento Load da TelaAdmin
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

        private void btnLimpar_Click(object sender, EventArgs e)
        {
            DialogResult confirmacao = MessageBox.Show(
                "ATENÇÃO: Esta ação é IRREVERSÍVEL!\n\n" +
                "Você tem certeza que deseja finalizar o semestre?\n" +
                "Isso irá APAGAR PERMANENTEMENTE todos os dados de login, frequência e notas de TODOS os alunos do sistema " +
                "E EXCLUIRÁ as pastas de cada aluno no servidor (\\server\\[Turno]\\[Usuario]).\n\n" +
                "Deseja continuar?",
                "Confirmação de Finalização de Semestre",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2); // Botão "Não" como padrão para segurança

            if (confirmacao == DialogResult.Yes)
            {
                try
                {
                    GerenciadorDados.LimparDadosSemestre();

                    MessageBox.Show("Todos os dados dos alunos e suas pastas no servidor foram limpos com sucesso!",
                                    "Limpeza Concluída",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);

                    this.Close(); // Fecha a tela de administração

                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ocorreu um erro durante a limpeza dos dados e pastas:\n{ex.Message}",
                                    "Erro na Limpeza",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Operação de limpeza cancelada.", "Cancelado", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnLive_Click(object sender, EventArgs e)
        {
            string url = "";
            if (_turnoSelecionadoAdmin == "Tarde")
            {
                url = "https://meet.jit.si/cedesp-tarde";
            }
            else if (_turnoSelecionadoAdmin == "Noite")
            {
                url = "https://meet.jit.si/cedesp-noite";
            }
            else // Caso o turno seja "Não Definido" (ex: admin logou sem turno específico) ou "Todos os Turnos"
            {
                MessageBox.Show("Não foi possível determinar o turno para abrir a aula ao vivo. Selecione um turno válido ou um admin com turno definido.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                Process.Start(new ProcessStartInfo(url) { UseShellExecute = true }); // Usando ProcessStartInfo para segurança e compatibilidade
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Não foi possível abrir o navegador para a aula ao vivo:\n{ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TelaAdmin_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Apenas remove o atalho se o turno for definido.
            // Se for "Não Definido", provavelmente o atalho não foi criado de forma específica.
            if (_turnoSelecionadoAdmin != "Não Definido")
            {
                string nomeAtalhoAdmin = $"Minha Pasta - Administrador ({_turnoSelecionadoAdmin})";
                string caminhoAtalhosRede = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Microsoft", "Windows", "Network Shortcuts");

                if (!Directory.Exists(caminhoAtalhosRede))
                {
                    caminhoAtalhosRede = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "Links");
                }

                string caminhoCompletoAtalhoAdmin = Path.Combine(caminhoAtalhosRede, nomeAtalhoAdmin + ".lnk");

                try
                {
                    if (System.IO.File.Exists(caminhoCompletoAtalhoAdmin))
                    {
                        System.IO.File.Delete(caminhoCompletoAtalhoAdmin);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Erro ao remover atalho da pasta do administrador: {ex.Message}");
                }
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // 1. Cria uma nova instância do formulário RelatorioAdmin
            RelatorioAdmin formRelatorio = new RelatorioAdmin();

            // 2. Exibe o formulário. O método ShowDialog() abre o formulário
            // como uma janela modal, ou seja, o usuário precisa fechá-la
            // para poder interagir com a tela anterior.
            formRelatorio.ShowDialog();
        }
    }
}