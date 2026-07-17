using System.Diagnostics;
using BoneLib.BoneMenu;
using BoneLib.Notifications;
using Il2CppOccaSoftware.Exposure.Runtime;
using UnityEngine;
using WideEye.CameraManagers;
using WideEye.Core;
using WideEye.Data;
using WideEye.Objects;
using WideEye.Utilities;

namespace WideEye.UI
{
    public static class ModMenu
    {
        //---------- | Main Page | ----------
        public static Page MainPage;
        public static FloatElement FOVSlider { get; private set; }
        private static Page OffsetPage { get; set; }
        private static Page SupportPage { get; set; }
        private static Page AutoExposurePage { get; set; }
        private static Page PostFXPage { get; set; }
        private static Page SmoothingPage { get; set; }
        private static Page AvatarMeshesPage { get; set; }
        private static Page LensDistortionPage { get; set; }
        private static Page ViewPage { get; set; }
        private static Page PresetsPage { get; set; }
        private static Page HandheldCameraPage { get; set; }
        private static Page FreeCamPage { get; set; }
        
        
        //---------- | Smoothing Page | ----------
        public static FloatElement PSmoothing { get; private set; }
        public static FloatElement RSmoothing { get; private set; }

        
        
        //---------- | View Page | ----------
        public static EnumElement ViewMode { get; private set; }
        public static BoolElement ChangeViewOnSpawn { get; private set; }
        public static EnumElement AudioSource { get; private set; }
        
        //---------- | Free Cam Page | ----------
        public static FloatElement FreeCamSpeed { get; private set; }
        public static FloatElement FreeCamFastSpeed { get; private set; }
        public static FloatElement FreeCamSensitivity { get; private set; }
        public static FloatElement FreeCamSmoothSpeed { get; private set; }
        public static FloatElement FreeCamScrollSensitivity { get; private set; }
        public static FloatElement FreeCamScrollSmoothing { get; private set; }
        public static BoolElement FreeCamShowIndicator { get; private set; }
        
        
        
        
        //---------- | Rotation Offset Page | ----------
        public static FloatElement XrOffset { get; private set; }
        public static FloatElement YrOffset { get; private set; }
        public static FloatElement ZrOffset { get; private set; }
        
        
        //---------- | Position Offset Page | ----------
        public static FloatElement XpOffset { get; private set; }
        public static FloatElement YpOffset { get; private set; }
        public static FloatElement ZpOffset { get; private set; }
        
        
        
        //---------- | Post-Processing Page | ----------
        public static BoolElement PostFXToggle { get; private set; }

        //---------- | MKGlow Page | ----------
        
        private static Page MkGlowPage { get; set; }
        public static BoolElement MkGlowEnabled { get; private set; }
        
        
        //---------- | Chromatic Aberration Page | ----------
        private static Page ChromaticAberrationPage { get; set; }
        public static BoolElement CaEnabled { get; private set; }
        public static FloatElement CaIntensity { get; private set; }
        
        
        
        //---------- | Lens Distortion Page | ----------
        public static BoolElement LdEnabled { get; private set; }
        public static FloatElement LdCenterX { get; private set; }
        public static FloatElement LdCenterY { get; private set; }
        public static FloatElement LdIntensity { get; private set; }
        public static FloatElement LdScale { get; private set; }
        public static FloatElement LdXMultiplier { get; private set; }
        public static FloatElement LdYMultiplier { get; private set; }
        
        
        
        //---------- | Auto Exposure Page | ----------
        public static BoolElement AeEnabled { get; private set; }
        public static EnumElement AeAdaptationMode { get; private set; }
        public static FloatElement AeD2Ls { get; private set; }
        public static FloatElement AeEvCompensation { get; private set; }
        public static FloatElement AeEvMax { get; private set; }
        public static FloatElement AeEvMin { get; private set; }
        public static FloatElement AeL2Ds { get; private set; }
        public static EnumElement AeMeteringMaskMode { get; private set; }
        public static FloatElement AeMeteringProceduralFalloff { get; private set; }
        
        
        
