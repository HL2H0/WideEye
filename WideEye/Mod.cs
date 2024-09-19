using BoneLib.BoneMenu;
using Page = BoneLib.BoneMenu.Page;
using BoneLib.Notifications;
using Il2CppSLZ.Bonelab;
using MelonLoader;
using UnityEngine;
using UnityEngine.Rendering;
using BoneLib;
using System.Collections;

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
        public static Page SmoothingPage { get; private set; }
        public static Page RotationOffsetPage { get; private set; }
        public static Page PositionOffsetPage { get; private set; }
        public static Page SupportPage { get; private set; }

        //BoneMenu Elements

        public static FunctionElement getCameraButton { get; private set; }
        public static FloatElement fovSlider { get; private set; }
        public static FunctionElement setFovButton { get; private set; }
        public static FunctionElement resetAllButton { get; private set; }
        public static BoolElement PostSFXToogle { get; private set; }
        public static FloatElement P_Smoothing { get; private set; }
        public static FloatElement R_Smoothing { get; private set; }
        public static FunctionElement ApplySmoothingButton { get; private set; }
        public static FloatElement X_R_Offset { get; private set; }
        public static FloatElement Y_R_Offset { get; private set; }
        public static FloatElement Z_R_Offset { get; private set; }
        public static FunctionElement Apply_R_OffsetButton { get; private set; }
        public static FloatElement X_P_Offset { get; private set; }
        public static FloatElement Y_P_Offset { get; private set; }
        public static FloatElement Z_P_Offset { get; private set; }
        public static FunctionElement Apply_P_OffsetButton { get; private set; }

        //Needed GameObjects and Components

        private static GameObject SC_GameObject;
        private static GameObject ST_GameObject;
        private static Camera SC_CameraComponent;
        private static SmoothFollower SC_SmootherComponent;
        private static Transform ST_Transform;
        private static Volume SC_VolumeComponent;

        //MelonPref

        private static MelonPreferences_Category Pref_WideEye;
        private static MelonPreferences_Entry<float> Pref_Fov;
        private static MelonPreferences_Entry<bool> Pref_PostSFX;
        private static MelonPreferences_Entry<Vector3> Pref_RotationOffset;
        private static MelonPreferences_Entry<Vector3> Pref_PositionOffset;
        private static MelonPreferences_Entry<float> Pref_RotationSmoothing;
        private static MelonPreferences_Entry<float> Pref_PositionSmoothing;


        //Variables

        public enum OffsetType { Position, Rotation }
        public enum ResetType { Fov, Smoothing, RotationOffset, PostionOffset, All }
        private static int defaultFOV = 75;
        private static bool gotcamera = false;
        private static bool AutoFind = true;

        //MelonLoader Events

        public override void OnInitializeMelon()
        {
            LoggerInstance.Msg("WideEye 1.3.0 Has Been Initialized.");
            SetupBoneMenu();
            Menu.OnPageOpened += OnPageOpened;
            Hooking.OnLevelUnloaded += BoneLib_OnLevelUnloaded;
            Hooking.OnUIRigCreated += BoneLib_OnUIRigCreated;

            //Pref
            Pref_WideEye = MelonPreferences.CreateCategory("WideEye");
            Pref_Fov = Pref_WideEye.CreateEntry<float>("Fov", 75);
            Pref_PostSFX = Pref_WideEye.CreateEntry<bool>("PostSFX", true);
            Pref_RotationOffset = Pref_WideEye.CreateEntry<Vector3>("RotationOffset", new(11,0,0));
            Pref_PositionOffset = Pref_WideEye.CreateEntry<Vector3>("PositionOffset", Vector3.zero);
            Pref_RotationSmoothing = Pref_WideEye.CreateEntry<float>("RotationSmoothing", 0);
            Pref_PositionSmoothing = Pref_WideEye.CreateEntry<float>("PositionSmoothing", 0);
        }

        private void BoneLib_OnUIRigCreated()
        {
            MelonCoroutines.Start(WaitForRig());
        }

        private void BoneLib_OnLevelUnloaded()
        {
            mainPage.Remove([fovSlider, setFovButton, resetAllButton, PostSFXToogle]);
            RotationOffsetPage.RemoveAll();
            PositionOffsetPage.RemoveAll();
            SmoothingPage.RemoveAll();
            //Menu.DestroyPage(SmoothingPage);
            //Menu.DestroyPage(RotationOffsetPage);       
            
            getCameraButton.ElementColor = Color.red;
            gotcamera = false;
        }

        //Custom Functions

        public static void SavePref()
        {
            Pref_Fov.Value = fovSlider.Value;
            Pref_PostSFX.Value = PostSFXToogle.Value;
            Pref_RotationOffset.Value = new(X_R_Offset.Value, Y_R_Offset.Value, Z_R_Offset.Value);
            Pref_PositionOffset.Value = new(X_P_Offset.Value, Y_P_Offset.Value, Z_P_Offset.Value);
            Pref_RotationSmoothing.Value = R_Smoothing.Value;
            Pref_PositionSmoothing.Value = P_Smoothing.Value;
            Pref_WideEye.SaveToFile();
        }


        public static void LoadPref()
        {
            SetFOV(Pref_Fov.Value, true, fovSlider);
            PostSFXToogle.Value = Pref_PostSFX.Value;
            ApplyOffset(Pref_RotationOffset.Value, OffsetType.Rotation, true, X_R_Offset, Y_R_Offset, Z_R_Offset);
            ApplyOffset(Pref_PositionOffset.Value, OffsetType.Position, true, X_P_Offset, Y_P_Offset, Z_P_Offset);
            ApplySmoothing(Pref_RotationSmoothing.Value, Pref_PositionSmoothing.Value, true, R_Smoothing, P_Smoothing);
        }

        private static IEnumerator WaitForRig()
        {
            yield return new WaitForSeconds(5f);
            GetTargetCamera();
        }

        public static void SendNotfi(string Title, string Message, NotificationType Type, float PopupLength, bool showTitleOnPopup)
        {
            var notfi = new Notification
            {
                Title = Title,
                Message = Message,
                Type = Type,
                PopupLength = PopupLength,
                ShowTitleOnPopup = showTitleOnPopup
            };
            Notifier.Send(notfi);
        }

        public static void GetTargetCamera()
        {
            SC_GameObject = GameObject.Find("GameplaySystems [0]/DisabledContainer/Spectator Camera/Spectator Camera");
            ST_GameObject = GameObject.Find("RigManager(bonelab) [0]/VRControllerRig/TrackingSpace/Headset/Spectator Target");

            if (SC_GameObject == null || ST_GameObject == null)
            {
                SendNotfi("Error", "Couldn't find the camera automatically.\nPlease open WideEye's menu and find it manually.", NotificationType.Error, 3, true);
                MelonLogger.Error("Couldn't find the camera automatically");
            }
            else if (!gotcamera)
            {
                if (!SC_GameObject.active)
                {
                    SendNotfi("Warning", "Spectator Camera Is Not Active.\nModifications will not take action.", NotificationType.Warning, 5, true);
                    MelonLogger.Warning("Spectator Camera Is Not Active. Modifications will not take action.");
                }
                ST_Transform = ST_GameObject.GetComponent<Transform>();
                SC_SmootherComponent = SC_GameObject.GetComponent<SmoothFollower>();
                SC_VolumeComponent = SC_GameObject.GetComponent<Volume>();
                SC_CameraComponent = SC_GameObject.GetComponent<Camera>();

                RefreshBoneMenu();
                LoadPref();
                SendNotfi("Scusses", "Found camera automatically", NotificationType.Success, 3, true);
                MelonLogger.Msg(System.ConsoleColor.Green, "Found camera automatically");
                gotcamera = true;  
            }
        }

        public static void SetFOV(float fov, bool SyncElementValue = false, FloatElement FovEle =null)
        {
            SC_CameraComponent.fieldOfView = fov;
            if (SyncElementValue)
            {
                FovEle.Value = fov;
            }
        }

        public static void ResetToDefault(ResetType resetType)
        {
            switch (resetType)
            {
                case ResetType.Fov:
                    SetFOV(75, true, fovSlider);
                    break;

                case ResetType.Smoothing:
                    ApplySmoothing(0, 0, true, R_Smoothing, P_Smoothing);
                    break;

                case ResetType.RotationOffset:
                    ApplyOffset(new(11, 0, 0), OffsetType.Rotation, true, X_R_Offset, Y_R_Offset, Z_R_Offset);
                    break;
                
                case ResetType.PostionOffset:
                    ApplyOffset(new(0, 0, 0), OffsetType.Position, true, X_P_Offset, Y_P_Offset, Z_P_Offset);
                    break;

                case ResetType.All:
                    SetFOV(75, true, fovSlider);
                    ApplySmoothing(0, 0, true, R_Smoothing, P_Smoothing);
                    ApplyOffset(new(11, 0, 0), OffsetType.Rotation, true, X_R_Offset, Y_R_Offset, Z_R_Offset);
                    ApplyOffset(new(0, 0, 0), OffsetType.Position, true, X_P_Offset, Y_P_Offset, Z_P_Offset);
                    SC_VolumeComponent.enabled = true;
                    PostSFXToogle.Value = true;
                    break;
            }

        }
        public static void ApplyOffset(Vector3 Offset, OffsetType offsetType, bool SyncElementValue = false, FloatElement EleX = null, FloatElement EleY = null, FloatElement EleZ = null)
        { 
            switch (offsetType)
            {
                case OffsetType.Position:
                    ST_Transform.localPosition = Offset;
                    break;

                case OffsetType.Rotation:
                    ST_Transform.localRotation = Quaternion.Euler(Offset);
                    break;
            }
            if (SyncElementValue)
            {
                EleX.Value = Offset.x;
                EleY.Value = Offset.y;
                EleZ.Value = Offset.z;
            }
        }


        public static void ApplySmoothing(float RotationSmoothingValue, float PositionSmoothingValue, bool SyncElementValue = false, FloatElement R_SmoothingEle = null, FloatElement P_SmoothingEle = null)
        {
            SC_SmootherComponent.RotationalSmoothTime = RotationSmoothingValue;
            SC_SmootherComponent.TranslationSmoothTime = PositionSmoothingValue;
            if (SyncElementValue)
            {
                R_SmoothingEle.Value = RotationSmoothingValue;
                P_SmoothingEle.Value = PositionSmoothingValue;
            }
        }

        public static void OnPageOpened(Page page)
        {
            if (page == RotationOffsetPage & page == PositionOffsetPage  & page == SmoothingPage & !gotcamera)
            {
                Menu.DisplayDialog("Error", "Camera Is Not Found.\nPlease click on the 'Find Camera' Button", Dialog.ErrorIcon, () => Menu.OpenPage(mainPage));
            }
        }

        //BoneMenu Creation And Setup

        public static void RefreshBoneMenu()
        {
            //Main Page
            mainPage.Remove(getCameraButton);
            fovSlider = mainPage.CreateFloat("FOV", Color.white, defaultFOV, 5, float.MinValue, float.MaxValue, (value) => SetFOV(value));
            PostSFXToogle = mainPage.CreateBool("Post-Processing", Color.yellow, true, isEnabled =>
            { 
                if (isEnabled) SC_VolumeComponent.enabled = true;
                else SC_VolumeComponent.enabled = false;
            });
            mainPage.Add(new FunctionElement("Save Settings", Color.green, SavePref));
            resetAllButton = mainPage.CreateFunction("Reset To Default", Color.red, () => ResetToDefault(ResetType.All));


            //Rotation Offset Page
            RotationOffsetPage = mainPage.CreatePage("Rotation Offset", Color.white);
            X_R_Offset = RotationOffsetPage.CreateFloat("X Rotation Offset", Color.red, 11, 0.1f, float.MinValue, float.MaxValue, null);
            Y_R_Offset = RotationOffsetPage.CreateFloat("Y Rotation Offset", Color.green, 0, 0.1f, float.MinValue, float.MaxValue, null);
            Z_R_Offset = RotationOffsetPage.CreateFloat("Z Rotation Offset", Color.blue, 0, 0.1f, float.MinValue, float.MaxValue, null);
            Apply_R_OffsetButton = RotationOffsetPage.CreateFunction("Apply Rotation Offset", Color.white, () => ApplyOffset(new(X_R_Offset.Value, Y_R_Offset.Value, Z_R_Offset.Value), OffsetType.Rotation));
            RotationOffsetPage.Add(new FunctionElement("Reset To Default", Color.red, () => ResetToDefault(ResetType.RotationOffset)));

            //Position Offset Page
            PositionOffsetPage = mainPage.CreatePage("Position Offset", Color.white);
            X_P_Offset = PositionOffsetPage.CreateFloat("X Position Offset", Color.red, 11, 0.1f, float.MinValue, float.MaxValue, null);
            Y_P_Offset = PositionOffsetPage.CreateFloat("Y Position Offset", Color.green, 0, 0.1f, float.MinValue, float.MaxValue, null);
            Z_P_Offset = PositionOffsetPage.CreateFloat("Z Position Offset", Color.blue, 0, 0.1f, float.MinValue, float.MaxValue, null);
            Apply_R_OffsetButton = PositionOffsetPage.CreateFunction("Apply Position Offset", Color.white, () => ApplyOffset(new(X_P_Offset.Value, Y_P_Offset.Value, Z_P_Offset.Value), OffsetType.Position));
            PositionOffsetPage.Add(new FunctionElement("Reset To Default", Color.red, () => ResetToDefault(ResetType.PostionOffset)));


            //Smoothing Page
            SmoothingPage = mainPage.CreatePage("Smoothing", Color.white);
            P_Smoothing = SmoothingPage.CreateFloat("Position Smoothing", Color.white, 0, 1, float.MinValue, int.MaxValue, null);
            R_Smoothing = SmoothingPage.CreateFloat("Rotation Smoothing", Color.white, 0, 1, float.MinValue, int.MaxValue, null);
            ApplySmoothingButton = SmoothingPage.CreateFunction("Apply Values", Color.white, () => ApplySmoothing(R_Smoothing.Value, P_Smoothing.Value));
            SmoothingPage.Add(new FunctionElement("Reset To Default", Color.red, () => ResetToDefault(ResetType.Smoothing)));
        }

        public static void SetupBoneMenu()
        {
            mainPage = Page.Root.CreatePage("Wide Eye",Color.white);
            getCameraButton = mainPage.CreateFunction("Find Camera", Color.red, GetTargetCamera);
            SupportPage = mainPage.CreatePage("Support", Color.white); 
            SupportPage = mainPage.CreatePage("Support", Color.white); 

            SupportPage.Add(new FunctionElement("Open GitHub Issues", Color.white, () =>
            {
                Application.OpenURL("https://github.com/HL2H0/WideEye/issues");
                SendNotfi("Success", "Opened the GitHub issues page for WideEye On Desktop", NotificationType.Success, 2, true);
            }));

            SupportPage.Add(new FunctionElement("Discord", Color.blue, () =>
            {
                GUIUtility.systemCopyBuffer = "@hiiiiiiiiiiiiiiiiii";
                SendNotfi("Success", "Copied Username to clipboard", NotificationType.Success, 3, true);
            }));

            SupportPage.Add(new FunctionElement("Version : 1.3.0", Color.white, null));
        }
    }
}