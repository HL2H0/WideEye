using BoneLib;
using UnityEngine;
using WideEye.Behaviors;
using WideEye.Core;
using WideEye.Data;
using Object = UnityEngine.Object;

namespace WideEye.CameraManagers;

public static class HandheldCameraManager
{
    public static GameObject HandheldCamera;
    public static HandheldCameraScript ActiveHandheldCameraScript;
    
    public static bool Spawned => HandheldCamera;
    
    public static void SpawnHandheldCamera()
    {
        if (Spawned) return;
        var pos = Player.Head.position + Player.Head.forward * 1f;
        HandheldCamera = Object.Instantiate(ResourcesManager.HandheldCameraPrefab, pos, Quaternion.identity);
        HandheldCamera.name = "[WideEye] Handheld Camera";
        ActiveHandheldCameraScript = HandheldCamera.GetComponent<HandheldCameraScript>();
        HandheldCamera.transform.position = pos;
        
    }

    public static void DestroyHandheldCamera()
    {
        if (!Spawned) return;
        Object.Destroy(HandheldCamera);
        HandheldCamera = null;
        CameraController.UpdateView(ModEnums.ViewMode.Head);
    }
    
    public static void TeleportHandheldCamera()
    { 
        if(!Spawned) return;
        var pos = Player.Head.position + Player.Head.forward * 0.5f;
        HandheldCamera.transform.position = pos;
    }
}