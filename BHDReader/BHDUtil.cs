using SoulsFormats;

namespace BHDReader;

public static class BHDUtil {
    private static Dictionary<BHDGame, string[]> _bhdPaths = new() {
        { BHDGame.DarkSouls1, new[] {"dvdbnd0", "dvdbnd1", "dvdbnd2", "dvdbnd3"} },
        { BHDGame.DarkSouls2, new[] {"GameDataEbl", "HqChrEbl", "HqMapEbl", "HqObjEbl"} },
        { BHDGame.DarkSouls2Scholar, new[] {"GameDataEbl", "LqChrEbl", "LqMapEbl", "LqObjEbl"} },
        { BHDGame.DarkSouls3, new[] {"Data1", "Data2", "Data3", "Data4", "Data5", "DLC1", "DLC2"} },
        { BHDGame.Sekiro, new[] {"Data1", "Data2", "Data3", "Data4", "Data5"} },
        { BHDGame.SekiroBonus, new[] {"Data"} },
        { BHDGame.EldenRing, new[] {"Data0", "Data1", "Data2", "Data3", "DLC", "sd\\sd", "sd\\sd_dlc02"} },
        { BHDGame.ArmoredCore6, new[] {"Data0", "Data1", "Data2", "Data3", "sd\\sd"} },
    };

    /// <summary>
    /// Get the names of the archives for the associated game
    /// </summary>
    /// <param name="bhdGame">Game the caller wants the archive names for</param>
    /// <returns></returns>
    public static string[] ArchiveNames(this BHDGame bhdGame) {
        return _bhdPaths[bhdGame];
    }
    
    /// <summary>
    /// Get RSA key for an archive
    /// </summary>
    /// <param name="game">Game that contains the archive</param>
    /// <param name="archiveName">Archive that needs decrypting</param>
    /// <returns>RSA Key for the encrypted archive, or null if there is no RSA key</returns>
    /// <exception cref="ArgumentOutOfRangeException">Throws for DS2 right now, because I don't have the keys.</exception>
    public static string? GetArchiveKey(this BHDGame game, string archiveName) {
        switch (game) {
            case BHDGame.DarkSouls1:
            case BHDGame.DarkSouls1Remasterd:
                return null;
            case BHDGame.DarkSouls2:
            case BHDGame.DarkSouls2Scholar:
                throw new ArgumentOutOfRangeException(nameof(game), game, "I forgor the RSA keys for DS2 :(");
                return ArchiveKeys.DarkSouls2Keys[archiveName];
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
            default:
                throw new ArgumentOutOfRangeException(nameof(game), game, "Game is not supported, don't have keys.");
        }
    }
    
        /// <summary>
    /// Converts BHDGame to the corresponding BHD5.Game, for use with BHD5s
    /// </summary>
    /// <param name="game">BHDGame to convert</param>
    /// <returns>BHD5.Game appropriate for the game passed in</returns>
    /// <exception cref="ArgumentOutOfRangeException">Fails with Dark Souls 1 PTDE and Remastered right now</exception>
    public static BHD5.Game ToBHD5Game(this BHDGame game) {
        switch (game) {
            case BHDGame.DarkSouls1:
                throw new ArgumentOutOfRangeException(nameof(game), game, "DS1 is not supported, yet.");
                 return BHD5.Game.DarkSouls1;
            case BHDGame.DarkSouls1Remasterd:
                throw new ArgumentOutOfRangeException(nameof(game), game, "You are trying to unpack an unpacked game...");
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
            default:
                throw new ArgumentOutOfRangeException(nameof(game), game, "Game is not supported");
        }
    }
    /// <summary>
    /// Gets the AppId of the BHDGame. This is for the SteamPath lookup. 
    /// </summary>
    /// <param name="game">Game to look up</param>
    /// <returns>string with the AppId number of the game</returns>
    /// <exception cref="ArgumentOutOfRangeException">Throws if you are trying to unpack Sekiro Bonus right now, because
    /// I don't know what to look up for the app id. Maybe it's just the base sekiro one?</exception>
    public static string GetAppId(this BHDGame game) {
        switch (game) {
            case BHDGame.DarkSouls1:
                return "211420";
            case BHDGame.DarkSouls1Remasterd:
                return "570940";
            case BHDGame.DarkSouls2:
                return "236430";
            case BHDGame.DarkSouls2Scholar:
                return "335300";
            case BHDGame.DarkSouls3:
                return "374320";
            case BHDGame.Sekiro:
                return "814380";
            case BHDGame.SekiroBonus:
                throw new ArgumentOutOfRangeException(nameof(game), game, "Sekiro Bonus is not supported. Please contact Nordgaren for support.");
                 return "814380";
            case BHDGame.EldenRing:
                return "1245620";
            case BHDGame.ArmoredCore6:
                return "1888160";
            default:
                throw new ArgumentOutOfRangeException(nameof(game), game, "Game is not supported, don't have app id");
        }
    }
}