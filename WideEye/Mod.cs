using System.Collections;

using BoneLib.BoneMenu;
using BoneLib;
using BoneLib.Notifications;

using Il2CppSLZ.Bonelab;
using MelonLoader;

using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

using Il2CppOccaSoftware.Exposure.Runtime;

using static WideEye.MenuSetup;
using static WideEye.ModPreferences;
using static WideEye.ModNotification;
using Il2CppSLZ.Marrow;

[assembly: MelonInfo(typeof(WideEye.Mod), "WideEye", "2.1.0", "HL2H0")]
[assembly: MelonGame("Stress Level Zero", "BONELAB")]

namespace WideEye
{
    public static class BuildInfo
    {
        public const string Name = "WideEye";
        public const string Version = "2.1.0";
        public const string Author = "HL2H0";
    }

    public class Mod : MelonMod
    {
        //Needed GameObjects and Components

        private static PlayerAvatarArt _rmPlayerArtComponent;

        private static GameObject _scGameObject;
        private static GameObject _stGameObject;
        private static Camera _scCameraComponent;
        private static SmoothFollower _scSmootherComponent;
        private static Transform _stTransform;
        private static Volume _scVolumeComponent;



        //Post-FX Overrides
        private static LensDistortion _lensDistortionOverride;
        private static ChromaticAberration _chromaticAberrationOverride;
        private static AutoExposure _autoExposureOverride;

        //Enums

        public enum OffsetType { Position, Rotation }
        public enum ResetType { Fov, Smoothing, RotationOffset, PositionOffset, LensDistortion, ChromaticAberration, AutoExposure, All }
        public enum CameraModeType { Head, Pinned }
        public enum LookAtPositionType { RightHand, LeftHand, Head }
        public enum OtherType { HairMeshes, HeadMesh, PostFX };

        //Variables
        private static Transform _lookAtTransform;
        private static CameraModeType _cameraMode = CameraModeType.Head;
        public static bool LookAtPlayer;
        private static bool _foundCamera;

        //Method made for making things easier

        //MelonLoader & BoneLib Events

        public override void OnInitializeMelon()
        {
            CreatePref();
            SetupBoneMenu();
            Hooking.OnLevelUnloaded += BoneLib_OnLevelUnloaded;
            Hooking.OnUIRigCreated += BoneLib_OnUIRigCreated;
            LoggerInstance.Msg("WideEye 2.1.0 Has Been Initialized.");
        }

        private void BoneLib_OnUIRigCreated()  
        {
            MelonCoroutines.Start(WaitForCameraRig());
            MelonLogger.Msg(System.ConsoleColor.Green, "UI Rig Created, Trying To Get Camera...");
        }

        private void BoneLib_OnLevelUnloaded()
        {
            _cameraMode = CameraModeType.Head;
            _foundCamera = false;
            LookAtPlayer = false;
            EleLookAtPlayer.Value = false;
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            if (LookAtPlayer && _cameraMode == CameraModeType.Pinned && _foundCamera)
            {
                _scGameObject.transform.LookAt(_lookAtTransform);
            }
        }

        

        //Methods to ensure everything is collected
        private static IEnumerator WaitForCameraRig()
        {
            yield return new WaitForSeconds(3f);
            GetTargetCamera(true);
        }

