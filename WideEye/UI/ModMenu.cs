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
        public static FunctionElement GetCameraButton = new("Get Camera", Color.red, () => SpectatorCameraManager.GetSpectatorCamera(false));
        
        
        //---------- | Main Page | ----------
        public static Page MainPage;
        public static FloatElement FOVSlider { get; private set; }
        private static Page OffsetPage { get; set; }
        private static Page SupportPage { get; set; }
        private static Page AutoExposurePage { get; set; }
        private static Page PostFXPage { get; set; }
        private static Page SmoothingPage { get; set; }
        private static Page LensDistortionPage { get; set; }
        private static Page ViewPage { get; set; }
        private static Page PresetsPage { get; set; }
        public static Page HandheldCameraPage { get; private set; }
        private static Page RotationOffsetPage { get; set; }
        
        
        //---------- | Smoothing Page | ----------
        public static FloatElement PSmoothing { get; private set; }
        public static FloatElement RSmoothing { get; private set; }
        
        

        
        
        //---------- | View Page | ----------
        public static EnumElement ViewMode { get; private set; }
        
        //---------- | Rotation Offset Page | ----------
        public static FloatElement XrOffset { get; private set; }
        public static FloatElement YrOffset { get; private set; }
        public static FloatElement ZrOffset { get; private set; }
        
        
        //---------- | Position Offset Page | ----------
        private static Page PositionOffsetPage { get; set; }
        public static FloatElement XpOffset { get; private set; }
        public static FloatElement YpOffset { get; private set; }
        public static FloatElement ZpOffset { get; private set; }
        
        
        
        //---------- | Post-Processing Page | ----------
        public static BoolElement PostFXToggle { get; private set; }

        
        
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
        private static Page ModSettingsPage { get; set; }
        public static BoolElement AutoSave { get; private set; }
        public static BoolElement OtherNotifi { get; private set; }
        public static BoolElement PrefNotifi { get; private set; }
        public static BoolElement CameraDisabledNotifi { get; private set; }
        public static BoolElement CameraFoundNotifi { get; private set; }


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
                if (preset.Version != BuildInfo.Version)
                {
                    page.Color = Color.yellow;
                    page.Name = $"{page.Name} [OUTDATED]";
                }
                page.CreateFunction("Save Current Setting To This", Color.cyan, () => PresetsManager.SavePreset(presetName));
                page.CreateFunction("Load This Preset", Color.green, () => PresetsManager.ApplyPreset(presetName));
                page.CreateFunction("View Path", Color.yellow, () => PresetsManager.ViewPath(presetName));
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
            MainPage = Page.Root.CreatePage("WideEye", Color.white);
            FOVSlider = MainPage.CreateFloat("FOV", Color.cyan, 72f, 1f, float.MinValue, float.MaxValue, value => SettingsApplier.ApplyFOV(value));
            MainPage.CreateFunction("Reset To Default", Color.red, () => SettingsApplier.ResetToDefault(ModEnums.ResetType.Fov));
            MainPage.CreateFunction("Save Preferences", Color.green, ModPreferences.SavePref);
            
            //---------------------------------
            
            PostFXPage = MainPage.CreatePage("Post-Processing", Color.yellow);
            
            PostFXToggle = PostFXPage.CreateBool("Enabled", Color.yellow, true, value => SettingsApplier.ApplyOther(ModEnums.OtherType.PostFX, value));
            
            //---------------------------------
            
            LensDistortionPage = PostFXPage.CreatePage("Lens Distortion", Color.white);
            LdEnabled = LensDistortionPage.CreateBool("Enabled", Color.white, true, _ => SettingsApplier.ApplyLd());
            LdCenterX = LensDistortionPage.CreateFloat("Center X", Color.red, 0.50f, 0.1f, 0f, 1f, _ => SettingsApplier.ApplyLd());
            LdCenterY = LensDistortionPage.CreateFloat("Center Y", Color.green, 0.50f, 0.1f, 0f, 1f, _ => SettingsApplier.ApplyLd());
            LdIntensity = LensDistortionPage.CreateFloat("Intensity", Color.white, 0.48f, 0.01f, 0f, 1f, _ => SettingsApplier.ApplyLd());
            LdScale = LensDistortionPage.CreateFloat("Scale", Color.white, 1f, 0.1f, 0, 1, _ => SettingsApplier.ApplyLd());
            LdXMultiplier = LensDistortionPage.CreateFloat("X Multiplier", Color.red, 0.59f, 0.01f, 0f, 1f, _ => SettingsApplier.ApplyLd());
            LdYMultiplier = LensDistortionPage.CreateFloat("Y Multiplier", Color.green, 1f, 0.01f, 0f, 1f, _ => SettingsApplier.ApplyLd());
            LensDistortionPage.CreateFunction("Reset To Default", Color.red, () => SettingsApplier.ResetToDefault(ModEnums.ResetType.LensDistortion));
            
            //---------------------------------
            
            ChromaticAberrationPage = PostFXPage.CreatePage("ChromaticAberration", Color.white);
            CaEnabled = ChromaticAberrationPage.CreateBool("Enabled", Color.white, true, _ => SettingsApplier.ApplyCa());
            CaIntensity = ChromaticAberrationPage.CreateFloat("Intensity", Color.white, 0.123f, 0.01f, 0f, 1f, _ => SettingsApplier.ApplyCa());
            ChromaticAberrationPage.CreateFunction("Reset To Default", Color.red, () => SettingsApplier.ResetToDefault(ModEnums.ResetType.ChromaticAberration));
            
            //---------------------------------
            
            AutoExposurePage = PostFXPage.CreatePage("AutoExposure", Color.white);
            AeEnabled = AutoExposurePage.CreateBool("Enabled", Color.white, true, _ => SettingsApplier.ApplyAe());
            AeAdaptationMode = AutoExposurePage.CreateEnum("Adaptation Mode", Color.white, AutoExposureAdaptationMode.Progressive, _ => SettingsApplier.ApplyAe());
            AeD2Ls = AutoExposurePage.CreateFloat("Dark To Light Speed", Color.white, 3f, 0.1f, float.MinValue, float.MaxValue, _ => SettingsApplier.ApplyAe());
            AeEvCompensation = AutoExposurePage.CreateFloat("EV Compensation", Color.white, 2.5f, 0.1f, float.MinValue, float.MaxValue, _ => SettingsApplier.ApplyAe());
            AeEvMax = AutoExposurePage.CreateFloat("EV Max", Color.white, 1.2f, 0.1f, float.MinValue, float.MaxValue, _ => SettingsApplier.ApplyAe());
            AeEvMin = AutoExposurePage.CreateFloat("EV Min", Color.white, -1.2f, 0.1f, float.MinValue, float.MaxValue, _ => SettingsApplier.ApplyAe());
            AeL2Ds = AutoExposurePage.CreateFloat("Light To Dark Speed", Color.white, 1f, 0.1f, float.MinValue, float.MaxValue, _ => SettingsApplier.ApplyAe());
            AeMeteringMaskMode = AutoExposurePage.CreateEnum("Metering Mask Mode", Color.white, AutoExposureMeteringMaskMode.Procedural, null);
            AeMeteringProceduralFalloff = AutoExposurePage.CreateFloat("Metering Procedural Falloff", Color.white, 2f, 0.1f, float.MinValue, float.MaxValue, _ => SettingsApplier.ApplyAe());
            AutoExposurePage.CreateFunction("Reset To Default", Color.red, () => SettingsApplier.ResetToDefault(ModEnums.ResetType.AutoExposure));
            
            //---------------------------------
            
            OffsetPage = MainPage.CreatePage("Offset", Color.white);
            RotationOffsetPage = OffsetPage.CreatePage("Rotation Offset", Color.white);
            XrOffset = RotationOffsetPage.CreateFloat("X Rotation Offset", Color.red, 11f, 1f, float.MinValue, float.MaxValue, _ => SettingsApplier.ApplyOffset(ModEnums.OffsetType.Rotation));
            YrOffset = RotationOffsetPage.CreateFloat("Y Rotation Offset", Color.green, 0f, 1f, float.MinValue, float.MaxValue, _ => SettingsApplier.ApplyOffset(ModEnums.OffsetType.Rotation));
            ZrOffset = RotationOffsetPage.CreateFloat("Z Rotation Offset", Color.blue, 0f, 1f, float.MinValue, float.MaxValue, _ => SettingsApplier.ApplyOffset(ModEnums.OffsetType.Rotation));
            RotationOffsetPage.CreateFunction("Reset To Default", Color.red, () => SettingsApplier.ResetToDefault(ModEnums.ResetType.RotationOffset));
            
            //---------------------------------
            
            PositionOffsetPage = OffsetPage.CreatePage("Position Offset", Color.white);
            XpOffset = PositionOffsetPage.CreateFloat("X Position Offset", Color.red, 0f, 0.1f, float.MinValue, float.MaxValue, _ => SettingsApplier.ApplyOffset(ModEnums.OffsetType.Position));
            YpOffset = PositionOffsetPage.CreateFloat("Y Position Offset", Color.green, 0f, 0.1f, float.MinValue, float.MaxValue, _ => SettingsApplier.ApplyOffset(ModEnums.OffsetType.Position));
            ZpOffset = PositionOffsetPage.CreateFloat("Z Position Offset", Color.blue, 0f, 0.1f, float.MinValue, float.MaxValue, _ => SettingsApplier.ApplyOffset(ModEnums.OffsetType.Position));
            PositionOffsetPage.Add(new FunctionElement("Reset To Default", Color.red, () => SettingsApplier.ResetToDefault(ModEnums.ResetType.PositionOffset)));
            
            //---------------------------------
            
            SmoothingPage = MainPage.CreatePage("Smoothing", Color.white);
            PSmoothing = SmoothingPage.CreateFloat("Position Smoothing", Color.white, 0f, 1f, float.MinValue, int.MaxValue, _ => SettingsApplier.ApplySmoothing());
            RSmoothing = SmoothingPage.CreateFloat("Rotation Smoothing", Color.white, 0f, 1f, float.MinValue, int.MaxValue, _ => SettingsApplier.ApplySmoothing());
            SmoothingPage.Add(new FunctionElement("Reset To Default", Color.red, () => SettingsApplier.ResetToDefault(ModEnums.ResetType.Smoothing)));
            
            //---------------------------------
            
            PresetsPage = MainPage.CreatePage("Presets", Color.magenta);
            var presetNameInput = PresetsPage.CreateString("Preset Name", Color.white, "", null);
            PresetsPage.CreateFunction("Create Preset From Current Settings", Color.white, () => PresetsManager.CreatePreset(presetNameInput.Value));
            CreatePresetsPage();
            Menu.OnPageOpened += page =>
            {
                if (page == PresetsPage)
                {
                    PresetsManager.RefreshPresetList();
                }
            };
            
            //---------------------------------
            
            ViewPage = MainPage.CreatePage("View", Color.cyan);
            ViewMode = ViewPage.CreateEnum("View Mode", Color.white, ModEnums.ViewMode.Head, v => CameraController.UpdateView((ModEnums.ViewMode)v));
            
            HandheldCameraPage = ViewPage.CreatePage("Handheld Camera", Color.white); 
            HandheldCameraPage.CreateFunction("Spawn Camera", Color.green, HandheldCameraManager.SpawnHandheldCamera);
            HandheldCameraPage.CreateFunction("Destroy Camera", Color.red, HandheldCameraManager.DestroyHandheldCamera);
            HandheldCameraPage.CreateFunction("Teleport Camera", Color.cyan, HandheldCameraManager.TeleportHandheldCamera);
            
            //---------------------------------
            ModSettingsPage = MainPage.CreatePage("Mod Settings", Color.green);
            
            PrefNotifi =
                ModSettingsPage.CreateBool("Preferences Notifications", Color.white, true, _ => ModNotification.ChangeSilentNotification());
            CameraDisabledNotifi = 
                ModSettingsPage.CreateBool("Camera Disabled Notifications", Color.white, true, _ => ModNotification.ChangeSilentNotification());
            CameraFoundNotifi =
                ModSettingsPage.CreateBool("Camera Found Notifications", Color.white, true, _ => ModNotification.ChangeSilentNotification());
            OtherNotifi = 
                ModSettingsPage.CreateBool("Other Notifications", Color.white, true, _ => ModNotification.ChangeSilentNotification());
            
            AutoSave = ModSettingsPage.CreateBool("Auto Save (Experimental)", Color.yellow, false, v => ModPreferences.AutoSave = v);
            ModSettingsPage.CreateBool("Head Meshes", Color.yellow, true, value => SettingsApplier.ApplyOther(ModEnums.OtherType.HeadMesh, value));
            ModSettingsPage.CreateBool("Hair Meshes", Color.yellow, true, value => SettingsApplier.ApplyOther(ModEnums.OtherType.HairMeshes, value));
            ModSettingsPage.CreateFunction("Reset All To Default", Color.red, () => SettingsApplier.ResetToDefault(ModEnums.ResetType.All));
            ModSettingsPage.CreateFunction("Load Preferences", Color.green, ModPreferences.LoadPref);
            ModSettingsPage.CreateFunction("Clear All Preferences", Color.red, ModPreferences.ClearPref);
            
            SupportPage = ModSettingsPage.CreatePage("Support", Color.white);
            SupportPage.CreateFunction("Open GitHub Issues", Color.white, () =>
            {
                Application.OpenURL("https://github.com/HL2H0/WideEye/issues");
                var notification = new ModNotification(ModNotification.ModNotificationType.Other, "WideEye | Success", "Opened the GitHub issues page for WideEye On Desktop", NotificationType.Success, 2);
                notification.Show();
            });

            SupportPage.CreateFunction("Discord", Color.blue, () =>
            {
                GUIUtility.systemCopyBuffer = "@hiiiiiiiiiiiiiiiiii";
                var notification = new ModNotification(ModNotification.ModNotificationType.Other, "WideEye | Success", "Copied username to clipboard", NotificationType.Success, 2);
                notification.Show();
            });

            SupportPage.CreateFunction("Support me on Ko-Fi", Color.magenta, () =>
            {
                Application.OpenURL("https://ko-fi.com/hl2h0");
                var notification = new ModNotification(ModNotification.ModNotificationType.Other, "WideEye | Success",
                    "Opened the Ko-Fi page for WideEye On Desktop", NotificationType.Success, 2);
                notification.Show();
            });

            SupportPage.CreateFunction($"Version :  {BuildInfo.Version}", Color.white, null);
        }
    }
}

