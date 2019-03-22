using ploner.classes;

namespace ploner
{
    internal class Program
    {
        private const string ConfigPath = "backups.json";
        
        public static void Main(string[] args) {
            var backupConfig = new BackupConfiguration(ConfigPath);
            
        }
    }
}