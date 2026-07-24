using System.Text.Json;
using Il2CppSystem.Text;
using MelonLoader;
using WideEye.Core;
using BuildInfo = WideEye.Core.BuildInfo;

namespace WideEye.Utilities;

public class VersionCheck
{
    private const string ApiUrl = "https://api.github.com/repos/HL2H0/WideEye/releases/latest";

    public static bool Fetched;
    public static bool IsLatest;
    public static string LatestVersion;
    
    public static async Task CheckForUpdates()
    {
        try
        {
            using HttpClient client = new();
            client.DefaultRequestHeaders.UserAgent.ParseAdd("WideEye-VersionCheck");
            client.DefaultRequestHeaders.Accept.ParseAdd("application/vnd.github+json");
            client.Timeout = TimeSpan.FromSeconds(5);

            var json = await client.GetStringAsync(ApiUrl);

            using var document = JsonDocument.Parse(json);
            var root = document.RootElement;

            if (root.TryGetProperty("tag_name", out var tagElement))
            {
                var rawTag = tagElement.GetString();

                var currentVersionStr = BuildInfo.Version;
                var remoteVersionStr = rawTag.StartsWith("v", StringComparison.OrdinalIgnoreCase)
                    ? rawTag.Substring(1)
                    : rawTag;

                if (Version.TryParse(remoteVersionStr, out var remoteVersion) &&
                    Version.TryParse(currentVersionStr, out var currentVersion))
                {
                    if (remoteVersion > currentVersion)
                    {
                        Melon<Mod>.Logger.Warning("New Version Available");
                        Melon<Mod>.Logger.Warning(
                            $"Current Version: {currentVersionStr} | Latest Version: {remoteVersionStr}");
                        Melon<Mod>.Logger.Warning("Please update for the latest features and bug fixes");
                        IsLatest = false;
                        LatestVersion = remoteVersionStr;
                    }
                    else
                    {
                        Melon<Mod>.Logger.Msg("WideEye is up to date");
                        IsLatest = true;
                    }
                    Fetched = true;
                }
            }
        }
        catch (Exception ex)
        {
            // Mod.Logger.Error($"Could not fetch updates from GitHub API: {ex.Message}");
            Melon<Mod>.Logger.Error($"Could not fetch updates from GitHub API: {ex.Message}");
            Fetched = false;
        }
    }
}