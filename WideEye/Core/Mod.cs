using System.Collections;

using Il2CppOccaSoftware.Exposure.Runtime;
using Il2CppSLZ.Bonelab;
using Il2CppSLZ.Marrow;

using MelonLoader;
using BoneLib;
using BoneLib.Notifications;
using Il2CppMK.Glow.URP;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using WideEye.CameraManagers;
using WideEye.Core;
using WideEye.Data;
using WideEye.UI;
using WideEye.Utilities;
using BuildInfo = WideEye.Core.BuildInfo;


[assembly: MelonInfo(typeof(Mod), "WideEye", BuildInfo.Version, "HL2H0")]
[assembly: MelonGame("Stress Level Zero", "BONELAB")]

namespace WideEye.Core
{

    public class Mod : MelonMod
    {
        //Needed GameObjects and Components

        public static PlayerAvatarArt PlayerArtComponent;
        public static GameObject ScGameObject;
        public static GameObject StGameObject;
        public static Camera ScCameraComponent;
        public static SmoothFollower ScSmootherComponent;
        public static Transform StTransform;
        public static Volume ScVolumeComponent;
        

        //Post-FX Overrides
        public static MKGlow MKGlowOverride;
        public static LensDistortion LensDistortionOverride;
        public static ChromaticAberration ChromaticAberrationOverride;
        public static AutoExposure AutoExposureOverride;

        //Variables
        public static bool FoundCamera;

        //MelonLoader & BoneLib Events
        public override void OnInitializeMelon()
        {
            Paths.InitFolders();
            ResourcesManager.Init();
            ModPreferences.CreatePreferences();
            PresetsManager.LoadPresets();
            ModMenu.SetupBoneMenu();
            Hooking.OnLevelUnloaded += BoneLib_OnLevelUnloaded;
            Hooking.OnUIRigCreated += BoneLib_OnUIRigCreated;
            LoggerInstance.Msg($"WideEye {BuildInfo.Version} Has Been Initialized.");
        }

        public override void OnUpdate()
        {
            
        }

        private void BoneLib_OnUIRigCreated()
        {
            MelonCoroutines.Start(StartWideEye(ModPreferences.StartupDelay));
            MenuShortcut.CreateShortcut();
            MelonLogger.Msg(System.ConsoleColor.Green, "UI Rig Created, Trying To Get Camera...");
        }
        
        private static IEnumerator StartWideEye(float waitTime)
        {
            yield return new WaitForSeconds(waitTime);
            
            SpectatorCameraManager.GetSpectatorCamera(true);
            FreeCamManager.Init();
            ModPreferences.LoadPreferences();
            ResourcesManager.CheckPallet();

            TimelineHelper.StartHelper();
            
            if (!ResourcesManager.Loaded || !ResourcesManager.PalletInstalled) yield break;
            if(ModPreferences.HideNonErrorNotification) yield break;
            Notifier.Send(new Notification
            {
                Title = "WideEye | Success",
                Message = "WideEye Has Launched Without errors (=",
                Type = NotificationType.Success,
                PopupLength = 2,
                ShowTitleOnPopup = true
            });
        }

        private void BoneLib_OnLevelUnloaded()
        {
            FoundCamera = false;
            if (TimelineHelper.UsingTimeline)
            {
                TimelineHelper.UsingTimeline = false;
                TimelineHelper.ResetReferences();
            }
            if (!HandheldCameraManager.Found) return;
            HandheldCameraManager.ActiveHandheldCamera = null;
            ModMenu.AudioSource.Value = ModEnums.AudioSource.Head;
            ModMenu.ViewMode.Value = ModEnums.ViewMode.Head;
        }
    }
}