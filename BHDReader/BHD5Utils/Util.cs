using SoulsFormats;

namespace BHDReader.BHD5Utils;

internal static class Util {
    
    private const uint PRIME = 37;
    private const ulong PRIME64 = 0x85ul;
    /// <summary>
    /// Compute the hash of a path, ensuring the right slash type for file paths. Works for both 32 and 64 bit hashes.
    /// </summary>
    /// <param name="path">Path to hash</param>
    /// <param name="game">Game that path belongs to.</param>
    /// <returns></returns>
    public static ulong ComputeHash(string path, BHD5.Game game) {
        string hashable = path.Trim().Replace('\\', '/').ToLowerInvariant();
        if (!hashable.StartsWith("/")) {
            hashable = '/' + hashable;
        }
        return game >= BHD5.Game.EldenRing ? hashable.Aggregate(0ul, (i, c) => i * PRIME64 + c) : hashable.Aggregate(0u, (i, c) => i * PRIME + c);
    }
}