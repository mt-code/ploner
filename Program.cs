using ploner.classes;

namespace ploner
{
    internal class Program
    {
        private const string ConfigPath = "backups.json";
        
        public static void Main(string[] args) {
            parseArguments(args);
        }

        private static void parseArguments(string[] args) {
            var backupConfig = new BackupConfiguration(ConfigPath);
            
            if (args != null && args.Length > 0) {
                if (args[0].ToLower() == "backup") {
                    Cloner.cloneAll(backupConfig);
                }
            }
            else {
                var parser = new InteractiveParser(backupConfig);
                parser.start();
            }
        }
    }
}