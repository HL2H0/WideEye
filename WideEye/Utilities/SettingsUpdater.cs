using BoneLib;
using BoneLib.BoneMenu;
using Il2CppMK.Glow;
using Il2CppOccaSoftware.Exposure.Runtime;
using UnityEngine;
using WideEye.Core;
using WideEye.Data;
using WideEye.UI;

namespace WideEye.Utilities
{
    public static class SettingsUpdater
    {
        public static void ResetToDefault(ModEnums.ResetType resetType)
        {
            switch (resetType)
            {
                case ModEnums.ResetType.Fov:
                    UpdateFOV(75f, true, ModMenu.FOVSlider);
                    break;

                case ModEnums.ResetType.Smoothing:
                    UpdateSmoothing(0f, 0f, true);
                    break;

                case ModEnums.ResetType.RotationOffset:
                    UpdateOffset(new(11f, 0f, 0f), ModEnums.OffsetType.Rotation, true);
                    break;

                case ModEnums.ResetType.PositionOffset:
                    UpdateOffset(new(0f, 0f, 0f), ModEnums.OffsetType.Position, true);
                    break;
                
                case ModEnums.ResetType.MKGlow:
                    UpdateMkGlow(true, true);
                    break;

                case ModEnums.ResetType.LensDistortion:
                    UpdateLensDistortion(true, new Vector2(0.5f, 0.5f), 0.48f, 1f, 0.59f, 1f, true);
                    break;

                case ModEnums.ResetType.ChromaticAberration:
                    UpdateChromaticAberration(true, 0.123f, true);
                    break;

                case ModEnums.ResetType.AutoExposure:
                    UpdateAutoExposure(true, AutoExposureAdaptationMode.Progressive, 3f, 2.5f, 1.2f, -1.2f, 1f,
                        AutoExposureMeteringMaskMode.Procedural, 2f, true);
                    break;

                case ModEnums.ResetType.All:
                    UpdateFOV(75f, true, ModMenu.FOVSlider);
                    UpdateSmoothing(0f, 0f, true);
                    UpdateOffset(new(11f, 0, 0), ModEnums.OffsetType.Rotation, true);
                    UpdateOffset(new(0f, 0f, 0f), ModEnums.OffsetType.Position, true);
                    UpdateLensDistortion(true, new Vector2(0.5f, 0.5f), 0.48f, 1f, 0.59f, 1f, true);
                    UpdateChromaticAberration(true, 0.123f, true);
                    UpdateAutoExposure(true, AutoExposureAdaptationMode.Progressive, 3f, 2.5f, 1.2f, -1.2f, 1, 
                        AutoExposureMeteringMaskMode.Procedural, 2f, true);
                    Mod.ScVolumeComponent.enabled = true;
                    ModMenu.PostFXToggle.Value = true;
                    break;
            }

        }

        public static void UpdateAllSettings()
        {
            UpdateFOV(ModMenu.FOVSlider.Value);
            UpdateOffset(ModEnums.OffsetType.Position);
            UpdateOffset(ModEnums.OffsetType.Rotation);
            TogglePostFX(true);
            UpdateChromaticAberration();
            UpdateAutoExposure();
            UpdateLensDistortion();
            UpdateMkGlow();
            UpdateSmoothing();
        }
        

        public static void UpdateFOV(float fov, bool syncElementValue = false, FloatElement fovEle = null)
        {
            Mod.ScCameraComponent.fieldOfView = fov;
            if(TimelineHelper.UsingTimeline) TimelineHelper.TimelineCamera.fieldOfView = fov;
            
            if (ModPreferences.AutoSave) ModPreferences.SavePreferences();
            if (!syncElementValue) return;
            if (fovEle != null) fovEle.Value = fov;
        }
        
        public static void TogglePostFX(bool enabled, bool syncElementValue = false)
        {
            Mod.ScVolumeComponent.enabled = enabled;
            if(TimelineHelper.UsingTimeline) TimelineHelper.TimelineVolume.enabled = enabled;
            
            if (syncElementValue) ModMenu.PostFXToggle.Value = enabled;
            if (ModPreferences.AutoSave) ModPreferences.SavePreferences();
        }
        
        private static bool _currentHeadMeshState = true;
        private static bool _currentHairMeshState = true;
        public static void ToggleAvatarMesh(ModEnums.MeshToggleType type)
        {
            switch (type)
            {
                case ModEnums.MeshToggleType.HeadMesh:
                    _currentHeadMeshState = !_currentHeadMeshState;
                    if(_currentHairMeshState) Mod.PlayerArtComponent.EnableHead();
                    else Mod.PlayerArtComponent.DisableHead();
                    foreach (var mesh in Player.Avatar.headMeshes) mesh.enabled = _currentHeadMeshState;
                    break;
                case ModEnums.MeshToggleType.HairMeshes:
                    _currentHairMeshState = !_currentHairMeshState;
                    if(_currentHairMeshState) Mod.PlayerArtComponent.EnableHair();
                    else Mod.PlayerArtComponent.DisableHair();
                    foreach (var mesh in Player.Avatar.hairMeshes) mesh.enabled = _currentHairMeshState;
                    break;
                case ModEnums.MeshToggleType.HeasdMeshOffset:
                    Mod.PlayerArtComponent.enabled = !Mod.PlayerArtComponent.enabled;
                    break;
            }
        }
        
