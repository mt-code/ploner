using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using ploner.model;

namespace ploner.classes
{
    public class BackupConfiguration
    {
        public readonly List<Backup> backups;
        private readonly string _configPath;
        
        /// <summary>
        /// Initialise our backup configuration using the specified config file.
        /// </summary>
        /// <param name="configPath">The file path for the config file to be used.</param>
        public BackupConfiguration(string configPath) {
            if (!File.Exists(configPath)) {
                File.Create(configPath);
            }
            
            _configPath = configPath;
            backups = getBackupsFromFile();          
        }

        /// <summary>
        /// Adds a new backup item to the configuration file.
        /// </summary>
        /// <param name="inputPath">The input root path that should be cloned.</param>
        /// <param name="outputPath">The output root path where the cloned files will be copied.</param>
        public void add(string inputPath, string outputPath) {
            backups.Add(new Backup(inputPath, outputPath));
            save();
        }

        /// <summary>
        /// Remove a backup item from the configuration file.
        /// </summary>
        /// <param name="index">The index of the backup item to be removed.</param>
        public void remove(int index) {
            backups.RemoveAt(index);
            save();
        }

        /// <summary>
        /// Lists the backup items and their index in the configuration file.
        /// </summary>
        public void list() {
            for (var i = 0; i < backups.Count; i++) {
                var backup = backups[i];

                Console.WriteLine($"[{i}] Input: {backup.inputPath}, Output: {backup.outputPath}");
            }
        }

        /// <summary>
        /// Saves the backup items to the configuration file.
        /// </summary>
        private void save() {
            File.WriteAllText(_configPath, JsonConvert.SerializeObject(backups));
        }

        /// <summary>
        /// Retrieves the backup items from the configuration file.
        /// </summary>
        /// <returns>
        ///     A list of backups that were stored in the configuration file. An empty list will be returned
        ///     if there are no backup items present in the file.
        /// </returns>
        private List<Backup> getBackupsFromFile() {
            var configContents = File.ReadAllText(_configPath);
            var parsedBackups = JsonConvert.DeserializeObject<List<Backup>>(configContents);

            return parsedBackups ?? new List<Backup>();
        }
    }
}