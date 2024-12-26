using SoulsFormats;

namespace BHDReader;

/// <summary>
/// Indicates the game being read
/// </summary>
public enum BHDGame {
    /// <summary>
    /// Dark Souls 1, both PC and console versions.
    /// </summary>
    DarkSouls1,

    /// <summary>
    /// Dark Souls 2 on PC.
    /// </summary>
    DarkSouls2,

    /// <summary>
    /// Dark Souls 2 Scholar of the First Sin on PC.
    /// </summary>
    DarkSouls2Scholar,

    /// <summary>
    /// Dark Souls 3 on PC.
    /// </summary>
    DarkSouls3,

    /// <summary>
    /// Sekiro on PC.
    /// </summary>
    Sekiro,

    /// <summary>
    /// Sekiro Bonus files on PC.
    /// </summary>
    SekiroBonus,

    /// <summary>
    /// Elden Ring on PC.
    /// </summary>
    EldenRing,

    /// <summary>
    /// Armored Core 6 on PC.
    /// </summary>
    ArmoredCore6,
}

public static class BHDGameExtensions {
    public static BHD5.Game ToBHD5Game(this BHDGame game) {
        switch (game) {
            // case BHDGame.DarkSouls1:
            //     return BHD5.Game.DarkSouls1;
            case BHDGame.DarkSouls2:
            case BHDGame.DarkSouls2Scholar:
                return BHD5.Game.DarkSouls2;
            case BHDGame.DarkSouls3:
            case BHDGame.Sekiro:
            case BHDGame.SekiroBonus:
                return BHD5.Game.DarkSouls3;
            case BHDGame.EldenRing:
            case BHDGame.ArmoredCore6:
                return BHD5.Game.EldenRing;
        }

        throw new ArgumentOutOfRangeException(nameof(game), game, "Game is not supported");
    }

    public static string GetAppId(this BHDGame game) {
        switch (game) {
            case BHDGame.DarkSouls1:
                return "211420";
            case BHDGame.DarkSouls2:
                return "236430";
            case BHDGame.DarkSouls2Scholar:
                return "335300";
            case BHDGame.DarkSouls3:
                return "374320";
            case BHDGame.Sekiro:
                return "814380";
            // case BHDGame.SekiroBonus:
            //     return "814380";
            case BHDGame.EldenRing:
                return "1245620";
            case BHDGame.ArmoredCore6:
                return "1888160";
        }

        throw new ArgumentOutOfRangeException(nameof(game), game, "Game is not supported, don't have app id");
    }
    
    public static string? GetArchiveKey(this BHDGame game, string archiveName) {
        switch (game) {
            case BHDGame.DarkSouls1:
                return null;
            // case BHDGame.DarkSouls2:
            // case BHDGame.DarkSouls2Scholar:
            //     return BHD5.Game.DarkSouls2;
            case BHDGame.DarkSouls3:
                return ArchiveKeys.DarkSouls3Keys[archiveName];
            case BHDGame.Sekiro:
                return ArchiveKeys.SekiroKeys[archiveName];
            case BHDGame.SekiroBonus:
                return ArchiveKeys.SekiroBonusKeys[archiveName];
            case BHDGame.EldenRing:
                return ArchiveKeys.EldenRingKeys[archiveName];
            case BHDGame.ArmoredCore6:
                return ArchiveKeys.ArmoredCore6Keys[archiveName];
        }

        throw new ArgumentOutOfRangeException(nameof(game), game, "Game is not supported, don't have keys.");
    }
}