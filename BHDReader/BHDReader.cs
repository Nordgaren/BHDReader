namespace BHDReader;

/// <summary>
/// A reader interface that abstracts the important parts of reading a BHD, and allows for future BHD formats.
/// </summary>
public interface BHDReader {
    /// <summary>
    /// Get the name of the archive that this reader is responsible for
    /// </summary>
    /// <returns>Archive name as a string</returns>
    public string ArchiveName();
    /// <summary>
    /// Gets the file you are looking for, by path. Must provide the full path of the file in the games file system.
    /// </summary>
    /// <param name="path">Path of the file to be unpacked</param>
    /// <returns>The file as an array of bytes if it is found, otherwise null</returns>
    public byte[]? GetFile(string path);
    /// <summary>
    /// Gets the file you are looking for, by hash.
    /// </summary>
    /// <param name="hash">Hash of the file to be unpacked</param>
    /// <returns>The file as an array of bytes if it is found, otherwise null</returns>
    public byte[]? GetFile(ulong hash);
}