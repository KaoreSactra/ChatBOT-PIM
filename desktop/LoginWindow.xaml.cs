using System.Windows;
using DesktopSql.Services;

namespace DesktopSql
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private readonly ApiService _apiService;

        public LoginWindow()
        {
            InitializeComponent();
            _apiService = new ApiService();
        }

        private async void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            string email = txtEmail.Text?.Trim() ?? "";
            string senha = txtSenha.Password ?? "";

            // Validação
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(senha))
            {
                ShowError("Por favor, preencha email e senha.");
                return;
            }

            try
            {
                btnLogin.IsEnabled = false;
                btnLogin.Content = "Autenticando...";

                // Chamar API de login
                var (success, message, user) = await _apiService.LoginAsync(email, senha);

                if (success && user != null)
                {
                    // Salvar informações do usuário
                    App.CurrentUser = user;
                    App.UserToken = message; // Token ou ID da sessão

                    // Abrir janela principal
                    ChatWindow chatWindow = new ChatWindow();
                    chatWindow.Show();
                    this.Close();
                }
                else
                {
                    ShowError(message ?? "Erro ao fazer login. Verifique suas credenciais.");
                }
            }
            catch (Exception ex)
            {
                ShowError($"Erro: {ex.Message}");
            }
            finally
            {
                btnLogin.IsEnabled = true;
                btnLogin.Content = "Entrar";
            }
        }

        private void LinkRegister_Click(object sender, RoutedEventArgs e)
        {
            // Abrir janela de registro
            RegisterWindow registerWindow = new RegisterWindow();
            registerWindow.Show();
            this.Close();
        }

        private void ShowError(string message)
        {
            ErrorMessage.Text = message;
            ErrorBorder.Visibility = Visibility.Visible;
        }

        private void HideError()
        {
            ErrorBorder.Visibility = Visibility.Collapsed;
        }

        private void TxtEmail_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            HideError();
        }

        private void TxtSenha_PasswordChanged(object sender, RoutedEventArgs e)
        {
            HideError();
        }
    }
}
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