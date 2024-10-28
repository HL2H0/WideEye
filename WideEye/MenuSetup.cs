using BoneLib.BoneMenu;
using BoneLib.Notifications;

using Il2CppOccaSoftware.Exposure.Runtime;

using UnityEngine;

using static WideEye.Mod;
using static WideEye.ModPreferences;

namespace WideEye
{
    public class MenuSetup
    {
        public static FunctionElement GetCameraButton = new("Get Camera", Color.red, () => GetTargetCamera(false));

        public static Page mainPage;
        public static FloatElement fovSlider { get; private set; }
        public static Page SmoothingPage { get; private set; }
        public static FloatElement P_Smoothing { get; private set; }
        public static FloatElement R_Smoothing { get; private set; }
        public static Page OffsetPage { get; private set; }
        public static Page RotationOffsetPage { get; private set; }
        public static FloatElement X_R_Offset { get; private set; }
        public static FloatElement Y_R_Offset { get; private set; }
        public static FloatElement Z_R_Offset { get; private set; }
        public static Page PositionOffsetPage { get; private set; }
        public static FloatElement X_P_Offset { get; private set; }
        public static FloatElement Y_P_Offset { get; private set; }
        public static FloatElement Z_P_Offset { get; private set; }
        public static Page PostFXPage { get; private set; }
        public static BoolElement PostFXToogle { get; private set; }
        public static Page OtherPage { get; private set; }
        public static Page CameraModePage { get; private set; } 
        public static BoolElement Ele_LookAtPlayer { get; private set; }
        public static Page SupportPage { get; private set; }

        public static Page LensDistortionPage { get; private set; }
        public static BoolElement LD_Enabled { get; private set; }
        public static FloatElement LD_CenterX { get; private set; }
        public static FloatElement LD_CenterY { get; private set; }
        public static FloatElement LD_Intensity { get; private set; }
        public static FloatElement LD_Scale { get; private set; }
        public static FloatElement LD_xMultiplier { get; private set; }
        public static FloatElement LD_yMultiplier { get; private set; }


        public static Page ChromaticAberrationPage { get; private set; }
        public static BoolElement CA_Enabled { get; private set; }
        public static FloatElement CA_Intensity { get; private set; }

        public static Page AutoExposurePage { get; private set; }
        public static BoolElement AE_enabled { get; private set; }
        public static EnumElement AE_adaptationMode { get; private set; }
        public static FloatElement AE_D2LS { get; private set; }
        public static FloatElement AE_evCompensation { get; private set; }
        public static FloatElement AE_evMax { get; private set; }
        public static FloatElement AE_evMin { get; private set; }
        public static FloatElement AE_L2DS { get; private set; }
        public static EnumElement AE_MeteringMaskMode { get; private set; }
        public static FloatElement AE_MeteringProceduralFalloff { get; private set; }



