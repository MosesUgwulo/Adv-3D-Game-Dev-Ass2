using UnityEngine;

namespace Player
{
    public class PlayerLook : MonoBehaviour
    {
        private float _xRotation = 0f;
        public float mouseSensitivity = 100f;
        public Transform playerBody;
        void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        
        void Update()
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
            
            _xRotation -= mouseY;
            _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);
            
            transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
            playerBody.Rotate(Vector3.up * mouseX);
        }
    }
}
