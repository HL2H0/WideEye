using BoneLib.Notifications;
using Il2CppOccaSoftware.Exposure.Runtime;
using MelonLoader;
using UnityEngine;
using static WideEye.Mod;
using static WideEye.MenuSetup;

namespace WideEye
{
    public class ModPreferences
    {
        //Pref

        private static MelonPreferences_Category Categ_WideEye;
        private static MelonPreferences_Entry<float> Pref_Fov;
        private static MelonPreferences_Entry<bool> Pref_PostFX;
        private static MelonPreferences_Entry<Vector3> Pref_RotationOffset;
        private static MelonPreferences_Entry<Vector3> Pref_PositionOffset;
        private static MelonPreferences_Entry<float> Pref_RotationSmoothing;
        private static MelonPreferences_Entry<float> Pref_PositionSmoothing;


        //Post-FX Pref

        private static MelonPreferences_Category Categ_PFX_LD;
        private static MelonPreferences_Entry<bool> Pref_LD_Enabled;
        private static MelonPreferences_Entry<Vector2> Pref_LD_Center;
        private static MelonPreferences_Entry<float> Pref_LD_Intensity;
        private static MelonPreferences_Entry<float> Pref_LD_Scale;
        private static MelonPreferences_Entry<float> Pref_LD_xMultiplier;
        private static MelonPreferences_Entry<float> Pref_LD_yMultiplier;

        private static MelonPreferences_Category Categ_PFX_CA;
        private static MelonPreferences_Entry<bool> Pref_CA_Enabled;
        private static MelonPreferences_Entry<float> Pref_CA_Intensity;

        private static MelonPreferences_Category Categ_PFX_AE;
        private static MelonPreferences_Entry<bool> Pref_AE_Enabled;
        private static MelonPreferences_Entry<AutoExposureAdaptationMode> Pref_AE_AdaptationMode;
        private static MelonPreferences_Entry<float> Pref_AE_D2LS;
        private static MelonPreferences_Entry<float> Pref_AE_evCompensation;
        private static MelonPreferences_Entry<float> Pref_AE_evMax;
        private static MelonPreferences_Entry<float> Pref_AE_evMin;
        private static MelonPreferences_Entry<float> Pref_AE_L2DS;
        private static MelonPreferences_Entry<AutoExposureMeteringMaskMode> Pref_AE_Metering_Mask;
        private static MelonPreferences_Entry<float> Pref_AE_MetProcedFalloff;


        public static void CreatePref()
        {
            Categ_WideEye = MelonPreferences.CreateCategory("WideEye");
            Pref_Fov = Categ_WideEye.CreateEntry<float>("Fov", 75);
            Pref_PostFX = Categ_WideEye.CreateEntry<bool>("PostFX", true);
            Pref_RotationOffset = Categ_WideEye.CreateEntry<Vector3>("RotationOffset", new(11, 0, 0));
            Pref_PositionOffset = Categ_WideEye.CreateEntry<Vector3>("PositionOffset", Vector3.zero);
            Pref_RotationSmoothing = Categ_WideEye.CreateEntry<float>("RotationSmoothing", 0);
            Pref_PositionSmoothing = Categ_WideEye.CreateEntry<float>("PositionSmoothing", 0);

            Categ_PFX_LD = MelonPreferences.CreateCategory("WideEye_PostFX_LensDistortion");
            Pref_LD_Enabled = Categ_PFX_LD.CreateEntry<bool>("Enabled", true);
            Pref_LD_Center = Categ_PFX_LD.CreateEntry<Vector2>("Center", new(0.50f, 0.50f));
            Pref_LD_Intensity = Categ_PFX_LD.CreateEntry<float>("Intensity", 0.48f);
            Pref_LD_Scale = Categ_PFX_LD.CreateEntry<float>("Scale", 1);
            Pref_LD_xMultiplier = Categ_PFX_LD.CreateEntry<float>("xMultiplier", 0.59f);
            Pref_LD_yMultiplier = Categ_PFX_LD.CreateEntry<float>("yMultiplier", 0.59f);

            Categ_PFX_CA = MelonPreferences.CreateCategory("WideEye_PostFX_ChromaticAberration");
            Pref_CA_Enabled = Categ_PFX_CA.CreateEntry<bool>("Enabled", true);
            Pref_CA_Intensity = Categ_PFX_CA.CreateEntry<float>("Intensity", 0.123f);

            Categ_PFX_AE = MelonPreferences.CreateCategory("WideEye_PostFX_AutoExposure");
            Pref_AE_Enabled = Categ_PFX_AE.CreateEntry<bool>("Enabled", true);
            Pref_AE_AdaptationMode = Categ_PFX_AE.CreateEntry<AutoExposureAdaptationMode>("AdaptationMode", AutoExposureAdaptationMode.Progressive);
            Pref_AE_D2LS = Categ_PFX_AE.CreateEntry<float>("DarkToLightSpeed", 3);
            Pref_AE_evCompensation = Categ_PFX_AE.CreateEntry<float>("evCompensation", 2.5f);
            Pref_AE_evMax = Categ_PFX_AE.CreateEntry<float>("evMax", 1.2f);
            Pref_AE_evMin = Categ_PFX_AE.CreateEntry<float>("evMin", -1.2f);
            Pref_AE_L2DS = Categ_PFX_AE.CreateEntry<float>("LightToDarkSpeed", 1);
            Pref_AE_Metering_Mask = Categ_PFX_AE.CreateEntry<AutoExposureMeteringMaskMode>("MeteringMaskMode", AutoExposureMeteringMaskMode.Procedural);
            Pref_AE_MetProcedFalloff = Categ_PFX_AE.CreateEntry<float>("MeteringProceduralFalloff", 2);
        }

