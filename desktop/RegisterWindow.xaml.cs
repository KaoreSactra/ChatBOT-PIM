using System.Windows;
using DesktopSql.Services;

namespace DesktopSql
{
    public partial class RegisterWindow : Window
    {
        private readonly ApiService _apiService;

        public RegisterWindow()
        {
            InitializeComponent();
            _apiService = new ApiService();
        }

        private async void BtnRegister_Click(object sender, RoutedEventArgs e)
        {
            string email = txtEmail.Text?.Trim() ?? "";
            string senha = txtSenha.Password ?? "";
            string confirmSenha = txtConfirmSenha.Password ?? "";

            // Validação
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(senha))
            {
                ShowError("Por favor, preencha todos os campos.");
                return;
            }

            if (senha != confirmSenha)
            {
                ShowError("As senhas não coincidem.");
                return;
            }

            if (senha.Length < 6)
            {
                ShowError("A senha deve ter no mínimo 6 caracteres.");
                return;
            }

            try
            {
                btnRegister.IsEnabled = false;
                btnRegister.Content = "Registrando...";

                var (success, message, user) = await _apiService.RegisterAsync(email, senha, confirmSenha);

                if (success && user != null)
                {
                    MessageBox.Show("Registro realizado com sucesso! Faça login para continuar.");
                    LoginWindow loginWindow = new LoginWindow();
                    loginWindow.Show();
                    this.Close();
                }
                else
                {
                    ShowError(message ?? "Erro ao registrar usuário.");
                }
            }
            catch (Exception ex)
            {
                ShowError($"Erro: {ex.Message}");
            }
            finally
            {
                btnRegister.IsEnabled = true;
                btnRegister.Content = "Registrar";
            }
        }

        private void LinkLogin_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
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
    }
}
