using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Minecraft.Player
{
    public class PlayerMovements : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform mainCamera;
        [SerializeField] private Transform player;

        [Header("Movement")]
        [SerializeField] private float moveSpeed = 4f; // the speed of the player
        [SerializeField] private float jumpForce = 6f; // the jump force of the player
        [SerializeField] private float mouseSensitivity = 25;

        [Header("Respawn point")]
        [SerializeField] private Vector3 respawnPoint;

        [Header("Ground check")]
        public bool isGrounded = false;
        public Transform blockBeneath;

        private Vector3 playerMovement;

        private float runFactor = 2.0f;
        private float playerSpeed;

        private bool jumpCooldown = false;

        // Input
        private PlayerInput playerInput;
        private InputAction moveAction;
        private InputAction runAction;
        private InputAction jumpAction;

        private float mouseX;
        private float mouseY;

        private float xRotation = 0.0f;
        private float yRotation = 0.0f;

        // Rigidbody
        private Rigidbody rb;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            Physics.IgnoreLayerCollision(1, 6);

            Cursor.lockState = CursorLockMode.Locked; // Lock the cursor
            Cursor.visible = false; // Make the cursor invisible

            playerInput = GetComponent<PlayerInput>();

            moveAction = playerInput.actions["Move"];
            runAction = playerInput.actions["Run"];
            jumpAction = playerInput.actions["Jump"];

            playerSpeed = moveSpeed;
        }

        private void Update()
        {
            mouseX =  playerInput.actions["MouseX"].ReadValue<float>();
            mouseY = playerInput.actions["MouseY"].ReadValue<float>();

            Move();
            Jump();
            MouseLook();
        }

        private void Move()
        {
            if (runAction.IsPressed())
            {
                playerSpeed = moveSpeed * runFactor;
            }
            else
            {
                playerSpeed = moveSpeed;
            }

            playerMovement = moveAction.ReadValue<Vector3>() * playerSpeed * Time.deltaTime; 
            transform.Translate(playerMovement.x, 0, playerMovement.z); // move the player
        }

        private void Jump()
        {
            if (jumpAction.triggered && isGrounded && !jumpCooldown)
            {
                rb.AddForce(transform.up * jumpForce, ForceMode.VelocityChange);
                StartCoroutine(JumpCooldown());
            }
        }

        private IEnumerator JumpCooldown()
        {
            jumpCooldown = true;
            yield return new WaitForSecondsRealtime(0.05f); // wait 0.05 seconds
            jumpCooldown = false;
        }

        private void MouseLook()
        {
            if(Cursor.lockState == CursorLockMode.Locked && Cursor.visible == false)
            {
                mouseX *= mouseSensitivity * Time.deltaTime;
                mouseY *= mouseSensitivity * Time.deltaTime;

                if(Mathf.Abs(mouseX) > 20 || Mathf.Abs(mouseY) > 20)
                {
                    return;
                }

                xRotation -= mouseY; // Camera X-Axis rotation (look up and down)
                xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Clamp the rotation

                yRotation += mouseX; // Camera Y-Axis rotation (look left and right)

                mainCamera.transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0); // Rotate the camera
                player.Rotate(Vector3.up * mouseX); // Rotate the player
            }
        }

        private void OnTriggerStay(Collider other)
        {
            isGrounded = true;
            blockBeneath = other.transform; // Get the block beneath the player

            if (other.tag == "Respawn")
            {
                transform.position = respawnPoint; // Respawn point
                Debug.Log("You fell into the void and respawned at the center of the map!");
            }
        }

        private void OnTriggerExit(Collider other)
        {
            isGrounded = false;
        }

        public bool IsMoving()
        {
            if(playerMovement.x != 0 || playerMovement.z != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
