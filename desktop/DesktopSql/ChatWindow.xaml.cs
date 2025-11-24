using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DesktopSql
{
    /// <summary>
    /// Lógica interna para Window1.xaml
    /// </summary>
    public partial class ChatWindow : Window
    {
        public ChatWindow()
        {
            InitializeComponent();
        }

        private void btnEnviar_Click(object sender, RoutedEventArgs e)
        {
            string mensagemUsuario = txtMensagem.Text;
            if (string.IsNullOrWhiteSpace(mensagemUsuario))
            {
                return; // Não faz nada se a mensagem estiver vazia
            }

            // 4. Limpa a caixa de mensagem e foca nela
            txtMensagem.Clear();
            txtMensagem.Focus();
        }
    }
}
