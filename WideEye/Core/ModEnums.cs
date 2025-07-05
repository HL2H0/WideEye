namespace WideEye.Core
{
    public static class ModEnums
    {
        public enum OffsetType { Position, Rotation }
        public enum ResetType { Fov, Smoothing, RotationOffset, PositionOffset, LensDistortion, ChromaticAberration, AutoExposure, All }
        public enum ViewMode { Head, Handheld, FreeCam }
        public enum OtherType { HairMeshes, HeadMesh, PostFX };
    }
}