        //---------- | Mod Settings Page | ----------
        public static Page ModSettingsPage { get; set; }
        
        public static BoolElement AutoSave { get; private set; }
        public static IntElement StartupDelay { get; private set; }
        public static BoolElement HideNonErrorNotifications { get; private set; }


        public static void CreatePresetsPage()
        {
            foreach (var presetPair in PresetsManager.Presets)
            {
               var preset = presetPair.Value;
               CreateOnePresetPage(preset.Name);
            }
        }
        
        public static void CreateOnePresetPage(string presetName)
        {
            var preset = PresetsManager.Presets[presetName];
            var page = PresetsPage.CreatePage(presetName, Color.white);
            if (preset != null)
            {
                page.CreateFunction("Override Preset with current settings", Color.cyan, () => PresetsManager.SavePreset(presetName));
                page.CreateFunction("Load This Preset", Color.green, () => PresetsManager.ApplyPreset(presetName));
                page.CreateFunction("View Path", Color.yellow, () => PresetsManager.ViewPath(presetName));
                var valuesPage = page.CreatePage("Values [Experimental]", Color.magenta);
                foreach (var property in preset.GetType().GetProperties())
                {
                    valuesPage.CreateFunction($"{property.Name} : {property.GetValue(preset)}", Color.white, null);
                }
                page.CreateFunction("Delete This Preset", Color.red, () => 
                {
                    PresetsManager.DeletePreset(presetName);
                    page.Name = $"[Deleted] {presetName}";
                    page.RemoveAll();
                    page.CreateFunction("This Preset Has Been Deleted", Color.white, null);
                    page.CreateFunction("Page will be deleted in next game launch", Color.white, null);
                });
            }
            else
            {
                page.Color = Color.red;
                page.CreateFunction("An Error Occured", Color.white, null);
                page.CreateFunction("While Loading Preset", Color.white, null);
            }
            
        }

        public static WideEyeSettings GetValues()
        {
            var result = new WideEyeSettings
            {
                FOV = FOVSlider.Value,
                PosSmoothing = PSmoothing.Value,
                RotSmoothing = RSmoothing.Value,
                PosOffset = new SerializableVector3(new Vector3(XpOffset.Value, YpOffset.Value, ZpOffset.Value)),
                RotOffset = new SerializableVector3(new Vector3(XrOffset.Value, YrOffset.Value, ZrOffset.Value)),
                PostFXEnabled = PostFXToggle.Value,
                MkGlowEnabled = MkGlowEnabled.Value,
                CaEnabled = CaEnabled.Value,
                CaIntensity = CaIntensity.Value,
                LdEnabled = LdEnabled.Value,
                LdCenter = new SerializableVector2(new Vector2(LdCenterX.Value, LdCenterY.Value)),
                LdIntensity = LdIntensity.Value,
                LdScale = LdScale.Value,
                LdMultiplyer = new SerializableVector2(new Vector2(LdXMultiplier.Value, LdYMultiplier.Value)),
                AeEnabled = AeEnabled.Value,
                AeAdaptationMode = (AutoExposureAdaptationMode)AeAdaptationMode.Value,
                AeD2Ls = AeD2Ls.Value,
                AeEvComp = AeEvCompensation.Value,
                AeEvMax = AeEvMax.Value,
                AeEvMin = AeEvMin.Value,
                AeL2ds = AeL2Ds.Value,
                AeMeetringMaskMode = (AutoExposureMeteringMaskMode)AeMeteringMaskMode.Value,
                AeMeetaeMeteringProceduralFalloff = AeMeteringProceduralFalloff.Value
            };
            return result;
        }

