namespace BHDReader;

public static class BHDNames {
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

    public static string[] GetBHDPaths(BHDGame bhdGame) {
        return _bhdPaths[bhdGame];
    }
}