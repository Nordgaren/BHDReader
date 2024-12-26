using SoulsFormats;

namespace BHDReader.BHD5Utils;

static class Util {
    
    private const uint PRIME = 37;
    private const ulong PRIME64 = 0x85ul;
    public static ulong ComputeHash(string path, BHD5.Game game) {
        string hashable = path.Trim().Replace('\\', '/').ToLowerInvariant();
        if (!hashable.StartsWith("/")) {
            hashable = '/' + hashable;
        }
        return game >= BHD5.Game.EldenRing ? hashable.Aggregate(0ul, (i, c) => i * PRIME64 + c) : hashable.Aggregate(0u, (i, c) => i * PRIME + c);
    }
}