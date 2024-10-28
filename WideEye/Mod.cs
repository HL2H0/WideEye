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
using Il2CppSLZ.Marrow;

[assembly: MelonInfo(typeof(WideEye.Mod), "WideEye", "2.0.0", "HL2H0", null)]
[assembly: MelonGame("Stress Level Zero", "BONELAB")]

namespace WideEye
{
    public static class BuildInfo
    {
        public const string Name = "WideEye";
        public const string Version = "2.0.0";
        public const string Author = "HL2H0";
    }

    public class Mod : MelonMod
    {
        //Needed GameObjects and Components

        public static PlayerAvatarArt RM_playerArtComponent;

        public static GameObject SC_GameObject;
        public static GameObject ST_GameObject;
        public static Camera SC_CameraComponent;
        public static SmoothFollower SC_SmootherComponent;
        public static Transform ST_Transform;
        public static Volume SC_VolumeComponent;



        //Post-FX Overrides
        private static LensDistortion LensDistortionOverride;
        private static ChromaticAberration chromaticAberrationOverride;
        private static AutoExposure autoExposureOverride;

        //Enums

        public enum OffsetType { Position, Rotation }
        public enum ResetType { Fov, Smoothing, RotationOffset, PostionOffset, LensDistortion, ChromaticAberration, AutoExposure, All }
        public enum PostFXType { LensDistortion, ChromaticAberration, AutoExposure }
        public enum CameraMode { Head, Pinned }
        public enum LookAtPositionType { RightHand, LeftHand, Head }
        public enum OtherType { HairMeshes, HeadMesh, PostFX };

        //Variables
        public static Transform LookAtTransform;
        public static CameraMode cameraMode = CameraMode.Head;
        public static bool LookAtPlayer = false;
        private static bool gotcamera = false;

        //Method made for makeing things easier
        public static void SendNotfi(string Title, string Message, NotificationType Type, float PopupLength, bool showTitleOnPopup)
        {
            var notfi = new Notification
            {
                Title = Title,
                Message = Message,
                Type = Type,
                PopupLength = PopupLength,
                ShowTitleOnPopup = showTitleOnPopup
            };
            Notifier.Send(notfi);
        }

        //MelonLoader & BoneLib Events

        public override void OnInitializeMelon()
        {
            CreatePref();
            SetupBoneMenu();
            Hooking.OnLevelUnloaded += BoneLib_OnLevelUnloaded;
            Hooking.OnUIRigCreated += BoneLib_OnUIRigCreated;
            LoggerInstance.Msg("WideEye 2.0.0 Has Been Initialized.");
        }

        private void BoneLib_OnUIRigCreated()  
        {
            MelonCoroutines.Start(WaitForCameraRig());
            MelonLogger.Msg(System.ConsoleColor.Green, "UI Rig Created, Trying To Get Camera...");
        }

        private void BoneLib_OnLevelUnloaded()
        {
            cameraMode = CameraMode.Head;
            gotcamera = false;
            LookAtPlayer = false;
            Ele_LookAtPlayer.Value = false;
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            if (LookAtPlayer && cameraMode == CameraMode.Pinned && gotcamera)
            {
                SC_GameObject.transform.LookAt(LookAtTransform);
            }
        }

        

        //Methods to ensure everything is collected
        private static IEnumerator WaitForCameraRig()
        {
            yield return new WaitForSeconds(5f);
            GetTargetCamera(true);
        }

