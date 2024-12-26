using SoulsFormats;

namespace BHDReader;

/// <summary>
/// Indicates the game to unpack from
/// </summary>
public enum BHDGame {
    /// <summary>
    /// Dark Souls 1, both PC and console versions.
    /// </summary>
    DarkSouls1,
    
    /// <summary>
    /// Dark Souls 1, both PC and console versions.
    /// </summary>
    DarkSouls1Remasterd,

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
