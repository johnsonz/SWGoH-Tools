using System.IO;
using System.IO.Compression;

namespace SWGOH.Tools.Extensions
{
    public static class MyStreamExtensions
    {
        public static byte[] UnGzip(this byte[] gzip)
        {
            using (GZipStream gzipStream = new GZipStream((Stream)new MemoryStream(gzip), CompressionMode.Decompress))
            {
                var ms = new MemoryStream();
                gzipStream.CopyTo(ms);
                return ms.ToArray();
            }
        }
    }
}
