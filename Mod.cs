using BoneLib.BoneMenu;
using Il2CppSLZ.Bonelab;
using MelonLoader;
using UnityEngine;
using UnityEngine.Rendering;
using Page = BoneLib.BoneMenu.Page;
using BoneLib;

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
        //BoneMenu Pages

        public static Page mainPage { get; private set; }
        public static Page ExperimentalPage { get; private set; }
        public static Page SupportPage { get; private set; }

        //BoneMenu Elements

        public static FunctionElement getCameraButton { get; private set; }
        public static IntElement fovSlider { get; private set; }
        public static FunctionElement setFovButton { get; private set; }
        public static FunctionElement resetFovButton { get; private set; }
        public static BoolElement PostSFXToogle { get; private set; }
        public static IntElement PositionSmoothing { get; private set; }
        public static IntElement RotationSmoothing { get; private set; }
        public static FunctionElement ApplySmoothingButton { get; private set; }
        public static FloatElement X_R_Offset { get; private set; }
        public static FloatElement Y_R_Offset { get; private set; }
        public static FloatElement Z_R_Offset { get; private set; }
        public static FunctionElement Apply_R_OffsetButton { get; private set; }

        //Needed GameObjects and Components

        private static GameObject SC_GameObject;
        private static Camera SC_CameraComponent;
        private static SmoothFollower SC_SmootherComponent;
        private static Transform ST_Transform;
        private static Volume SC_VolumeComponent;
        private static Color ModColor = new Color(69, 28, 232);
        
        //Variables

        private static int defaultFOV = 70;
        private static bool gotcamera = false;

        //MelonLoader Events

        public override void OnInitializeMelon()
        {
            LoggerInstance.Msg("WideEye 1.2.0 Has Been Initialized.");
            Menu.OnPageOpened += OnPageOpened;
            SetupBoneMenu();
        }

        public override void OnSceneWasUnloaded(int buildIndex, string sceneName)
        {
            base.OnSceneWasUnloaded(buildIndex, sceneName);

            mainPage.Remove([fovSlider, setFovButton, resetFovButton, PostSFXToogle]);
            ExperimentalPage.RemoveAll();
            getCameraButton.ElementColor = Color.red;
            gotcamera = false;
        }

        //Custom Functions

        public static void GetTargetCamera()
        {
            SC_GameObject = GameObject.Find("GameplaySystems [0]/DisabledContainer/Spectator Camera/Spectator Camera");
            ST_Transform = GameObject.Find("RigManager(bonelab) [0]/VRControllerRig/TrackingSpace/Headset/Spectator Target").GetComponent<Transform>();
            SC_SmootherComponent = SC_GameObject.GetComponent<SmoothFollower>();
            SC_VolumeComponent = SC_GameObject.GetComponent<Volume>();
            SC_CameraComponent = SC_GameObject.GetComponent<Camera>();

            List<object> objectsToCheck = new List<object>
            {
                SC_GameObject,
                ST_Transform,
                SC_SmootherComponent,
                SC_VolumeComponent,
                SC_CameraComponent,
            };

            bool anyNull = false;
            foreach (var obj in objectsToCheck)
            {
                if (obj == null)
                {
                    anyNull = true;
                    break;
                }
            }

            if (anyNull)
            {
                MelonLogger.Error("Spectator Camera Components Not Found");
                Menu.DisplayDialog("Error", "Wow, that's rare! It looks like the camera couldn't be found.\n Try again.If the error keeps happening, go to the support page.", Dialog.ErrorIcon);
            }
            else
            {
                if (!gotcamera)
                { 
                    RefreshBoneMenu();
                    gotcamera = true; 
                }
            }

            
        }

        public static void SetCameraFOV()
        {
            if (SC_GameObject != null)
            {
                SC_CameraComponent.fieldOfView = fovSlider.Value;
            }
        }

        public static void ResetFOV()
        {
            if (SC_GameObject != null)
            {
                SC_CameraComponent .fieldOfView = defaultFOV;
            }
        }

        public static void Apply_R_Offset()
        {
            if (ST_Transform != null)
            {
                Vector3 offset = new Vector3(X_R_Offset.Value, Y_R_Offset.Value, Z_R_Offset.Value);
                ST_Transform.localRotation = Quaternion.Euler(offset);
            }
        }

        public static void ApplySmoothing()
        {
            SC_SmootherComponent.RotationalSmoothTime = RotationSmoothing.Value;
            SC_SmootherComponent.TranslationSmoothTime = PositionSmoothing.Value;
        }

        private static void OnPageOpened(Page page)
        {
            if(page == ExperimentalPage & !gotcamera)
            {
                Menu.DisplayDialog("Error", "Camera Is Not Found, Please click on the 'Find Camera Button'", Dialog.ErrorIcon);
            }
        }

        //BoneMenu Creation And Setup

        public static void RefreshBoneMenu()
        {
            //Main Page
            getCameraButton.ElementColor = Color.green;
            fovSlider = mainPage.CreateInt("FOV", Color.white, defaultFOV, 10, int.MinValue, int.MaxValue, null);
            setFovButton = mainPage.CreateFunction("Set FOV", Color.green, SetCameraFOV);
            resetFovButton = mainPage.CreateFunction("Reset FOV", Color.red, ResetFOV);
            PostSFXToogle = mainPage.CreateBool("Post-Processing", Color.yellow, true, isEnabled =>
            { 
                if (isEnabled) SC_VolumeComponent.enabled = true;
                else SC_VolumeComponent.enabled = false;
            });

            //Experimental Page
            X_R_Offset = ExperimentalPage.CreateFloat("X Rotation Offset", Color.red, 11, 0.1f, float.MinValue, float.MaxValue, null);
            Y_R_Offset = ExperimentalPage.CreateFloat("Y Rotation Offset", Color.green, 0, 0.1f, float.MinValue, float.MaxValue, null);
            Z_R_Offset = ExperimentalPage.CreateFloat("Z Rotation Offset", Color.blue, 0, 0.1f, float.MinValue, float.MaxValue, null);
            Apply_R_OffsetButton = ExperimentalPage.CreateFunction("Apply Rotation Offset", Color.white, Apply_R_Offset);

            PositionSmoothing = ExperimentalPage.CreateInt("Position Smoothing", Color.white, 0, 1, int.MinValue, int.MaxValue, null);
            RotationSmoothing = ExperimentalPage.CreateInt("Rotation Smoothing", Color.white, 0, 1, int.MinValue, int.MaxValue, null);
            ApplySmoothingButton = ExperimentalPage.CreateFunction("Apply Values", Color.white, ApplySmoothing);
        }

        public static void SetupBoneMenu()
        {
            mainPage = Page.Root.CreatePage("Wide View", ModColor);
            SupportPage = mainPage.CreatePage("Support", Color.white); 
            SupportPage.Add(new FunctionElement("Github issues", Color.white, () =>
            {
                Application.OpenURL("https://github.com/HL2H0/WideEye/issues");
            }));
            SupportPage.Add(new FunctionElement("Discord", Color.cyan, () =>
            {
                GUIUtility.systemCopyBuffer = "@hiiiiiiiiiiiiiiiiii";
                Menu.DisplayDialog("Copied!", "Copied username to clipboard\n Username : @hiiiiiiiiiiiiiiiiii", Dialog.InfoIcon);
            }));
            ExperimentalPage = mainPage.CreatePage("Experimental", Color.yellow);
            getCameraButton = mainPage.CreateFunction("Find Camera", Color.red, GetTargetCamera);
        }
    }
}