        public static void LoadPref()
        {

            ApplyFOV(Pref_Fov.Value, true, fovSlider);
            ApplyOther(Pref_PostFX.Value, SyncElements: true);
            PostFXToogle.Value = Pref_PostFX.Value;
            ApplyOffset(Pref_RotationOffset.Value, OffsetType.Rotation, true, X_R_Offset, Y_R_Offset, Z_R_Offset);
            ApplyOffset(Pref_PositionOffset.Value, OffsetType.Position, true, X_P_Offset, Y_P_Offset, Z_P_Offset);
            ApplySmoothing(Pref_RotationSmoothing.Value, Pref_PositionSmoothing.Value, true);
            ApplyLD(Pref_LD_Enabled.Value, Pref_LD_Center.Value, Pref_LD_Intensity.Value, Pref_LD_Scale.Value, Pref_LD_xMultiplier.Value, Pref_LD_yMultiplier.Value, true);
            ApplyCA(Pref_CA_Enabled.Value, Pref_CA_Intensity.Value, true);
            ApplyAE(Pref_AE_Enabled.Value, Pref_AE_AdaptationMode.Value, Pref_AE_D2LS.Value, Pref_AE_evCompensation.Value, Pref_AE_evMax.Value, Pref_AE_evMin.Value, Pref_AE_L2DS.Value, Pref_AE_Metering_Mask.Value, Pref_AE_MetProcedFalloff.Value, true);
        }

        public static void SavePref()
        {
            Pref_Fov.Value = fovSlider.Value;
            Pref_PostFX.Value = PostFXToogle.Value;
            Pref_RotationOffset.Value = new(X_R_Offset.Value, Y_R_Offset.Value, Z_R_Offset.Value);
            Pref_PositionOffset.Value = new(X_P_Offset.Value, Y_P_Offset.Value, Z_P_Offset.Value);
            Pref_RotationSmoothing.Value = R_Smoothing.Value;
            Pref_PositionSmoothing.Value = P_Smoothing.Value;

            Pref_LD_Enabled.Value = LD_Enabled.Value;
            Pref_LD_Center.Value = new(LD_CenterX.Value, LD_CenterY.Value);
            Pref_LD_Intensity.Value = LD_Intensity.Value;
            Pref_LD_Scale.Value = LD_Scale.Value;
            Pref_LD_xMultiplier.Value = LD_xMultiplier.Value;
            Pref_LD_yMultiplier.Value = LD_yMultiplier.Value;

            Pref_CA_Enabled.Value = CA_Enabled.Value;
            Pref_CA_Intensity.Value = CA_Intensity.Value;

            Pref_AE_Enabled.Value = AE_enabled.Value;
            Pref_AE_AdaptationMode.Value = (AutoExposureAdaptationMode)AE_adaptationMode.Value;
            Pref_AE_D2LS.Value = AE_D2LS.Value;
            Pref_AE_evCompensation.Value = AE_evCompensation.Value;
            Pref_AE_evMax.Value = AE_evMax.Value;
            Pref_AE_evMin.Value = AE_evMin.Value;
            Pref_AE_L2DS.Value = AE_L2DS.Value;
            Pref_AE_Metering_Mask.Value = (AutoExposureMeteringMaskMode)AE_MeteringMaskMode.Value;
            Pref_AE_MetProcedFalloff.Value = AE_MeteringProceduralFalloff.Value;

            Categ_WideEye.SaveToFile(false);
            Categ_PFX_LD.SaveToFile(false);
            Categ_PFX_CA.SaveToFile(false);
            Categ_PFX_AE.SaveToFile(false);
            SendNotfi("Success", "Saved Preferences.", NotificationType.Success, 1, true);
            MelonLogger.Msg(System.ConsoleColor.Green, "Saved Preferences.");

        }

        public static void ClearPref()
        {
            SendNotfi("Please Wait", "Clearing Preferences...", NotificationType.Information, 2, true);
            MelonLogger.Msg("Clearing Preferences...");
            var categ = Categ_WideEye.Entries
            .Concat(Categ_PFX_LD.Entries)
            .Concat(Categ_PFX_CA.Entries)
            .Concat(Categ_PFX_AE.Entries)
            .ToList();

            foreach (var entry in categ)
            {
                entry.ResetToDefault();
            }
            LoadPref();
            SendNotfi("Done!", "Cleared All Preferences", NotificationType.Success, 2, true);
            MelonLogger.Msg(System.ConsoleColor.Green, "Done!, Cleared All Preferences");
        }

    }
}
