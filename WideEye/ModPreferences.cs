using MelonLoader;

using UnityEngine;

using BoneLib.Notifications;

using Il2CppOccaSoftware.Exposure.Runtime;

using static WideEye.Mod;
using static WideEye.MenuSetup;

namespace WideEye
{
    public static class ModPreferences
    {
        //Pref

        private static MelonPreferences_Category _categWideEye;
        private static MelonPreferences_Entry<float> _prefFov;
        private static MelonPreferences_Entry<bool> _prefPostFX;
        private static MelonPreferences_Entry<Vector3> _prefRotationOffset;
        private static MelonPreferences_Entry<Vector3> _prefPositionOffset;
        private static MelonPreferences_Entry<float> _prefRotationSmoothing;
        private static MelonPreferences_Entry<float> _prefPositionSmoothing;


        //Post-FX Pref

        private static MelonPreferences_Category _categPfxLd;
        private static MelonPreferences_Entry<bool> _prefLdEnabled;
        private static MelonPreferences_Entry<Vector2> _prefLdCenter;
        private static MelonPreferences_Entry<float> _prefLdIntensity;
        private static MelonPreferences_Entry<float> _prefLdScale;
        private static MelonPreferences_Entry<float> _prefLdXMultiplier;
        private static MelonPreferences_Entry<float> _prefLdYMultiplier;

        private static MelonPreferences_Category _categPfxCa;
        private static MelonPreferences_Entry<bool> _prefCaEnabled;
        private static MelonPreferences_Entry<float> _prefCaIntensity;

        private static MelonPreferences_Category _categPfxAe;
        private static MelonPreferences_Entry<bool> _prefAeEnabled;
        private static MelonPreferences_Entry<AutoExposureAdaptationMode> _prefAeAdaptationMode;
        private static MelonPreferences_Entry<float> _prefAeD2Ls;
        private static MelonPreferences_Entry<float> _prefAeEvCompensation;
        private static MelonPreferences_Entry<float> _prefAeEvMax;
        private static MelonPreferences_Entry<float> _prefAeEvMin;
        private static MelonPreferences_Entry<float> _prefAeL2Ds;
        private static MelonPreferences_Entry<AutoExposureMeteringMaskMode> _prefAeMeteringMask;
        private static MelonPreferences_Entry<float> _prefAeMetProcedFalloff;


        public static void CreatePref()
        {
            _categWideEye = MelonPreferences.CreateCategory("WideEye");
            _prefFov = _categWideEye.CreateEntry<float>("Fov", 75);
            _prefPostFX = _categWideEye.CreateEntry<bool>("PostFX", true);
            _prefRotationOffset = _categWideEye.CreateEntry<Vector3>("RotationOffset", new(11, 0, 0));
            _prefPositionOffset = _categWideEye.CreateEntry<Vector3>("PositionOffset", Vector3.zero);
            _prefRotationSmoothing = _categWideEye.CreateEntry<float>("RotationSmoothing", 0);
            _prefPositionSmoothing = _categWideEye.CreateEntry<float>("PositionSmoothing", 0);

            _categPfxLd = MelonPreferences.CreateCategory("WideEye_PostFX_LensDistortion");
            _prefLdEnabled = _categPfxLd.CreateEntry<bool>("Enabled", true);
            _prefLdCenter = _categPfxLd.CreateEntry<Vector2>("Center", new(0.50f, 0.50f));
            _prefLdIntensity = _categPfxLd.CreateEntry<float>("Intensity", 0.48f);
            _prefLdScale = _categPfxLd.CreateEntry<float>("Scale", 1);
            _prefLdXMultiplier = _categPfxLd.CreateEntry<float>("xMultiplier", 0.59f);
            _prefLdYMultiplier = _categPfxLd.CreateEntry<float>("yMultiplier", 0.59f);

            _categPfxCa = MelonPreferences.CreateCategory("WideEye_PostFX_ChromaticAberration");
            _prefCaEnabled = _categPfxCa.CreateEntry<bool>("Enabled", true);
            _prefCaIntensity = _categPfxCa.CreateEntry<float>("Intensity", 0.123f);

            _categPfxAe = MelonPreferences.CreateCategory("WideEye_PostFX_AutoExposure");
            _prefAeEnabled = _categPfxAe.CreateEntry<bool>("Enabled", true);
            _prefAeAdaptationMode = _categPfxAe.CreateEntry<AutoExposureAdaptationMode>("AdaptationMode", AutoExposureAdaptationMode.Progressive);
            _prefAeD2Ls = _categPfxAe.CreateEntry<float>("DarkToLightSpeed", 3);
            _prefAeEvCompensation = _categPfxAe.CreateEntry<float>("evCompensation", 2.5f);
            _prefAeEvMax = _categPfxAe.CreateEntry<float>("evMax", 1.2f);
            _prefAeEvMin = _categPfxAe.CreateEntry<float>("evMin", -1.2f);
            _prefAeL2Ds = _categPfxAe.CreateEntry<float>("LightToDarkSpeed", 1);
            _prefAeMeteringMask = _categPfxAe.CreateEntry<AutoExposureMeteringMaskMode>("MeteringMaskMode", AutoExposureMeteringMaskMode.Procedural);
            _prefAeMetProcedFalloff = _categPfxAe.CreateEntry<float>("MeteringProceduralFalloff", 2);
        }

