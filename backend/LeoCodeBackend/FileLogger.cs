using System.Diagnostics;

namespace LeoCodeBackend
{
    public class FileLogger
    {
        private readonly string _logFilePath;

        public FileLogger(string logFilePath)
        {
            _logFilePath = logFilePath;
        }

        public void Log(string message)
        {
            try
            {
                // Überprüfen, ob die Log-Datei existiert, wenn nicht, erstellen
                if (!File.Exists(_logFilePath))
                {
                    using (StreamWriter sw = File.CreateText(_logFilePath))
                    {
                        sw.WriteLine($"Log File created at: {DateTime.Now}");
                    }
                }

                // Log-Nachricht anhängen
                using (StreamWriter sw = File.AppendText(_logFilePath))
                {
                    sw.WriteLine($"{DateTime.Now}: {message}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while logging: {ex.Message}");
            }
        }
    }
}
