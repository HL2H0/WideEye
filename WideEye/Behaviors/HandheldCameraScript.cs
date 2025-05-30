using Il2CppTMPro;
using UnityEngine;

namespace WideEye.Behaviors
{
    public class HandheldCameraScript : MonoBehaviour
    {

        public Transform cameraTarget;
        public Light cameraLight;
        
        public GameObject previewParent;
        public Camera previewCamera;
        
        public TextMeshPro fovLabel;
        public TextMeshPro lightIntensityLabel;
        
        //Value Holders
        public float FOV
        {
            get => previewCamera.fieldOfView;
            set
            {
                previewCamera.fieldOfView = value;
                if (_isTarget) SyncCamera.fieldOfView = value;
                fovLabel.text = $"FOV : {MathF.Round(value, 2)}";
            }
        }
        
        private Rigidbody _rb;
        private bool _isTarget;

        private Color[] _colors = {Color.white, Color.red, Color.yellow, Color.green, Color.blue, Color.cyan, Color.magenta};
        private int _currentColorIndex;
        
            
        public Camera SyncCamera { set; get; }
        
        private void Start()
        {
            _rb = GetComponent<Rigidbody>();
            fovLabel.text = $"FOV : {FOV}";
            lightIntensityLabel.text = $"Light Intensity : {lightIntensityLabel.text}";
        }
        private void Update()
        {
            _isTarget = SyncCamera;
        }
        
        public void ToggleKinematic()
        {
            _rb.isKinematic = !_rb.isKinematic;
        }

        public void ToggleLight()
        {
            cameraLight.intensity = (cameraLight.intensity + 1f) % 5;
            lightIntensityLabel.text = $"Light Intensity : {cameraLight.intensity}";
        }

        public void ToggleLightColor()
        {
            _currentColorIndex = (_currentColorIndex + 1) % _colors.Length;
            cameraLight.color = _colors[_currentColorIndex];
        }
        
        public void AddFOV(float fov)
        {
            FOV += fov;
        }
    
        public void TogglePreview()
        {
            previewParent.SetActive(!previewParent.activeSelf);
        }
    }

}