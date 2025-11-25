using Microsoft.AspNetCore.Mvc;
using api_back.Models;
using System.Text.Json;

namespace api_back.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly string _apiKey;
        private readonly ILogger<ChatController> _logger;
        private readonly HttpClient _httpClient;
        private const string GeminiApiBaseUrl = "https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash:generateContent";

        // Sistema de rastreamento de conversa para validar primeira mensagem
        private static Dictionary<string, int> _conversationMessageCount = new();

        public ChatController(IConfiguration configuration, ILogger<ChatController> logger, HttpClient httpClient)
        {
            // Tentar carregar da variável de ambiente primeiro, depois do appsettings
            _apiKey = Environment.GetEnvironmentVariable("GOOGLE_GEMINI_API_KEY") 
                ?? configuration["GoogleGeminiApiKey"] 
                ?? "";
            _logger = logger;
            _httpClient = httpClient;
        }

        private string GetSessionId()
        {
            // Usar ID da sessão HTTP para rastrear conversa
            return HttpContext.Session.Id ?? "default";
        }

        private bool IsHardwareSoftwareProblem(string message)
        {
            // Palavras-chave para problemas de hardware/software em português
            var hwSwKeywords = new[]
            {
                "hardware", "software", "erro", "bug", "crash", "falha", "problema",
                "não funciona", "lento", "travado", "congelado", "tela azul", "bsod",
                "driver", "gpu", "cpu", "ram", "memória", "disco", "ssd", "hd",
                "processador", "placa gráfica", "placa mãe", "fonte", "cooler",
                "bateria", "carregador", "teclado", "mouse", "monitor", "impressora",
                "wifi", "internet", "conexão", "rede", "windows", "linux", "mac",
                "aplicativo", "app", "programa", "sistema", "operacional", "plugin",
                "compatibilidade", "versão", "atualização", "instalação", "desinstalação",
                "arquivo", "pasta", "documento", "perda de dados", "corrupção",
                "sensor", "webcam", "microfone", "som", "áudio", "vídeo"
            };

            var lowerMessage = message.ToLower();
            return hwSwKeywords.Any(keyword => lowerMessage.Contains(keyword));
        }

        private bool IsHardwareProblem(string message)
        {
            // Palavras-chave específicas para problemas de HARDWARE
            var hardwareKeywords = new[]
            {
                "hardware", "gpu", "cpu", "ram", "memória", "disco", "ssd", "hd",
                "processador", "placa gráfica", "placa mãe", "fonte", "cooler",
                "bateria", "carregador", "teclado", "mouse", "monitor", "impressora",
                "webcam", "microfone", "som", "áudio", "vídeo", "sensor",
                "processador lento", "memória insuficiente", "disco cheio"
            };

            var lowerMessage = message.ToLower();
            return hardwareKeywords.Any(keyword => lowerMessage.Contains(keyword));
        }

        private bool IsSoftwareProblem(string message)
        {
            // Palavras-chave específicas para problemas de SOFTWARE
            var softwareKeywords = new[]
            {
                "software", "erro", "bug", "crash", "falha", "driver",
                "windows", "linux", "mac", "aplicativo", "app", "programa",
                "sistema", "operacional", "plugin", "compatibilidade", "versão",
                "atualização", "instalação", "desinstalação", "arquivo", "pasta",
                "documento", "perda de dados", "corrupção", "travado", "congelado",
                "tela azul", "bsod", "não funciona", "lento", "wifi", "internet",
                "conexão", "rede", "navegador", "browser", "chrome", "firefox",
                "edge", "safari", "excel", "word", "powerpoint", "database", "sql",
                "erro de acesso", "permissão", "script", "código", "vírus", "malware",
                "antivírus", "firewall", "proxy", "vpn", "ativar", "desativar",
                "reiniciar", "reinicializar", "boot", "startup", "processo", "serviço"
            };

            var lowerMessage = message.ToLower();
            return softwareKeywords.Any(keyword => lowerMessage.Contains(keyword));
        }

        private string RemoveMarkdownFormatting(string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;

            // Remover **negrito**
            text = System.Text.RegularExpressions.Regex.Replace(text, @"\*\*(.+?)\*\*", "$1");
            
            // Remover *itálico*
            text = System.Text.RegularExpressions.Regex.Replace(text, @"\*(.+?)\*", "$1");
            
            // Remover __negrito__
            text = System.Text.RegularExpressions.Regex.Replace(text, @"__(.+?)__", "$1");
            
            // Remover _itálico_
            text = System.Text.RegularExpressions.Regex.Replace(text, @"_(.+?)_", "$1");
            
            // Remover `código inline`
            text = System.Text.RegularExpressions.Regex.Replace(text, @"`(.+?)`", "$1");
            
            // Remover ```código em bloco```
            text = System.Text.RegularExpressions.Regex.Replace(text, @"```[\s\S]*?```", "");
            
            // Remover # cabeçalhos
            text = System.Text.RegularExpressions.Regex.Replace(text, @"^#+\s+(.+)$", "$1", System.Text.RegularExpressions.RegexOptions.Multiline);
            
            // Remover [link](url)
            text = System.Text.RegularExpressions.Regex.Replace(text, @"\[(.+?)\]\(.+?\)", "$1");
            
            return text;
        }

        [HttpPost("send")]
        public async Task<ActionResult<ChatResponse>> SendMessage([FromBody] ChatRequest request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.Message))
                {
                    return BadRequest(new ChatResponse
                    {
                        Success = false,
                        Error = "A mensagem não pode estar vazia"
                    });
                }

                var sessionId = GetSessionId();
                
                // Rastrear contagem de mensagens por sessão
                if (!_conversationMessageCount.ContainsKey(sessionId))
                {
                    _conversationMessageCount[sessionId] = 0;
                }
                _conversationMessageCount[sessionId]++;

                // Detectar tipo de problema para resposta apropriada
                bool isHardware = IsHardwareProblem(request.Message);
                bool isSoftware = IsSoftwareProblem(request.Message);

                if (string.IsNullOrWhiteSpace(_apiKey))
                {
                    _logger.LogWarning("Chave de API do Google Gemini não configurada. Usando resposta mock.");
                    
                    // Mock com resposta diferente para hardware e software
                    if (isHardware)
                    {
                        return Ok(new ChatResponse
                        {
                            Response = "Para problemas de hardware, recomendamos abrir um email para: suporte@sqlsolucoes.com\n\n📧 Descreva seu problema técnico de hardware neste email e nossa equipe técnica entrará em contato.",
                            Success = true
                        });
                    }
                    else if (isSoftware)
                    {
                        return Ok(new ChatResponse
                        {
                            Response = $"Entendi seu problema de software. Deixe comigo!\n\nVocê disse: \"{request.Message}\"\n\nConfigure GoogleGeminiApiKey para usar a API real do Gemini e receberá ajuda completa.",
                            Success = true
                        });
                    }
                    else
                    {
                        return Ok(new ChatResponse
                        {
                            Response = "Desculpe, não entendi. Por favor, descreva um problema de hardware ou software.",
                            Success = true
                        });
                    }
                }

                // System prompt diferenciado baseado no tipo de problema
                string systemPrompt;
                
                if (isHardware)
                {
                    systemPrompt = @"Você é um assistente de suporte técnico especializado em problemas de HARDWARE.
INSTRUÇÕES OBRIGATÓRIAS:
1. Responda SEMPRE em português brasileiro (pt-BR)
2. Para problemas de hardware, recomende abrir um email para: suporte@sqlsolucoes.com
3. Descreva brevemente o que fazer no email
4. Seja amigável e profissional
5. Não tente resolver problemas de hardware, apenas recomende contatar o suporte";
                }
                else
                {
                    systemPrompt = @"Você é um assistente de suporte técnico especializado em problemas de SOFTWARE.
INSTRUÇÕES OBRIGATÓRIAS:
1. Responda SEMPRE em português brasileiro (pt-BR)
2. Tente ajudar a resolver o problema de software passo a passo
3. Seja conciso e prático nas soluções
4. Forneça passos claros e numerados quando possível
5. Se não conseguir resolver, sugira contato com suporte profissional em: suporte@sqlsolucoes.com
6. Mantenha um tom amigável e profissional";
                }

                // Combinar system prompt com a mensagem do usuário
                string fullPrompt = systemPrompt + "\n\nPergunta do usuário: " + request.Message;

                // Construir o corpo da requisição para a API Gemini
                var requestBody = new
                {
                    contents = new object[]
                    {
                        new
                        {
                            parts = new object[]
                            {
                                new { text = fullPrompt }
                            }
                        }
                    }
                };

                var jsonContent = new StringContent(
                    JsonSerializer.Serialize(requestBody),
                    System.Text.Encoding.UTF8,
                    "application/json"
                );

                var url = $"{GeminiApiBaseUrl}?key={_apiKey}";
                var response = await _httpClient.PostAsync(url, jsonContent);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError($"Erro da API Gemini: {response.StatusCode} - {errorContent}");
                    
                    // Se for erro 503 ou 500, usar fallback com resposta mock
                    if ((int)response.StatusCode >= 500)
                    {
                        _logger.LogWarning($"Erro {response.StatusCode} - Usando resposta de fallback");
                        
                        if (isHardware)
                        {
                            return Ok(new ChatResponse
                            {
                                Response = "Para problemas de hardware, recomendamos abrir um email para: suporte@sqlsolucoes.com\n\n📧 Descreva seu problema técnico de hardware neste email e nossa equipe técnica entrará em contato.",
                                Success = true
                            });
                        }
                        else if (isSoftware)
                        {
                            return Ok(new ChatResponse
                            {
                                Response = $"Entendi seu problema de software: \"{request.Message}\"\n\nPor favor, envie um email para: suporte@sqlsolucoes.com com detalhes do seu problema para que nossa equipe possa ajudar melhor.",
                                Success = true
                            });
                        }
                        else
                        {
                            return Ok(new ChatResponse
                            {
                                Response = "Desculpe, não consegui processar sua solicitação no momento. Por favor, entre em contato conosco em: suporte@sqlsolucoes.com",
                                Success = true
                            });
                        }
                    }
                    
                    // Se for erro 400, pode ser configuração. Tenta novamente com fallback
                    if ((int)response.StatusCode == 400)
                    {
                        _logger.LogWarning("Erro 400 - Tentando com sistema mais simples...");
                        var simpleRequest = new
                        {
                            contents = new object[]
                            {
                                new
                                {
                                    parts = new object[]
                                    {
                                        new { text = request.Message }
                                    }
                                }
                            }
                        };
                        
                        var simpleContent = new StringContent(
                            JsonSerializer.Serialize(simpleRequest),
                            System.Text.Encoding.UTF8,
                            "application/json"
                        );
                        
                        response = await _httpClient.PostAsync(url, simpleContent);
                        
                        if (!response.IsSuccessStatusCode)
                        {
                            var simpleError = await response.Content.ReadAsStringAsync();
                            _logger.LogError($"Erro da API Gemini (2ª tentativa): {response.StatusCode} - {simpleError}");
                            return StatusCode((int)response.StatusCode, new ChatResponse
                            {
                                Success = false,
                                Error = $"Erro ao conectar com Gemini: {response.StatusCode}"
                            });
                        }
                    }
                    else
                    {
                        return StatusCode((int)response.StatusCode, new ChatResponse
                        {
                            Success = false,
                            Error = $"Erro ao conectar com Gemini: {response.StatusCode}"
                        });
                    }
                }

                var responseContent = await response.Content.ReadAsStringAsync();
                using var jsonDoc = JsonDocument.Parse(responseContent);
                var root = jsonDoc.RootElement;

                // Extrair a resposta do JSON
                if (root.TryGetProperty("candidates", out var candidates) && candidates.GetArrayLength() > 0)
                {
                    var firstCandidate = candidates[0];
                    if (firstCandidate.TryGetProperty("content", out var content) &&
                        content.TryGetProperty("parts", out var parts) && parts.GetArrayLength() > 0)
                    {
                        var text = parts[0].GetProperty("text").GetString();
                        if (!string.IsNullOrEmpty(text))
                        {
                            // Remover formatação Markdown
                            text = RemoveMarkdownFormatting(text);
                            
                            return Ok(new ChatResponse
                            {
                                Response = text,
                                Success = true
                            });
                        }
                    }
                }

                return StatusCode(500, new ChatResponse
                {
                    Success = false,
                    Error = "Resposta inválida da API Gemini"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao processar mensagem do chatbot: {ex.Message}");
                return StatusCode(500, new ChatResponse
                {
                    Success = false,
                    Error = $"Erro ao processar sua mensagem: {ex.Message}"
                });
            }
        }
    }
}
