using BoneLib.BoneMenu;
using BoneLib.Notifications;

using Il2CppOccaSoftware.Exposure.Runtime;

using UnityEngine;

using static WideEye.Mod;
using static WideEye.ModPreferences;

namespace WideEye
{
    public static class MenuSetup
    {
        public static FunctionElement GetCameraButton = new("Get Camera", Color.red, () => GetTargetCamera(false));

        public static Page MainPage;
        public static FloatElement FOVSlider { get; private set; }
        private static Page SmoothingPage { get; set; }
        public static FloatElement PSmoothing { get; private set; }
        public static FloatElement RSmoothing { get; private set; }
        private static Page OffsetPage { get; set; }
        private static Page RotationOffsetPage { get; set; }
        public static FloatElement XROffset { get; private set; }
        public static FloatElement YrOffset { get; private set; }
        public static FloatElement ZrOffset { get; private set; }
        private static Page PositionOffsetPage { get; set; }
        public static FloatElement XpOffset { get; private set; }
        public static FloatElement YpOffset { get; private set; }
        public static FloatElement ZpOffset { get; private set; }
        private static Page PostFXPage { get; set; }
        public static BoolElement PostFXToggle { get; private set; }
        private static Page OtherPage { get; set; }
        private static Page CameraModePage { get; set; } 
        public static BoolElement EleLookAtPlayer { get; private set; }
        private static Page SupportPage { get; set; }

        private static Page LensDistortionPage { get; set; }
        public static BoolElement LdEnabled { get; private set; }
        public static FloatElement LdCenterX { get; private set; }
        public static FloatElement LdCenterY { get; private set; }
        public static FloatElement LdIntensity { get; private set; }
        public static FloatElement LdScale { get; private set; }
        public static FloatElement LdXMultiplier { get; private set; }
        public static FloatElement LdYMultiplier { get; private set; }


        private static Page ChromaticAberrationPage { get; set; }
        public static BoolElement CaEnabled { get; private set; }
        public static FloatElement CaIntensity { get; private set; }

        private static Page AutoExposurePage { get; set; }
        public static BoolElement AeEnabled { get; private set; }
        public static EnumElement AeAdaptationMode { get; private set; }
        public static FloatElement AeD2Ls { get; private set; }
        public static FloatElement AeEvCompensation { get; private set; }
        public static FloatElement AeEvMax { get; private set; }
        public static FloatElement AeEvMin { get; private set; }
        public static FloatElement AeL2Ds { get; private set; }
        public static EnumElement AeMeteringMaskMode { get; private set; }
        public static FloatElement AeMeteringProceduralFalloff { get; private set; }