        public static void GetTargetCamera(bool isAuto)
        {
            if (HelperMethods.IsAndroid())
            {
                SendNotfi("WideEye | Error", "WideEye doesn't work with Android", NotificationType.Error, 3f, true);
            }
            else
            {
                SC_GameObject = GameObject.Find("GameplaySystems [0]/DisabledContainer/Spectator Camera/Spectator Camera");
                ST_GameObject = GameObject.Find("RigManager(bonelab) [0]/VRControllerRig/TrackingSpace/Headset/Spectator Target");

                if (SC_GameObject == null || ST_GameObject == null)
                {
                    if (isAuto)
                    {
                        SendNotfi("WideEye | Error", "Couldn't find the camera automatically.\nPlease open WideEye's menu and find it manually.", NotificationType.Error, 3, true);
                        MelonLogger.Error("Couldn't find the camera automatically");
                        mainPage.Add(GetCameraButton);
                    }
                    else
                    {
                        SendNotfi("WideEye | Error", "Couldn't find the camera.", NotificationType.Error, 3, true);
                        MelonLogger.Error("Couldn't find the camera");
                    }

                }
                else if (!gotcamera)
                {
                    if (!SC_GameObject.active)
                    {
                        SendNotfi("WideEye | Warning", "Spectator Camera Is Not Active.\nModifications will not take action.", NotificationType.Warning, 5, true);
                        MelonLogger.Warning("Spectator Camera Is Not Active. Modifications will not take action.");
                    }

                    RM_playerArtComponent = Player.ControllerRig.gameObject.GetComponent<PlayerAvatarArt>();
                    ST_Transform = ST_GameObject.GetComponent<Transform>();
                    SC_SmootherComponent = SC_GameObject.GetComponent<SmoothFollower>();
                    SC_VolumeComponent = SC_GameObject.GetComponent<Volume>();
                    SC_CameraComponent = SC_GameObject.GetComponent<Camera>();
                    GetPostFXOverrides();
                    if (isAuto)
                    {
                        SendNotfi("WideEye | Scusses", "Found camera automatically", NotificationType.Success, 3, true);
                        MelonLogger.Msg(System.ConsoleColor.Green, "Found camera automatically");
                    }
                    else
                    {
                        mainPage.Remove(GetCameraButton);
                        SendNotfi("WideEye | Scusses", "Found camera manually", NotificationType.Success, 2, true);
                        MelonLogger.Msg(System.ConsoleColor.Green, "Found camera manually");
                    }
                    
                    LoadPref();
                    gotcamera = true;
                    LookAtTransform = Player.Head.transform;
                }
            }
        }

        public static void GetPostFXOverrides()
        {
            SC_VolumeComponent.profile.TryGet(out LensDistortionOverride);
            SC_VolumeComponent.profile.TryGet(out chromaticAberrationOverride);
            SC_VolumeComponent.profile.TryGet(out autoExposureOverride);        
        }

        //"Apply Methods"

        public static void ResetToDefault(ResetType resetType)
        {
            switch (resetType)
            {
                case ResetType.Fov:
                    ApplyFOV(75, true, fovSlider);
                    break;

                case ResetType.Smoothing:
                    ApplySmoothing(0, 0, true);
                    break;

                case ResetType.RotationOffset:
                    ApplyOffset(new(11, 0, 0), OffsetType.Rotation, true, X_R_Offset, Y_R_Offset, Z_R_Offset);
                    break;
                
                case ResetType.PostionOffset:
                    ApplyOffset(new(0, 0, 0), OffsetType.Position, true, X_P_Offset, Y_P_Offset, Z_P_Offset);
                    break;

                case ResetType.LensDistortion:
                    ApplyLD(true, new Vector2(0.5f, 0.5f), 0.48f, 1, 0.59f, 1, true);
                    break;

                case ResetType.ChromaticAberration:
                    ApplyCA(true, 0.123f, true);
                    break;

                case ResetType.AutoExposure:
                    ApplyAE(true, AutoExposureAdaptationMode.Progressive, 3, 2.5f, 1.2f, -1.2f, 1, AutoExposureMeteringMaskMode.Procedural, 2, true);
                    break;

                case ResetType.All:
                    ApplyFOV(75, true, fovSlider);
                    ApplySmoothing(0, 0, true);
                    ApplyOffset(new(11, 0, 0), OffsetType.Rotation, true, X_R_Offset, Y_R_Offset, Z_R_Offset);
                    ApplyOffset(new(0, 0, 0), OffsetType.Position, true, X_P_Offset, Y_P_Offset, Z_P_Offset);
                    ApplyLD(true, new Vector2(0.5f, 0.5f), 0.48f, 1, 0.59f, 1, true);
                    ApplyCA(true, 0.123f, true);
                    ApplyAE(true, AutoExposureAdaptationMode.Progressive, 3, 2.5f, 1.2f, -1.2f, 1, AutoExposureMeteringMaskMode.Procedural, 2, true);
                    SC_VolumeComponent.enabled = true;
                    PostFXToogle.Value = true;
                    break;
            }

        }

