namespace ploner.model
{
    public class Backup
    {
        public string inputPath;
        public string outputPath;

        public Backup(string inputPath, string outputPath) {
            this.inputPath = inputPath;
            this.outputPath = outputPath;
        }
    }
}