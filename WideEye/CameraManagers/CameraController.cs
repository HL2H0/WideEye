using BoneLib;
using WideEye.Core;

namespace WideEye.CameraManagers;

public static class CameraController
{
    public static void UpdateView(ModEnums.ViewMode viewMode)
    {
        
        switch (viewMode)
        {
            case ModEnums.ViewMode.Head:
                Mod.ScSmootherComponent.targetTransform = Mod.StTransform;
                if (HandheldCameraManager.Spawned) HandheldCameraManager.ActiveHandheldCameraScript.SyncCamera = null;
                FreeCamManager.FreeCamObject.SetActive(false);
                break;
            
            case ModEnums.ViewMode.Handheld:
                if(!HandheldCameraManager.Spawned) return;
                Mod.ScSmootherComponent.targetTransform = HandheldCameraManager.ActiveHandheldCameraScript.cameraTarget;
                FreeCamManager.FreeCamObject.SetActive(false);
                break;
            
            case ModEnums.ViewMode.FreeCam:
                Mod.ScSmootherComponent.targetTransform = FreeCamManager.FreeCamObject.transform;
                if (HandheldCameraManager.Spawned) HandheldCameraManager.ActiveHandheldCameraScript.SyncCamera = null;
                FreeCamManager.FreeCamObject.transform.position = Player.Head.position;
                FreeCamManager.FreeCamObject.transform.rotation = Player.Head.rotation;
                FreeCamManager.FreeCamObject.SetActive(true);
                break;
        }
    }
}