using BoneLib;
using BoneLib.Notifications;
using Il2CppSLZ.Bonelab;
using Il2CppSLZ.Marrow;
using MelonLoader;
using UnityEngine;
using UnityEngine.Rendering;

using WideEye.Core;
using WideEye.Data;
using WideEye.UI;

namespace WideEye.CameraManagers;
public static class SpectatorCameraManager
{   
    public static void GetSpectatorCamera(bool isAuto)
    {
        if (HelperMethods.IsAndroid())
        {
            var notification = new ModNotification(ModNotification.ModNotificationType.Force, "WideEye | Error", "WideEye doesn't work with Android", NotificationType.Error, 3f);
            notification.Show();
        }
        else
        {
            Mod.ScGameObject = GameObject.Find("GameplaySystems [0]/DisabledContainer/Spectator Camera/Spectator Camera");
            Mod.StGameObject = GameObject.Find("RigManager(bonelab) [0]/VRControllerRig/TrackingSpace/Headset/Spectator Target");

            if (!Mod.ScGameObject || !Mod.StGameObject)
            {
                if (isAuto)
                {
                    var notification = new ModNotification(ModNotification.ModNotificationType.Force, "WideEye | Error", "Couldn't find the camera automatically.\nPlease open WideEye's menu and find it manually.", NotificationType.Error, 3);
                    notification.Show();
                    MelonLogger.Error("Couldn't find the camera automatically");
                    ModMenu.MainPage.Add(ModMenu.GetCameraButton);
                }
                else
                {
                    var notification = new ModNotification(ModNotification.ModNotificationType.Force, "WideEye | Error", "Couldn't find the camera.", NotificationType.Error, 3);
                    notification.Show();
                    MelonLogger.Error("Couldn't find the camera");
                }
            }
            else if (!Mod.FoundCamera)
            {
                if (!Mod.ScGameObject.active)
                {
                    var notification = new ModNotification(ModNotification.ModNotificationType.CameraDisabled, "WideEye | Warning", "Spectator Camera Is Not Active.\nModifications will not take action.", NotificationType.Warning, 3);
                    notification.Show();
                    MelonLogger.Warning("Spectator Camera Is Not Active. Modifications will not take action.");
                }

                Mod.RmPlayerArtComponent = Player.ControllerRig.gameObject.GetComponent<PlayerAvatarArt>();
                Mod.StTransform = Mod.StGameObject.GetComponent<Transform>();
                Mod.ScSmootherComponent = Mod.ScGameObject.GetComponent<SmoothFollower>();
                Mod.ScVolumeComponent = Mod.ScGameObject.GetComponent<Volume>();
                Mod.ScCameraComponent = Mod.ScGameObject.GetComponent<Camera>();
                GetPostFXOverrides();
                if (isAuto)
                {
                    var notification = new ModNotification(ModNotification.ModNotificationType.CameraFound, "WideEye | Success", "Found camera automatically", NotificationType.Success, 3);
                    notification.Show();
                    MelonLogger.Msg(ConsoleColor.Green, "Found camera automatically");
                }
                else
                {
                    ModMenu.MainPage.Remove(ModMenu.GetCameraButton);
                    var notification = new ModNotification(ModNotification.ModNotificationType.CameraFound, "WideEye | Success", "Found camera manually", NotificationType.Success, 3);
                    notification.Show();
                    MelonLogger.Msg(ConsoleColor.Green, "Found camera manually");
                }

                ModPreferences.LoadPref();
                Mod.FoundCamera = true;
            }
        }
    }

    private static void GetPostFXOverrides()
    {
        Mod.ScVolumeComponent.profile.TryGet(out Mod.LensDistortionOverride);
        Mod.ScVolumeComponent.profile.TryGet(out Mod.ChromaticAberrationOverride);
        Mod.ScVolumeComponent.profile.TryGet(out Mod.AutoExposureOverride);
    }
}
