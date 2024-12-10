using MelonLoader;

using UnityEngine;

using BoneLib.Notifications;

using Il2CppOccaSoftware.Exposure.Runtime;

namespace WideEye
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
            Mod.ApplyFOV(_prefFov.Value, true, MenuSetup.FOVSlider);
            Mod.ApplyOther(Mod.OtherType.PostFX, _prefPostFX.Value, true);
            MenuSetup.PostFXToggle.Value = _prefPostFX.Value;
            Mod.ApplyOffset(_prefRotationOffset.Value, Mod.OffsetType.Rotation, true, MenuSetup.XROffset, MenuSetup.YrOffset, MenuSetup.ZrOffset);
            Mod.ApplyOffset(_prefPositionOffset.Value, Mod.OffsetType.Position, true, MenuSetup.XpOffset, MenuSetup.YpOffset, MenuSetup.ZpOffset);
            Mod.ApplySmoothing(_prefRotationSmoothing.Value, _prefPositionSmoothing.Value, true);
                
            ModNotification.ChangeSilentNotification(_prefShowOtherNotifi.Value, _prefShowPrefNotifi.Value,
                _prefShowCameraDisabledNotifi.Value, _prefShowCameraFoundNotifi.Value, MenuSetup.OtherNotifi,
                MenuSetup.PrefNotifi, MenuSetup.CameraDisabledNotifi, MenuSetup.CameraFoundNotifi);
            
            MenuSetup.AutoSave.Value = _prefAutoSave.Value;
            AutoSave = _prefAutoSave.Value;
            
            Mod.ApplyLd(_prefLdEnabled.Value, _prefLdCenter.Value, _prefLdIntensity.Value, _prefLdScale.Value, _prefLdXMultiplier.Value, _prefLdYMultiplier.Value, true);
            Mod.ApplyCa(_prefCaEnabled.Value, _prefCaIntensity.Value, true);
            Mod.ApplyAe(_prefAeEnabled.Value, _prefAeAdaptationMode.Value, _prefAeD2Ls.Value, _prefAeEvCompensation.Value, _prefAeEvMax.Value, _prefAeEvMin.Value, _prefAeL2Ds.Value, _prefAeMeteringMask.Value, _prefAeMetProcedFalloff.Value, true);
            var notification = new ModNotification(ModNotification.ModNotificationType.Preferences, "WideEye | Success", "Loaded Preferences.", NotificationType.Success, 2);
            notification.Show();
            MelonLogger.Msg(ConsoleColor.Green, "Loaded Preferences.");
        }

        public static void SavePref()
        {
            _prefFov.Value = MenuSetup.FOVSlider.Value;
            _prefPostFX.Value = MenuSetup.PostFXToggle.Value;
            _prefRotationOffset.Value = new Vector3(MenuSetup.XROffset.Value, MenuSetup.YrOffset.Value, MenuSetup.ZrOffset.Value);
            _prefPositionOffset.Value = new Vector3(MenuSetup.XpOffset.Value, MenuSetup.YpOffset.Value, MenuSetup.ZpOffset.Value);
            _prefRotationSmoothing.Value = MenuSetup.RSmoothing.Value;
            _prefPositionSmoothing.Value = MenuSetup.PSmoothing.Value;

            _prefShowOtherNotifi.Value = MenuSetup.OtherNotifi.Value;
            _prefShowPrefNotifi.Value = MenuSetup.PrefNotifi.Value;
            _prefShowCameraDisabledNotifi.Value = MenuSetup.CameraDisabledNotifi.Value;
            _prefShowCameraFoundNotifi.Value = MenuSetup.CameraFoundNotifi.Value;

            _prefAutoSave.Value = MenuSetup.AutoSave.Value;
            
            _prefLdEnabled.Value = MenuSetup.LdEnabled.Value;
            _prefLdCenter.Value = new Vector2(MenuSetup.LdCenterX.Value, MenuSetup.LdCenterY.Value);
            _prefLdIntensity.Value = MenuSetup.LdIntensity.Value;
            _prefLdScale.Value = MenuSetup.LdScale.Value;
            _prefLdXMultiplier.Value = MenuSetup.LdXMultiplier.Value;
            _prefLdYMultiplier.Value = MenuSetup.LdYMultiplier.Value;

            _prefCaEnabled.Value = MenuSetup.CaEnabled.Value;
            _prefCaIntensity.Value = MenuSetup.CaIntensity.Value;

            _prefAeEnabled.Value = MenuSetup.AeEnabled.Value;
            _prefAeAdaptationMode.Value = (AutoExposureAdaptationMode)MenuSetup.AeAdaptationMode.Value;
            _prefAeD2Ls.Value = MenuSetup.AeD2Ls.Value;
            _prefAeEvCompensation.Value = MenuSetup.AeEvCompensation.Value;
            _prefAeEvMax.Value = MenuSetup.AeEvMax.Value;
            _prefAeEvMin.Value = MenuSetup.AeEvMin.Value;
            _prefAeL2Ds.Value = MenuSetup.AeL2Ds.Value;
            _prefAeMeteringMask.Value = (AutoExposureMeteringMaskMode)MenuSetup.AeMeteringMaskMode.Value;
            _prefAeMetProcedFalloff.Value = MenuSetup.AeMeteringProceduralFalloff.Value;

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
