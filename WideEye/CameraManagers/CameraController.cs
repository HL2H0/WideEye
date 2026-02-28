using BoneLib;
using BoneLib.Notifications;
using WideEye.Core;
using WideEye.UI;
using WideEye.Utilities;

namespace WideEye.CameraManagers;

public static class CameraController
{
    public static void UpdateView(ModEnums.ViewMode viewMode, bool syncElements = false)
    {
        switch (viewMode)
        {
            case ModEnums.ViewMode.Head:
                Mod.ScSmootherComponent.targetTransform = Mod.StTransform;
                if(TimelineHelper.UsingTimeline) TimelineHelper.TimelineSmoothFollower.targetTransform = Mod.StTransform;
                if (HandheldCameraManager.Found) HandheldCameraManager.ActiveScript.SyncCamera = null;
                SettingsUpdater.UpdateFOV(ModMenu.FOVSlider.Value);
                FreeCamManager.FreeCamObject.SetActive(false);
                break;
            
            case ModEnums.ViewMode.Handheld:
                HandheldCameraManager.FindHandheldCamera();
                if (!HandheldCameraManager.Found) 
                {
                    Notifier.Send(new Notification
                    {
                        Title = "WideEye | Error",
                        Message = "Handheld camera not spawned\nPlease spawn the camera first",
                        Type = NotificationType.Error,
                        PopupLength = 3,
                        ShowTitleOnPopup = true
                    });
                }
                
                if (TimelineHelper.UsingTimeline)
                {
                    TimelineHelper.TimelineSmoothFollower.targetTransform = HandheldCameraManager.ActiveScript.cameraTarget;
                    HandheldCameraManager.ActiveScript.SyncCamera = TimelineHelper.TimelineCamera.gameObject.active
                        ? TimelineHelper.TimelineCamera
                        : Mod.ScCameraComponent;
                }
                Mod.ScSmootherComponent.targetTransform = HandheldCameraManager.ActiveScript.cameraTarget;
                SettingsUpdater.UpdateFOV(HandheldCameraManager.ActiveScript.FOV);
                FreeCamManager.FreeCamObject.SetActive(false);
                break;
            
            case ModEnums.ViewMode.FreeCam:
                Mod.ScSmootherComponent.targetTransform = FreeCamManager.FreeCamObject.transform;
                if (HandheldCameraManager.Found) HandheldCameraManager.ActiveScript.SyncCamera = null;
                FreeCamManager.FreeCamObject.transform.position = Player.Head.position;
                FreeCamManager.FreeCamObject.transform.rotation = Player.Head.rotation;
                FreeCamManager.FreeCamObject.SetActive(true);
                break;
        }

        if (syncElements)
            ModMenu.ViewMode.Value = viewMode;
    }

    public static void UpdateAudioSource(ModEnums.AudioSource audioSource, bool syncElements = false)
    {
        switch (audioSource)
        {
            case ModEnums.AudioSource.Head:
                if (HandheldCameraManager.Found) HandheldCameraManager.ActiveScript.audioListener.enabled = false;
                break;
            case ModEnums.AudioSource.Handheld:
                if (HandheldCameraManager.Found) HandheldCameraManager.ActiveScript.audioListener.enabled = true;
                break;
        }
        if (syncElements)
            ModMenu.AudioSource.Value = audioSource;
    }
}