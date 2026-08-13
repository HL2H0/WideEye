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
        public static bool HideNonErrorNotification
        {
            get => _hideNonErrorNotification.Value;
            set
            {
                _hideNonErrorNotification.Value = value;
                if (AutoSave) SavePreferences();
            } 
        }

        public static bool AutoSave;

        public static int StartupDelay
        {
            get => _startupDelay.Value;
            set
            {
                _startupDelay.Value = value;
                if (AutoSave) SavePreferences();
            }
        }

        public static bool ChangeViewOnSpawn
        {
            get => _changeViewOnSpawn.Value;
            set
            {
                _changeViewOnSpawn.Value = value;
                if (AutoSave) SavePreferences();
            }
        }

        public static float HandheldZoomSpeed
        {
            get => _handheldZoomSpeed.Value;
            set
            {
                _handheldZoomSpeed.Value = value;
                if (AutoSave) SavePreferences();
            }

        }

        public static float HandheldZoomSmoothing
        {
            get => _handheldZoomSmoothing.Value;
            set
            {
                _handheldZoomSmoothing.Value = value;
                if (AutoSave) SavePreferences();
            }
        }

        private static MelonPreferences_Category _categWideEye;
        private static MelonPreferences_Entry<float> _fov;
        private static MelonPreferences_Entry<bool> _postFX;
        private static MelonPreferences_Entry<Vector3> _rotationOffset;
        private static MelonPreferences_Entry<Vector3> _positionOffset;
        private static MelonPreferences_Entry<float> _rotationSmoothing;
        private static MelonPreferences_Entry<float> _positionSmoothing;

        private static MelonPreferences_Entry<bool> _hideNonErrorNotification;
        private static MelonPreferences_Entry<bool> _autoSave;
        private static MelonPreferences_Entry<int> _startupDelay;
        private static MelonPreferences_Entry<bool> _changeViewOnSpawn;
        private static MelonPreferences_Entry<float> _handheldZoomSpeed;
        private static MelonPreferences_Entry<float> _handheldZoomSmoothing;
        
        private static MelonPreferences_Entry<float> _freeCamSpeed;
        private static MelonPreferences_Entry<float> _freeCamFastSpeed;
        private static MelonPreferences_Entry<float> _freeCamSensitivity;
        private static MelonPreferences_Entry<float> _freeCamSmoothSpeed;
        private static MelonPreferences_Entry<float> _freeCamScrollSensitivity;
        private static MelonPreferences_Entry<float> _freeCamScrollSmoothing;
        private static MelonPreferences_Entry<bool> _freecamIndicator;

        private static MelonPreferences_Category _categPfxMk;
        private static MelonPreferences_Entry<bool> _mkEnabled;
        
        
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
        
        public static void CreatePreferences()
        {
            _categWideEye = MelonPreferences.CreateCategory("WideEye");
            _fov = _categWideEye.CreateEntry("Fov", 75f);
            _postFX = _categWideEye.CreateEntry("PostFX", true);
            _rotationOffset = _categWideEye.CreateEntry("RotationOffset", new Vector3(11f, 0f, 0f));
            _positionOffset = _categWideEye.CreateEntry("PositionOffset", Vector3.zero);
            _rotationSmoothing = _categWideEye.CreateEntry("RotationSmoothing", 0f);
            _positionSmoothing = _categWideEye.CreateEntry("PositionSmoothing", 0f);
            _startupDelay = _categWideEye.CreateEntry("StartupDelay", 5);
            _hideNonErrorNotification = _categWideEye.CreateEntry("HideNonErrorNotification", false);
            _autoSave = _categWideEye.CreateEntry("AutoSave", false);
            _freeCamSpeed = _categWideEye.CreateEntry("FreeCamSpeed", 3f);
            _freeCamFastSpeed = _categWideEye.CreateEntry("FreeCamFastSpeed", 7f);
            _freeCamSensitivity = _categWideEye.CreateEntry("FreeCamSensitivity", 3f);
            _freeCamSmoothSpeed = _categWideEye.CreateEntry("FreeCamSmoothSpeed", 10f);
            _freeCamScrollSensitivity = _categWideEye.CreateEntry("FreeCamScrollSensitivity", 15f);
            _freeCamScrollSmoothing = _categWideEye.CreateEntry("FreeCamScrollSmoothing", 10f);
            _freecamIndicator = _categWideEye.CreateEntry("FreeCamIndicator", true);
            _changeViewOnSpawn = _categWideEye.CreateEntry("ChangeViewOnSpawn", true);
            _handheldZoomSpeed = _categWideEye.CreateEntry("HandheldZoomSpeed", 1f);
            _handheldZoomSmoothing = _categWideEye.CreateEntry("HandheldZoomSmoothing", 0f);
            
            _categPfxMk = MelonPreferences.CreateCategory("WideEye_PostFX_MKGlow");
            _mkEnabled = _categPfxMk.CreateEntry("Enabled", true);
            
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

        public static void LoadPreferences()
        {
            SettingsUpdater.UpdateFOV(_fov.Value, true, ModMenu.FOVSlider);
            SettingsUpdater.TogglePostFX(_postFX.Value, true);
            ModMenu.PostFXToggle.Value = _postFX.Value;
            SettingsUpdater.UpdateOffset(_rotationOffset.Value, ModEnums.OffsetType.Rotation, true);
            SettingsUpdater.UpdateOffset(_positionOffset.Value, ModEnums.OffsetType.Position, true);
            SettingsUpdater.UpdateSmoothing(_rotationSmoothing.Value, _positionSmoothing.Value, true);
            
            ModMenu.HideNonErrorNotifications.Value = _hideNonErrorNotification.Value;
            ModMenu.StartupDelay.Value = _startupDelay.Value;
            ModMenu.ChangeViewOnSpawn.Value = _changeViewOnSpawn.Value;
            ModMenu.HandheldZoomSpeed.Value = _handheldZoomSpeed.Value;
            ModMenu.HandheldZoomSmoothing.Value = _handheldZoomSmoothing.Value;
            ModMenu.AutoSave.Value = _autoSave.Value;
            AutoSave = _autoSave.Value;
            
            FreeCamManager.MoveSpeed = _freeCamSpeed.Value;
            ModMenu.FreeCamSpeed.Value = _freeCamSpeed.Value;
            FreeCamManager.FastMoveSpeed = _freeCamFastSpeed.Value;
            ModMenu.FreeCamFastSpeed.Value = _freeCamFastSpeed.Value;
            FreeCamManager.Sensitivity = _freeCamSensitivity.Value;
            ModMenu.FreeCamSensitivity.Value = _freeCamSensitivity.Value;
            FreeCamManager.SmoothSpeed = _freeCamSmoothSpeed.Value;
            ModMenu.FreeCamSmoothSpeed.Value = _freeCamSmoothSpeed.Value;
            FreeCamManager.ScrollSensitivity = _freeCamScrollSensitivity.Value;
            ModMenu.FreeCamScrollSensitivity.Value = _freeCamScrollSensitivity.Value;
            FreeCamManager.ScrollSmoothing = _freeCamScrollSmoothing.Value;
            ModMenu.FreeCamScrollSmoothing.Value = _freeCamScrollSmoothing.Value;
            FreeCamManager.ShowIndicator = _freecamIndicator.Value;

            SettingsUpdater.UpdateMkGlow(_mkEnabled.Value, true);
            
            SettingsUpdater.UpdateLensDistortion(_ldEnabled.Value, _ldCenter.Value, _ldIntensity.Value, _ldScale.Value,
                _ldXMultiplier.Value, _ldYMultiplier.Value, true);
            
            SettingsUpdater.UpdateChromaticAberration(_caEnabled.Value, _caIntensity.Value, true);
            
            SettingsUpdater.UpdateAutoExposure(_aeEnabled.Value, _aeAdaptationMode.Value, _aeD2Ls.Value,
                _aeEvCompensation.Value, _aeEvMax.Value, _aeEvMin.Value, _aeL2Ds.Value, _aeMeteringMask.Value,
                _aeMetProcedFalloff.Value, true);
            
            Melon<Mod>.Logger.Msg(ConsoleColor.Green, "Loaded Preferences.");
        }

        public static void SavePreferences()
        {
            _fov.Value = ModMenu.FOVSlider.Value;
            _postFX.Value = ModMenu.PostFXToggle.Value;
            _rotationOffset.Value = new Vector3(ModMenu.XrOffset.Value, ModMenu.YrOffset.Value, ModMenu.ZrOffset.Value);
            _positionOffset.Value = new Vector3(ModMenu.XpOffset.Value, ModMenu.YpOffset.Value, ModMenu.ZpOffset.Value);
            _rotationSmoothing.Value = ModMenu.RSmoothing.Value;
            _positionSmoothing.Value = ModMenu.PSmoothing.Value;

            _startupDelay.Value = ModMenu.StartupDelay.Value;
            _changeViewOnSpawn.Value = ModMenu.ChangeViewOnSpawn.Value;
            _handheldZoomSpeed.Value = ModMenu.HandheldZoomSpeed.Value;
            _handheldZoomSmoothing.Value = ModMenu.HandheldZoomSmoothing.Value;
            
            _autoSave.Value = ModMenu.AutoSave.Value;
            _hideNonErrorNotification.Value = ModMenu.HideNonErrorNotifications.Value;
            
            _freeCamSpeed.Value = ModMenu.FreeCamSpeed.Value;
            _freeCamFastSpeed.Value = ModMenu.FreeCamFastSpeed.Value;
            _freeCamSensitivity.Value = ModMenu.FreeCamSensitivity.Value;
            _freeCamSmoothSpeed.Value = ModMenu.FreeCamSmoothSpeed.Value;
            _freeCamScrollSensitivity.Value = ModMenu.FreeCamScrollSensitivity.Value;
            _freeCamScrollSmoothing.Value = ModMenu.FreeCamScrollSmoothing.Value;
            
            _mkEnabled.Value = ModMenu.MkGlowEnabled.Value;
            
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
            
            Melon<Mod>.Logger.Msg(ConsoleColor.Green, "Saved Preferences.");
            if(HideNonErrorNotification) return;
            Notifier.Send(new Notification
            {
                Title = "WideEye | Success",
                Message = "Saved Preferences",
                Type = NotificationType.Success,
                PopupLength = 2,
                ShowTitleOnPopup = true
            });
        }
        
        
        public static void ClearPreferences()
        {
            var categ = _categWideEye.Entries
            .Concat(_categPfxLd.Entries)
            .Concat(_categPfxCa.Entries)
            .Concat(_categPfxAe.Entries)
            .Concat(_categPfxMk.Entries)
            .ToList();

            foreach (var entry in categ)
            {
                entry.ResetToDefault();
            }
            
            LoadPreferences();
            
            Melon<Mod>.Logger.Msg(ConsoleColor.Green, "Done!, Cleared All Preferences");
            if(HideNonErrorNotification) return;
            Notifier.Send(new Notification
            {
                Title = "WideEye | Success",
                Message = "Cleared All Preferences",
                Type = NotificationType.Success,
                PopupLength = 2,
                ShowTitleOnPopup = true
            });
        }
    }
}
