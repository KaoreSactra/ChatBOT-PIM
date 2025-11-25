using System.Configuration;
using System.Data;
using System.IO;
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

            bool loaded = false;
            foreach (var envPath in possiblePaths)
            {
                var fullPath = Path.GetFullPath(envPath);
                System.Diagnostics.Debug.WriteLine($"[App] Procurando .env em: {fullPath}");
                
                if (File.Exists(fullPath))
                {
                    DotNetEnv.Env.Load(fullPath);
                    System.Diagnostics.Debug.WriteLine($"[App] .env carregado com sucesso de: {fullPath}");
                    loaded = true;
                    break;
                }
            }
            
            if (!loaded)
            {
                System.Diagnostics.Debug.WriteLine($"[App] AVISO: Arquivo .env nao encontrado! Usando valores padrao.");
            }
        }
    }

}
