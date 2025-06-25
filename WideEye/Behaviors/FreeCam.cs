using UnityEngine;

namespace WideEye.Behaviors;

public class FreeCam : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float fastMoveSpeed = 7f;
    public float sensitivity = 3f;
    
    private bool _isLooking;
    private bool _EnableFreeCam = true;

    private void Update()
    {
        if (!_EnableFreeCam) return;
        
        var fastMove = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        var speed = fastMove ? fastMoveSpeed : moveSpeed;
        
        
        //Basic FreeCam Movement(WASD)
        if (Input.GetKey(KeyCode.A)) transform.position += (-transform.right * (speed * Time.deltaTime));   
        if (Input.GetKey(KeyCode.D)) transform.position += (transform.right * (speed * Time.deltaTime));
        if (Input.GetKey(KeyCode.W)) transform.position += (transform.forward * (speed * Time.deltaTime));
        if (Input.GetKey(KeyCode.S)) transform.position += (-transform.forward * (speed * Time.deltaTime));
        
        //FreeCam Up/Down Movement
        if (Input.GetKey(KeyCode.Q)) transform.position += (-transform.up * (speed * Time.deltaTime));  
        if (Input.GetKey(KeyCode.E)) transform.position += (transform.up * (speed * Time.deltaTime));
        
        //Freecam Rotation
        if(_isLooking) 
        {
            var rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivity;
            var rotationY = transform.localEulerAngles.x - Input.GetAxis("Mouse Y") * sensitivity;
            transform.localEulerAngles = new Vector3(rotationX, rotationY, 0f);
        }
        
        //Toggle Free Look
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            _isLooking = true;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            _isLooking = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    private void OnDisable()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        _isLooking = false;
        _EnableFreeCam = false;
    }
    private void OnEnable()
    {
        _EnableFreeCam = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}