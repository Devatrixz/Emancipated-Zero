using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Emancipated_Zero
{
    public class FantiaDownloader
    {
        private readonly HttpClient _client;

        public FantiaDownloader(string userAgent)
        {
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Add("User-Agent", userAgent);
        }

        public static async Task RunFantiaDownloaderTask()
        {
            try
            {
                await Task.Run(() => FantiaDownloaderSystem.FantiaMain(new string[] { }));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}", Console.ForegroundColor = ConsoleColor.Red);
                Console.ReadKey();
            }
        }

        public async Task<bool> LoginAsync(string sessionCookie)
        {
            _client.DefaultRequestHeaders.Add("Cookie", $"_session_id={sessionCookie}");

            var response = await _client.GetAsync("https://fantia.jp/api/v1/me");
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Giriş başarılı!");
                return true;
            }

            Console.WriteLine("Giriş başarısız. Lütfen oturum çerezinizi kontrol edin.");
            Console.ReadKey();
            return false;
        }

        public async Task<string> GetFanclubMetadataAsync(int fanclubId)
        {
            string url = $"https://fantia.jp/api/v1/fanclubs/{fanclubId}";
            var response = await _client.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine("Fanclub metaverileri alınamadı.");
                Console.ReadKey();
                return null;
            }

            string jsonResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine("Metaveriler başarıyla alındı.");
            Console.ReadKey();
            return jsonResponse;
        }

        public async Task DownloadFileAsync(string fileUrl, string outputPath)
        {
            var response = await _client.GetAsync(fileUrl, HttpCompletionOption.ResponseHeadersRead);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine("Dosya indirilemedi: " + fileUrl);
                Console.ReadKey();
                return;
            }

            using (var fileStream = new FileStream(outputPath, FileMode.Create, FileAccess.Write, FileShare.None))
            using (var responseStream = await response.Content.ReadAsStreamAsync())
            {
                await responseStream.CopyToAsync(fileStream);
            }

            Console.WriteLine($"Dosya başarıyla indirildi: {outputPath}");
            Console.ReadKey();
        }

        public static string SanitizeForPath(string value)
        {
            foreach (char c in Path.GetInvalidFileNameChars())
            {
                value = value.Replace(c, '_');
            }

            return value.Trim();
        }
    }

    public class FantiaDownloaderSystem
    {
        public static void FantiaMain(string[] args)
        {
            var downloader = new FantiaDownloader("fantiadl/1.0");
            
            Console.WriteLine("Oturum çerezinizi girin: ");
            string sessionCookie = Console.ReadLine();

            var loginTask = downloader.LoginAsync(sessionCookie);
            loginTask.Wait();

            if (!loginTask.Result)
            {
                return;
            }

            Console.WriteLine("Metaverilerini almak istediğiniz fanclub ID'sini girin: ");
            int fanclubId = int.Parse(Console.ReadLine());

            var metadataTask = downloader.GetFanclubMetadataAsync(fanclubId);
            metadataTask.Wait();

            string metadata = metadataTask.Result;
            if (metadata != null)
            {
                Console.WriteLine(metadata);
            }

            Console.WriteLine("İndirmek istediğiniz dosyanın URL'sini girin: ");
            string fileUrl = Console.ReadLine();

            Console.WriteLine("Dosyanın kaydedileceği yolu girin: ");
            string outputPath = Console.ReadLine();

            var downloadTask = downloader.DownloadFileAsync(fileUrl, outputPath);
            downloadTask.Wait();
        }
    }
}