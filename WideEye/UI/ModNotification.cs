using BoneLib.BoneMenu;
using BoneLib.Notifications;
using WideEye.Data;

namespace WideEye.UI
{
    public class ModNotification(
        ModNotification.ModNotificationType type,
        string title,
        string message,
        NotificationType notificationType,
        float popupLength)
    {
        private static bool _showOther;
        private static bool _showPreferences;
        private static bool _showCameraDisabled;
        private static bool _showCameraFound;
        
        public enum ModNotificationType { Preferences, CameraDisabled, CameraFound, Other, Force }

        private ModNotificationType _type = type;
        private string _title = title;
        private string _message = message;
        private NotificationType _notificationType = notificationType;
        private float _popupLength = popupLength;

        public static void ChangeSilentNotification()
        {
            _showOther = ModMenu.OtherNotifi.Value;
            _showPreferences = ModMenu.PrefNotifi.Value;
            _showCameraDisabled = ModMenu.CameraDisabledNotifi.Value;
            _showCameraFound = ModMenu.CameraFoundNotifi.Value;
            _showCameraDisabled = ModMenu.CameraDisabledNotifi.Value;
            if (ModPreferences.AutoSave) ModPreferences.SavePref();
            
        }

        public static void ChangeSilentNotification(bool other, bool preference, bool cameraFound, bool cameraDisabled,
            BoolElement otherElement, BoolElement preferenceElement, BoolElement cameraFoundElement,
            BoolElement cameraDisabledElement)
        {
            _showOther = other;
            _showPreferences = preference;
            _showCameraDisabled = cameraDisabled;
            _showCameraFound = cameraFound;
            _showCameraDisabled = cameraDisabled;
            otherElement.Value = other;
            preferenceElement.Value = preference;
            cameraFoundElement.Value = cameraFound;
            cameraDisabledElement.Value = cameraDisabled;
            cameraFoundElement.Value = cameraFound;
            
        }

        public void Show()
        {
            
            switch (_type)
            {
                case ModNotificationType.Preferences:
                    if (!_showPreferences) return;
                    break;
                case ModNotificationType.CameraDisabled:
                    if (!_showCameraDisabled) return;
                    break;
                case ModNotificationType.CameraFound:
                    if (!_showCameraFound) return;
                    break;
                case ModNotificationType.Other:
                    if (!_showOther) return;
                    break;  
            }
            
            var notification = new Notification
            {
                Title = _title,
                Message = _message,
                PopupLength = _popupLength,
                Type = _notificationType,
                ShowTitleOnPopup = true
            };
            Notifier.Send(notification);
        }
    }    
}

