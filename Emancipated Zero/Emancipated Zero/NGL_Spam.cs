using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

namespace Emancipated_Zero
{
    internal class NGL_Spam
    {
        public static async Task StartNGLSpammer()
        {
            try
            {
                await NGL_Spam.NGLMain(new string[] { });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}", Console.ForegroundColor = ConsoleColor.Red);
                Console.ReadKey();
            }
        }
        public static async Task NGLMain(string[] args)
        {
            try
            {
                while (true)
                {
                    Console.Clear();
                    drawLogo();
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Menü:");
                    Console.WriteLine("1- Gönderimi başlat");
                    Console.WriteLine("2- Dosyaları yapılandır (txt dosyaların bulunduğu klasörü aç)");
                    Console.WriteLine("3- Çık");
                    Console.Write("Seçiminizi yapın: ");

                    string secim = Console.ReadLine();

                    switch (secim)
                    {
                        case "1":
                            await Baslat();
                            break;
                        case "2":
                            Yapilandir();
                            break;
                        case "3":
                            await Program.Main();
                            break;
                        default:
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Geçersiz seçim! Lütfen tekrar deneyin.");
                            Console.ForegroundColor = ConsoleColor.White;
                            await Task.Delay(2000);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[-] Genel hata oluştu! Hata: {ex.Message}");
            }
        }

        private static async Task Baslat()
        {
            int count = 0;
            Console.Title = $"NGL Spammer Tool for Emancipated Zero | Gönderilen Sorular: {count}";

            string deviceID = GenerateRandomDeviceID();

            Console.Write("Kaç adet hesaba gönderilecek? ");
            Console.ForegroundColor = ConsoleColor.White;
            int hesapSayisi = int.Parse(Console.ReadLine());

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write("Hesapları hesaplar.txt üzerinden almak ister misiniz? (evet/hayır): ");
            Console.ForegroundColor = ConsoleColor.White;
            bool hesapDosyaKullan = Console.ReadLine().ToLower() == "evet";

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write("Mesajları mesajlar.txt üzerinden göndermek ister misiniz? (evet/hayır): ");
            Console.ForegroundColor = ConsoleColor.White;
            bool mesajDosyaKullan = Console.ReadLine().ToLower() == "evet";

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write("Kaç adet istek göndereceksiniz? ");
            Console.ForegroundColor = ConsoleColor.White;
            int istekSayisi = int.Parse(Console.ReadLine());

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write("Ne sıklıkla göndereceksiniz? (milisaniye cinsinden, geçmek için 'skip' yazın): ");
            Console.ForegroundColor = ConsoleColor.White;
            string aralikGirdi = Console.ReadLine();
            int aralik = aralikGirdi.ToLower() == "skip" ? 0 : int.Parse(aralikGirdi);

            List<string> hesaplar = new List<string>();
            List<string> mesajlar = new List<string>();

            if (hesapDosyaKullan)
            {
                string hesaplarPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "source", "ngl-spammer/hesaplar.txt");

                if (!File.Exists(hesaplarPath))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("[-] 'hesaplar.txt' dosyası bulunamadı!");
                    return;
                }
                hesaplar.AddRange(File.ReadAllLines(hesaplarPath));
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.Write("NGL kullanıcı adı: ");
                Console.ForegroundColor = ConsoleColor.White;
                string username = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(username))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("[-] Geçersiz kullanıcı adı!");
                    return;
                }

                hesaplar.Add(username);
            }

            if (mesajDosyaKullan)
            {
                string mesajlarPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "source", "ngl-spammer/mesajlar.txt");

                if (!File.Exists(mesajlarPath))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("[-] 'mesajlar.txt' dosyası bulunamadı!");
                    return;
                }
                mesajlar.AddRange(File.ReadAllLines(mesajlarPath));
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.Write("Mesaj: ");
                Console.ForegroundColor = ConsoleColor.White;
                string mesaj = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(mesaj))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("[-] Geçersiz mesaj!");
                    return;
                }

                mesajlar.Add(mesaj);
            }

            drawLogo();

            foreach (var username in hesaplar)
            {
                for (int i = 0; i < istekSayisi; i++)
                {
                    foreach (var question in mesajlar)
                    {
                        var parameters = new Dictionary<string, string>
                        {
                            { "username", username },
                            { "question", question },
                            { "deviceId", "Discord: _matrixdeveloper59" + deviceID },
                            { "gameSlug", "" },
                            { "referrer", "" }
                        };

                        using (var httpClient = new HttpClient())
                        {
                            var content = new FormUrlEncodedContent(parameters);
                            try
                            {
                                var response = await httpClient.PostAsync("https://ngl.link/api/submit", content);

                                if (response.StatusCode == HttpStatusCode.OK)
                                {
                                    var responseContent = await response.Content.ReadAsStringAsync();
                                    dynamic responseObject = JsonConvert.DeserializeObject(responseContent);
                                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                                    Console.WriteLine($"[+] Gönderildi! Kullanıcı: {username}, Mesaj: {question}");
                                    count++;
                                    Console.Title = $"NGL Spammer Tool for Emancipated Zero | Gönderilen Sorular: {count}";
                                }
                                else
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine($"[-] Başarısız! Hata Kodu: {response.StatusCode} | Tekrar deneniyor...");
                                }
                            }
                            catch (HttpRequestException ex)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine($"[-] Ağ hatası! Hata: {ex.Message}");
                                break;
                            }
                            catch (Exception ex)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine($"[-] Başarısız! Hata: {ex.Message}");
                            }
                        }

                        if (aralik > 0)
                        {
                            await Task.Delay(aralik);
                        }
                    }
                }
            }

            Console.ForegroundColor = ConsoleColor.White;
        }

        private static void Yapilandir()
        {
            string sourcePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "source");

            if (!Directory.Exists(sourcePath))
            {
                Directory.CreateDirectory(sourcePath);
            }

            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
            {
                FileName = sourcePath,
                UseShellExecute = true,
                Verb = "open"
            });

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("[+] Klasör açıldı. Lütfen gerekli dosyaları düzenleyin.");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Devam etmek için bir tuşa basın...");
            Console.ReadKey();
        }

        static string GenerateRandomDeviceID()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[25];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }
            return new string(stringChars);
        }

        public static void drawLogo()
        {
            Console.WriteLine(@"
    _   __________       ____  ____  ______
   / | / / ____/ /      / __ )/ __ \/ _  __/
  /  |/ / / __/ /      / __  / / / / / /   
 / /|  / /_/ / /___   / /_/ / /_/ / / /    
/_/ |_|\____/_____/  /_____/_____/ /_/     
                                           
 _matrixdeveloper59 tarafından yapıldı
", Console.ForegroundColor = ConsoleColor.Cyan);
        }
    }
}