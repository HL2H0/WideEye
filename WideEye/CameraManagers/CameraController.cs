using BoneLib;
using BoneLib.Notifications;
using WideEye.Core;
using WideEye.UI;
using WideEye.Utilities;

namespace WideEye.CameraManagers;

public static class CameraController
{
    public static ModEnums.ViewMode ActiveViewMode;
    
    public static void UpdateView(ModEnums.ViewMode viewMode, bool syncElements = false)
    {
        switch (viewMode)
        {
            case ModEnums.ViewMode.Head:
                SetViewHead();
                break;
            case ModEnums.ViewMode.Handheld:
                SetViewHandheld();
                break;
            case ModEnums.ViewMode.FreeCam:
                SetViewFreeCam();
                break;
        }

        if (syncElements)
            ModMenu.ViewMode.Value = viewMode;
    }

    private static void SetViewHead()
    {
        Mod.ScSmootherComponent.targetTransform = Mod.StTransform;
        if(TimelineHelper.UsingTimeline) TimelineHelper.TimelineSmoothFollower.targetTransform = Mod.StTransform;
        if (HandheldCameraManager.Registered) HandheldCameraManager.ActiveScript.SyncCamera = null;
        SettingsUpdater.UpdateFOV(ModMenu.FOVSlider.Value);
        FreeCamManager.FreeCamObject.SetActive(false);
        
        ActiveViewMode = ModEnums.ViewMode.Head;
    }

    private static void SetViewHandheld()
    {
        // HandheldCameraManager.FindHandheldCamera();
        if (!HandheldCameraManager.Registered) 
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
                
        HandheldCameraManager.ActiveScript.SyncCamera = Mod.ScCameraComponent;
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
        
        ActiveViewMode = ModEnums.ViewMode.Handheld;
    }
    
    private static void SetViewFreeCam()
    {
        Mod.ScSmootherComponent.targetTransform = FreeCamManager.FreeCamObject.transform;
        if (HandheldCameraManager.Registered) HandheldCameraManager.ActiveScript.SyncCamera = null;
        FreeCamManager.FreeCamObject.transform.position = Player.Head.position;
        FreeCamManager.FreeCamObject.transform.rotation = Player.Head.rotation;
        FreeCamManager.FreeCamObject.SetActive(true);
        
        ActiveViewMode = ModEnums.ViewMode.FreeCam;
    }

    public static void UpdateAudioSource(ModEnums.AudioSource audioSource, bool syncElements = false)
    {
        switch (audioSource)
        {
            case ModEnums.AudioSource.Head:
                if (HandheldCameraManager.Registered) HandheldCameraManager.ActiveScript.audioListener.enabled = false;
                break;
            case ModEnums.AudioSource.Handheld:
                if (HandheldCameraManager.Registered) HandheldCameraManager.ActiveScript.audioListener.enabled = true;
                break;
        }
        if (syncElements)
            ModMenu.AudioSource.Value = audioSource;
    }
}