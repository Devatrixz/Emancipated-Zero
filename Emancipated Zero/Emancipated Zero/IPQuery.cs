using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Emancipated_Zero
{
    class IPQuery
    {
        public static void IPQueryMain()
        {
            Console.WriteLine("IP Adresi Giriniz: ");
            string ipAddress = Console.ReadLine().Trim();

            if (!ValidateIP(ipAddress))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Bu IP Adresi Hatalı!");
                Console.ResetColor();
                Console.ReadKey();
                return;
            }

            Console.Write("Sonuçları source/logs konumuna kaydetmek ister misiniz? (E/H): ");
            string saveChoice = Console.ReadLine().Trim().ToUpper();
            bool saveToFile = saveChoice == "E";

            QueryIP(ipAddress, saveToFile).Wait();
        }

        static bool ValidateIP(string ip)
        {
            string[] parts = ip.Split('.');
            if (parts.Length != 4) return false;

            foreach (string part in parts)
            {
                int num;
                if (!int.TryParse(part, out num) || num < 0 || num > 255)
                    return false;
            }
            return true;
        }

        static async Task QueryIP(string ipAddress, bool saveToFile)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string url = $"http://ip-api.com/json/{ipAddress}";
                    string response = await client.GetStringAsync(url);
                    JObject json = JObject.Parse(response);

                    if (json["status"].ToString() == "success")
                    {
                        string result = $"IP Adresi: {ipAddress}\n" +
                                        $"Ülke: {json["country"] ?? "Bilinmiyor"}\n" +
                                        $"Şehir: {json["city"] ?? "Bilinmiyor"}\n" +
                                        $"Enlem: {json["lat"] ?? "Bilinmiyor"}\n" +
                                        $"Boylam: {json["lon"] ?? "Bilinmiyor"}\n" +
                                        $"ISP: {json["isp"] ?? "Bilinmiyor"}\n" +
                                        $"Bölge: {json["regionName"] ?? "Bilinmiyor"}\n" +
                                        $"Saat Dilimi: {json["timezone"] ?? "Bilinmiyor"}\n" +
                                        $"AS: {json["as"] ?? "Bilinmiyor"}";

                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine(result);

                        if (saveToFile)
                        {
                            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "source/logs/IPQueryResults.txt");
                            File.AppendAllText(filePath, result + "\n\n");
                            Console.WriteLine($"Sonuçlar {filePath} konumuna kaydedildi.");
                        }
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Bu IP Adresi Hatalı!");
                    }
                }
                catch (Exception)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Bir hata oluştu!");
                }
                finally
                {
                    Console.ResetColor();
                    Console.ReadKey();
                }
            }
        }
    }
}
