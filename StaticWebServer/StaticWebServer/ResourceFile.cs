
using System.IO;
namespace StaticWebServer
{
    public class ResourceFile
    {
        public byte[] ByteArray { get; private set; }
        public string ContentType { get; private set; }

        public ResourceFile(string path, string contentType)
        {
            ByteArray = File.ReadAllBytes(path);
            ContentType = contentType;
        }
    }
}
