using System.Collections;

using Il2CppOccaSoftware.Exposure.Runtime;
using Il2CppSLZ.Bonelab;
using Il2CppSLZ.Marrow;

using MelonLoader;
using BoneLib;
using BoneLib.Notifications;
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

        public static PlayerAvatarArt RmPlayerArtComponent;
        public static GameObject ScGameObject;
        public static GameObject StGameObject;
        public static Camera ScCameraComponent;
        public static SmoothFollower ScSmootherComponent;
        public static Transform StTransform;
        public static Volume ScVolumeComponent;
        

        //Post-FX Overrides
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
            ModPreferences.CreatePref();
            PresetsManager.LoadPresets();
            ModMenu.SetupBoneMenu();
            Hooking.OnLevelUnloaded += BoneLib_OnLevelUnloaded;
            Hooking.OnUIRigCreated += BoneLib_OnUIRigCreated;
            LoggerInstance.Msg($"WideEye {BuildInfo.Version} Has Been Initialized.");
        }

        private void BoneLib_OnUIRigCreated()
        {
            MelonCoroutines.Start(StartWideEye(ModPreferences.StartupDelay));
            MelonLogger.Msg(System.ConsoleColor.Green, "UI Rig Created, Trying To Get Camera...");
        }
        
        private static IEnumerator StartWideEye(float waitTime)
        {
            yield return new WaitForSeconds(waitTime);
            
            SpectatorCameraManager.GetSpectatorCamera(true);
            
            FreeCamManager.Init();
            ModPreferences.LoadPref();
            if (!ResourcesManager.Loaded)
            {
                var notification = new ModNotification(ModNotification.ModNotificationType.Force, "Error",
                    "Mod resources isn't loaded correctly or couldn't be found.", NotificationType.Error, 5);
                notification.Show();
                MelonLogger.Error("Mod resources isn't loaded correctly or couldn't be found.");
            }
            else
            {
                var notification = new ModNotification(ModNotification.ModNotificationType.CameraFound, "Success",
                    "WideEye Has Launched Without errors (=", NotificationType.Success, 2);
                notification.Show();
            }
        }

        private void BoneLib_OnLevelUnloaded()
        {
            FoundCamera = false;
            if (!HandheldCameraManager.Spawned) return;
            GameObject.Destroy(HandheldCameraManager.HandheldCamera);
            HandheldCameraManager.HandheldCamera = null;
            ModMenu.ViewMode.Value = ModEnums.ViewMode.Head;
        }
    }
}