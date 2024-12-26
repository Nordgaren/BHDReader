using SoulsFormats;
using static SoulsFormats.BHD5;

namespace BHDReader.BHD5Utils;

public class BHD5Data {
    private readonly Lazy<FileStream> _fileStream;
    private readonly Lazy<BHD5> _header;

    public BHD5Data(Func<BHD5> header, Func<FileStream> fs) {
        _header = new Lazy<BHD5>(header);
        _fileStream = new Lazy<FileStream>(fs);

    }
    public byte[]? GetFile(ulong hash) {
        Bucket bucket = _header.Value.Buckets[(int)(hash % (ulong)_header.Value.Buckets.Count)];

        foreach (FileHeader header in bucket) {
            if (header.FileNameHash == hash) {
                return header.ReadFile(_fileStream.Value);
            }
        }

        return null;
    }
    public string GetSalt() {
        return _header.Value.Salt;
    }
}
