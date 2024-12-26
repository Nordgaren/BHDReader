using System.Text;
using SoulsFormats;

namespace BHDReader.BHD5Utils;

internal class BHD5Reader : BHDReader   {
    private string _path { get; }
    private string _archiveName { get; }
    private BHD5.Game _game { get; }
    string? _cachePath { get; }
    string? _key { get; }
    BHD5Data _BHDinfo { get; }

    public BHD5Reader(string path, BHD5.Game game, string? key = null, string? cachePath = null) {
        // ChangeExtension will only change the string if there is an extension.
        _path = Path.ChangeExtension(path, null);
        if (!File.Exists($"{_path}.bhd") || !File.Exists($"{_path}.bdt")) {
            throw new FileNotFoundException($"BHD or BDT not found in {Path.GetDirectoryName(_path)}.");
        }

        _archiveName = Path.GetFileName(_path);
        _game = game;
        _cachePath = cachePath != null ? $"{cachePath}/{_archiveName}" : null;
        _key = key;
        _BHDinfo = new BHD5Data(() => readHeaders(_path, _game, _key, _cachePath), () => File.OpenRead($"{_path}.bdt"));
    }
    public string ArchiveName() {
        return _archiveName;
    }
    public byte[]? GetFile(string path) {
        ulong hash = Util.ComputeHash(path, _game);
        return _BHDinfo.GetFile(hash);
    }
    public byte[]? GetFile(ulong hash) {
        return _BHDinfo.GetFile(hash);
    }
    
    private static BHD5 readHeaders(string path, BHD5.Game game, string? key = null, string? cachePath = null) {
        string cachedBHD = $"{cachePath}.bhd";
        if (cachePath != null && File.Exists(cachedBHD)) {
            using MemoryStream cached = new(File.ReadAllBytes(cachedBHD));
            return BHD5.Read(cached, BHD5.Game.EldenRing);
        }

        byte[] bytes = File.ReadAllBytes($"{path}.bhd");
        if (bytes[..4] != Encoding.Default.GetBytes("BHD5")) {
            using MemoryStream ms = CryptoUtil.DecryptRsa(bytes, key ?? throw new ArgumentNullException(nameof(key)));
            return BHD5.Read(ms, game);
        }

        using MemoryStream fs = new(bytes);
        return BHD5.Read(fs, game);
    }
    
}