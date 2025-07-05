using UnityEngine;
using WideEye.Behaviors;
using WideEye.Data;
using Object = UnityEngine.Object;

namespace WideEye.CameraManagers;

public static class FreeCamManager
{
    public static GameObject FreeCamObject { get; private set; }
    
    public static float MoveSpeed {
        get => _freeCam.moveSpeed;
        set => _freeCam.moveSpeed = value;
    }
    public static float FastMoveSpeed {
        get => _freeCam.fastMoveSpeed;
        set => _freeCam.fastMoveSpeed = value;
    }
    public static float Sensitivity {
        get => _freeCam.sensitivity;
        set => _freeCam.sensitivity = value;
    }
    public static float SmoothSpeed {
        get => _freeCam.smoothSpeed;
        set => _freeCam.smoothSpeed = value;
    }
    public static float ScrollSensitivity {
        get => _freeCam.scrollSensitivity;
        set => _freeCam.scrollSensitivity = value;
    }
    public static float ScrollSmoothing {
        get => _freeCam.scrollSmoothing;
        set => _freeCam.scrollSmoothing = value;
    }
    public static bool ShowIndicator {
        get => _indicator.activeSelf;
        set => _indicator.SetActive(value);
    }
    
    private static FreeCam _freeCam;
    private static GameObject _indicator;
    public static void Init()
    {
        FreeCamObject = new GameObject("[WideEye] FreeCam");
        _indicator = Object.Instantiate(ResourcesManager.FreeCamIndicatorPrefab, FreeCamObject.transform);
        _freeCam = FreeCamObject.AddComponent<FreeCam>();
        FreeCamObject.SetActive(false);
        _indicator.SetActive(false);
    }
}