        public static void LoadPref()
        {
            ApplyFOV(_prefFov.Value, true, FOVSlider);
            ApplyOther(OtherType.PostFX, _prefPostFX.Value, true);
            PostFXToggle.Value = _prefPostFX.Value;
            ApplyOffset(_prefRotationOffset.Value, OffsetType.Rotation, true, XROffset, YrOffset, ZrOffset);
            ApplyOffset(_prefPositionOffset.Value, OffsetType.Position, true, XpOffset, YpOffset, ZpOffset);
            ApplySmoothing(_prefRotationSmoothing.Value, _prefPositionSmoothing.Value, true);
            ApplyLd(_prefLdEnabled.Value, _prefLdCenter.Value, _prefLdIntensity.Value, _prefLdScale.Value, _prefLdXMultiplier.Value, _prefLdYMultiplier.Value, true);
            ApplyCa(_prefCaEnabled.Value, _prefCaIntensity.Value, true);
            ApplyAe(_prefAeEnabled.Value, _prefAeAdaptationMode.Value, _prefAeD2Ls.Value, _prefAeEvCompensation.Value, _prefAeEvMax.Value, _prefAeEvMin.Value, _prefAeL2Ds.Value, _prefAeMeteringMask.Value, _prefAeMetProcedFalloff.Value, true);
            SendNotification("WideEye | Success", "Loaded Preferences.", NotificationType.Success, 2, true);
            MelonLogger.Msg(ConsoleColor.Green, "Loaded Preferences.");
        }

        public static void SavePref()
        {
            _prefFov.Value = FOVSlider.Value;
            _prefPostFX.Value = PostFXToggle.Value;
            _prefRotationOffset.Value = new(XROffset.Value, YrOffset.Value, ZrOffset.Value);
            _prefPositionOffset.Value = new(XpOffset.Value, YpOffset.Value, ZpOffset.Value);
            _prefRotationSmoothing.Value = RSmoothing.Value;
            _prefPositionSmoothing.Value = PSmoothing.Value;

            _prefLdEnabled.Value = LdEnabled.Value;
            _prefLdCenter.Value = new(LdCenterX.Value, LdCenterY.Value);
            _prefLdIntensity.Value = LdIntensity.Value;
            _prefLdScale.Value = LdScale.Value;
            _prefLdXMultiplier.Value = LdXMultiplier.Value;
            _prefLdYMultiplier.Value = LdYMultiplier.Value;

            _prefCaEnabled.Value = CaEnabled.Value;
            _prefCaIntensity.Value = CaIntensity.Value;

            _prefAeEnabled.Value = AeEnabled.Value;
            _prefAeAdaptationMode.Value = (AutoExposureAdaptationMode)AeAdaptationMode.Value;
            _prefAeD2Ls.Value = AeD2Ls.Value;
            _prefAeEvCompensation.Value = AeEvCompensation.Value;
            _prefAeEvMax.Value = AeEvMax.Value;
            _prefAeEvMin.Value = AeEvMin.Value;
            _prefAeL2Ds.Value = AeL2Ds.Value;
            _prefAeMeteringMask.Value = (AutoExposureMeteringMaskMode)AeMeteringMaskMode.Value;
            _prefAeMetProcedFalloff.Value = AeMeteringProceduralFalloff.Value;

            _categWideEye.SaveToFile(false);
            _categPfxLd.SaveToFile(false);
            _categPfxCa.SaveToFile(false);
            _categPfxAe.SaveToFile(false);
            SendNotification("WideEye | Success", "Saved Preferences.", NotificationType.Success, 2, true);
            MelonLogger.Msg(ConsoleColor.Green, "Saved Preferences.");
        }

        public static void ClearPref()
        {
            SendNotification("WideEye | Please Wait", "Clearing Preferences...", NotificationType.Information, 2, true);
            MelonLogger.Msg("Clearing Preferences...");
            var categ = _categWideEye.Entries
            .Concat(_categPfxLd.Entries)
            .Concat(_categPfxCa.Entries)
            .Concat(_categPfxAe.Entries)
            .ToList();

            foreach (var entry in categ)
            {
                entry.ResetToDefault();
            }
            LoadPref();
            SendNotification("WideEye | Done!", "Cleared All Preferences", NotificationType.Success, 2, true);
            MelonLogger.Msg(ConsoleColor.Green, "Done!, Cleared All Preferences");
        }

    }
}