        public static void SetupBoneMenu()
        {
            mainPage = Page.Root.CreatePage("Wide Eye", Color.white);
            fovSlider = mainPage.CreateFloat("FOV", Color.cyan, 72, 1, float.MinValue, float.MaxValue, (value) => ApplyFOV(value));
            mainPage.Add(new FunctionElement("Reset To Default", Color.red, () => ResetToDefault(ResetType.Fov)));
            mainPage.Add(new FunctionElement("Save Preferences", Color.green, SavePref));

            PostFXPage = mainPage.CreatePage("Post-Processing", Color.yellow);
            PostFXToogle = PostFXPage.CreateBool("Enabled", Color.yellow, true, (value) => ApplyOther(OtherType.PostFX, value)) ;

            LensDistortionPage = PostFXPage.CreatePage("Lens Distortion", Color.white);
            LD_Enabled = LensDistortionPage.CreateBool("Enabled", Color.white, true, (value) => ApplyLD());
            LD_CenterX = LensDistortionPage.CreateFloat("Center X", Color.red, 0.50f, 0.1f, 0, 1, (value) => ApplyLD());
            LD_CenterY = LensDistortionPage.CreateFloat("Center Y", Color.green, 0.50f, 0.1f, 0, 1, (value) => ApplyLD());
            LD_Intensity = LensDistortionPage.CreateFloat("Intensity", Color.white, 0.48f, 0.01f, 0, 1, (value) => ApplyLD());
            LD_Scale = LensDistortionPage.CreateFloat("Scale", Color.white, 1, 0.1f, 0, 1, (value) => ApplyLD());
            LD_xMultiplier = LensDistortionPage.CreateFloat("X Multiplier", Color.red, 0.59f, 0.01f, 0, 1, (value) => ApplyLD());
            LD_yMultiplier = LensDistortionPage.CreateFloat("Y Multiplier", Color.green, 1, 0.01f, 0, 1, (value) => ApplyLD());
            LensDistortionPage.CreateFunction("Reset To Default", Color.red, () => ResetToDefault(ResetType.LensDistortion));

            ChromaticAberrationPage = PostFXPage.CreatePage("ChromaticAberration", Color.white);
            CA_Enabled = ChromaticAberrationPage.CreateBool("Enabled", Color.white, true, (value) => ApplyCA());
            CA_Intensity = ChromaticAberrationPage.CreateFloat("Intensity", Color.white, 0.123f, 0.01f, 0, 1, (value) => ApplyCA());
            ChromaticAberrationPage.CreateFunction("Reset To Default", Color.red, () => ResetToDefault(ResetType.ChromaticAberration));

            AutoExposurePage = PostFXPage.CreatePage("AutoExposure", Color.white);
            AE_enabled = AutoExposurePage.CreateBool("Enabled", Color.white, true, (value) => ApplyAE());
            AE_adaptationMode = AutoExposurePage.CreateEnum("Adaptation Mode", Color.white, AutoExposureAdaptationMode.Progressive, (value) => ApplyAE());
            AE_D2LS = AutoExposurePage.CreateFloat("Dark To Light Speed", Color.white, 3, 0.1f, float.MinValue, float.MaxValue, (value) => ApplyAE());
            AE_evCompensation = AutoExposurePage.CreateFloat("EV Compensation", Color.white, 2.5f, 0.1f, float.MinValue, float.MaxValue, (value) => ApplyAE());
            AE_evMax = AutoExposurePage.CreateFloat("EV Max", Color.white, 1.2f, 0.1f, float.MinValue, float.MaxValue, (value) => ApplyAE());
            AE_evMin = AutoExposurePage.CreateFloat("EV Min", Color.white, -1.2f, 0.1f, float.MinValue, float.MaxValue, (value) => ApplyAE());
            AE_L2DS = AutoExposurePage.CreateFloat("Light To Dark Speed", Color.white, 1, 0.1f, float.MinValue, float.MaxValue, (value) => ApplyAE());
            AE_MeteringMaskMode = AutoExposurePage.CreateEnum("Metering Mask Mode", Color.white, AutoExposureMeteringMaskMode.Procedural, null);
            AE_MeteringProceduralFalloff = AutoExposurePage.CreateFloat("Metering Procedural Falloff", Color.white, 2, 0.1f, float.MinValue, float.MaxValue, (value) => ApplyAE());
            AutoExposurePage.CreateFunction("Reset To Default", Color.red, () => ResetToDefault(ResetType.AutoExposure));

            OffsetPage = mainPage.CreatePage("Offset", Color.white);
            RotationOffsetPage = OffsetPage.CreatePage("Rotation Offset", Color.white);
            X_R_Offset = RotationOffsetPage.CreateFloat("X Rotation Offset", Color.red, 11, 1, float.MinValue, float.MaxValue, (value) => ApplyOffset(OffsetType.Rotation));
            Y_R_Offset = RotationOffsetPage.CreateFloat("Y Rotation Offset", Color.green, 0, 1, float.MinValue, float.MaxValue, (value) => ApplyOffset(OffsetType.Rotation));
            Z_R_Offset = RotationOffsetPage.CreateFloat("Z Rotation Offset", Color.blue, 0, 1, float.MinValue, float.MaxValue, (value) => ApplyOffset(OffsetType.Rotation));
            RotationOffsetPage.CreateFunction("Reset To Default", Color.red, () => ResetToDefault(ResetType.RotationOffset));

            PositionOffsetPage = OffsetPage.CreatePage("Position Offset", Color.white);
            X_P_Offset = PositionOffsetPage.CreateFloat("X Position Offset", Color.red, 0, 0.1f, float.MinValue, float.MaxValue, (value) => ApplyOffset(OffsetType.Position));
            Y_P_Offset = PositionOffsetPage.CreateFloat("Y Position Offset", Color.green, 0, 0.1f, float.MinValue, float.MaxValue, (value) => ApplyOffset(OffsetType.Position));
            Z_P_Offset = PositionOffsetPage.CreateFloat("Z Position Offset", Color.blue, 0, 0.1f, float.MinValue, float.MaxValue, (value) => ApplyOffset(OffsetType.Position));
            PositionOffsetPage.Add(new FunctionElement("Reset To Default", Color.red, () => ResetToDefault(ResetType.PostionOffset)));

            SmoothingPage = mainPage.CreatePage("Smoothing", Color.white);
            P_Smoothing = SmoothingPage.CreateFloat("Position Smoothing", Color.white, 0, 1f, float.MinValue, int.MaxValue, (value) => ApplySmoothing());
            R_Smoothing = SmoothingPage.CreateFloat("Rotation Smoothing", Color.white, 0, 1f, float.MinValue, int.MaxValue, (value) => ApplySmoothing());
            SmoothingPage.Add(new FunctionElement("Reset To Default", Color.red, () => ResetToDefault(ResetType.Smoothing)));

            CameraModePage = mainPage.CreatePage("Camera Mode", Color.white);
            CameraModePage.Add(new EnumElement("Camera Mode", Color.white, CameraMode.Head, value => ApplyCameraMode((CameraMode)value)));
            Ele_LookAtPlayer = CameraModePage.CreateBool("Look At Player", Color.white, false, value => LookAtPlayer = value);
            CameraModePage.Add(new EnumElement("Look At :", Color.white, LookAtPositionType.Head, value => ApplyLookAtTransform((LookAtPositionType)value)));

            OtherPage = mainPage.CreatePage("Other", Color.gray);
            OtherPage.Add(new BoolElement("Head Meshes", Color.yellow, true, (value) => ApplyOther(OtherType.HeadMesh, value)));
            OtherPage.Add(new BoolElement("Hair Meshes", Color.yellow, true, (value) => ApplyOther(OtherType.HairMeshes, value)));
            OtherPage.Add(new FunctionElement("Reset All To Default", Color.red, () => ResetToDefault(ResetType.All)));
            OtherPage.Add(new FunctionElement("Load Preferences", Color.green, LoadPref));
            OtherPage.Add(new FunctionElement("Clear All Preferences", Color.red, ClearPref));

            SupportPage = OtherPage.CreatePage("Support", Color.white);
            SupportPage.Add(new FunctionElement("Open GitHub Issues", Color.white, () =>
            {
                Application.OpenURL("https://github.com/HL2H0/WideEye/issues");
                SendNotfi("WideEye | Success", "Opened the GitHub issues page for WideEye On Desktop", NotificationType.Success, 2, true);
            }));

            SupportPage.Add(new FunctionElement("Discord", Color.blue, () =>
            {
                GUIUtility.systemCopyBuffer = "@hiiiiiiiiiiiiiiiiii";
                SendNotfi("WideEye | Success", "Copied Username to clipboard", NotificationType.Success, 3, true);
            }));

            SupportPage.Add(new FunctionElement("Version : 2.0.0", Color.white, null));
        }
    }
}