        public static void GetTargetCamera(bool isAuto)
        {
            if (HelperMethods.IsAndroid())
            {
                var notification = new ModNotification(ModNotificationType.Force, "WideEye | Error", "WideEye doesn't work with Android", NotificationType.Error, 3f);
                notification.Show();
            }
            else
            {
                _scGameObject = GameObject.Find("GameplaySystems [0]/DisabledContainer/Spectator Camera/Spectator Camera");
                _stGameObject = GameObject.Find("RigManager(bonelab) [0]/VRControllerRig/TrackingSpace/Headset/Spectator Target");

                if (_scGameObject == null || _stGameObject == null)
                {
                    if (isAuto)
                    {
                        var notification = new ModNotification(ModNotificationType.Force, "WideEye | Error", "Couldn't find the camera automatically.\nPlease open WideEye's menu and find it manually.", NotificationType.Error, 3);
                        notification.Show();
                        MelonLogger.Error("Couldn't find the camera automatically");
                        MainPage.Add(GetCameraButton);
                    }
                    else
                    {
                        var notification = new ModNotification(ModNotificationType.Force, "WideEye | Error", "Couldn't find the camera.", NotificationType.Error, 3);
                        notification.Show();
                        MelonLogger.Error("Couldn't find the camera");
                    }

                }
                else if (!_foundCamera)
                {
                    if (!_scGameObject.active)
                    {
                        var notification = new ModNotification(ModNotificationType.CameraDisabled, "WideEye | Warning", "Spectator Camera Is Not Active.\nModifications will not take action.", NotificationType.Warning, 3);
                        notification.Show();
                        MelonLogger.Warning("Spectator Camera Is Not Active. Modifications will not take action.");
                    }

                    _rmPlayerArtComponent = Player.ControllerRig.gameObject.GetComponent<PlayerAvatarArt>();
                    _stTransform = _stGameObject.GetComponent<Transform>();
                    _scSmootherComponent = _scGameObject.GetComponent<SmoothFollower>();
                    _scVolumeComponent = _scGameObject.GetComponent<Volume>();
                    _scCameraComponent = _scGameObject.GetComponent<Camera>();
                    GetPostFXOverrides();
                    if (isAuto)
                    {
                        var notification = new ModNotification(ModNotificationType.CameraFound, "WideEye | Success", "Found camera automatically", NotificationType.Success, 3);
                        notification.Show();
                        MelonLogger.Msg(System.ConsoleColor.Green, "Found camera automatically");
                    }
                    else
                    {
                        MainPage.Remove(GetCameraButton);
                        var notification = new ModNotification(ModNotificationType.CameraFound, "WideEye | Success", "Found camera manually", NotificationType.Success, 3);
                        notification.Show();
                        MelonLogger.Msg(System.ConsoleColor.Green, "Found camera manually");
                    }
                    
                    LoadPref();
                    _foundCamera = true;
                    _lookAtTransform = Player.Head.transform;
                }
            }
        }

        private static void GetPostFXOverrides()
        {
            _scVolumeComponent.profile.TryGet(out _lensDistortionOverride);
            _scVolumeComponent.profile.TryGet(out _chromaticAberrationOverride);
            _scVolumeComponent.profile.TryGet(out _autoExposureOverride);        
        }

        //"Apply Methods"

        public static void ResetToDefault(ResetType resetType)
        {
            switch (resetType)
            {
                case ResetType.Fov:
                    ApplyFOV(75f, true, FOVSlider);
                    break;

                case ResetType.Smoothing:
                    ApplySmoothing(0f, 0f, true);
                    break;

                case ResetType.RotationOffset:
                    ApplyOffset(new(11f, 0f, 0f), OffsetType.Rotation, true, XROffset, YrOffset, ZrOffset);
                    break;
                
                case ResetType.PositionOffset:
                    ApplyOffset(new(0f, 0f, 0f), OffsetType.Position, true, XpOffset, YpOffset, ZpOffset);
                    break;

                case ResetType.LensDistortion:
                    ApplyLd(true, new Vector2(0.5f, 0.5f), 0.48f, 1f, 0.59f, 1f, true);
                    break;

                case ResetType.ChromaticAberration:
                    ApplyCa(true, 0.123f, true);
                    break;

                case ResetType.AutoExposure:
                    ApplyAe(true, AutoExposureAdaptationMode.Progressive, 3f, 2.5f, 1.2f, -1.2f, 1f, AutoExposureMeteringMaskMode.Procedural, 2f, true);
                    break;

                case ResetType.All:
                    ApplyFOV(75f, true, FOVSlider);
                    ApplySmoothing(0f, 0f, true);
                    ApplyOffset(new(11f, 0, 0), OffsetType.Rotation, true, XROffset, YrOffset, ZrOffset);
                    ApplyOffset(new(0f, 0f, 0f), OffsetType.Position, true, XpOffset, YpOffset, ZpOffset);
                    ApplyLd(true, new Vector2(0.5f, 0.5f), 0.48f, 1f, 0.59f, 1f, true);
                    ApplyCa(true, 0.123f, true);
                    ApplyAe(true, AutoExposureAdaptationMode.Progressive, 3f, 2.5f, 1.2f, -1.2f, 1, AutoExposureMeteringMaskMode.Procedural, 2f, true);
                    _scVolumeComponent.enabled = true;
                    PostFXToggle.Value = true;
                    break;
            }

        }

