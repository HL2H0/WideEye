using BoneLib.Notifications;
using Il2CppOccaSoftware.Exposure.Runtime;
using MelonLoader;
using UnityEngine;
using WideEye.CameraManagers;
using WideEye.Core;
using WideEye.UI;
using WideEye.Utilities;

namespace WideEye.Data
{
    public static class ModPreferences
    {
        public static bool AutoSave;
        public static int StartupDelay { get => _startupDelay.Value; set => _startupDelay.Value = value; }
        
        private static MelonPreferences_Category _categWideEye;
        private static MelonPreferences_Entry<float> _fov;
        private static MelonPreferences_Entry<bool> _postFX;
        private static MelonPreferences_Entry<Vector3> _rotationOffset;
        private static MelonPreferences_Entry<Vector3> _positionOffset;
        private static MelonPreferences_Entry<float> _rotationSmoothing;
        private static MelonPreferences_Entry<float> _positionSmoothing;
        private static MelonPreferences_Entry<bool> _showOtherNotifi;
        private static MelonPreferences_Entry<bool> _showPrefNotifi;
        private static MelonPreferences_Entry<bool> _showCameraDisabledNotifi;
        private static MelonPreferences_Entry<bool> _showCameraFoundNotifi;
        private static MelonPreferences_Entry<bool> _autoSave;
        private static MelonPreferences_Entry<int> _startupDelay;
        private static MelonPreferences_Entry<float> _freeCamSpeed;
        private static MelonPreferences_Entry<float> _freeCamFastSpeed;
        private static MelonPreferences_Entry<float> _freeCamSensitivity;
        private static MelonPreferences_Entry<float> _freeCamSmoothSpeed;
        private static MelonPreferences_Entry<float> _freeCamScrollSensitivity;
        private static MelonPreferences_Entry<float> _freeCamScrollSmoothing;
        private static MelonPreferences_Entry<bool> _freecamIndicator;
        
        
        private static MelonPreferences_Category _categPfxLd;
        private static MelonPreferences_Entry<bool> _ldEnabled;
        private static MelonPreferences_Entry<Vector2> _ldCenter;
        private static MelonPreferences_Entry<float> _ldIntensity;
        private static MelonPreferences_Entry<float> _ldScale;
        private static MelonPreferences_Entry<float> _ldXMultiplier;
        private static MelonPreferences_Entry<float> _ldYMultiplier;

        private static MelonPreferences_Category _categPfxCa;
        private static MelonPreferences_Entry<bool> _caEnabled;
        private static MelonPreferences_Entry<float> _caIntensity;

        private static MelonPreferences_Category _categPfxAe;
        private static MelonPreferences_Entry<bool> _aeEnabled;
        private static MelonPreferences_Entry<AutoExposureAdaptationMode> _aeAdaptationMode;
        private static MelonPreferences_Entry<float> _aeD2Ls;
        private static MelonPreferences_Entry<float> _aeEvCompensation;
        private static MelonPreferences_Entry<float> _aeEvMax;
        private static MelonPreferences_Entry<float> _aeEvMin;
        private static MelonPreferences_Entry<float> _aeL2Ds;
        private static MelonPreferences_Entry<AutoExposureMeteringMaskMode> _aeMeteringMask;
        private static MelonPreferences_Entry<float> _aeMetProcedFalloff;
        
