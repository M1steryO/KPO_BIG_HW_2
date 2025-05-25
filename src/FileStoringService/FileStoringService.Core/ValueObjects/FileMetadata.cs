using FileStoringService.Core.Common;

namespace FileStoringService.Core.ValueObjects
{
    public class FileMetadata : ValueObject
    {
        public string FileName { get; private set; }
        public string ContentType { get; private set; }
        public long Size { get; private set; }
        public string Hash { get; private set; }


        public FileMetadata(string fileName, string contentType, long size, string hash)
        {
            FileName = fileName;
            ContentType = contentType;
            Size = size;
            Hash = hash;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return FileName;
            yield return ContentType;
            yield return Size;
            yield return Hash;
        }
    }
}