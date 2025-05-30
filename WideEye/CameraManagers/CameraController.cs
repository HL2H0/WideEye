using WideEye.Behaviors;
using WideEye.Core;

namespace WideEye.CameraManagers;

public static class CameraController
{
    public static void UpdateView(ModEnums.ViewMode viewMode)
    {
        var script = HandheldCameraManager.HandheldCamera.GetComponent<HandheldCameraScript>();
        if (!script) return;
        var result = viewMode switch
        {
            ModEnums.ViewMode.Head => (null, Mod.StTransform),
            ModEnums.ViewMode.Handheld when HandheldCameraManager.Spawned =>
                (Mod.ScCameraComponent, script.cameraTarget),
            _ => (script.SyncCamera, Mod.ScSmootherComponent.targetTransform)
        };
        script.SyncCamera = result.Item1;
        Mod.ScSmootherComponent.targetTransform = result.Item2;
    }
}