        public static void ApplyCameraMode(CameraModeType modeType)
        {
            _cameraMode = modeType;
            switch (modeType)
            {
                case CameraModeType.Head:
                    _scSmootherComponent.enabled = true;
                    LookAtPlayer = false;
                    EleLookAtPlayer.Value = false;
                    break;
                
                case CameraModeType.Pinned:
                    _scSmootherComponent.enabled = false;
                    break;
            }
        }

        public static void ApplyLookAtTransform(LookAtPositionType type)
        {
            switch (type)
            {
                case LookAtPositionType.Head:
                    _lookAtTransform = Player.Head.transform;
                    break;
                case LookAtPositionType.RightHand:
                    _lookAtTransform = Player.RightHand.transform;
                    break;
                case LookAtPositionType.LeftHand:
                    _lookAtTransform = Player.LeftHand.transform;
                    break;
            }
        }

        public static void ApplyFOV(float fov, bool syncElementValue = false, FloatElement fovEle = null)
        {
            _scCameraComponent.fieldOfView = fov;
            if (ModPreferences.AutoSave) SavePref();
            if (!syncElementValue) return;
            if (fovEle != null) fovEle.Value = fov;
        }

        public static void ApplyOther(OtherType type, bool value, bool syncElement = false)
        {
            switch(type)
            {
                case OtherType.PostFX:
                    _scVolumeComponent.enabled = value;
                    break;
                case OtherType.HeadMesh:
                    if (value == false) _rmPlayerArtComponent.DisableHead();
                    else _rmPlayerArtComponent.EnableHead();
                    foreach(var mesh in Player.Avatar.headMeshes) mesh.enabled = value;
                    break;

                case OtherType.HairMeshes:
                    if (value == false) _rmPlayerArtComponent.DisableHair();
                    else _rmPlayerArtComponent.EnableHair();
                    foreach (var mesh in Player.Avatar.hairMeshes) mesh.enabled = value;
                    break;
            }
            
            if (syncElement)
            {
                PostFXToggle.Value = value;
            }

        }

        public static void ApplyOffset(Vector3 offset, OffsetType offsetType, bool syncElementValue = false, FloatElement eleX = null, FloatElement eleY = null, FloatElement eleZ = null)
        { 
            switch (offsetType)
            {
                case OffsetType.Position:
                    _stTransform.localPosition = offset;
                    break;

                case OffsetType.Rotation:
                    _stTransform.localRotation = Quaternion.Euler(offset);
                    break;
            }
            if (syncElementValue)
            {
                if (eleX != null) eleX.Value = offset.x;
                if (eleY != null) eleY.Value = offset.y;
                if (eleZ != null) eleZ.Value = offset.z;
            }
        }
        public static void ApplyOffset(OffsetType type)
        {
            switch (type)
            {
                case OffsetType.Rotation:
                    _stTransform.localRotation = Quaternion.Euler(XROffset.Value, YrOffset.Value, ZrOffset.Value);
                    break;
                case OffsetType.Position:
                    _stTransform.localPosition = new(XpOffset.Value, YpOffset.Value, ZpOffset.Value);
                    break;
            }
            if (ModPreferences.AutoSave) SavePref();
        }
        public static void ApplyLd(bool enabled, Vector2 center, float intensity, float scale, float xMulti, float yMulti, bool syncElements)
        {
            _lensDistortionOverride.active = enabled;
            _lensDistortionOverride.center.value = center;
            _lensDistortionOverride.intensity.value = intensity;
            _lensDistortionOverride.scale.value = scale;
            _lensDistortionOverride.xMultiplier.value = xMulti;
            _lensDistortionOverride.yMultiplier.value = yMulti;
            if (syncElements)
            {
                LdEnabled.Value = enabled;
                LdCenterX.Value = center.x;
                LdCenterY.Value = center.y;
                LdIntensity.Value = intensity;
                LdScale.Value = scale;
                LdXMultiplier.Value = xMulti;
                LdYMultiplier.Value = yMulti;
            }
        }
        public static void ApplyLd()
        {
            _lensDistortionOverride.active = LdEnabled.Value;
            _lensDistortionOverride.center.value = new(LdCenterX.Value, LdCenterY.Value);
            _lensDistortionOverride.intensity.value = LdIntensity.Value;
            _lensDistortionOverride.scale.value = LdScale.Value;
            _lensDistortionOverride.xMultiplier.value = LdXMultiplier.Value;
            _lensDistortionOverride.yMultiplier.value = LdYMultiplier.Value;
            if (ModPreferences.AutoSave) SavePref();
        }

