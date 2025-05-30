using MelonLoader.Utils;

namespace WideEye.Utilities;

public static class Paths
{
    private static string ResourcesFolder => Path.Combine(MelonEnvironment.GameRootDirectory, "UserData", "WideEye Resources");
    public static string ResourcesPath => Path.Combine(MelonEnvironment.GameRootDirectory, "UserData", "WideEye Resources", "md_resources.bundle");
    public static string PresetsPath => Path.Combine(MelonEnvironment.GameRootDirectory, "UserData", "WideEye Resources", "Presets");
    
    public static void InitFolders()
    {
        if (!Directory.Exists(ResourcesFolder))
            Directory.CreateDirectory(ResourcesFolder);
        
        if (!Directory.Exists(PresetsPath))
            Directory.CreateDirectory(PresetsPath);
        
        if (!File.Exists(ResourcesPath))
            File.Create(ResourcesPath);
    }
}