        public static void UpdateOffset(Vector3 offset, ModEnums.OffsetType offsetType, bool syncElementValue = false)
        {
            switch (offsetType)
            {
                case ModEnums.OffsetType.Position:
                    Mod.StTransform.localPosition = offset;
                    if(!syncElementValue) return;
                    ModMenu.XpOffset.Value = offset.x;
                    ModMenu.YpOffset.Value = offset.y;
                    ModMenu.ZpOffset.Value = offset.z;
                    break;

                case ModEnums.OffsetType.Rotation:
                    Mod.StTransform.localRotation = Quaternion.Euler(offset);
                    if (!syncElementValue) return;
                    ModMenu.XrOffset.Value = offset.x;
                    ModMenu.YrOffset.Value = offset.y;
                    ModMenu.ZrOffset.Value = offset.z;
                    break;
            }
        }

        public static void UpdateOffset(ModEnums.OffsetType type)
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

            if (ModPreferences.AutoSave) ModPreferences.SavePreferences();
        }

        public static void UpdateSmoothing(float rotationSmoothingValue, float positionSmoothingValue, bool syncElementValue)
        {
            Mod.ScSmootherComponent.RotationalSmoothTime = rotationSmoothingValue;
            Mod.ScSmootherComponent.TranslationSmoothTime = positionSmoothingValue;
            if (TimelineHelper.UsingTimeline)
            {
                TimelineHelper.TimelineSmoothFollower.RotationalSmoothTime = rotationSmoothingValue;
                TimelineHelper.TimelineSmoothFollower.TranslationSmoothTime = positionSmoothingValue;
            }
            
            if (!syncElementValue) return;

            ModMenu.RSmoothing.Value = rotationSmoothingValue;
            ModMenu.PSmoothing.Value = positionSmoothingValue;
        }

        public static void UpdateSmoothing()
        {
            Mod.ScSmootherComponent.RotationalSmoothTime = ModMenu.RSmoothing.Value;
            Mod.ScSmootherComponent.TranslationSmoothTime = ModMenu.PSmoothing.Value;
            if (TimelineHelper.UsingTimeline)
            {
                TimelineHelper.TimelineSmoothFollower.RotationalSmoothTime = ModMenu.RSmoothing.Value;
                TimelineHelper.TimelineSmoothFollower.TranslationSmoothTime = ModMenu.PSmoothing.Value;
            }

            if (ModPreferences.AutoSave) ModPreferences.SavePreferences();
        }

        public static void UpdateMkGlow(bool enabled, bool syncElements)
        {
            Mod.MKGlowOverride.active = enabled;
            if (syncElements)
            {
                ModMenu.MkGlowEnabled.Value = enabled;
            }
        }

        public static void UpdateMkGlow()
        {
            Mod.MKGlowOverride.active = ModMenu.MkGlowEnabled.Value;
            if (ModPreferences.AutoSave) ModPreferences.SavePreferences();
        }

        public static void UpdateLensDistortion(bool enabled, Vector2 center, float intensity, float scale, float xMulti,
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

        public static void UpdateLensDistortion()
        {
            Mod.LensDistortionOverride.active = ModMenu.LdEnabled.Value;
            Mod.LensDistortionOverride.center.value = new(ModMenu.LdCenterX.Value, ModMenu.LdCenterY.Value);
            Mod.LensDistortionOverride.intensity.value = ModMenu.LdIntensity.Value;
            Mod.LensDistortionOverride.scale.value = ModMenu.LdScale.Value;
            Mod.LensDistortionOverride.xMultiplier.value = ModMenu.LdXMultiplier.Value;
            Mod.LensDistortionOverride.yMultiplier.value = ModMenu.LdYMultiplier.Value;
            if (ModPreferences.AutoSave) ModPreferences.SavePreferences();
        }

        public static void UpdateChromaticAberration(bool enabled, float intensity, bool syncElements)
        {
            Mod.ChromaticAberrationOverride.active = enabled;
            Mod.ChromaticAberrationOverride.intensity.value = intensity;
            if (!syncElements) return;
            ModMenu.CaEnabled.Value = enabled;
            ModMenu.CaIntensity.Value = intensity;
        }

        public static void UpdateChromaticAberration()
        {
            Mod.ChromaticAberrationOverride.active = ModMenu.CaEnabled.Value;
            Mod.ChromaticAberrationOverride.intensity.value = ModMenu.CaIntensity.Value;
            if (ModPreferences.AutoSave) ModPreferences.SavePreferences();
        }

        public static void UpdateAutoExposure(bool enabled, AutoExposureAdaptationMode adaptationMode, float d2Ls, float evCompen,
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

        public static void UpdateAutoExposure()
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
            if (ModPreferences.AutoSave) ModPreferences.SavePreferences();
        }
    }
}

