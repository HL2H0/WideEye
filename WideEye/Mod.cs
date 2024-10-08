﻿using BoneLib;
using BoneLib.BoneMenu;
using Page = BoneLib.BoneMenu.Page;
using BoneLib.Notifications;
using Il2CppSLZ.Bonelab;
using MelonLoader;
using UnityEngine;
using UnityEngine.Rendering;

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
        public static FunctionElement resetAllButton { get; private set; }
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
        private static GameObject ST_GameObject;
        private static Camera SC_CameraComponent;
        private static SmoothFollower SC_SmootherComponent;
        private static Transform ST_Transform;
        private static Volume SC_VolumeComponent;
        
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

            mainPage.Remove([fovSlider, setFovButton, resetAllButton, PostSFXToogle]);
            ExperimentalPage.RemoveAll();
            getCameraButton.ElementColor = Color.red;
            gotcamera = false;
        }


        //Custom Functions

        public static void GetTargetCamera()
        {
            SC_GameObject = GameObject.Find("GameplaySystems [0]/DisabledContainer/Spectator Camera/Spectator Camera");
            ST_GameObject = GameObject.Find("RigManager(bonelab) [0]/VRControllerRig/TrackingSpace/Headset/Spectator Target");

            List<object> objectsToCheck =
            [
                SC_GameObject,
                ST_GameObject
            ];

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
                ST_Transform = ST_GameObject.GetComponent<Transform>();
                SC_SmootherComponent = SC_GameObject.GetComponent<SmoothFollower>();
                SC_VolumeComponent = SC_GameObject.GetComponent<Volume>();
                SC_CameraComponent = SC_GameObject.GetComponent<Camera>();
                if (!gotcamera)
                {
                    RefreshBoneMenu();
                    gotcamera = true;
                    var notifi = new Notification
                    {
                        Title = "Success",
                        Message = "Camera Found !",
                        Type  = NotificationType.Success,
                        PopupLength = 2
                    };
                    Notifier.Send(notifi);
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

        public static void ResetAll()
        {
            //Reset Fov
            fovSlider.Value = 70;
            SC_CameraComponent.fieldOfView = defaultFOV;

            //Reset Rotation Offset
            ST_Transform.localRotation = Quaternion.Euler(0, 10, 0);
            X_R_Offset.Value = 11;
            Y_R_Offset.Value = 0;
            Z_R_Offset.Value = 0;

            //Reset Position And Rotation Smoothing
            SC_SmootherComponent.TranslationSmoothTime = 0;
            SC_SmootherComponent.RotationalSmoothTime = 0.07f;
            PositionSmoothing.Value = 0;
            RotationSmoothing.Value = 0;

            //Reset Post-Processing
            SC_VolumeComponent.enabled = true;
            PostSFXToogle.Value = true;

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
            if (page == ExperimentalPage & !gotcamera)
            {
                Menu.DisplayDialog("Error", "Camera Is Not Found, Please click on the 'Find Camera Button'", Dialog.ErrorIcon, () => Menu.OpenPage(mainPage), null);
            }
        }

        //BoneMenu Creation And Setup

        public static void RefreshBoneMenu()
        {
            //Main Page
            getCameraButton.ElementColor = Color.green;
            fovSlider = mainPage.CreateInt("FOV", Color.white, defaultFOV, 10, int.MinValue, int.MaxValue, null);
            setFovButton = mainPage.CreateFunction("Set FOV", Color.green, SetCameraFOV);
            resetAllButton = mainPage.CreateFunction("Reset All", Color.red, ResetAll);
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
            mainPage = Page.Root.CreatePage("Wide Eye", Color.white);

            SupportPage = mainPage.CreatePage("Support", Color.white); 
            FunctionElement githubButton = new("Open GitHub Issues", Color.white, () =>
            {
                Application.OpenURL("https://github.com/HL2H0/WideEye/issues");
                var notifi = new Notification
                {
                    Title = "Success",
                    Message = "Opened the GitHub issues page for WideEye",
                    Type = NotificationType.Success,
                    PopupLength = 2
                };
                Notifier.Send(notifi);
            });
            SupportPage.Add(githubButton);

            SupportPage.Add(new FunctionElement("Discord", Color.blue, () =>
            {
                var notifi = new Notification
                {
                    Title = "Success",
                    Message = "Copied Username to clipboard",
                    Type = NotificationType.Success,
                    PopupLength = 3
                };
                Notifier.Send(notifi);
                GUIUtility.systemCopyBuffer = "@hiiiiiiiiiiiiiiiiii";
            }));
            ExperimentalPage = mainPage.CreatePage("Experimental", Color.yellow);
            getCameraButton = mainPage.CreateFunction("Find Camera", Color.red, GetTargetCamera);
        }
    }
}