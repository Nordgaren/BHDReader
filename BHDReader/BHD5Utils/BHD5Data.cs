using System.Text;
using SoulsFormats;
using static SoulsFormats.BHD5;

namespace BHDReader.BHD5Utils;

internal class BHD5Data {
    private readonly Lazy<FileStream> _fileStream;
    private readonly Lazy<BHD5> _header;

    public BHD5Data(Func<BHD5> header, Func<FileStream> fs) {
        _header = new Lazy<BHD5>(header);
        _fileStream = new Lazy<FileStream>(fs);
    }
    /// <summary>
    /// Get file using the hash value of the file path.
    /// </summary>
    /// <param name="hash">Hash of the file in the DL2 file system</param>
    /// <returns>The file as bytes if found, otherwise null.</returns>
    public byte[]? GetFile(ulong hash) {
        Bucket bucket = _header.Value.Buckets[(int)(hash % (ulong)_header.Value.Buckets.Count)];

        foreach (FileHeader header in bucket) {
            if (header.FileNameHash == hash) {
                return header.ReadFile(_fileStream.Value);
            }
        }

        return null;
    }

    /// <summary>
    /// Gets the header salt
    /// </summary>
    /// <returns>header salt as a string</returns>
    public string GetSalt() {
        return _header.Value.Salt;
    }

    /// <summary>
    /// Makes a BHD5Data object, who's private fields ae lazily updated...
    /// </summary>
    /// <param name="path">Path to the BHD5 archive</param>
    /// <param name="game">Game associated with the BHD5 archive</param>
    /// <param name="key">RSA Key for decrypting encrypted archives, if applicable...</param>
    /// <param name="cachePath">Path to cached version of the BHD5. Leave null if no cache is to be used.</param>
    /// <returns>BHD5Data which will read the headers and open file streams when/where appropriate, instead of immediately.</returns>
    public static BHD5Data MakeBHD5Data(string path, Game game, string? key = null, string? cachePath = null) {
        return new BHD5Data(
            () => readHeaders(path, game, key, cachePath),
            () => File.OpenRead($"{path}.bdt")
        );
    }

    /// <summary>
    /// First checks cache
    /// </summary>
    /// <param name="path">Path to the BHD5 archive</param>
    /// <param name="game">Game associated with the BHD5 archive</param>
    /// <param name="key">RSA Key for decrypting encrypted archives, if applicable...</param>
    /// <param name="cachePath">Path to cached version of the BHD5. Leave null if no cache is to be used.</param>
    /// <returns>BHD5 object from SoulsFormatsNEXT</returns>
    /// <exception cref="ArgumentNullException">Will throw if the key is null and the archive headers are encrypted</exception>
    private static BHD5 readHeaders(string path, Game game, string? key = null, string? cachePath = null) {
        // We will need this path multiple times if cachePath is not null, so concatenate the path and the extension, and 
        // check if `cachePath != null
        string cachedBHD = $"{cachePath}.bhd";
        if (cachePath != null && File.Exists(cachedBHD)) {
            using MemoryStream cached = new(File.ReadAllBytes(cachedBHD));
            return Read(cached, game);
        }

        byte[] bytes = File.ReadAllBytes($"{path}.bhd");
        if (bytes[..4] != Encoding.Default.GetBytes("BHD5")) {
            using MemoryStream ms = CryptoUtil.DecryptRsa(bytes,
                key ?? throw new ArgumentNullException(nameof(key),
                    "Key cannot be null when archive headers are encrypted."));
            if (cachePath != null) {
                File.WriteAllBytes(cachedBHD, ms.ToArray());
            }
            
            return Read(ms, game);
        }

        using MemoryStream fs = new(bytes);
        return Read(fs, game);
    }
}