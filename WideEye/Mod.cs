using MelonLoader;
using UnityEngine;
using BoneLib.BoneMenu;

[assembly: MelonInfo(typeof(WideEye.Mod), "WideEye", "1.2.0", "HL2H0", null)]
[assembly: MelonGame("Stress Level Zero", "BONELAB")]

namespace WideEye
{
    public static class BuildInfo
    {
        public const string Name = "WideEye";
        public const string Version = "1.2.0";
        public const string Author = "HL2H0";
    }

    public class Mod : MelonMod
    {
        public static Page mainPage { get; private set; }
        public static FunctionElement getCameraButton { get; private set; }
        public static IntElement fovSlider { get; private set; }
        public static FunctionElement setFovButton { get; private set; }
        public static FunctionElement resetFovButton { get; private set; }

        private static Camera targetCamera;
        private static int defaultFOV = 70;

        public override void OnInitializeMelon()
        {
            LoggerInstance.Msg("WideEye Has Been Initialized.");
            SetupBoneMenu();
        }

        public static void GetTargetCamera()
        {
            targetCamera = GameObject.Find("GameplaySystems [0]/DisabledContainer/Spectator Camera/Spectator Camera").GetComponent<Camera>();
            if (targetCamera == null)
            {
                MelonLogger.Error("Spectator Camera not found.");
            }
        }

        public static void SetCameraFOV()
        {
            if (targetCamera != null)
            {
                targetCamera.fieldOfView = fovSlider.Value;
            }
        }

        public static void ResetFOV()
        {
            if (targetCamera != null)
            {
                targetCamera.fieldOfView = defaultFOV;
            }
        }

        public static void SetupBoneMenu()
        {
            mainPage = Page.Root.CreatePage("Wide View", Color.white);
            getCameraButton = mainPage.CreateFunction("Find Camera", Color.white, GetTargetCamera);
            fovSlider = mainPage.CreateInt("FOV", Color.white, defaultFOV, 10, int.MinValue, int.MaxValue, null);
            setFovButton = mainPage.CreateFunction("Set FOV", Color.white, SetCameraFOV);
            resetFovButton = mainPage.CreateFunction("Reset FOV", Color.white, ResetFOV);
        }
    }
}