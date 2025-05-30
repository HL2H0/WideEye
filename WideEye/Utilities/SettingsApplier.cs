using BoneLib;
using BoneLib.BoneMenu;
using Il2CppOccaSoftware.Exposure.Runtime;
using UnityEngine;
using WideEye.Core;
using WideEye.Data;
using WideEye.UI;

namespace WideEye.Utilities
{
    public static class SettingsApplier
    {
        public static void ResetToDefault(ModEnums.ResetType resetType)
        {
            switch (resetType)
            {
                case ModEnums.ResetType.Fov:
                    ApplyFOV(75f, true, ModMenu.FOVSlider);
                    break;

                case ModEnums.ResetType.Smoothing:
                    ApplySmoothing(0f, 0f, true);
                    break;

                case ModEnums.ResetType.RotationOffset:
                    ApplyOffset(new(11f, 0f, 0f), ModEnums.OffsetType.Rotation, true, ModMenu.XrOffset, ModMenu.YrOffset, ModMenu.ZrOffset);
                    break;

                case ModEnums.ResetType.PositionOffset:
                    ApplyOffset(new(0f, 0f, 0f), ModEnums.OffsetType.Position, true, ModMenu.XpOffset, ModMenu.YpOffset, ModMenu.ZpOffset);
                    break;

                case ModEnums.ResetType.LensDistortion:
                    ApplyLd(true, new Vector2(0.5f, 0.5f), 0.48f, 1f, 0.59f, 1f, true);
                    break;

                case ModEnums.ResetType.ChromaticAberration:
                    ApplyCa(true, 0.123f, true);
                    break;

                case ModEnums.ResetType.AutoExposure:
                    ApplyAe(true, AutoExposureAdaptationMode.Progressive, 3f, 2.5f, 1.2f, -1.2f, 1f,
                        AutoExposureMeteringMaskMode.Procedural, 2f, true);
                    break;

                case ModEnums.ResetType.All:
                    ApplyFOV(75f, true, ModMenu.FOVSlider);
                    ApplySmoothing(0f, 0f, true);
                    ApplyOffset(new(11f, 0, 0), ModEnums.OffsetType.Rotation, true, ModMenu.XrOffset, ModMenu.YrOffset, ModMenu.ZrOffset);
                    ApplyOffset(new(0f, 0f, 0f), ModEnums.OffsetType.Position, true, ModMenu.XpOffset, ModMenu.YpOffset, ModMenu.ZpOffset);
                    ApplyLd(true, new Vector2(0.5f, 0.5f), 0.48f, 1f, 0.59f, 1f, true);
                    ApplyCa(true, 0.123f, true);
                    ApplyAe(true, AutoExposureAdaptationMode.Progressive, 3f, 2.5f, 1.2f, -1.2f, 1, 
                        AutoExposureMeteringMaskMode.Procedural, 2f, true);
                    Mod.ScVolumeComponent.enabled = true;
                    ModMenu.PostFXToggle.Value = true;
                    break;
            }

        }
        

        public static void ApplyFOV(float fov, bool syncElementValue = false, FloatElement fovEle = null)
        {
            Mod.ScCameraComponent.fieldOfView = fov;
            if (ModPreferences.AutoSave) ModPreferences.SavePref();
            if (!syncElementValue) return;
            if (fovEle != null) fovEle.Value = fov;
        }

        public static void ApplyOther(ModEnums.OtherType type, bool value, bool syncElement = false)
        {
            switch (type)
            {
                case ModEnums.OtherType.PostFX:
                    Mod.ScVolumeComponent.enabled = value;
                    break;
                case ModEnums.OtherType.HeadMesh:
                    if (value == false) Mod.RmPlayerArtComponent.DisableHead();
                    else Mod.RmPlayerArtComponent.EnableHead();
                    foreach (var mesh in Player.Avatar.headMeshes) mesh.enabled = value;
                    break;

                case ModEnums.OtherType.HairMeshes:
                    if (value == false) Mod.RmPlayerArtComponent.DisableHair();
                    else Mod.RmPlayerArtComponent.EnableHair();
                    foreach (var mesh in Player.Avatar.hairMeshes) mesh.enabled = value;
                    break;
            }

            if (syncElement)
            {
                ModMenu.PostFXToggle.Value = value;
            }

        }

