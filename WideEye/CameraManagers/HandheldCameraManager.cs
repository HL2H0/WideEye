using BoneLib;
using UnityEngine;
using WideEye.Core;
using WideEye.Data;
namespace WideEye.CameraManagers;

public static class HandheldCameraManager
{
    public static GameObject HandheldCamera;
    
    public static bool Spawned => HandheldCamera;
    
    public static void SpawnHandheldCamera()
    {
        if (Spawned) return;
        var pos = Player.Head.position + Player.Head.forward * 1f;
        HandheldCamera = GameObject.Instantiate(ResourcesManager.HandheldCameraPrefab, pos, Quaternion.identity);
        HandheldCamera.name = "WideEye Handheld Camera";

        HandheldCamera.transform.position = pos;
        
    }

    public static void DestroyHandheldCamera()
    {
        if (!Spawned) return;
        GameObject.Destroy(HandheldCamera);
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