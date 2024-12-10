using BoneLib.BoneMenu;
using BoneLib.Notifications;

namespace WideEye
{
    public class ModNotification(
        ModNotification.ModNotificationType type,
        string title,
        string message,
        NotificationType notificationType,
        float popupLength)
    {
        private static bool _showOther = true;
        private static bool _showPreferences = true;
        private static bool _showCameraDisabled = true;
        private static bool _showCameraFound = true;
        
        public enum ModNotificationType { Preferences, CameraDisabled, CameraFound, Other, Force }

        private ModNotificationType _type = type;
        private string _title = title;
        private string _message = message;
        private NotificationType _notificationType = notificationType;
        private float _popupLength = popupLength;

        public static void ChangeSilentNotification()
        {
            _showOther = MenuSetup.OtherNotifi.Value;
            _showPreferences = MenuSetup.PrefNotifi.Value;
            _showCameraDisabled = MenuSetup.CameraDisabledNotifi.Value;
            _showCameraFound = MenuSetup.CameraFoundNotifi.Value;
            _showCameraDisabled = MenuSetup.CameraDisabledNotifi.Value;
            ModPreferences.SavePref();
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
            otherElement.Value = otherElement.Value;
            preferenceElement.Value = preferenceElement.Value;
            cameraFoundElement.Value = cameraFoundElement.Value;
            cameraDisabledElement.Value = cameraDisabledElement.Value;
            cameraFoundElement.Value = cameraFoundElement.Value;
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

