using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

namespace Minecraft.UI.Intro
{
    public class FadeAway : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Image image;
        [SerializeField] private TextMeshProUGUI text;

        [Header("Parameters")]
        [SerializeField] private float waitFor = 1.0f; // Amount of seconds before gradually fading away;
        [SerializeField] private float fadeSpeed = 0.33f;
        private float startTime;

        // Color
        private Color textColor;

        // Player Input
        private PlayerInput playerInput;
        private InputAction skipIntroAction;

        private void Awake()
        {
            playerInput = GetComponent<PlayerInput>();
            skipIntroAction = playerInput.actions["Skip Intro"];

            startTime = Time.time + waitFor;
        }

        private void Update()
        {
            Fade();
            Skip();
        }

        private void Fade()
        {
            if(image == null || text == null)
            {
                Debug.Log("Missing references!");
                return;
            }

            image.color = Color.Lerp(Color.black, Color.clear, (Time.time - startTime) * fadeSpeed);

            textColor = text.color;
            textColor.a = Mathf.Lerp(1.0f, 0.0f, (Time.time - startTime) * fadeSpeed);
            text.color = textColor;

            if(textColor.a == 0)
            {
                Disable();
            }
        }

        private void Skip()
        {
            if(skipIntroAction.triggered)
            {
                Disable();
            }
        }

        private void Disable()
        {
            this.gameObject.SetActive(false);
        }
    }
}
