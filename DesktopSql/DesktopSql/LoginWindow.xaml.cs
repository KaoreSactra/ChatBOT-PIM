using System;
using System.Windows;
using System.Windows.Controls;
using DesktopSql.Services; // <--- IMPORTANTE: Adicione isso para achar o ApiService

namespace DesktopSql
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        // 1. Criamos uma variável para o serviço
        private readonly ApiService _apiService;

        public LoginWindow()
        {
            InitializeComponent();
            // 2. Inicializamos o serviço quando a janela abre
            _apiService = new ApiService();
        }

        // 3. Adicionamos 'async' aqui para poder esperar a resposta da API
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("O botão foi clicado!");

            string usuario = txtUsuario.Text;
            
            // ATENÇÃO: Se no XAML você usou <PasswordBox>, troque .Text por .Password
            // Se usou <TextBox>, mantenha .Text (mas não é seguro para senhas)
            string senha = txtSenha.Text; 

            // Validação simples
            if (string.IsNullOrWhiteSpace(usuario) || string.IsNullOrWhiteSpace(senha))
            {
                MessageBox.Show("Por favor, preencha usuário e senha.");
                return;
            }

            try
            {
                // Desabilita o botão para o usuário não clicar várias vezes
                // (Converta sender para Button se necessário, ou use o x:Name do botão)
                if (sender is Button btn) btn.IsEnabled = false;

                // 4. Chama o Backend e espera a resposta
                bool loginSucesso = await _apiService.LoginAsync(usuario, senha);

                if (loginSucesso)
                {
                    // Login OK: Abre o Chat e fecha o Login
                    ChatWindow chat = new ChatWindow();
                    chat.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Usuário ou senha incorretos!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro inesperado: {ex.Message}");
            }
            finally
            {
                // Reabilita o botão independente do resultado
                if (sender is Button btn) btn.IsEnabled = true;
            }
        }
    }
}