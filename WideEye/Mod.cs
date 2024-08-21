using BoneLib.BoneMenu;
using MelonLoader;
using UnityEngine;
using UnityEngine.Rendering;

[assembly: MelonInfo(typeof(WideEye.Mod), "WideEye", "1.1.0", "HL2H0", null)]
[assembly: MelonGame("Stress Level Zero", "BONELAB")]

namespace WideEye
{
    public static class BuildInfo
    {
        public const string Name = "WideEye";
        public const string Version = "1.1.0";
        public const string Author = "HL2H0";
    }

    public class Mod : MelonMod
    {
        public static Page mainPage { get; private set; }
        public static FunctionElement getCameraButton { get; private set; }
        public static IntElement fovSlider { get; private set; }
        public static FunctionElement setFovButton { get; private set; }
        public static FunctionElement resetFovButton { get; private set; }
        public static BoolElement PostSFXToogle { get; private set; }

        private static Camera targetCamera;
        private static int defaultFOV = 70;
        private static Volume targetVolume;
        private static bool gotcamera = false;

        public override void OnInitializeMelon()
        {
            LoggerInstance.Msg("WideEye 1.1.0 Has Been Initialized.");
            SetupBoneMenu();
        }

        public override void OnSceneWasUnloaded(int buildIndex, string sceneName)
        {
            base.OnSceneWasUnloaded(buildIndex, sceneName);

            mainPage.Remove([fovSlider, setFovButton, resetFovButton, PostSFXToogle]);
            getCameraButton.ElementColor = Color.red;
            gotcamera = false;
       
        }

        public static void GetTargetCamera()
        {
            targetCamera = GameObject.Find("GameplaySystems [0]/DisabledContainer/Spectator Camera/Spectator Camera").GetComponent<Camera>();
            targetVolume = targetCamera.GetComponent<Volume>();
            if (targetCamera == null & targetVolume == null)
            {
                MelonLogger.Error("Spectator Camera Components Not Found");
            }
            else if (!gotcamera) 
            { 
                GotCamera(); 
                gotcamera = true; 
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

        public static void GotCamera()
        {
            getCameraButton.ElementColor = Color.green;
            fovSlider = mainPage.CreateInt("FOV", Color.white, defaultFOV, 10, int.MinValue, int.MaxValue, null);
            setFovButton = mainPage.CreateFunction("Set FOV", Color.green, SetCameraFOV);
            PostSFXToogle = mainPage.CreateBool("Post-Processing", Color.yellow, true, isEnabled =>
            {

                if (isEnabled)
                {
                    targetVolume.enabled = true;
                }
                else
                {
                    targetVolume.enabled = false;
                }

            });
            resetFovButton = mainPage.CreateFunction("Reset FOV", Color.red, ResetFOV);


        }

        public static void SetupBoneMenu()
        {
            mainPage = Page.Root.CreatePage("Wide View", Color.white);
            getCameraButton = mainPage.CreateFunction("Find Camera", Color.red, GetTargetCamera);
        }
    }
}