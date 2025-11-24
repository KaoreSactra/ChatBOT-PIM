using System.Windows;
using System.Windows.Documents;
using DesktopSql.Services;
using DesktopSql.Models;

namespace DesktopSql
{
    public partial class ChatWindow : Window
    {
        private readonly ApiService _apiService;

        public ChatWindow()
        {
            InitializeComponent();
            _apiService = new ApiService();
            InitializeWindow();
        }

        private void InitializeWindow()
        {
            if (App.CurrentUser != null)
            {
                lblUserEmail.Text = App.CurrentUser.Email ?? "Usuário";
                lblUserRole.Text = App.CurrentUser.Role == "admin" ? "(Admin)" : "";
                AddSystemMessage($"Bem-vindo, {App.CurrentUser.Email}! Como posso ajudá-lo?");
            }
        }

        private async void BtnEnviar_Click(object sender, RoutedEventArgs e)
        {
            string mensagem = txtMensagem.Text?.Trim() ?? "";
            
            if (string.IsNullOrWhiteSpace(mensagem))
            {
                return;
            }

            // Mostrar mensagem do usuário
            AddUserMessage(mensagem);
            txtMensagem.Clear();
            txtMensagem.Focus();

            // Desabilitar botão durante a requisição
            btnEnviar.IsEnabled = false;

            try
            {
                if (App.CurrentUser == null)
                {
                    AddSystemMessage("Erro: Usuário não autenticado.");
                    return;
                }

                // Chamar API
                var (success, response) = await _apiService.SendChatMessageAsync(mensagem, App.CurrentUser.Id);

                if (success)
                {
                    AddBotMessage(response);
                }
                else
                {
                    AddSystemMessage($"Erro: {response}");
                }
            }
            catch (Exception ex)
            {
                AddSystemMessage($"Erro ao enviar mensagem: {ex.Message}");
            }
            finally
            {
                btnEnviar.IsEnabled = true;
            }
        }

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            App.CurrentUser = null;
            App.UserToken = null;
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }

        private void Window_Closed(object sender, System.EventArgs e)
        {
            // Limpar dados ao fechar
            App.CurrentUser = null;
            App.UserToken = null;
        }

        private void AddUserMessage(string text)
        {
            var paragraph = new Paragraph();
            paragraph.Margin = new Thickness(0, 5, 0, 5);
            paragraph.Inlines.Add(new Bold(new Run($"Você: ")) { Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(102, 126, 234)) });
            paragraph.Inlines.Add(new Run(text) { Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(17, 24, 39)) });
            rtbChat.Document.Blocks.Add(paragraph);
            rtbChat.ScrollToEnd();
        }

        private void AddBotMessage(string text)
        {
            var paragraph = new Paragraph();
            paragraph.Margin = new Thickness(0, 5, 0, 5);
            paragraph.Inlines.Add(new Bold(new Run($"🤖 Bot: ")) { Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(118, 75, 162)) });
            paragraph.Inlines.Add(new Run(text) { Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(17, 24, 39)) });
            rtbChat.Document.Blocks.Add(paragraph);
            rtbChat.ScrollToEnd();
        }

        private void AddSystemMessage(string text)
        {
            var paragraph = new Paragraph();
            paragraph.Margin = new Thickness(0, 5, 0, 5);
            paragraph.Inlines.Add(new Run(text) { Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(107, 114, 128)) });
            paragraph.TextAlignment = TextAlignment.Center;
            rtbChat.Document.Blocks.Add(paragraph);
            rtbChat.ScrollToEnd();
        }
    }
}

