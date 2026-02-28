using BoneLib;
using BoneLib.BoneMenu;
using Il2CppSLZ.Bonelab;
using Il2CppTMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace WideEye.UI;

public class MenuShortcut
{
    private static PreferencesPanelView _panelView;
    
    public static void CreateShortcut()
    {
        _panelView = Player.UIRig.popUpMenu.preferencesPanelView;
        
        var ogButton = _panelView.transform.Find("page_SPECTATOR/Viewport_Spectator/grid_Graphics/Fullscreen");
        var newButton = GameObject.Instantiate(ogButton.gameObject, ogButton.transform.parent);

        var wideEyeColor = new Color(0.5f, 0.36f, 1f);

        newButton.name = "WideEyeSettings";
        var label = newButton.transform.Find("text_Button").GetComponent<TextMeshProUGUI>();
        label.text = "WideEye Settings";
        label.color = wideEyeColor;

        var labelValue = newButton.transform.Find("button_toggle/text_Toggle_val").GetComponent<TextMeshProUGUI>();
        labelValue.text = "Open Menu";

        var image = newButton.transform.Find("button_toggle/image_backline").GetComponent<Image>();
        image.color = wideEyeColor;

        var buttonComponent = newButton.transform.Find("button_toggle").GetComponent<Button>();

        buttonComponent.onClick.RemoveAllListeners();
        buttonComponent.onClick.AddListener((UnityAction)OpenMenu);
        newButton.SetActive(true);
    }

    private static void OpenMenu()
    {
        _panelView = Player.UIRig.popUpMenu.preferencesPanelView;
        _panelView.PAGESELECT(11);
        _panelView.transform.Find("image_bgFade").gameObject.SetActive(false);
        Menu.OpenPage(ModMenu.MainPage);
    }
}