using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace Emancipated_Zero
{
    class CheckPythonRequirements
    {
        public static void CheckPythonRequirementsMain(string applicationName, string pipRequirementsRaw)
        {
            // Python ve pip'in yüklü olup olmadığını kontrol et
            if (!IsPythonInstalled())
            {
                Console.WriteLine("Python yüklü değil.");
                return;
            }

            if (!IsPipInstalled())
            {
                Console.WriteLine("pip yüklü değil.");
                return;
            }

            // Gereksinim duyulan tüm pip paketlerini kontrol et ve yükle
            var pipRequirements = ParsePipRequirements(pipRequirementsRaw);
            foreach (var requirement in pipRequirements)
            {
                CheckAndInstallPythonPackage(requirement.Key, requirement.Value);
            }

            // Tüm gereksinimler sağlandıysa, başlatılacak dosyayı çalıştır
            Console.WriteLine("Tüm gereksinimler sağlandı, başlatılıyor...");
            RunApplication(applicationName); // Uygulamanın adını parametre olarak al
            Console.ReadKey();
        }

        static bool IsPythonInstalled()
        {
            return RunCommand("python --version", true).Contains("Python");
        }

        static bool IsPipInstalled()
        {
            return RunCommand("pip --version", true).Contains("pip");
        }

        static void CheckAndInstallPythonPackage(string packageName, string version)
        {
            string installedPackages = RunCommand($"pip show {packageName}", false);
            if (installedPackages.Contains($"Version: {version}"))
            {
                Console.WriteLine($"{packageName} {version} zaten yüklü.");
            }
            else
            {
                Console.WriteLine($"{packageName} {version} yüklü değil, yükleniyor...");
                RunCommand($"pip install {packageName}=={version}", true);
            }
        }

        static string RunCommand(string command, bool outputVisible)
        {
            var processStartInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = $"/C {command}",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = !outputVisible
            };

            var process = Process.Start(processStartInfo);
            using (var reader = new StreamReader(process.StandardOutput.BaseStream))
            {
                string output = reader.ReadToEnd();
                if (outputVisible)
                {
                    Console.WriteLine(output);
                }
                return output;
            }
        }

        static void RunApplication(string applicationName)
        {
            string applicationPath = GetApplicationPath(applicationName);
            if (applicationPath != null)
            {
                // Verilen dosya yolunu çalıştır
                Process.Start(applicationPath);
            }
            else
            {
                Console.WriteLine("Geçersiz uygulama adı.");
            }
        }

        static string GetApplicationPath(string applicationName)
        {
            // Uygulamanın adı ile ilgili dosya yolunu belirle
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory; // Uygulamanın bulunduğu dizin

            switch (applicationName.ToLower())
            {
                case "smsbomber":
                    return Path.Combine(baseDirectory, "source/external-tools/sms-bomber/clinet.py"); // Uygulamanın bulunduğu dizinden çalıştır
                case "matrix-main":
                    return Path.Combine(baseDirectory, "source/external-tools/matrix-main/build.bat"); // Matrix-Main uygulamasının yolu
                default:
                    return null; // Geçersiz uygulama adı
            }
        }

        static Dictionary<string, string> ParsePipRequirements(string requirements)
        {
            var pipRequirements = new Dictionary<string, string>();
            var entries = requirements.Split(',');

            foreach (var entry in entries)
            {
                var parts = entry.Split('=');
                if (parts.Length == 2)
                {
                    pipRequirements[parts[0].Trim()] = parts[1].Trim();
                }
            }

            return pipRequirements;
        }
    }
}
