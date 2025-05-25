namespace FileAnalysisService.Infrastructure.Persistence
{
    public class StorageOptions
    {
        public StorageOptions(string connectionString, string basePath)
        {
            ConnectionString = connectionString;
            BasePath = basePath;
        }

        public StorageOptions()
        {
        }

        public string ConnectionString { get; init; }
        public string BasePath { get; set; }
    }
}