        public static void CreatePref()
        {
            _categWideEye = MelonPreferences.CreateCategory("WideEye");
            _fov = _categWideEye.CreateEntry("Fov", 75f);
            _postFX = _categWideEye.CreateEntry("PostFX", true);
            _rotationOffset = _categWideEye.CreateEntry("RotationOffset", new Vector3(11f, 0f, 0f));
            _positionOffset = _categWideEye.CreateEntry("PositionOffset", Vector3.zero);
            _rotationSmoothing = _categWideEye.CreateEntry("RotationSmoothing", 0f);
            _positionSmoothing = _categWideEye.CreateEntry("PositionSmoothing", 0f);
            _startupDelay = _categWideEye.CreateEntry("StartupDelay", 5);
            _showOtherNotifi = _categWideEye.CreateEntry("ShowOtherNotification", true);
            _showPrefNotifi = _categWideEye.CreateEntry("ShowPrefNotification", true);
            _showCameraDisabledNotifi = _categWideEye.CreateEntry("ShowCameraDisabledNotification", true);
            _showCameraFoundNotifi = _categWideEye.CreateEntry("ShowCameraFoundNotification", true);
            _autoSave = _categWideEye.CreateEntry("AutoSave", false);
            _freeCamSpeed = _categWideEye.CreateEntry("FreeCamSpeed", 3f);
            _freeCamFastSpeed = _categWideEye.CreateEntry("FreeCamFastSpeed", 7f);
            _freeCamSensitivity = _categWideEye.CreateEntry("FreeCamSensitivity", 3f);
            _freeCamSmoothSpeed = _categWideEye.CreateEntry("FreeCamSmoothSpeed", 10f);
            _freeCamScrollSensitivity = _categWideEye.CreateEntry("FreeCamScrollSensitivity", 15f);
            _freeCamScrollSmoothing = _categWideEye.CreateEntry("FreeCamScrollSmoothing", 10f);
            _freecamIndicator = _categWideEye.CreateEntry("FreeCamIndicator", true);
            
            _categPfxLd = MelonPreferences.CreateCategory("WideEye_PostFX_LensDistortion");
            _ldEnabled = _categPfxLd.CreateEntry("Enabled", true);
            _ldCenter = _categPfxLd.CreateEntry("Center", new Vector2(0.50f, 0.50f));
            _ldIntensity = _categPfxLd.CreateEntry("Intensity", 0.48f);
            _ldScale = _categPfxLd.CreateEntry("Scale", 1f);
            _ldXMultiplier = _categPfxLd.CreateEntry("xMultiplier", 0.59f);
            _ldYMultiplier = _categPfxLd.CreateEntry("yMultiplier", 0.59f);

            _categPfxCa = MelonPreferences.CreateCategory("WideEye_PostFX_ChromaticAberration");
            _caEnabled = _categPfxCa.CreateEntry("Enabled", true);
            _caIntensity = _categPfxCa.CreateEntry("Intensity", 0.123f);

            _categPfxAe = MelonPreferences.CreateCategory("WideEye_PostFX_AutoExposure");
            _aeEnabled = _categPfxAe.CreateEntry("Enabled", true);
            _aeAdaptationMode = _categPfxAe.CreateEntry("AdaptationMode", AutoExposureAdaptationMode.Progressive);
            _aeD2Ls = _categPfxAe.CreateEntry("DarkToLightSpeed", 3f);
            _aeEvCompensation = _categPfxAe.CreateEntry("evCompensation", 2.5f);
            _aeEvMax = _categPfxAe.CreateEntry("evMax", 1.2f);
            _aeEvMin = _categPfxAe.CreateEntry("evMin", -1.2f);
            _aeL2Ds = _categPfxAe.CreateEntry<float>("LightToDarkSpeed", 1);
            _aeMeteringMask = _categPfxAe.CreateEntry("MeteringMaskMode", AutoExposureMeteringMaskMode.Procedural);
            _aeMetProcedFalloff = _categPfxAe.CreateEntry("MeteringProceduralFalloff", 2f);
        }

        public static void LoadPref()
        {
            SettingsApplier.ApplyFOV(_fov.Value, true, ModMenu.FOVSlider);
            SettingsApplier.ApplyOther(ModEnums.OtherType.PostFX, _postFX.Value, true);
            ModMenu.PostFXToggle.Value = _postFX.Value;
            SettingsApplier.ApplyOffset(_rotationOffset.Value, ModEnums.OffsetType.Rotation, true, ModMenu.XrOffset, ModMenu.YrOffset, ModMenu.ZrOffset);
            SettingsApplier.ApplyOffset(_positionOffset.Value, ModEnums.OffsetType.Position, true, ModMenu.XpOffset, ModMenu.YpOffset, ModMenu.ZpOffset);
            SettingsApplier.ApplySmoothing(_rotationSmoothing.Value, _positionSmoothing.Value, true);
            
            ModMenu.StartupDelay.Value = _startupDelay.Value;
            
            ModNotification.ChangeSilentNotification(_showOtherNotifi.Value, _showPrefNotifi.Value,
                _showCameraDisabledNotifi.Value, _showCameraFoundNotifi.Value, ModMenu.OtherNotifi,
                ModMenu.PrefNotifi, ModMenu.CameraDisabledNotifi, ModMenu.CameraFoundNotifi);
            
            ModMenu.AutoSave.Value = _autoSave.Value;
            AutoSave = _autoSave.Value;
            
            FreeCamManager.MoveSpeed = _freeCamSpeed.Value;
            FreeCamManager.FastMoveSpeed = _freeCamFastSpeed.Value;
            FreeCamManager.Sensitivity = _freeCamSensitivity.Value;
            FreeCamManager.SmoothSpeed = _freeCamSmoothSpeed.Value;
            FreeCamManager.ScrollSensitivity = _freeCamScrollSensitivity.Value;
            FreeCamManager.ScrollSmoothing = _freeCamScrollSmoothing.Value;
            FreeCamManager.ShowIndicator = _freecamIndicator.Value;
            
            SettingsApplier.ApplyLd(_ldEnabled.Value, _ldCenter.Value, _ldIntensity.Value, _ldScale.Value, _ldXMultiplier.Value, _ldYMultiplier.Value, true);
            SettingsApplier.ApplyCa(_caEnabled.Value, _caIntensity.Value, true);
            SettingsApplier.ApplyAe(_aeEnabled.Value, _aeAdaptationMode.Value, _aeD2Ls.Value, _aeEvCompensation.Value, _aeEvMax.Value, _aeEvMin.Value, _aeL2Ds.Value, _aeMeteringMask.Value, _aeMetProcedFalloff.Value, true);
            MelonLogger.Msg(ConsoleColor.Green, "Loaded Preferences.");
        }

