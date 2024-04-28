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
            var (_, _, result) = QuestManager.Instance.GetCurrentQuestStageResult();
            
            if (hit.collider.CompareTag("NPC"))
            {

                if (result.target != hit.collider.GetComponent<Interactable>().npcName)
                {
                    Debug.Log("Wrong NPC");
                    return;
                }
                var interactable = hit.gameObject.GetComponent<Interactable>();
                Debug.Log("Interacting with " + interactable.npcName);
                interactable.Interact();
            }

            if (hit.collider.CompareTag("Barrier"))
            {
                if (result.target != "Barrier")
                {
                    Debug.Log("Wrong Barrier");
                    return;
                }
                Destroy(hit.collider.gameObject.transform.parent.gameObject);
                result.isCompleted = true;
                HUDManager.Instance.ShowXpText();
                QuestManager.Instance.UpdateQuestUI();
            }
            
            if (hit.collider.CompareTag("Echo Gorge"))
            {
                if (result.target != "Echo Gorge")
                {
                    Debug.Log("Wrong Location");
                    return;
                }
                result.isCompleted = true;
                HUDManager.Instance.ShowXpText();
                QuestManager.Instance.UpdateQuestUI();
            }

            if (hit.collider.CompareTag("Village"))
            {
                if (result.target != "Village")
                {
                    Debug.Log("Wrong Location");
                    return;
                }
                Destroy(hit.collider.gameObject);
                result.isCompleted = true;
                HUDManager.Instance.ShowXpText();
                QuestManager.Instance.UpdateQuestUI();
            }

            if (hit.collider.CompareTag("Cursed Statue"))
            {
                if (result.target != "Cursed Statue")
                {
                    Debug.Log("Wrong Location");
                    return;
                }
                result.isCompleted = true;
                HUDManager.Instance.ShowXpText();
                QuestManager.Instance.UpdateQuestUI();
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
