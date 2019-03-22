using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading;
using ploner.model;

namespace ploner.classes
{
    public class Cloner
    {
        private readonly Backup _backup;

        /// <summary>
        /// Performs all the clones that are present in the backup configuration.
        /// </summary>
        /// <param name="backupConfig">The backup configuration containing the various input/output paths.</param>
        public static void cloneAll(BackupConfiguration backupConfig) {
            foreach (var backup in backupConfig.backups) {
                var cloner = new Cloner(backup);
                cloner.start();
            }
        }
        
        private Cloner(Backup backup) {
            _backup = backup;
        }

        /// <summary>
        /// Initialises the required directories and begins cloning the backup item.
        /// </summary>
        private void start() {
            initialise();
            cloneDirectory(_backup.inputPath);
        }

        /// <summary>
        /// Checks the input path directory exists and creates the output directory if it doesn't already exist.
        /// </summary>
        /// <exception cref="FileNotFoundException">Thrown if the input path directory does not exist.</exception>
        private void initialise() {
            if (!Directory.Exists(_backup.inputPath)) {
                throw new FileNotFoundException($"The input directory does not exist. ({_backup.inputPath})");
            }

            if (!Directory.Exists(_backup.outputPath)) {
                Directory.CreateDirectory(_backup.outputPath);
            }
        }

        /// <summary>
        /// Recursively clones the files and directories.
        /// </summary>
        /// <param name="directory">The directory to the cloned.</param>
        private void cloneDirectory(string directory) {
            var relativeDirectory =
                directory.Replace(_backup.inputPath, "").TrimStart('/').TrimStart('\\');
            
            // Copy files in directory.
            foreach (var file in Directory.EnumerateFiles(directory)) {
                var fullFilePath = Path.GetFullPath(file);
                var outputFilePath = Path.Combine(_backup.outputPath, relativeDirectory, Path.GetFileName(file));

                if (File.Exists(outputFilePath) && getMd5(fullFilePath) == getMd5(outputFilePath)) {
                    continue;
                }

                // Add the file copy delegate to the thread pool.
                ThreadPool.QueueUserWorkItem(delegate(object paths) {
                    var filePaths = paths as object[];
                    var sourcePath = filePaths[0].ToString();
                    var targetPath = filePaths[1].ToString();
                    
                    File.Copy(sourcePath, targetPath, true);
                    Console.WriteLine($"[F] {targetPath}");
                }, new object[] { fullFilePath, outputFilePath });
            }
            
            // Copy directories in directory.
            foreach (var dir in Directory.EnumerateDirectories(directory)) {
                var outputDirectory = Path.Combine(_backup.outputPath, relativeDirectory, Path.GetFileName(dir));

                if (!Directory.Exists(outputDirectory)) {
                    Directory.CreateDirectory(outputDirectory);
                    Console.WriteLine($"[D] {outputDirectory}");
                }
                
                cloneDirectory(Path.GetFullPath(dir));
            }
        }

        /// <summary>
        /// Gets the MD5 hash of the file.
        /// </summary>
        /// <param name="filePath">The full file path of the file to be hashed.</param>
        /// <returns>The MD5 hash as a string.</returns>
        private string getMd5(string filePath) {
            using (var md5 = MD5.Create()) {
                using (var stream = File.OpenRead(filePath)) {
                    return BitConverter.ToString(md5.ComputeHash(stream));
                }
            }
        }
    }
}