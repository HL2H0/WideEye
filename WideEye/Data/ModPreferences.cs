using BoneLib.Notifications;
using Il2CppOccaSoftware.Exposure.Runtime;
using MelonLoader;
using UnityEngine;
using WideEye.Core;
using WideEye.UI;
using WideEye.Utilities;

namespace WideEye.Data
{
    public static class ModPreferences
    {
        public static bool AutoSave;
        //Pref

        public static MelonPreferences_Category CategWideEye;
        private static MelonPreferences_Entry<float> _prefFov;
        private static MelonPreferences_Entry<bool> _prefPostFX;
        private static MelonPreferences_Entry<Vector3> _prefRotationOffset;
        private static MelonPreferences_Entry<Vector3> _prefPositionOffset;
        private static MelonPreferences_Entry<float> _prefRotationSmoothing;
        private static MelonPreferences_Entry<float> _prefPositionSmoothing;
        private static MelonPreferences_Entry<bool> _prefShowOtherNotifi;
        private static MelonPreferences_Entry<bool> _prefShowPrefNotifi;
        private static MelonPreferences_Entry<bool> _prefShowCameraDisabledNotifi;
        private static MelonPreferences_Entry<bool> _prefShowCameraFoundNotifi;
        private static MelonPreferences_Entry<bool> _prefAutoSave;


        //Post-FX Pref

        private static MelonPreferences_Category _categPfxLd;
        private static MelonPreferences_Entry<bool> _prefLdEnabled;
        private static MelonPreferences_Entry<Vector2> _prefLdCenter;
        private static MelonPreferences_Entry<float> _prefLdIntensity;
        private static MelonPreferences_Entry<float> _prefLdScale;
        private static MelonPreferences_Entry<float> _prefLdXMultiplier;
        private static MelonPreferences_Entry<float> _prefLdYMultiplier;

        private static MelonPreferences_Category _categPfxCa;
        private static MelonPreferences_Entry<bool> _prefCaEnabled;
        private static MelonPreferences_Entry<float> _prefCaIntensity;

        private static MelonPreferences_Category _categPfxAe;
        private static MelonPreferences_Entry<bool> _prefAeEnabled;
        private static MelonPreferences_Entry<AutoExposureAdaptationMode> _prefAeAdaptationMode;
        private static MelonPreferences_Entry<float> _prefAeD2Ls;
        private static MelonPreferences_Entry<float> _prefAeEvCompensation;
        private static MelonPreferences_Entry<float> _prefAeEvMax;
        private static MelonPreferences_Entry<float> _prefAeEvMin;
        private static MelonPreferences_Entry<float> _prefAeL2Ds;
        private static MelonPreferences_Entry<AutoExposureMeteringMaskMode> _prefAeMeteringMask;
        private static MelonPreferences_Entry<float> _prefAeMetProcedFalloff;
        
