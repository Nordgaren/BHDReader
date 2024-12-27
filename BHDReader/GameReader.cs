using BHDReader.BHD5Utils;

namespace BHDReader;
/// <summary>
/// A GameReader that reads all the archives for the game, and will enumerate all of them to find the files you need.
/// You can also search in a specific archive, by name, just as well.
/// </summary>
public class GameReader {
    private BHDGame _game { get; }
    private List<BHDReader> _bhdReaders { get; }
    /// <summary>
    /// Path to the folder that has the game exe
    /// </summary>
    private string _gamePath { get; }

    /// <summary>
    /// A GameReader that reads all the archives for the game, and will enumerate all of them to find the files you need.
    /// You can also search in a specific archive, by name, just as well.
    /// </summary>
    /// <param name="game">The game you want to unpack from</param>
    /// <param name="gamePath">Path to the game exe or the folder that contains the game exe</param>
    /// <param name="cachePath">A path to cache the decrypted BHD5 header, so you don't have to do it over and over</param>
    /// <exception cref="DirectoryNotFoundException">Throws a DirectoryNotFoundException if the Steam path to the game you are for, is not found</exception>
    public GameReader(BHDGame game, string? gamePath = null, string? cachePath = null) {
        _game = game;
        _bhdReaders = new List<BHDReader>();

        _gamePath = getGamePathFromArgs(game, gamePath);
        
        foreach (string archive in _game.ArchiveNames()) {
            _bhdReaders.Add(new BHD5Reader(
                $"{_gamePath}/{archive}",
                _game.ToBHD5Game(),
                _game.GetArchiveKey(archive),
                cachePath
            ));
        }
    }
    /// <summary>
    /// Private method to sort out the args passed to the constructor
    /// </summary>
    /// <param name="game">The game to search for</param>
    /// <param name="gamePath">Constructor arg that is optional</param>
    /// <returns>String path to the folder that contains the game exe</returns>
    /// <exception cref="DirectoryNotFoundException">Throws a DirectoryNotFoundException if the Steam path to the game you are for, is not found</exception>
    static string getGamePathFromArgs(BHDGame game, string? gamePath) {
        if (gamePath == null) {
            string steamPath = SteamPath.SteamPath.Find(game.GetAppId()) ?? throw new DirectoryNotFoundException($"Could not find the steam path to the game {game}.");
            return $"{steamPath}/Game/";
        }
      
        return File.Exists(gamePath) ? Path.GetDirectoryName(gamePath)! : gamePath;
    }
    
    /// <summary>
    /// Returns the path to the folder where the games exe is.
    /// </summary>
    /// <returns>String path to the game folder</returns>
    public string GetGameFolderPath() {
        return _gamePath;
    }
    /// <summary>
    /// Gets the file you are looking for, by path. Must provide the full path of the file in the games file system.
    /// </summary>
    /// <param name="filePath">Path of the file to be unpacked</param>
    /// <returns>The file as an array of bytes if it is found, otherwise null</returns>
    public byte[]? GetFile(string filePath) {
        foreach (BHDReader reader in _bhdReaders) {
            byte[]? bytes = reader.GetFile(filePath);
            if (bytes == null) {
                continue;
            }
            return bytes;
        }

        return null;
    }
    /// <summary>
    /// Gets the file you are looking for, by hash.
    /// </summary>
    /// <param name="fileHash">Hash of the file to be unpacked</param>
    /// <returns>The file as an array of bytes if it is found, otherwise null</returns>
    public byte[]? GetFileByHash(ulong fileHash) {
        foreach (BHDReader reader in _bhdReaders) {
            byte[]? bytes = reader.GetFile(fileHash);
            if (bytes == null) {
                continue;
            }
            return bytes;
        }

        return null;
    }
    /// <summary>
    /// Gets the file you are looking for, by path. Must provide the full path of the file in the games file system. Limits
    /// the search to a single archive header.
    /// </summary>
    /// <param name="filePath">Path of the file to be unpacked</param>
    /// <param name="archive">Archive name of the archive you want to search</param>
    /// <returns>The file as an array of bytes if it is found, otherwise null</returns>
    public byte[]? GetFileInArchive(string filePath, string archive) {
        foreach (BHDReader reader in _bhdReaders) {
            if (reader.ArchiveName() != archive) {
                continue;
            }
            return reader.GetFile(filePath);
        }

        return null;
    }
    /// <summary>
    /// Gets the file you are looking for, by hash. Limits the search to a single archive header.
    /// </summary>
    /// <param name="fileHash">Hash of the file to be unpacked</param>
    /// <param name="archive">Archive name of the archive you want to search</param>
    /// <returns>The file as an array of bytes if it is found, otherwise null</returns>
    public byte[]? GetFileByHashInArchive(ulong fileHash, string archive) {
        foreach (BHDReader reader in _bhdReaders) {
            if (reader.ArchiveName() != archive) {
                continue;
            }
            return reader.GetFile(fileHash);
        }

        return null;
    }
}

