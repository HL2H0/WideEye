using BoneLib;
using Il2CppSLZ.Marrow.Pool;
using MelonLoader;
using UnityEngine;
using WideEye.Behaviors;
using WideEye.Core;
using Object = UnityEngine.Object;

namespace WideEye.CameraManagers;

public static class HandheldCameraManager
{
    public static GameObject ActiveHandheldCamera;
    public static HandheldCamera ActiveScript;
    
    
    public static bool Registered => ActiveHandheldCamera;
    
    // public static void FindHandheldCamera()
    // {
    //     if(Found) return;
    //     try
    //     {
    //         ActiveHandheldCamera = GameObject.Find("Handheld Camera [0]");
    //         if (ActiveHandheldCamera == null) return;
    //         ActiveScript = ActiveHandheldCamera.GetComponent<HandheldCamera>();
    //         if (ActiveScript == null) return;
    //         ActiveScript.SyncCamera = Mod.ScCameraComponent;
    //     }
    //     catch(Exception ex)
    //     {
    //         Melon<Mod>.Logger.Error($"Failed to find Handheld Camera : {ex}");
    //     }
    // }

    public static void RegisterCamera(GameObject camera)
    {
        ActiveHandheldCamera = camera;
        ActiveScript = camera.GetComponent<HandheldCamera>();
    }
    public static void UnregisterCamera()
    {
        ActiveHandheldCamera = null;
        ActiveScript = null;
    }
    

    public static void SpawnHandheldCamera()
    {
        Transform head = Player.Head;   
        HelperMethods.SpawnCrate("HL2H0.WideEye.Spawnable.WideEyeHandheldCamera", head.position + head.forward, default, Vector3.one, false, null);
    }

    public static void DestroyHandheldCamera()
    {
        if (!Registered) return;
        AssetSpawner.Despawn(ActiveHandheldCamera.GetComponent<Poolee>());
        Object.Destroy(ActiveHandheldCamera);
        ActiveHandheldCamera = null;
        CameraController.UpdateView(ModEnums.ViewMode.Head);
    }
    
    public static void TeleportHandheldCamera()
    { 
        if(!Registered) return;
        var pos = Player.Head.position + Player.Head.forward * 0.5f;
        ActiveHandheldCamera.transform.position = pos;
    }
}