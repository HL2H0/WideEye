namespace WideEye.Core
{
    public static class ModEnums
    {
        public enum OffsetType { Position, Rotation }
        public enum ResetType { Fov, Smoothing, RotationOffset, PositionOffset, MKGlow,LensDistortion, ChromaticAberration, AutoExposure, All }
        public enum ViewMode { Head, Handheld, FreeCam }
        public enum MeshToggleType { HeadMesh, HairMeshes, HeasdMeshOffset }
        public enum AudioSource { Head, Handheld }
    }
}
