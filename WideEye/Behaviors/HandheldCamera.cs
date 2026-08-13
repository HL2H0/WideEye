using System;
using Il2CppTMPro;
using Unity.Mathematics;
using UnityEngine;
using WideEye.CameraManagers;
using WideEye.Core;
using WideEye.Data;

namespace WideEye.Behaviors
{
    public class HandheldCamera : MonoBehaviour
    {
        public Transform cameraTarget;
        public Light cameraLight;
        
        public GameObject previewParent;
        public Camera previewCamera;
        
        public TextMeshPro fovLabel;
        public TextMeshPro lightIntensityLabel;
        
        public AudioListener audioListener;
        
        
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
        private float _targetFOV;


        private Color[] _lightColors = { new(1f, 0.847f, 0.694f), new(0.831f, 0.945f, 0.976f), new(0.961f, 0.961f, 0.961f) };
        private int _currentColorIndex;
            
        
        public Camera SyncCamera { set; get; }

        private void OnEnable()
        {
            HandheldCameraManager.RegisterCamera(gameObject);
            if (ModPreferences.ChangeViewOnSpawn)
                CameraController.UpdateView(ModEnums.ViewMode.Handheld, true);
        }

        private void OnDisable()
        {
            HandheldCameraManager.UnregisterCamera();
            CameraController.UpdateView(ModEnums.ViewMode.Head, true);
            CameraController.UpdateAudioSource(ModEnums.AudioSource.Head, true);
        }
        
        
        private void Start()
        {
            _rb = GetComponent<Rigidbody>();
            fovLabel.text = $"FOV : {FOV}";
            lightIntensityLabel.text = $"Light Intensity : {cameraLight.intensity}";
            _targetFOV = FOV;
        }
        
        private void Update()
        {
            _isTarget = SyncCamera;
            FOV = Mathf.Lerp(FOV, _targetFOV, Time.deltaTime / ModPreferences.HandheldZoomSmoothing );
        }
        
        public void ToggleKinematic()
        {
            _rb.isKinematic = !_rb.isKinematic;
        }

        public void ToggleLight()
        {
            cameraLight.intensity = (cameraLight.intensity + .5f) % 5f;
            lightIntensityLabel.text = $"Light Intensity : {cameraLight.intensity}";
        }

        public void ToggleLightColor()
        {
            _currentColorIndex = (_currentColorIndex + 1) % _lightColors.Length;
            cameraLight.color = _lightColors[_currentColorIndex];
        }
        
        public void AddFOV(float fov)
        {
            _targetFOV += fov * ModPreferences.HandheldZoomSpeed;
        }
    
        public void TogglePreview()
        {
            previewParent.SetActive(!previewParent.activeSelf);
        }

    }

}