        public static void SetupBoneMenu()
        {
            
            MainPage = Page.Root.CreatePage("WideEye", new Color(0.474f, 0.314f, 0.98f));
            FOVSlider = MainPage.CreateFloat("FOV", Color.cyan, 72f, 1f, float.MinValue, float.MaxValue, value => SettingsUpdater.UpdateFOV(value));
            MainPage.CreateFunction("Reset To Default", Color.red, () => SettingsUpdater.ResetToDefault(ModEnums.ResetType.Fov));
            MainPage.CreateFunction("Save Preferences", Color.green, ModPreferences.SavePreferences);
            
            //---------------------------------

            PostFXPage = MainPage.CreatePage("Post-Processing", Color.yellow);
            PostFXToggle = PostFXPage.CreateBool("Enabled", Color.yellow, true, value => SettingsUpdater.TogglePostFX(value));
            
            //---------------------------------
            
            MkGlowPage = PostFXPage.CreatePage("MKGlow", Color.white);
            MkGlowEnabled = MkGlowPage.CreateBool("Enabled", Color.cyan, true, _ => SettingsUpdater.UpdateMkGlow());
            MkGlowPage.CreateFunction("More Features are coming soon!", Color.white, null);
            MkGlowPage.CreateFunction("Reset To Default", Color.red , () => SettingsUpdater.ResetToDefault(ModEnums.ResetType.MKGlow));
            
            //---------------------------------
            LensDistortionPage = PostFXPage.CreatePage("Lens Distortion", Color.white);
            LdEnabled = LensDistortionPage.CreateBool("Enabled", Color.cyan, true, _ => SettingsUpdater.UpdateLensDistortion());
            LdCenterX = LensDistortionPage.CreateFloat("Center X", Color.red, 0.50f, 0.1f, 0f, 1f, _ => SettingsUpdater.UpdateLensDistortion());
            LdCenterY = LensDistortionPage.CreateFloat("Center Y", Color.green, 0.50f, 0.1f, 0f, 1f, _ => SettingsUpdater.UpdateLensDistortion());
            LdIntensity = LensDistortionPage.CreateFloat("Intensity", Color.white, 0.48f, 0.01f, 0f, 1f, _ => SettingsUpdater.UpdateLensDistortion());
            LdScale = LensDistortionPage.CreateFloat("Scale", Color.white, 1f, 0.1f, 0, 1, _ => SettingsUpdater.UpdateLensDistortion());
            LdXMultiplier = LensDistortionPage.CreateFloat("X Multiplier", Color.red, 0.59f, 0.01f, 0f, 1f, _ => SettingsUpdater.UpdateLensDistortion());
            LdYMultiplier = LensDistortionPage.CreateFloat("Y Multiplier", Color.green, 1f, 0.01f, 0f, 1f, _ => SettingsUpdater.UpdateLensDistortion());
            LensDistortionPage.CreateFunction("Reset To Default", Color.red, () => SettingsUpdater.ResetToDefault(ModEnums.ResetType.LensDistortion));
            
            //---------------------------------
            
            ChromaticAberrationPage = PostFXPage.CreatePage("ChromaticAberration", Color.white);
            CaEnabled = ChromaticAberrationPage.CreateBool("Enabled", Color.cyan, true, _ => SettingsUpdater.UpdateChromaticAberration());
            CaIntensity = ChromaticAberrationPage.CreateFloat("Intensity", Color.white, 0.123f, 0.01f, 0f, 1f, _ => SettingsUpdater.UpdateChromaticAberration());
            ChromaticAberrationPage.CreateFunction("Reset To Default", Color.red, () => SettingsUpdater.ResetToDefault(ModEnums.ResetType.ChromaticAberration));
            
            //---------------------------------
            
            AutoExposurePage = PostFXPage.CreatePage("AutoExposure", Color.white);
            AeEnabled = AutoExposurePage.CreateBool("Enabled", Color.cyan, true, _ => SettingsUpdater.UpdateAutoExposure());
            AeAdaptationMode = AutoExposurePage.CreateEnum("Adaptation Mode", Color.white, AutoExposureAdaptationMode.Progressive, _ => SettingsUpdater.UpdateAutoExposure());
            AeD2Ls = AutoExposurePage.CreateFloat("Dark To Light Speed", Color.white, 3f, 0.1f, float.MinValue, float.MaxValue, _ => SettingsUpdater.UpdateAutoExposure());
            AeEvCompensation = AutoExposurePage.CreateFloat("EV Compensation", Color.white, 2.5f, 0.1f, float.MinValue, float.MaxValue, _ => SettingsUpdater.UpdateAutoExposure());
            AeEvMax = AutoExposurePage.CreateFloat("EV Max", Color.white, 1.2f, 0.1f, float.MinValue, float.MaxValue, _ => SettingsUpdater.UpdateAutoExposure());
            AeEvMin = AutoExposurePage.CreateFloat("EV Min", Color.white, -1.2f, 0.1f, float.MinValue, float.MaxValue, _ => SettingsUpdater.UpdateAutoExposure());
            AeL2Ds = AutoExposurePage.CreateFloat("Light To Dark Speed", Color.white, 1f, 0.1f, float.MinValue, float.MaxValue, _ => SettingsUpdater.UpdateAutoExposure());
            AeMeteringMaskMode = AutoExposurePage.CreateEnum("Metering Mask Mode", Color.white, AutoExposureMeteringMaskMode.Procedural, null);
            AeMeteringProceduralFalloff = AutoExposurePage.CreateFloat("Metering Procedural Falloff", Color.white, 2f, 0.1f, float.MinValue, float.MaxValue, _ => SettingsUpdater.UpdateAutoExposure());
            AutoExposurePage.CreateFunction("Reset To Default", Color.red, () => SettingsUpdater.ResetToDefault(ModEnums.ResetType.AutoExposure));
            
            //---------------------------------
            
            AvatarMeshesPage = MainPage.CreatePage("Avatar Meshes Toggles", Color.yellow); 
            AvatarMeshesPage.CreateFunction("Toggle Head Mesh Offset", Color.cyan, () => SettingsUpdater.ToggleAvatarMesh(ModEnums.MeshToggleType.HeadMeshOffset));
            AvatarMeshesPage.CreateFunction("Toggle Head Meshes", Color.white, () => SettingsUpdater.ToggleAvatarMesh(ModEnums.MeshToggleType.HeadMesh));
            AvatarMeshesPage.CreateFunction("Toggle Hair Meshes", Color.white, () => SettingsUpdater.ToggleAvatarMesh(ModEnums.MeshToggleType.HairMeshes));
            AvatarMeshesPage.CreateFunction("-------------------", Color.white, null);
            AvatarMeshesPage.CreateFunction("Note: You might need to", Color.yellow, null);
            AvatarMeshesPage.CreateFunction("Press the button twice", Color.yellow, null);
            
            //---------------------------------
            
            OffsetPage = MainPage.CreatePage("Offset", Color.white);
            
            XrOffset = OffsetPage.CreateFloat("X Rotation Offset", Color.red, 11f, 1f, float.MinValue, float.MaxValue, _ => SettingsUpdater.UpdateOffset(ModEnums.OffsetType.Rotation));
            YrOffset = OffsetPage.CreateFloat("Y Rotation Offset", Color.green, 0f, 1f, float.MinValue, float.MaxValue, _ => SettingsUpdater.UpdateOffset(ModEnums.OffsetType.Rotation));
            ZrOffset = OffsetPage.CreateFloat("Z Rotation Offset", Color.blue, 0f, 1f, float.MinValue, float.MaxValue, _ => SettingsUpdater.UpdateOffset(ModEnums.OffsetType.Rotation));
            OffsetPage.CreateFunction("--------------------", Color.white, null).SetProperty(ElementProperties.NoBorder);
            XpOffset = OffsetPage.CreateFloat("X Position Offset", Color.red, 0f, 0.1f, float.MinValue, float.MaxValue, _ => SettingsUpdater.UpdateOffset(ModEnums.OffsetType.Position));
            YpOffset = OffsetPage.CreateFloat("Y Position Offset", Color.green, 0f, 0.1f, float.MinValue, float.MaxValue, _ => SettingsUpdater.UpdateOffset(ModEnums.OffsetType.Position));
            ZpOffset = OffsetPage.CreateFloat("Z Position Offset", Color.blue, 0f, 0.1f, float.MinValue, float.MaxValue, _ => SettingsUpdater.UpdateOffset(ModEnums.OffsetType.Position));
            OffsetPage.CreateFunction(" -------------------- ", Color.white, null).SetProperty(ElementProperties.NoBorder);
            OffsetPage.CreateFunction("Reset Position", Color.red, () => SettingsUpdater.ResetToDefault(ModEnums.ResetType.PositionOffset));
            OffsetPage.CreateFunction("Reset Rotation", Color.red, () => SettingsUpdater.ResetToDefault(ModEnums.ResetType.RotationOffset));
            
            //---------------------------------
            
            SmoothingPage = MainPage.CreatePage("Smoothing", Color.white);
            PSmoothing = SmoothingPage.CreateFloat("Position Smoothing", Color.white, 0f, 1f, float.MinValue, int.MaxValue, _ => SettingsUpdater.UpdateSmoothing());
            RSmoothing = SmoothingPage.CreateFloat("Rotation Smoothing", Color.white, 0f, 1f, float.MinValue, int.MaxValue, _ => SettingsUpdater.UpdateSmoothing());
            SmoothingPage.Add(new FunctionElement("Reset To Default", Color.red, () => SettingsUpdater.ResetToDefault(ModEnums.ResetType.Smoothing)));
            
            //---------------------------------
            
            PresetsPage = MainPage.CreatePage("Presets", Color.magenta);
            var presetInput = PresetsPage.CreateString("Preset Name", Color.white, "", null);
            PresetsPage.CreateFunction("Create Preset", Color.green, () => PresetsManager.CreatePreset(presetInput.Value));
            PresetsPage.CreateFunction("Refresh Presets", Color.cyan, PresetsManager.RefreshPresetList);
            PresetsPage.CreateFunction("Open Presets Folder", Color.yellow, () =>
            {
                Process.Start("explorer.exe", Paths.PresetsPath);
                if(ModPreferences.HideNonErrorNotification) return;
                Notifier.Send(new Notification
                {
                    Title = "WideEye | Success",
                    Message = "Opened the presets folder in desktop",
                    Type = NotificationType.Success,
                    PopupLength = 2,
                    ShowTitleOnPopup = true
                });
                
            });
            PresetsPage.CreateFunction("--------------------", Color.white, null).SetProperty(ElementProperties.NoBorder);
            CreatePresetsPage();
            
            //---------------------------------
            
            ViewPage = MainPage.CreatePage("View", Color.cyan);
            ViewMode = ViewPage.CreateEnum("View Mode", Color.white, ModEnums.ViewMode.Head, v => CameraController.UpdateView((ModEnums.ViewMode)v));
            AudioSource = ViewPage.CreateEnum("Audio Source", Color.white, ModEnums.AudioSource.Head, v => CameraController.UpdateAudioSource((ModEnums.AudioSource)v));
            
            HandheldCameraPage = ViewPage.CreatePage("Handheld Camera Settings", Color.white);
            ChangeViewOnSpawn = HandheldCameraPage.CreateBool("Change View On Spawn", Color.yellow, true, v => ModPreferences.ChangeViewOnSpawn = v);
            
            HandheldCameraPage.CreateFunction("Spawn Camera [Client Side]", Color.green, HandheldCameraManager.SpawnHandheldCamera);
            HandheldCameraPage.CreateFunction("Teleport Camera", Color.cyan, HandheldCameraManager.TeleportHandheldCamera);
            HandheldCameraPage.CreateFunction("Destroy Camera [Client Side]", Color.red, HandheldCameraManager.DestroyHandheldCamera);
            
            
            FreeCamPage = ViewPage.CreatePage("FreeCam Settings", Color.white);
            FreeCamSpeed = FreeCamPage.CreateFloat("Move Speed", Color.white, 3f, 1, 0, float.MaxValue, v => FreeCamManager.MoveSpeed = v);
            FreeCamFastSpeed = FreeCamPage.CreateFloat("Fast Move Speed", Color.white, 7f, 1, 0, float.MaxValue,v => FreeCamManager.FastMoveSpeed = v);
            FreeCamSensitivity = FreeCamPage.CreateFloat("Mouse Sensitivity", Color.white, 3f, 1, 0, float.MaxValue,v => FreeCamManager.Sensitivity = v);
            FreeCamSmoothSpeed = FreeCamPage.CreateFloat("Smooth Speed", Color.white, 10f, 1, 0, float.MaxValue,v => FreeCamManager.SmoothSpeed = v);
            FreeCamScrollSensitivity = FreeCamPage.CreateFloat("Scroll Sensitivity", Color.white, 15f, 1, 0, float.MaxValue,v => FreeCamManager.ScrollSensitivity = v);
            FreeCamScrollSmoothing = FreeCamPage.CreateFloat("Scroll Smoothing", Color.white, 10f, 1, 0, float.MaxValue,v => FreeCamManager.ScrollSmoothing = v);
            FreeCamShowIndicator = FreeCamPage.CreateBool("Show Indicator", Color.white, true, v => FreeCamManager.ShowIndicator = v);
            
            
            //---------------------------------
            ModSettingsPage = MainPage.CreatePage("Mod Settings", Color.green);

            ModSettingsPage.CreateFunction("Get Camera Manually", Color.red, () => SpectatorCameraManager.GetSpectatorCamera(false));
            StartupDelay = ModSettingsPage.CreateInt("Startup Delay (Seconds)", Color.cyan, 5, 1, 2, int.MaxValue, v => ModPreferences.StartupDelay = v);
            AutoSave = ModSettingsPage.CreateBool("Auto Save", Color.magenta, false, v => ModPreferences.AutoSave = v);
            HideNonErrorNotifications = ModSettingsPage.CreateBool("Hide Non Error Notification", Color.magenta, false, v => ModPreferences.HideNonErrorNotification = v);
            ModSettingsPage.CreateFunction("Reset All To Default", Color.red, () => SettingsUpdater.ResetToDefault(ModEnums.ResetType.All));
            ModSettingsPage.CreateFunction("Load Preferences", Color.green, ModPreferences.LoadPreferences);
            ModSettingsPage.CreateFunction("Clear All Preferences", Color.red, ModPreferences.ClearPreferences);
            SupportPage = ModSettingsPage.CreatePage("Support", Color.white);
            SupportPage.CreateFunction("Open GitHub Issues", Color.white, () =>
            {
                Application.OpenURL("https://github.com/HL2H0/WideEye/issues");
                if(ModPreferences.HideNonErrorNotification) return;
                Notifier.Send(new Notification
                {
                    Title = "WideEye | Success",
                    Message = "Opened the GitHub issues page for WideEye On Desktop",
                    Type = NotificationType.Success,
                    PopupLength = 2,
                    ShowTitleOnPopup = true
                });
            });

            SupportPage.CreateFunction("Discord", Color.blue, () =>
            {
                GUIUtility.systemCopyBuffer = "@HL2H0";
                if(ModPreferences.HideNonErrorNotification) return;
                Notifier.Send(new Notification
                {
                    Title = "WideEye | Success",
                    Message = "Copied Discord Tag to Clipboard, You can paste it in Discord",
                    Type = NotificationType.Success,
                    PopupLength = 2,
                    ShowTitleOnPopup = true
                });
            });

            SupportPage.CreateFunction("Support me on Ko-Fi", Color.magenta, () =>
            {
                Application.OpenURL("https://ko-fi.com/hl2h0");
                Notifier.Send(new Notification
                {
                    Title = "WideEye | Success",
                    Message = "Opened Ko-Fi Page On Desktop",
                    Type = NotificationType.Success,
                    PopupLength = 2,
                    ShowTitleOnPopup = true
                });
            });

            SupportPage.CreateFunction($"Version :  {BuildInfo.Version}", Color.white, null);
        }
    }
}

