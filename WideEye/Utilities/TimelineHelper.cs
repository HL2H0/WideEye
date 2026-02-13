using Il2CppSLZ.Bonelab;
using MelonLoader;
using UnityEngine;
using UnityEngine.Rendering;
using WideEye.UI;

namespace WideEye.Utilities;

public class TimelineHelper
{
    private static bool _createdMenu = false;
    public static bool UsingTimeline { get; set; }
    public static Camera TimelineCamera { get; set; }
    public static SmoothFollower TimelineSmoothFollower { get; set; }
    public static Volume TimelineVolume { get; set; }

    public static void StartHelper()
    {
        if (! CheckForTimeline()) return;
        UsingTimeline = true;
        CreateMenuOptions();
        GetReferences();
    }
    
    public static void ResetReferences()
    {
        TimelineCamera = null;
        TimelineSmoothFollower = null;
        TimelineVolume = null;
    }
    
    private static bool CheckForTimeline()
    {
        var mod = MelonMod.FindMelon("Timeline", "notnotnotswipez");
        return mod != null;
    }

    private static void GetReferences()
    {
        var timelineCam = GameObject.Find("Spectator Camera(Clone)/Spectator Camera");
        if (timelineCam != null)
        {
            TimelineCamera = timelineCam.GetComponent<Camera>();
            TimelineSmoothFollower = timelineCam.GetComponent<SmoothFollower>();
            TimelineVolume = timelineCam.GetComponent<Volume>();
        }
        SettingsUpdater.UpdateAllSettings();
    }
    

    private static void CreateMenuOptions()
    {
        if (_createdMenu) return;
        var page = ModMenu.ModSettingsPage.CreatePage("Timeline", Color.yellow);
        page.CreateFunction("Timeline Helper is Running", Color.green, null);
        page.CreateFunction("Update Timeline References", Color.yellow, GetReferences);
        _createdMenu = true;
    }
}