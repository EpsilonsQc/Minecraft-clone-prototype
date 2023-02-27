using System.Collections;
using UnityEngine;

namespace Minecraft.Core
{
    public class GameManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject intro;
        [SerializeField] private GameObject menuManager;
        [SerializeField] private GameObject player;
        [SerializeField] private GameObject HUD; // crosshair + toolbar

        private void Awake()
        {
            StartCoroutine(StartGameCoroutine());
        }

        private IEnumerator StartGameCoroutine()
        {
            yield return new WaitUntil(() => !intro.activeSelf);
            EnableMenuManager();
            EnablePlayer();
            EnableHUD();
        }

        private void EnableMenuManager()
        {
            menuManager.SetActive(true);
        }

        private void EnablePlayer()
        {
            player.SetActive(true);
        }

        private void EnableHUD()
        {
            HUD.SetActive(true);
        }
    }
}
