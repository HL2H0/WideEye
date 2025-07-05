using BoneLib;

using UnityEngine;
using WideEye.Behaviors;

using WideEye.Utilities;

namespace WideEye.Data;

public static class ResourcesManager
{
    public static GameObject HandheldCameraPrefab;
    public static GameObject FreeCamIndicatorPrefab;
    
    public static bool Loaded => HandheldCameraPrefab || FreeCamIndicatorPrefab;

    public static void Init()
    {
        FieldInjector.SerialisationHandler.Inject<CustomGripEvents>();
        FieldInjector.SerialisationHandler.Inject<HandheldCameraScript>();
        FieldInjector.SerialisationHandler.Inject<FreeCam>();
        
        if(!File.Exists(Paths.ResourcesPath)) return;
        
        var bundle = AssetBundle.LoadFromFile(Paths.ResourcesPath);
        HandheldCameraPrefab = HelperMethods.LoadPersistentAsset<GameObject>(bundle, "Handheld Camera");
        FreeCamIndicatorPrefab = HelperMethods.LoadPersistentAsset<GameObject>(bundle, "FreeCamIndicator");
    } 
}