        public static void ApplyOffset(Vector3 offset, ModEnums.OffsetType offsetType, bool syncElementValue = false,
            FloatElement eleX = null, FloatElement eleY = null, FloatElement eleZ = null)
        {
            switch (offsetType)
            {
                case ModEnums.OffsetType.Position:
                    Mod.StTransform.localPosition = offset;
                    break;

                case ModEnums.OffsetType.Rotation:
                    Mod.StTransform.localRotation = Quaternion.Euler(offset);
                    break;
            }

            if (syncElementValue)
            {
                if (eleX != null) eleX.Value = offset.x;
                if (eleY != null) eleY.Value = offset.y;
                if (eleZ != null) eleZ.Value = offset.z;
            }
        }

        public static void ApplyOffset(ModEnums.OffsetType type)
        {
            switch (type)
            {
                case ModEnums.OffsetType.Rotation:
                    Mod.StTransform.localRotation = Quaternion.Euler(ModMenu.XrOffset.Value, ModMenu.YrOffset.Value, ModMenu.ZrOffset.Value);
                    break;
                case ModEnums.OffsetType.Position:
                    Mod.StTransform.localPosition = new(ModMenu.XpOffset.Value, ModMenu.YpOffset.Value, ModMenu.ZpOffset.Value);
                    break;
            }

            if (ModPreferences.AutoSave) ModPreferences.SavePref();
        }

        public static void ApplySmoothing(float rotationSmoothingValue, float positionSmoothingValue,
            bool syncElementValue)
        {
            Mod.ScSmootherComponent.RotationalSmoothTime = rotationSmoothingValue;
            Mod.ScSmootherComponent.TranslationSmoothTime = positionSmoothingValue;
            if (!syncElementValue) return;

            ModMenu.RSmoothing.Value = rotationSmoothingValue;
            ModMenu.PSmoothing.Value = positionSmoothingValue;
        }

        public static void ApplySmoothing()
        {
            Mod.ScSmootherComponent.RotationalSmoothTime = ModMenu.RSmoothing.Value;
            Mod.ScSmootherComponent.TranslationSmoothTime = ModMenu.PSmoothing.Value;
            if (ModPreferences.AutoSave) ModPreferences.SavePref();
        }

        public static void ApplyLd(bool enabled, Vector2 center, float intensity, float scale, float xMulti,
            float yMulti, bool syncElements)
        {
            Mod.LensDistortionOverride.active = enabled;
            Mod.LensDistortionOverride.center.value = center;
            Mod.LensDistortionOverride.intensity.value = intensity;
            Mod.LensDistortionOverride.scale.value = scale;
            Mod.LensDistortionOverride.xMultiplier.value = xMulti;
            Mod.LensDistortionOverride.yMultiplier.value = yMulti;
            if (syncElements)
            {
                ModMenu.LdEnabled.Value = enabled;
                ModMenu.LdCenterX.Value = center.x;
                ModMenu.LdCenterY.Value = center.y;
                ModMenu.LdIntensity.Value = intensity;
                ModMenu.LdScale.Value = scale;
                ModMenu.LdXMultiplier.Value = xMulti;
                ModMenu.LdYMultiplier.Value = yMulti;
            }
        }

