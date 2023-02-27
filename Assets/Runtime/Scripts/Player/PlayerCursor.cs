using UnityEngine;
using UnityEngine.InputSystem;

namespace Minecraft.Player
{
    public class PlayerCursor : MonoBehaviour
    {
        [Header("Layer mask")]
        [SerializeField] private LayerMask layerMaskBlockPlacing;

        // Reference
        private Transform cam; // the camera is required for correct cursor position
        private Transform cursor;

        // Input
        private PlayerInput playerInput;
        private InputAction mouseCursorAction;

        private void Awake()
        {
            cam = FindObjectOfType<Camera>().transform;
            cursor = this.transform;

            playerInput = GetComponent<PlayerInput>();
            mouseCursorAction = playerInput.actions["Mouse Cursor"];
        }

        private void Update()
        {
            PlaceCursor();
            MouseCursorState(); // show or hide the mouse cursor (mouse pointer)
        }

        private void PlaceCursor()
        {
            RaycastHit hitInfo;
            Physics.Raycast(cam.transform.position, cam.transform.forward, out hitInfo, 5, layerMaskBlockPlacing);

            if(hitInfo.transform == null)
            {
                cursor.position = Vector3.zero; // if the raycast doesn't hit anything, set the cursor position to 0,0,0
            }

            if (hitInfo.transform != null)
            {
                if (hitInfo.transform.name == "top_face")
                {
                    cursor.position = new Vector3(hitInfo.transform.position.x, hitInfo.transform.position.y + 0.02f, hitInfo.transform.position.z);
                    cursor.eulerAngles = new Vector3(-90, 0, 0);
                }
                else if (hitInfo.transform.name == "bottom_face")
                {
                    cursor.position = new Vector3(hitInfo.transform.position.x, hitInfo.transform.position.y - 0.02f, hitInfo.transform.position.z);
                    cursor.eulerAngles = new Vector3(90, 0, 0);
                }
                else if (hitInfo.transform.name == "front_face")
                {
                    cursor.position = new Vector3(hitInfo.transform.position.x, hitInfo.transform.position.y, hitInfo.transform.position.z + 0.02f);
                    cursor.eulerAngles = new Vector3(0, 0, 0);
                }
                else if (hitInfo.transform.name == "back_face")
                {
                    cursor.position = new Vector3(hitInfo.transform.position.x, hitInfo.transform.position.y, hitInfo.transform.position.z - 0.02f);
                    cursor.eulerAngles = new Vector3(180, 0, 0);
                }
                else if (hitInfo.transform.name == "left_face")
                {
                    cursor.position = new Vector3(hitInfo.transform.position.x + 0.02f, hitInfo.transform.position.y, hitInfo.transform.position.z);
                    cursor.eulerAngles = new Vector3(0, 90, 0);
                }
                else if (hitInfo.transform.name == "right_face")
                {
                    cursor.position = new Vector3(hitInfo.transform.position.x - 0.02f, hitInfo.transform.position.y, hitInfo.transform.position.z);
                    cursor.eulerAngles = new Vector3(0, -90, 0);
                }
            }
        }

        private void MouseCursorState()
        {
            if (mouseCursorAction.triggered && Cursor.lockState == CursorLockMode.None)
            {
                Cursor.lockState = CursorLockMode.Locked; // Lock the cursor
                Cursor.visible = false; // Make the cursor invisible
            }
            else if (mouseCursorAction.triggered && Cursor.lockState == CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.None; // Unlock the cursor
                Cursor.visible = true; // Make the cursor visible
            }
        }
    }
}
