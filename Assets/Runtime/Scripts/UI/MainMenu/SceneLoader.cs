using System.Collections; // used for "IEnumerator"
using UnityEngine;
using UnityEngine.SceneManagement; // used for "SceneManager"
using UnityEngine.UI; // used for "Button" and "Image"
using TMPro; // used for "TextMeshProGUI"

namespace Minecraft.UI.MainMenu
{
    public class SceneLoader : MonoBehaviour
    {
        // References
        private Button singleplayerButton;
        private Image loadingBarImg;
        private Image loadingBarProgressImg;
        private TextMeshProUGUI singleplayerButtonText;
        private TextMeshProUGUI loadingPercentageText;

        [Header("Settings")]
        [SerializeField] private bool startLoadingScene = false;
        [SerializeField] private bool sceneIsLoaded = false;
        [SerializeField] private bool enterScene = false;
        [Space(10)]

        [Header("Loading Scene")]
        [SerializeField] private float changePerSecond = 50.0f;
        [SerializeField] private float loadingProgress = 0.0f;

        private void Awake()
        {
            singleplayerButton = GameObject.Find("Buttons/Singleplayer").GetComponent<Button>();
            singleplayerButton.onClick.AddListener(StartLoadingScene);
            singleplayerButton.onClick.AddListener(EnterScene);

            loadingBarImg = GameObject.Find("Images/Loading Bar").GetComponent<Image>();
            loadingBarProgressImg = GameObject.Find("Images/Loading Bar/Loading Bar Progress").GetComponent<Image>();

            singleplayerButtonText = GameObject.Find("Buttons/Singleplayer/Text (Singleplayer)").GetComponent<TextMeshProUGUI>();
            loadingPercentageText = GameObject.Find("Images/Loading Bar/Text (Loading Percentage)").GetComponent<TextMeshProUGUI>();
        }

        private void Update()
        {            
            if (startLoadingScene == true && loadingProgress <= 100)
            {
                loadingBarImg.enabled = true;
                loadingBarProgressImg.enabled = true;

                loadingProgress += changePerSecond * Time.deltaTime;
                loadingPercentageText.text = ((int)loadingProgress + "%");

                loadingBarProgressImg.fillAmount = (float)loadingProgress / 100f;
            }
        }

        // Start loading scene
        private void StartLoadingScene()
        {
            StartCoroutine(LoadScene());
        }

        // Load scene (coroutine)
        private IEnumerator LoadScene()
        {
            if (!startLoadingScene)
            {
                AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("InGame");
                asyncLoad.allowSceneActivation = false;
            
                while (!asyncLoad.isDone)
                {
                    startLoadingScene = true;

                    if(loadingProgress >= 100)
                    {
                        singleplayerButtonText.text = "Start";
                        sceneIsLoaded = true;
                        singleplayerButton.onClick.RemoveListener(StartLoadingScene);

                        if(enterScene == true)
                        {
                            asyncLoad.allowSceneActivation = true;
                            singleplayerButton.onClick.RemoveListener(EnterScene);
                        }
                    }
                    yield return null; // wait until the next frame before continuing
                }
            }
        }

        private void EnterScene()
        {
            if(sceneIsLoaded)
            {
                enterScene = true;
            }
        }
    }
}