        public static void SetupBoneMenu()
        {
            MainPage = Page.Root.CreatePage("Wide Eye", Color.white);
            FOVSlider = MainPage.CreateFloat("FOV", Color.cyan, 72, 1, float.MinValue, float.MaxValue, value => ApplyFOV(value));
            MainPage.Add(new FunctionElement("Reset To Default", Color.red, () => ResetToDefault(ResetType.Fov)));
            MainPage.Add(new FunctionElement("Save Preferences", Color.green, SavePref));

            PostFXPage = MainPage.CreatePage("Post-Processing", Color.yellow);
            PostFXToggle = PostFXPage.CreateBool("Enabled", Color.yellow, true, value => ApplyOther(OtherType.PostFX, value)) ;

            LensDistortionPage = PostFXPage.CreatePage("Lens Distortion", Color.white);
            LdEnabled = LensDistortionPage.CreateBool("Enabled", Color.white, true, _ => ApplyLd());
            LdCenterX = LensDistortionPage.CreateFloat("Center X", Color.red, 0.50f, 0.1f, 0, 1, _ => ApplyLd());
            LdCenterY = LensDistortionPage.CreateFloat("Center Y", Color.green, 0.50f, 0.1f, 0, 1, _ => ApplyLd());
            LdIntensity = LensDistortionPage.CreateFloat("Intensity", Color.white, 0.48f, 0.01f, 0, 1, _ => ApplyLd());
            LdScale = LensDistortionPage.CreateFloat("Scale", Color.white, 1, 0.1f, 0, 1, _ => ApplyLd());
            LdXMultiplier = LensDistortionPage.CreateFloat("X Multiplier", Color.red, 0.59f, 0.01f, 0, 1, _ => ApplyLd());
            LdYMultiplier = LensDistortionPage.CreateFloat("Y Multiplier", Color.green, 1, 0.01f, 0, 1, _ => ApplyLd());
            LensDistortionPage.CreateFunction("Reset To Default", Color.red, () => ResetToDefault(ResetType.LensDistortion));

            ChromaticAberrationPage = PostFXPage.CreatePage("ChromaticAberration", Color.white);
            CaEnabled = ChromaticAberrationPage.CreateBool("Enabled", Color.white, true, _ => ApplyCa());
            CaIntensity = ChromaticAberrationPage.CreateFloat("Intensity", Color.white, 0.123f, 0.01f, 0, 1, _ => ApplyCa());
            ChromaticAberrationPage.CreateFunction("Reset To Default", Color.red, () => ResetToDefault(ResetType.ChromaticAberration));

            AutoExposurePage = PostFXPage.CreatePage("AutoExposure", Color.white);
            AeEnabled = AutoExposurePage.CreateBool("Enabled", Color.white, true, _ => ApplyAe());
            AeAdaptationMode = AutoExposurePage.CreateEnum("Adaptation Mode", Color.white, AutoExposureAdaptationMode.Progressive, _ => ApplyAe());
            AeD2Ls = AutoExposurePage.CreateFloat("Dark To Light Speed", Color.white, 3, 0.1f, float.MinValue, float.MaxValue, _ => ApplyAe());
            AeEvCompensation = AutoExposurePage.CreateFloat("EV Compensation", Color.white, 2.5f, 0.1f, float.MinValue, float.MaxValue, _ => ApplyAe());
            AeEvMax = AutoExposurePage.CreateFloat("EV Max", Color.white, 1.2f, 0.1f, float.MinValue, float.MaxValue, _ => ApplyAe());
            AeEvMin = AutoExposurePage.CreateFloat("EV Min", Color.white, -1.2f, 0.1f, float.MinValue, float.MaxValue, _ => ApplyAe());
            AeL2Ds = AutoExposurePage.CreateFloat("Light To Dark Speed", Color.white, 1, 0.1f, float.MinValue, float.MaxValue, _ => ApplyAe());
            AeMeteringMaskMode = AutoExposurePage.CreateEnum("Metering Mask Mode", Color.white, AutoExposureMeteringMaskMode.Procedural, null);
            AeMeteringProceduralFalloff = AutoExposurePage.CreateFloat("Metering Procedural Falloff", Color.white, 2, 0.1f, float.MinValue, float.MaxValue, _ => ApplyAe());
            AutoExposurePage.CreateFunction("Reset To Default", Color.red, () => ResetToDefault(ResetType.AutoExposure));

            OffsetPage = MainPage.CreatePage("Offset", Color.white);
            RotationOffsetPage = OffsetPage.CreatePage("Rotation Offset", Color.white);
            XROffset = RotationOffsetPage.CreateFloat("X Rotation Offset", Color.red, 11, 1, float.MinValue, float.MaxValue, _ => ApplyOffset(OffsetType.Rotation));
            YrOffset = RotationOffsetPage.CreateFloat("Y Rotation Offset", Color.green, 0, 1, float.MinValue, float.MaxValue, _ => ApplyOffset(OffsetType.Rotation));
            ZrOffset = RotationOffsetPage.CreateFloat("Z Rotation Offset", Color.blue, 0, 1, float.MinValue, float.MaxValue, _ => ApplyOffset(OffsetType.Rotation));
            RotationOffsetPage.CreateFunction("Reset To Default", Color.red, () => ResetToDefault(ResetType.RotationOffset));

            PositionOffsetPage = OffsetPage.CreatePage("Position Offset", Color.white);
            XpOffset = PositionOffsetPage.CreateFloat("X Position Offset", Color.red, 0, 0.1f, float.MinValue, float.MaxValue, _ => ApplyOffset(OffsetType.Position));
            YpOffset = PositionOffsetPage.CreateFloat("Y Position Offset", Color.green, 0, 0.1f, float.MinValue, float.MaxValue, _ => ApplyOffset(OffsetType.Position));
            ZpOffset = PositionOffsetPage.CreateFloat("Z Position Offset", Color.blue, 0, 0.1f, float.MinValue, float.MaxValue, _ => ApplyOffset(OffsetType.Position));
            PositionOffsetPage.Add(new FunctionElement("Reset To Default", Color.red, () => ResetToDefault(ResetType.PositionOffset)));

            SmoothingPage = MainPage.CreatePage("Smoothing", Color.white);
            PSmoothing = SmoothingPage.CreateFloat("Position Smoothing", Color.white, 0, 1f, float.MinValue, int.MaxValue, _ => ApplySmoothing());
            RSmoothing = SmoothingPage.CreateFloat("Rotation Smoothing", Color.white, 0, 1f, float.MinValue, int.MaxValue, _ => ApplySmoothing());
            SmoothingPage.Add(new FunctionElement("Reset To Default", Color.red, () => ResetToDefault(ResetType.Smoothing)));

            CameraModePage = MainPage.CreatePage("Camera Mode", Color.white);
            CameraModePage.Add(new EnumElement("Camera Mode", Color.white, CameraModeType.Head, value => ApplyCameraMode((CameraModeType)value)));
            EleLookAtPlayer = CameraModePage.CreateBool("Look At Player", Color.white, false, value => LookAtPlayer = value);
            CameraModePage.Add(new EnumElement("Look At :", Color.white, LookAtPositionType.Head, value => ApplyLookAtTransform((LookAtPositionType)value)));

            OtherPage = MainPage.CreatePage("Other", Color.gray);
            OtherPage.Add(new BoolElement("Head Meshes", Color.yellow, true, value => ApplyOther(OtherType.HeadMesh, value)));
            OtherPage.Add(new BoolElement("Hair Meshes", Color.yellow, true, value => ApplyOther(OtherType.HairMeshes, value)));
            OtherPage.Add(new FunctionElement("Reset All To Default", Color.red, () => ResetToDefault(ResetType.All)));
            OtherPage.Add(new FunctionElement("Load Preferences", Color.green, LoadPref));
            OtherPage.Add(new FunctionElement("Clear All Preferences", Color.red, ClearPref));

            SupportPage = OtherPage.CreatePage("Support", Color.white);
            SupportPage.Add(new FunctionElement("Open GitHub Issues", Color.white, () =>
            {
                Application.OpenURL("https://github.com/HL2H0/WideEye/issues");
                SendNotification("WideEye | Success", "Opened the GitHub issues page for WideEye On Desktop", NotificationType.Success, 2, true);
            }));

            SupportPage.Add(new FunctionElement("Discord", Color.blue, () =>
            {
                GUIUtility.systemCopyBuffer = "@hiiiiiiiiiiiiiiiiii";
                SendNotification("WideEye | Success", "Copied Username to clipboard", NotificationType.Success, 3, true);
            }));

            SupportPage.Add(new FunctionElement("Version : 2.0.0", Color.white, null));
        }
    }
}

