using System;
using Managers;
using NPCs;
using UnityEngine;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        private Vector3 _velocity;
        private bool _isGrounded;
        public CharacterController controller;
        public Transform groundCheck;
        public LayerMask groundMask;
        public float groundDistance = 0.4f;
        public float speed = 12f;
        public float gravity = -9.81f;
        public float jumpHeight = 3f;
        void Start()
        {
        
        }

        void Update()
        {
            _isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
            
            if (_isGrounded && _velocity.y < 0)
            {
                _velocity.y = -2f;
            }
            
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");
            
            Vector3 move = transform.right * x + transform.forward * z;
            
            controller.Move(move * (speed * Time.deltaTime));
            
            if (Input.GetButton("Jump") && _isGrounded)
            {
                _velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }
            
            _velocity.y += gravity * Time.deltaTime;
            
            controller.Move(_velocity * Time.deltaTime);
        }

        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            if (hit.collider.CompareTag("NPC"))
            {
                var interactable = hit.gameObject.GetComponent<Interactable>();
                Debug.Log("Interacting with " + interactable.npcName);
                
                interactable.Interact();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag("NPC"))
            {
                HUDManager.Instance.anim.SetBool(HUDManager.IsOpen, false);
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }
}
