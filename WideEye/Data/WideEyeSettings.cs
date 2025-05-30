using Il2CppOccaSoftware.Exposure.Runtime;
using WideEye.Objects;


namespace WideEye.Data;

public class WideEyeSettings
{
    public string Name { get; set; }
    public string Version { get; set; }
    public float FOV { get; set; }
    public float PosSmoothing { get; set; }
    public float RotSmoothing { get; set; }
    public SerializableVector3 PosOffset { get; set; }
    public SerializableVector3 RotOffset { get; set; }
    public bool PostFXEnabled { get; set; }
    public bool CaEnabled { get; set; }
    public float CaIntensity { get; set; }
    public bool LdEnabled { get; set; }
    public SerializableVector2 LdCenter { get; set; }
    public float LdIntensity { get; set; }
    public float LdScale { get; set; }
    public SerializableVector2 LdMultiplyer { get; set; }
    public bool AeEnabled { get; set; }
    public AutoExposureAdaptationMode AeAdaptationMode { get; set; }
    public float AeD2Ls { get; set; }
    public float AeEvComp { get; set; }
    public float AeEvMax { get; set; }
    public float AeEvMin { get; set; }
    public float AeL2ds { get; set; }
    public AutoExposureMeteringMaskMode AeMeetringMaskMode { get; set; }
    public float AeMeetaeMeteringProceduralFalloff { get; set; }
    public AutoExposureMode AeMode { get; set; }
}