        public static void CreatePref()
        {
            CategWideEye = MelonPreferences.CreateCategory("WideEye");
            _prefFov = CategWideEye.CreateEntry("Fov", 75f);
            _prefPostFX = CategWideEye.CreateEntry("PostFX", true);
            _prefRotationOffset = CategWideEye.CreateEntry("RotationOffset", new Vector3(11f, 0f, 0f));
            _prefPositionOffset = CategWideEye.CreateEntry("PositionOffset", Vector3.zero);
            _prefRotationSmoothing = CategWideEye.CreateEntry("RotationSmoothing", 0f);
            _prefPositionSmoothing = CategWideEye.CreateEntry("PositionSmoothing", 0f);
            _prefShowOtherNotifi = CategWideEye.CreateEntry("ShowOtherNotification", true);
            _prefShowPrefNotifi = CategWideEye.CreateEntry("ShowPrefNotification", true);
            _prefShowCameraDisabledNotifi = CategWideEye.CreateEntry("ShowCameraDisabledNotification", true);
            _prefShowCameraFoundNotifi = CategWideEye.CreateEntry("ShowCameraFoundNotification", true);
            _prefAutoSave = CategWideEye.CreateEntry("AutoSave", false);

            _categPfxLd = MelonPreferences.CreateCategory("WideEye_PostFX_LensDistortion");
            _prefLdEnabled = _categPfxLd.CreateEntry("Enabled", true);
            _prefLdCenter = _categPfxLd.CreateEntry("Center", new Vector2(0.50f, 0.50f));
            _prefLdIntensity = _categPfxLd.CreateEntry("Intensity", 0.48f);
            _prefLdScale = _categPfxLd.CreateEntry("Scale", 1f);
            _prefLdXMultiplier = _categPfxLd.CreateEntry("xMultiplier", 0.59f);
            _prefLdYMultiplier = _categPfxLd.CreateEntry("yMultiplier", 0.59f);

            _categPfxCa = MelonPreferences.CreateCategory("WideEye_PostFX_ChromaticAberration");
            _prefCaEnabled = _categPfxCa.CreateEntry("Enabled", true);
            _prefCaIntensity = _categPfxCa.CreateEntry("Intensity", 0.123f);

            _categPfxAe = MelonPreferences.CreateCategory("WideEye_PostFX_AutoExposure");
            _prefAeEnabled = _categPfxAe.CreateEntry("Enabled", true);
            _prefAeAdaptationMode = _categPfxAe.CreateEntry("AdaptationMode", AutoExposureAdaptationMode.Progressive);
            _prefAeD2Ls = _categPfxAe.CreateEntry("DarkToLightSpeed", 3f);
            _prefAeEvCompensation = _categPfxAe.CreateEntry("evCompensation", 2.5f);
            _prefAeEvMax = _categPfxAe.CreateEntry("evMax", 1.2f);
            _prefAeEvMin = _categPfxAe.CreateEntry("evMin", -1.2f);
            _prefAeL2Ds = _categPfxAe.CreateEntry<float>("LightToDarkSpeed", 1);
            _prefAeMeteringMask = _categPfxAe.CreateEntry("MeteringMaskMode", AutoExposureMeteringMaskMode.Procedural);
            _prefAeMetProcedFalloff = _categPfxAe.CreateEntry("MeteringProceduralFalloff", 2f);
        }

        public static void LoadPref()
        {
            SettingsApplier.ApplyFOV(_prefFov.Value, true, ModMenu.FOVSlider);
            SettingsApplier.ApplyOther(ModEnums.OtherType.PostFX, _prefPostFX.Value, true);
            ModMenu.PostFXToggle.Value = _prefPostFX.Value;
            SettingsApplier.ApplyOffset(_prefRotationOffset.Value, ModEnums.OffsetType.Rotation, true, ModMenu.XrOffset, ModMenu.YrOffset, ModMenu.ZrOffset);
            SettingsApplier.ApplyOffset(_prefPositionOffset.Value, ModEnums.OffsetType.Position, true, ModMenu.XpOffset, ModMenu.YpOffset, ModMenu.ZpOffset);
            SettingsApplier.ApplySmoothing(_prefRotationSmoothing.Value, _prefPositionSmoothing.Value, true);
                
            ModNotification.ChangeSilentNotification(_prefShowOtherNotifi.Value, _prefShowPrefNotifi.Value,
                _prefShowCameraDisabledNotifi.Value, _prefShowCameraFoundNotifi.Value, ModMenu.OtherNotifi,
                ModMenu.PrefNotifi, ModMenu.CameraDisabledNotifi, ModMenu.CameraFoundNotifi);
            
            ModMenu.AutoSave.Value = _prefAutoSave.Value;
            AutoSave = _prefAutoSave.Value;
            
            SettingsApplier.ApplyLd(_prefLdEnabled.Value, _prefLdCenter.Value, _prefLdIntensity.Value, _prefLdScale.Value, _prefLdXMultiplier.Value, _prefLdYMultiplier.Value, true);
            SettingsApplier.ApplyCa(_prefCaEnabled.Value, _prefCaIntensity.Value, true);
            SettingsApplier.ApplyAe(_prefAeEnabled.Value, _prefAeAdaptationMode.Value, _prefAeD2Ls.Value, _prefAeEvCompensation.Value, _prefAeEvMax.Value, _prefAeEvMin.Value, _prefAeL2Ds.Value, _prefAeMeteringMask.Value, _prefAeMetProcedFalloff.Value, true);
            var notification = new ModNotification(ModNotification.ModNotificationType.Preferences, "WideEye | Success", "Loaded Preferences.", NotificationType.Success, 2);
            notification.Show();
            MelonLogger.Msg(ConsoleColor.Green, "Loaded Preferences.");
        }

