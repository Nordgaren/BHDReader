using System.Text;
using SoulsFormats;

namespace BHDReader.BHD5Utils;

/// <summary>
/// A BHD5Reader which can read a BHD5 Archive and read files from it.
/// </summary>
public class BHD5Reader : BHDReader   {
    private string _archiveName { get; }
    private BHD5.Game _game { get; }
    private BHD5Data _BHDinfo { get; }

    /// <summary>
    /// BHD5Reader constructor which takes a path to the archive files, which must be side by side, and the game they are for
    /// the user can provide a key for decryption, and a cachePath to cache the decrypted header files, for future use.
    /// </summary>
    /// <param name="path">Path to the bhd, bdt, or just the name of the archive header/data blob file</param>
    /// <param name="game">Game the archives are for</param>
    /// <param name="key">RSA key, if necessary, to decrypt the archive</param>
    /// <param name="cachePath">A path to check for and store decrypted archive headers, for caching</param>
    /// <exception cref="FileNotFoundException"></exception>
    public BHD5Reader(string path, BHD5.Game game, string? key = null, string? cachePath = null) {
        // ChangeExtension will only change the string if there is an extension.
        string filePath = Path.ChangeExtension(path, null);
        if (!File.Exists($"{filePath}.bhd") || !File.Exists($"{filePath}.bdt")) {
            throw new FileNotFoundException($"BHD or BDT not found in {Path.GetDirectoryName(filePath)}.");
        }
        _archiveName = Path.GetFileName(filePath);
        _game = game;
        string? cache = cachePath != null ? $"{cachePath}/{_archiveName}" : null;
        _BHDinfo = BHD5Data.MakeBHD5Data(filePath, _game, key, cache);
    }
    /// <summary>
    /// Get the archive name
    /// </summary>
    /// <returns>string version of the archive name</returns>
    public string ArchiveName() {
        return _archiveName;
    }
    /// <summary>
    /// Get file using a file path.
    /// </summary>
    /// <param name="path">Path to the file in the DL2 file system.</param>
    /// <returns>The file as bytes if found, otherwise null.</returns>
    public byte[]? GetFile(string path) {
        ulong hash = Util.ComputeHash(path, _game);
        return _BHDinfo.GetFile(hash);
    }
    /// <summary>
    /// Get file using the hash value stored in the headers.
    /// </summary>
    /// <param name="hash">Hash of the file in the DL2 file system</param>
    /// <returns>The file as bytes if found, otherwise null.</returns>
    public byte[]? GetFile(ulong hash) {
        return _BHDinfo.GetFile(hash);
    }
    
}