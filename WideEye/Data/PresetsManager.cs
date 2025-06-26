using System.Diagnostics;
using MelonLoader;
using System.Text.Json;
using System.Text.Json.Serialization;
using BoneLib.BoneMenu;
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
                var presetClass = JsonSerializer.Deserialize<WideEyeSettings>(File.ReadAllText(preset));
                if (presetClass != null)
                {
                    Presets.Add(presetClass.Name, presetClass);
                    MelonLogger.Msg($"Loaded Preset: {presetClass.Name}");
                }
                else
                {
                    MelonLogger.Error($"Preset {preset} Couldn't be Loaded");
                }
            }
        }

        public static void ApplyPreset(string presetName)
        {
            var preset = Presets[presetName];
            if (preset != null)
            {
                SettingsApplier.ApplyFOV(preset.FOV, true);
                SettingsApplier.ApplySmoothing(preset.RotSmoothing, preset.PosSmoothing, true);
                SettingsApplier.ApplyOffset(preset.RotOffset.ToVector3(), ModEnums.OffsetType.Rotation);
                SettingsApplier.ApplyOffset(preset.PosOffset.ToVector3(), ModEnums.OffsetType.Position);
                
                SettingsApplier.ApplyCa(preset.CaEnabled, preset.CaIntensity, true);
                
                SettingsApplier.ApplyAe(preset.AeEnabled, preset.AeAdaptationMode, preset.AeD2Ls, preset.AeEvComp,
                    preset.AeEvMax, preset.AeEvMin, preset.AeL2ds, preset.AeMeetringMaskMode,
                    preset.AeMeetaeMeteringProceduralFalloff, true);

                SettingsApplier.ApplyLd(preset.LdEnabled, preset.LdCenter.ToVector2(), preset.LdIntensity, preset.LdScale,
                    preset.LdMultiplyer.X, preset.LdMultiplyer.Y, true);
                
                MelonLogger.Msg($"Applied Preset: {presetName}");
            }
        }

        public static void SavePreset(string presetName)
        {
            if (Presets.ContainsKey(presetName))
            {
                var newValues = ModMenu.GetValues();
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
            foreach (var preset in presets)
            {
                if (Presets.ContainsKey(Path.GetFileNameWithoutExtension(preset))) return;
                var presetClass = JsonSerializer.Deserialize<WideEyeSettings>(File.ReadAllText(preset));
                if (presetClass != null)
                {
                    Presets.Add(presetClass.Name, presetClass);
                    MelonLogger.Msg($"Loaded Preset: {presetClass.Name}");
                }
                else
                {
                    MelonLogger.Error($"Preset {preset} Couldn't be Loaded");
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
    }
}