        public static void SavePref()
        {
            _prefFov.Value = ModMenu.FOVSlider.Value;
            _prefPostFX.Value = ModMenu.PostFXToggle.Value;
            _prefRotationOffset.Value = new Vector3(ModMenu.XrOffset.Value, ModMenu.YrOffset.Value, ModMenu.ZrOffset.Value);
            _prefPositionOffset.Value = new Vector3(ModMenu.XpOffset.Value, ModMenu.YpOffset.Value, ModMenu.ZpOffset.Value);
            _prefRotationSmoothing.Value = ModMenu.RSmoothing.Value;
            _prefPositionSmoothing.Value = ModMenu.PSmoothing.Value;

            _prefShowOtherNotifi.Value = ModMenu.OtherNotifi.Value;
            _prefShowPrefNotifi.Value = ModMenu.PrefNotifi.Value;
            _prefShowCameraDisabledNotifi.Value = ModMenu.CameraDisabledNotifi.Value;
            _prefShowCameraFoundNotifi.Value = ModMenu.CameraFoundNotifi.Value;

            _prefAutoSave.Value = ModMenu.AutoSave.Value;
            
            _prefLdEnabled.Value = ModMenu.LdEnabled.Value;
            _prefLdCenter.Value = new Vector2(ModMenu.LdCenterX.Value, ModMenu.LdCenterY.Value);
            _prefLdIntensity.Value = ModMenu.LdIntensity.Value;
            _prefLdScale.Value = ModMenu.LdScale.Value;
            _prefLdXMultiplier.Value = ModMenu.LdXMultiplier.Value;
            _prefLdYMultiplier.Value = ModMenu.LdYMultiplier.Value;

            _prefCaEnabled.Value = ModMenu.CaEnabled.Value;
            _prefCaIntensity.Value = ModMenu.CaIntensity.Value;

            _prefAeEnabled.Value = ModMenu.AeEnabled.Value;
            _prefAeAdaptationMode.Value = (AutoExposureAdaptationMode)ModMenu.AeAdaptationMode.Value;
            _prefAeD2Ls.Value = ModMenu.AeD2Ls.Value;
            _prefAeEvCompensation.Value = ModMenu.AeEvCompensation.Value;
            _prefAeEvMax.Value = ModMenu.AeEvMax.Value;
            _prefAeEvMin.Value = ModMenu.AeEvMin.Value;
            _prefAeL2Ds.Value = ModMenu.AeL2Ds.Value;
            _prefAeMeteringMask.Value = (AutoExposureMeteringMaskMode)ModMenu.AeMeteringMaskMode.Value;
            _prefAeMetProcedFalloff.Value = ModMenu.AeMeteringProceduralFalloff.Value;

            CategWideEye.SaveToFile(false);
            _categPfxLd.SaveToFile(false);
            _categPfxCa.SaveToFile(false);
            _categPfxAe.SaveToFile(false);

            if (AutoSave) return;
            
            var notification = new ModNotification(ModNotification.ModNotificationType.Preferences, "WideEye | Success", "Saved Preferences.", NotificationType.Success, 2);
            notification.Show();
            MelonLogger.Msg(ConsoleColor.Green, "Saved Preferences.");
        }
        
        
        public static void ClearPref()
        {
            var categ = CategWideEye.Entries
            .Concat(_categPfxLd.Entries)
            .Concat(_categPfxCa.Entries)
            .Concat(_categPfxAe.Entries)
            .ToList();

            foreach (var entry in categ)
            {
                entry.ResetToDefault();
            }
            LoadPref();
            var notification = new ModNotification(ModNotification.ModNotificationType.Preferences, "WideEye | Success", "Cleared All Preferences", NotificationType.Success, 2);
            notification.Show();
            MelonLogger.Msg(ConsoleColor.Green, "Done!, Cleared All Preferences");
        }

    }
}
