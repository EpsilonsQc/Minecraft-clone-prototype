using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

namespace Minecraft.UI.PauseMenu
{
    public class PauseMenu : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject pauseMenu;
        [SerializeField] private GameObject pauseMenuButtons; 
        [SerializeField] private GameObject optionsMenu;
        [SerializeField] private GameObject HUD; // crosshair + toolbar

        // Player Input
        private PlayerInput playerInput;
        private InputAction showHideMenuAction;

        private void Awake()
        {
            playerInput = GetComponent<PlayerInput>();
            showHideMenuAction = playerInput.actions["Show or Hide Menu"];
            HideCursor();
        }

        private void Update()
        {
            if (pauseMenu.activeSelf == false && showHideMenuAction.triggered)
            {
                Pause(); // open the pause menu
            }
            else if (pauseMenu.activeSelf == true && showHideMenuAction.triggered)
            {
                if(optionsMenu.activeSelf == false)
                {
                    Resume(); // close the pause menu
                }
                else if(optionsMenu.activeSelf == true)
                {
                    Back(); // return to the pause menu
                }
            }
        }

        private void HideCursor()
        {
            Cursor.lockState = CursorLockMode.Locked; // Lock the cursor
            Cursor.visible = false; // Make the cursor invisible
        }

        private void ShowCursor()
        {
            Cursor.lockState = CursorLockMode.None; // Unlock the cursor
            Cursor.visible = true; // Make the cursor visible
        }

        public void Pause()
        {
            ShowCursor();
            Time.timeScale = 0f; // Pause the game
            HUD.SetActive(false);
            pauseMenuButtons.SetActive(true);
            pauseMenu.SetActive(true);
        }

        public void Resume()
        {
            HideCursor();
            Time.timeScale = 1f; // Resume the game
            HUD.SetActive(true);
            optionsMenu.SetActive(false);
            pauseMenu.SetActive(false);
        }

        public void ReturnToMainMenu()
        {
            Time.timeScale = 1f; // Resume the game
            SceneManager.LoadScene("MainMenu");
        }

        public void Options()
        {
            pauseMenuButtons.SetActive(false);
            optionsMenu.SetActive(true);
        }

        public void Back()
        {
            pauseMenuButtons.SetActive(true);
            optionsMenu.SetActive(false);
        }

        public void Quit()
        {
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif
        }
    }
}