using System.Configuration;
using System.Data;
using System.Windows;
using DotNetEnv;
using DesktopSql.Models;

namespace DesktopSql
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static LoginModel? CurrentUser { get; set; }
        public static string? UserToken { get; set; }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            // Carregar arquivo .env - procura em vários locais
            string[] possiblePaths = new[]
            {
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ".env"),                           // ./bin/Debug/net8.0-windows/.env
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", ".env"),                // ../../.env
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", ".env"),          // ../../../.env
                Path.Combine(Directory.GetCurrentDirectory(), ".env"),                                   // Diretório raiz do projeto
            };

            foreach (var envPath in possiblePaths)
            {
                if (File.Exists(envPath))
                {
                    DotNetEnv.Env.Load(envPath);
                    break;
                }
            }
        }
    }

}
