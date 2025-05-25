namespace FileStoringService.Infrastructure.Persistence
{
    public class StorageOptions
    {
        public StorageOptions(string basePath, string connectionString)
        {
            BasePath = basePath;
            ConnectionString = connectionString;
        }

        public StorageOptions()
        {
        }

        public string BasePath { get; set; }

        public string ConnectionString { get; set; }
    }
}