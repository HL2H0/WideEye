using BoneLib;
using BoneLib.Notifications;
using Il2CppSLZ.Bonelab;
using Il2CppSLZ.Marrow;
using MelonLoader;
using UnityEngine;
using UnityEngine.Rendering; 
using WideEye.Core;
using WideEye.UI;

namespace WideEye.CameraManagers;
public static class SpectatorCameraManager
{   
    public static void GetSpectatorCamera(bool isAuto)
    {
        if (HelperMethods.IsAndroid())
        {
            Notifier.Send(new Notification
            {
                Title = "WideEye | Error",
                Message = "WideEye doesn't work with Quest",
                Type = NotificationType.Error,
                PopupLength = 3,
                ShowTitleOnPopup = true
            });
            
            Melon<Mod>.Logger.Error("WideEye doesn't work with Quest");
            return;
        }

        if (Mod.FoundCamera) return;
        
        Mod.ScGameObject = GameObject.Find("GameplaySystems [0]/DisabledContainer/Spectator Camera/Spectator Camera");
        Mod.StGameObject = GameObject.Find("RigManager(bonelab) [0]/VRControllerRig/TrackingSpace/Headset/Spectator Target");

        if (!Mod.ScGameObject || !Mod.StGameObject)
        {
            var message = isAuto 
                ? "Couldn't find the camera automatically.\nYou can use the manual button in the settings"
                : "Couldn't find the camera.\nPlease use the manual button ";
            
            Notifier.Send(new Notification
            {
                Title = "WideEye | Error",
                Message = message,
                Type = NotificationType.Error,
                PopupLength = 5,
                ShowTitleOnPopup = true
            });
            Melon<Mod>.Logger.Error(isAuto ? "Couldn't find the camera automatically" : "Couldn't find the camera");
            return;
        }

        if (!Mod.FoundCamera)
        {
            if (!Mod.ScGameObject.active)
            {
                Notifier.Send(new Notification
                {
                    Title = "WideEye | Warning",
                    Message = "Spectator Camera is not active.\nChange the spectator mode to \"Fish Eye\" ",
                    Type = NotificationType.Warning,
                    PopupLength = 5,
                    ShowTitleOnPopup = true
                });
                Melon<Mod>.Logger.Warning("Spectator Mode isn't set to \"Fish Eye\"Change it so WideEye can work");
            }

            InitializeComponents();
            HandleCameraFound(isAuto);
            Mod.FoundCamera = true;
        }
    }

    private static void InitializeComponents()
    {
        Mod.PlayerArtComponent = Player.ControllerRig.gameObject.GetComponent<PlayerAvatarArt>();
        Mod.StTransform = Mod.StGameObject.GetComponent<Transform>();
        Mod.ScSmootherComponent = Mod.ScGameObject.GetComponent<SmoothFollower>();
        Mod.ScVolumeComponent = Mod.ScGameObject.GetComponent<Volume>();
        Mod.ScCameraComponent = Mod.ScGameObject.GetComponent<Camera>();
        Mod.ScVolumeComponent.sharedProfile.TryGet(out Mod.MKGlowOverride);
        Mod.ScVolumeComponent.sharedProfile.TryGet(out Mod.LensDistortionOverride);
        Mod.ScVolumeComponent.sharedProfile.TryGet(out Mod.ChromaticAberrationOverride);
        Mod.ScVolumeComponent.sharedProfile.TryGet(out Mod.AutoExposureOverride);
    }

    private static void HandleCameraFound(bool isAuto)
    {
        
        if (!isAuto)
        {
            var notification = new Notification
            {
                Title = "WideEye | Success",
                Message = "Found camera manually",
                Type = NotificationType.Success,
                PopupLength = 3,
                ShowTitleOnPopup = true
            };
            Notifier.Send(notification);
        }
        
        Melon<Mod>.Logger.Msg(ConsoleColor.Green, isAuto ? "Found camera automatically" : "Found camera manually");
    }
}