using System.Reflection;
using BoneLib;
using Il2CppSLZ.Marrow.Warehouse;
using MelonLoader;
using UnityEngine;
using WideEye.Behaviors;
using WideEye.Core;

namespace WideEye.Data;

public static class ResourcesManager
{
    public static GameObject FreeCamIndicatorPrefab;
    public static bool Loaded =>  FreeCamIndicatorPrefab;
    public static bool PalletInstalled = false;

    public static void Init()
    {
        FieldInjector.SerialisationHandler.Inject<CustomGripEvents>();
        FieldInjector.SerialisationHandler.Inject<FreeCam>();
        FieldInjector.SerialisationHandler.Inject<HandheldCamera>();
        
        var bundlePath = "WideEye.Resources.md_resources.bundle";
        var bundle = HelperMethods.LoadEmbeddedAssetBundle(Assembly.GetExecutingAssembly(), bundlePath);
        FreeCamIndicatorPrefab = HelperMethods.LoadPersistentAsset<GameObject>(bundle, "FreeCamIndicator");
    }

    public static void CheckPallet()
    {
        PalletReference palletRef = new("HL2H0.WideEye");
        var minVersion = new Version(1,0,0);
        
        var warehouse = AssetWarehouse.Instance;
        var pallets = warehouse.GetPallets();
        foreach(var pallet in pallets)
        {
            if(pallet.Barcode != palletRef.Barcode) continue;
            var version = new Version(pallet.Version);
            if (version < minVersion)
            {
                Melon<Mod>.Logger.Error($"WideEye pallet version {version} is too old. Minimum required version is {minVersion}.");
                return;
            }
            Melon<Mod>.Logger.Msg($"Found WideEye pallet version {version}.");
            PalletInstalled = true;
        }

        if (PalletInstalled) return;
        Melon<Mod>.Logger.Error($"WideEye pallet not found in AssetWarehouse.");
        BoneLib.Notifications.Notifier.Send(new BoneLib.Notifications.Notification
        {
            Title = "WideEye | Error",
            Message = "WideEye pallet not found or outdated\nPlease install the WideEye pallet from Mod.io then reload the level",
            Type = BoneLib.Notifications.NotificationType.Error,
            PopupLength = 5,
            ShowTitleOnPopup = true
        });
    }
}