        public static void ApplyCameraMode(CameraMode mode)
        {
            cameraMode = mode;
            switch (mode)
            {
                case CameraMode.Head:
                    
                    SC_SmootherComponent.enabled = true;
                    LookAtPlayer = false;
                    Ele_LookAtPlayer.Value = false;
                    break;
                case CameraMode.Pinned:
                    SC_SmootherComponent.enabled = false;
                    break;
            }
        }

        public static void ApplyLookAtTransform(LookAtPositionType type)
        {
            switch (type)
            {
                case LookAtPositionType.Head:
                    LookAtTransform = Player.Head.transform;
                    break;
                case LookAtPositionType.RightHand:
                    LookAtTransform = Player.RightHand.transform;
                    break;
                case LookAtPositionType.LeftHand:
                    LookAtTransform = Player.LeftHand.transform;
                    break;
            };
        }

        public static void ApplyFOV(float fov, bool SyncElementValue = false, FloatElement FovEle = null)
        {
            SC_CameraComponent.fieldOfView = fov;
            if (SyncElementValue)
            {
                FovEle.Value = fov;
            }
        }

        public static void ApplyOther(OtherType type, bool value, bool syncElemnent = false)
        {
            switch(type)
            {
                case OtherType.PostFX:
                    SC_VolumeComponent.enabled = value;
                    break;
                case OtherType.HeadMesh:
                    if (value == false) RM_playerArtComponent.DisableHead();
                    else RM_playerArtComponent.EnableHead();
                    foreach(var mesh in Player.Avatar.headMeshes) mesh.enabled = value;
                    break;

                case OtherType.HairMeshes:
                    if (value == false) RM_playerArtComponent.DisableHair();
                    else RM_playerArtComponent.EnableHair();
                    foreach (var mesh in Player.Avatar.hairMeshes) mesh.enabled = value;
                    break;
            }
            
            if (syncElemnent)
            {
                PostFXToogle.Value = value;
            }

        }

        public static void ApplyOffset(Vector3 Offset, OffsetType offsetType, bool SyncElementValue = false, FloatElement EleX = null, FloatElement EleY = null, FloatElement EleZ = null)
        { 
            switch (offsetType)
            {
                case OffsetType.Position:
                    ST_Transform.localPosition = Offset;
                    break;

                case OffsetType.Rotation:
                    ST_Transform.localRotation = Quaternion.Euler(Offset);
                    break;
            }
            if (SyncElementValue)
            {
                EleX.Value = Offset.x;
                EleY.Value = Offset.y;
                EleZ.Value = Offset.z;
            }
        }
        public static void ApplyOffset(OffsetType type)
        {
            switch (type)
            {
                case OffsetType.Rotation:
                    ST_Transform.localRotation = Quaternion.Euler(X_R_Offset.Value, Y_R_Offset.Value, Z_R_Offset.Value);
                    break;
                case OffsetType.Position:
                    ST_Transform.localPosition = new(X_P_Offset.Value, Y_P_Offset.Value, Z_P_Offset.Value);
                    break;
            }
        }
        public static void ApplyLD(bool Enabled, Vector2 Center, float Intensity, float Scale, float xMulti, float yMulti, bool SyncElements)
        {
            LensDistortionOverride.active = Enabled;
            LensDistortionOverride.center.value = Center;
            LensDistortionOverride.intensity.value = Intensity;
            LensDistortionOverride.scale.value = Scale;
            LensDistortionOverride.xMultiplier.value = xMulti;
            LensDistortionOverride.yMultiplier.value = yMulti;
            if (SyncElements)
            {
                LD_Enabled.Value = Enabled;
                LD_CenterX.Value = Center.x;
                LD_CenterY.Value = Center.y;
                LD_Intensity.Value = Intensity;
                LD_Scale.Value = Scale;
                LD_xMultiplier.Value = xMulti;
                LD_yMultiplier.Value = yMulti;
            }
        }
        public static void ApplyLD()
        {
            LensDistortionOverride.active = LD_Enabled.Value;
            LensDistortionOverride.center.value = new(LD_CenterX.Value, LD_CenterY.Value);
            LensDistortionOverride.intensity.value = LD_Intensity.Value;
            LensDistortionOverride.scale.value = LD_Scale.Value;
            LensDistortionOverride.xMultiplier.value = LD_xMultiplier.Value;
            LensDistortionOverride.yMultiplier.value = LD_yMultiplier.Value;
        }

