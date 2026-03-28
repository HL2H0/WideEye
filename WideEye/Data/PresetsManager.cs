using System.Diagnostics;
using MelonLoader;
using System.Text.Json;
using System.Text.Json.Serialization;
using WideEye.Core;
using WideEye.Utilities;
using WideEye.UI;

namespace WideEye.Data
{
    public static class PresetsManager
    {
        public static Dictionary<string, WideEyeSettings> Presets = new();

        private static JsonSerializerOptions _options = new JsonSerializerOptions
        {
            WriteIndented = true,
            ReferenceHandler = ReferenceHandler.Preserve
        };
        
        public static void LoadPresets()
        {
            var presets = Directory.GetFiles(Paths.PresetsPath, "*.json");
            foreach (var preset in presets)
            {
                try
                {
                    var presetClass = JsonSerializer.Deserialize<WideEyeSettings>(File.ReadAllText(preset));
                    if (presetClass != null)
                    {
                        Presets.Add(presetClass.Name, presetClass);
                        if (presetClass.Version != BuildInfo.Version)
                        {
                            presetClass.MkGlowEnabled = true;
                        }
                        MelonLogger.Msg($"Loaded Preset: {presetClass.Name}");
                    }
                    else
                    {
                        MelonLogger.Error($"Preset {preset} Couldn't be Loaded");
                    }
                }
                catch (Exception e)
                {
                    MelonLogger.Error($"Preset \"{preset}\" Is Corrupted\nError Message {e.Message}\n");
                }

            }
        }

        public static void ApplyPreset(string presetName)
        {
            var preset = Presets[presetName];
            if (preset != null)
            {
                SettingsUpdater.UpdateFOV(preset.FOV, true);
                SettingsUpdater.UpdateSmoothing(preset.RotSmoothing, preset.PosSmoothing, true);
                SettingsUpdater.UpdateOffset(preset.RotOffset.ToVector3(), ModEnums.OffsetType.Rotation);
                SettingsUpdater.UpdateOffset(preset.PosOffset.ToVector3(), ModEnums.OffsetType.Position);
                
                SettingsUpdater.TogglePostFX(preset.PostFXEnabled, true);
                
                SettingsUpdater.UpdateMkGlow(preset.MkGlowEnabled, true);
                
                SettingsUpdater.UpdateChromaticAberration(preset.CaEnabled, preset.CaIntensity, true);
                
                SettingsUpdater.UpdateAutoExposure(preset.AeEnabled, preset.AeAdaptationMode, preset.AeD2Ls, preset.AeEvComp,
                    preset.AeEvMax, preset.AeEvMin, preset.AeL2ds, preset.AeMeetringMaskMode,
                    preset.AeMeetaeMeteringProceduralFalloff, true);

                SettingsUpdater.UpdateLensDistortion(preset.LdEnabled, preset.LdCenter.ToVector2(), preset.LdIntensity, preset.LdScale,
                    preset.LdMultiplyer.X, preset.LdMultiplyer.Y, true);
                
                MelonLogger.Msg($"Applied Preset: {presetName}");
            }
        }

        public static void SavePreset(string presetName)
        {
            if (Presets.ContainsKey(presetName))
            {
                var newValues = ModMenu.GetValues();
                newValues.Name = presetName;
                newValues.Version = BuildInfo.Version;
                Presets[presetName] = newValues;
                
                var json = JsonSerializer.Serialize(newValues, _options);
                var path = Path.Combine(Paths.PresetsPath, $"{presetName}.json");
                File.WriteAllText(path, json);
                
                MelonLogger.Msg($"Saved Preset: {presetName}");
            }
        }

        public static void RefreshPresetList()
        {
            var presets = Directory.GetFiles(Paths.PresetsPath, "*.json");
            var presetNames = presets.Select(Path.GetFileNameWithoutExtension).ToList();
            foreach(var presetName in presetNames)
            {
                if (!Presets.ContainsKey(presetName))
                {
                    var presetClass = JsonSerializer.Deserialize<WideEyeSettings>(File.ReadAllText(Path.Combine(Paths.PresetsPath, $"{presetName}.json")));
                    if (presetClass != null)
                    {
                        Presets.Add(presetName, presetClass);
                        MelonLogger.Msg($"Loaded Preset: {presetName}");
                        ModMenu.CreateOnePresetPage(presetName);
                    }
                    else
                    {
                        MelonLogger.Error($"Preset {presetName} Couldn't be Loaded");
                    }
                }
            }
        }

        public static void ViewPath(string presetName)
        {
            var path = Path.Combine(Paths.PresetsPath, $"{presetName}.json");
            if (!File.Exists(path)) return;
            MelonLogger.Msg($"Path: {path}");
            ProcessStartInfo processStartInfo = new("explorer.exe", $"/select, \"{path}\"");
            Process.Start(processStartInfo);
        }

        public static void CreatePreset(string presetName)
        {
            presetName = presetName.ToLower();
            var preset = ModMenu.GetValues();
            Presets.Add(presetName, preset);
            preset.Version = BuildInfo.Version;
            preset.Name = presetName;
            var json = JsonSerializer.Serialize(preset, _options);
            var path = Path.Combine(Paths.PresetsPath, $"{presetName}.json");
            File.WriteAllText(path, json);
            
            ModMenu.CreateOnePresetPage(presetName);
            
            MelonLogger.Msg($"Created Preset: {presetName}");
        }
        
        public static void DeletePreset(string presetName)
        {
            if (Presets.ContainsKey(presetName))
            {
                Presets.Remove(presetName);
                var path = Path.Combine(Paths.PresetsPath, $"{presetName}.json");
                if (File.Exists(path)) File.Delete(path);
                
                MelonLogger.Msg($"Deleted Preset: {presetName}");
            }
        }
    }
}