        public static void SavePref()
        {
            _fov.Value = ModMenu.FOVSlider.Value;
            _postFX.Value = ModMenu.PostFXToggle.Value;
            _rotationOffset.Value = new Vector3(ModMenu.XrOffset.Value, ModMenu.YrOffset.Value, ModMenu.ZrOffset.Value);
            _positionOffset.Value = new Vector3(ModMenu.XpOffset.Value, ModMenu.YpOffset.Value, ModMenu.ZpOffset.Value);
            _rotationSmoothing.Value = ModMenu.RSmoothing.Value;
            _positionSmoothing.Value = ModMenu.PSmoothing.Value;

            _showOtherNotifi.Value = ModMenu.OtherNotifi.Value;
            _showPrefNotifi.Value = ModMenu.PrefNotifi.Value;
            _showCameraDisabledNotifi.Value = ModMenu.CameraDisabledNotifi.Value;
            _showCameraFoundNotifi.Value = ModMenu.CameraFoundNotifi.Value;

            _autoSave.Value = ModMenu.AutoSave.Value;
            
            _freeCamSpeed.Value = ModMenu.FreeCamSpeed.Value;
            _freeCamFastSpeed.Value = ModMenu.FreeCamFastSpeed.Value;
            _freeCamSensitivity.Value = ModMenu.FreeCamSensitivity.Value;
            _freeCamSmoothSpeed.Value = ModMenu.FreeCamSmoothSpeed.Value;
            _freeCamScrollSensitivity.Value = ModMenu.FreeCamScrollSensitivity.Value;
            _freeCamScrollSmoothing.Value = ModMenu.FreeCamScrollSmoothing.Value;
            
            
            _ldEnabled.Value = ModMenu.LdEnabled.Value;
            _ldCenter.Value = new Vector2(ModMenu.LdCenterX.Value, ModMenu.LdCenterY.Value);
            _ldIntensity.Value = ModMenu.LdIntensity.Value;
            _ldScale.Value = ModMenu.LdScale.Value;
            _ldXMultiplier.Value = ModMenu.LdXMultiplier.Value;
            _ldYMultiplier.Value = ModMenu.LdYMultiplier.Value;

            _caEnabled.Value = ModMenu.CaEnabled.Value;
            _caIntensity.Value = ModMenu.CaIntensity.Value;

            _aeEnabled.Value = ModMenu.AeEnabled.Value;
            _aeAdaptationMode.Value = (AutoExposureAdaptationMode)ModMenu.AeAdaptationMode.Value;
            _aeD2Ls.Value = ModMenu.AeD2Ls.Value;
            _aeEvCompensation.Value = ModMenu.AeEvCompensation.Value;
            _aeEvMax.Value = ModMenu.AeEvMax.Value;
            _aeEvMin.Value = ModMenu.AeEvMin.Value;
            _aeL2Ds.Value = ModMenu.AeL2Ds.Value;
            _aeMeteringMask.Value = (AutoExposureMeteringMaskMode)ModMenu.AeMeteringMaskMode.Value;
            _aeMetProcedFalloff.Value = ModMenu.AeMeteringProceduralFalloff.Value;

            _categWideEye.SaveToFile(false);
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
            var categ = _categWideEye.Entries
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
