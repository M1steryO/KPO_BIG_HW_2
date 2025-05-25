using FileStoringService.Core.ValueObjects;

namespace FileStoringService.Core.Entities
{
    public class File
    {
        public long Id { get; private set; }
        public FileMetadata Metadata { get; private set; }

        public string Location { get; private set; }

        public File()
        {
        }

        public File(FileMetadata metadata)
        {
            Metadata = metadata;
        }

        public File(FileMetadata metadata, string location)
        {
            Metadata = metadata;
            Location = location;
        }
    }
}