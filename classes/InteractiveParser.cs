using System;

namespace ploner.classes
{
    public class InteractiveParser
    {
        private readonly BackupConfiguration _backupConfig;
        
        public InteractiveParser(BackupConfiguration backupConfig) {
            _backupConfig = backupConfig;
        }

        /// <summary>
        /// Starts the interactive console loop.
        /// </summary>
        public void start() {
            string command = "";
            outputChoices();
            
            while (command != "exit") {
                Console.Write("ploner> ");
                
                command = Console.ReadLine();
                handleInput(command);
            }
        }

        /// <summary>
        /// Output the various command options.
        /// </summary>
        private void outputChoices() {
            Console.WriteLine("list\tLists the backup items stored in the configuration file.");
            Console.WriteLine("add\tAdds a new backup item to the configuration.");
            Console.WriteLine("remove\tRemoves a backup item from the configuration.");
            Console.WriteLine("backup\tPerforms a backup for the configured backup items.");
            Console.WriteLine("help\tShows this list of commands.");
        }

        /// <summary>
        /// Handles the input command and processes the relevant task.
        /// </summary>
        /// <param name="command">The command that was entered into the console.</param>
        private void handleInput(string command) {
            switch (command) {
                case "list":
                    _backupConfig.list();
                    break;
                
                case "add":
                    addBackup();
                    break;
                
                case "remove":
                    removeBackup();
                    break;
                
                case "backup":
                    Cloner.cloneAll(_backupConfig);
                    break;
            }
        }

        /// <summary>
        /// Prompts the user for the required inputs and adds a backup item to the config.
        /// </summary>
        private void addBackup() {
            Console.Write("Enter the input root path: ");
            var inputPath = Console.ReadLine();
            
            Console.Write("Enter the output root path: ");
            var outputPath = Console.ReadLine();
            
            _backupConfig.add(inputPath, outputPath);
        }

        /// <summary>
        /// Shows the list of backup items and prompts for the index to remove.
        /// </summary>
        private void removeBackup() {
            _backupConfig.list();
            
            Console.Write("Enter the backup index: ");
            int backupIndex = Convert.ToInt32(Console.ReadLine());
            
            _backupConfig.remove(backupIndex);
        }
    }
}