        public static void ApplyCa(bool enabled, float intensity, bool syncElements)
        {
            _chromaticAberrationOverride.active = enabled;
            _chromaticAberrationOverride.intensity.value = intensity;
            if (!syncElements) return;
            CaEnabled.Value = enabled;
            CaIntensity.Value = intensity;
        }
        public static void ApplyCa()
        {
            _chromaticAberrationOverride.active = CaEnabled.Value;
            _chromaticAberrationOverride.intensity.value = CaIntensity.Value;
            if (ModPreferences.AutoSave) SavePref();
        }

        public static void ApplyAe(bool enabled, AutoExposureAdaptationMode adaptationMode, float d2Ls, float evCompen, float evMax, float evMin, float l2Ds, AutoExposureMeteringMaskMode meteringMaskMode, float meteringProceduralFalloff, bool syncElements)
        {
            _autoExposureOverride.active = enabled;
            _autoExposureOverride.adaptationMode.value = adaptationMode;
            _autoExposureOverride.darkToLightSpeed.value = d2Ls;
            _autoExposureOverride.evCompensation.value = evCompen;
            _autoExposureOverride.evMax.value = evMax;
            _autoExposureOverride.evMin.value = evMin;
            _autoExposureOverride.lightToDarkSpeed.value = l2Ds;
            _autoExposureOverride.meteringMaskMode.value = meteringMaskMode;
            _autoExposureOverride.meteringProceduralFalloff.value = meteringProceduralFalloff;
            
            if (!syncElements) return;
            
            AeEnabled.Value = enabled;
            AeAdaptationMode.Value = adaptationMode;
            AeD2Ls.Value = d2Ls;
            AeEvCompensation.Value = evCompen;
            AeEvMax.Value = evMax;
            AeEvMin.Value = evMin;
            AeL2Ds.Value = l2Ds;
            AeMeteringMaskMode.Value = meteringMaskMode;
            AeMeteringProceduralFalloff.Value = meteringProceduralFalloff;
        }
        public static void ApplyAe()
        {
            _autoExposureOverride.active = AeEnabled.Value;
            _autoExposureOverride.adaptationMode.value = (AutoExposureAdaptationMode)AeAdaptationMode.Value;
            _autoExposureOverride.darkToLightSpeed.value = AeD2Ls.Value;
            _autoExposureOverride.evCompensation.value = AeEvCompensation.Value;
            _autoExposureOverride.evMax.value = AeEvMax.Value;
            _autoExposureOverride.evMin.value = AeEvMin.Value;
            _autoExposureOverride.lightToDarkSpeed.value = AeL2Ds.Value;
            _autoExposureOverride.meteringMaskMode.value = (AutoExposureMeteringMaskMode)AeMeteringMaskMode.Value;
            _autoExposureOverride.meteringProceduralFalloff.value = AeMeteringProceduralFalloff.Value;
            if (ModPreferences.AutoSave) SavePref();
        }

        public static void ApplySmoothing(float rotationSmoothingValue, float positionSmoothingValue, bool syncElementValue)
        {
            _scSmootherComponent.RotationalSmoothTime = rotationSmoothingValue;
            _scSmootherComponent.TranslationSmoothTime = positionSmoothingValue;
            if (!syncElementValue) return;
            
            RSmoothing.Value = rotationSmoothingValue;
            PSmoothing.Value = positionSmoothingValue;
        }
        public static void ApplySmoothing()
        {
            _scSmootherComponent.RotationalSmoothTime = RSmoothing.Value;
            _scSmootherComponent.TranslationSmoothTime = PSmoothing.Value;
            if (ModPreferences.AutoSave) SavePref();
        }
    }
}