        public static void ApplyLd()
        {
            Mod.LensDistortionOverride.active = ModMenu.LdEnabled.Value;
            Mod.LensDistortionOverride.center.value = new(ModMenu.LdCenterX.Value, ModMenu.LdCenterY.Value);
            Mod.LensDistortionOverride.intensity.value = ModMenu.LdIntensity.Value;
            Mod.LensDistortionOverride.scale.value = ModMenu.LdScale.Value;
            Mod.LensDistortionOverride.xMultiplier.value = ModMenu.LdXMultiplier.Value;
            Mod.LensDistortionOverride.yMultiplier.value = ModMenu.LdYMultiplier.Value;
            if (ModPreferences.AutoSave) ModPreferences.SavePref();
        }

        public static void ApplyCa(bool enabled, float intensity, bool syncElements)
        {
            Mod.ChromaticAberrationOverride.active = enabled;
            Mod.ChromaticAberrationOverride.intensity.value = intensity;
            if (!syncElements) return;
            ModMenu.CaEnabled.Value = enabled;
            ModMenu.CaIntensity.Value = intensity;
        }

        public static void ApplyCa()
        {
            Mod.ChromaticAberrationOverride.active = ModMenu.CaEnabled.Value;
            Mod.ChromaticAberrationOverride.intensity.value = ModMenu.CaIntensity.Value;
            if (ModPreferences.AutoSave) ModPreferences.SavePref();
        }

        public static void ApplyAe(bool enabled, AutoExposureAdaptationMode adaptationMode, float d2Ls, float evCompen,
            float evMax, float evMin, float l2Ds, AutoExposureMeteringMaskMode meteringMaskMode,
            float meteringProceduralFalloff, bool syncElements)
        {
            Mod.AutoExposureOverride.active = enabled;
            Mod.AutoExposureOverride.adaptationMode.value = adaptationMode;
            Mod.AutoExposureOverride.darkToLightSpeed.value = d2Ls;
            Mod.AutoExposureOverride.evCompensation.value = evCompen;
            Mod.AutoExposureOverride.evMax.value = evMax;
            Mod.AutoExposureOverride.evMin.value = evMin;
            Mod.AutoExposureOverride.lightToDarkSpeed.value = l2Ds;
            Mod.AutoExposureOverride.meteringMaskMode.value = meteringMaskMode;
            Mod.AutoExposureOverride.meteringProceduralFalloff.value = meteringProceduralFalloff;

            if (!syncElements) return;

            ModMenu.AeEnabled.Value = enabled;
            ModMenu.AeAdaptationMode.Value = adaptationMode;
            ModMenu.AeD2Ls.Value = d2Ls;
            ModMenu.AeEvCompensation.Value = evCompen;
            ModMenu.AeEvMax.Value = evMax;
            ModMenu.AeEvMin.Value = evMin;
            ModMenu.AeL2Ds.Value = l2Ds;
            ModMenu.AeMeteringMaskMode.Value = meteringMaskMode;
            ModMenu.AeMeteringProceduralFalloff.Value = meteringProceduralFalloff;
        }

        public static void ApplyAe()
        {
            Mod.AutoExposureOverride.active = ModMenu.AeEnabled.Value;
            Mod.AutoExposureOverride.adaptationMode.value = (AutoExposureAdaptationMode)ModMenu.AeAdaptationMode.Value;
            Mod.AutoExposureOverride.darkToLightSpeed.value = ModMenu.AeD2Ls.Value;
            Mod.AutoExposureOverride.evCompensation.value = ModMenu.AeEvCompensation.Value;
            Mod.AutoExposureOverride.evMax.value = ModMenu.AeEvMax.Value;
            Mod.AutoExposureOverride.evMin.value = ModMenu.AeEvMin.Value;
            Mod.AutoExposureOverride.lightToDarkSpeed.value = ModMenu.AeL2Ds.Value;
            Mod.AutoExposureOverride.meteringMaskMode.value =
                (AutoExposureMeteringMaskMode)ModMenu.AeMeteringMaskMode.Value;
            Mod.AutoExposureOverride.meteringProceduralFalloff.value = ModMenu.AeMeteringProceduralFalloff.Value;
            if (ModPreferences.AutoSave) ModPreferences.SavePref();
        }
    }
}