        public static void ApplyCA(bool Enabled, float Intensity, bool SyncElements)
        {
            chromaticAberrationOverride.active = Enabled;
            chromaticAberrationOverride.intensity.value = Intensity;
            if(SyncElements)
            {
                CA_Enabled.Value = Enabled;
                CA_Intensity.Value = Intensity;
            }
        }
        public static void ApplyCA()
        {
            chromaticAberrationOverride.active = CA_Enabled.Value;
            chromaticAberrationOverride.intensity.value = CA_Intensity.Value;
        }

        public static void ApplyAE(bool Enabled, AutoExposureAdaptationMode adaptationMode, float D2LS, float evCompen, float evMax, float evMin, float L2DS, AutoExposureMeteringMaskMode meteringMaskMode, float MeteringProceduralFalloff, bool SyncElements)
        {
            autoExposureOverride.active = Enabled;
            autoExposureOverride.adaptationMode.value = adaptationMode;
            autoExposureOverride.darkToLightSpeed.value = D2LS;
            autoExposureOverride.evCompensation.value = evCompen;
            autoExposureOverride.evMax.value = evMax;
            autoExposureOverride.evMin.value = evMin;
            autoExposureOverride.lightToDarkSpeed.value = L2DS;
            autoExposureOverride.meteringMaskMode.value = meteringMaskMode;
            autoExposureOverride.meteringProceduralFalloff.value = MeteringProceduralFalloff;
            if ( SyncElements )
            {
                AE_enabled.Value = Enabled;
                AE_adaptationMode.Value = adaptationMode;
                AE_D2LS.Value = D2LS;
                AE_evCompensation.Value = evCompen;
                AE_evMax.Value = evMax;
                AE_evMin.Value = evMin;
                AE_L2DS.Value = L2DS;
                AE_MeteringMaskMode.Value = meteringMaskMode;
                AE_MeteringProceduralFalloff.Value = MeteringProceduralFalloff;
            }
        }
        public static void ApplyAE()
        {
            autoExposureOverride.active = AE_enabled.Value;
            autoExposureOverride.adaptationMode.value = (AutoExposureAdaptationMode)AE_adaptationMode.Value;
            autoExposureOverride.darkToLightSpeed.value = AE_D2LS.Value;
            autoExposureOverride.evCompensation.value = AE_evCompensation.Value;
            autoExposureOverride.evMax.value = AE_evMax.Value;
            autoExposureOverride.evMin.value = AE_evMin.Value;
            autoExposureOverride.lightToDarkSpeed.value = AE_L2DS.Value;
            autoExposureOverride.meteringMaskMode.value = (AutoExposureMeteringMaskMode)AE_MeteringMaskMode.Value;
            autoExposureOverride.meteringProceduralFalloff.value = AE_MeteringProceduralFalloff.Value;
        }

        public static void ApplySmoothing(float RotationSmoothingValue, float PositionSmoothingValue, bool SyncElementValue)
        {
            SC_SmootherComponent.RotationalSmoothTime = RotationSmoothingValue;
            SC_SmootherComponent.TranslationSmoothTime = PositionSmoothingValue;
            if (SyncElementValue)
            {
                P_Smoothing.Value = RotationSmoothingValue;
                R_Smoothing.Value = PositionSmoothingValue;
            }
        }
        public static void ApplySmoothing()
        {
            SC_SmootherComponent.RotationalSmoothTime = R_Smoothing.Value;
            SC_SmootherComponent.TranslationSmoothTime = P_Smoothing.Value;
        }
    }
}