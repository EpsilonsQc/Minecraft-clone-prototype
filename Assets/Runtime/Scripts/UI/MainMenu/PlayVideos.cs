using UnityEngine;
using UnityEngine.Video;
using UnityEngine.InputSystem;

namespace Minecraft.UI.MainMenu
{
    public class PlayVideos : MonoBehaviour
    {
        [Header("Resources")]
        [SerializeField] private VideoClip[] videoClip;
        private VideoPlayer videoPlayer;

        private PlayerInput playerInput;
        private InputAction previousVideoAction;
        private InputAction nextVideoAction;

        private void Awake()
        {
            videoPlayer = GetComponent<VideoPlayer>();
            playerInput = GetComponent<PlayerInput>();
            previousVideoAction = playerInput.actions["Previous Video"];
            nextVideoAction = playerInput.actions["Next Video"];
        }

        private void Start()
        {
            PlayRandomVideos(); // start with a random video
        }

        private void Update ()
        {
            SkipVideos(); // let user skip to the next video
        }

        private void PlayRandomVideos()
        {
            videoPlayer.clip = videoClip[Random.Range(0, videoClip.Length)]; // play a random video
        }

        private void SkipVideos()
        {
            if (previousVideoAction.triggered) // previous video
            {
                if (videoPlayer.clip == videoClip[0])
                {
                    videoPlayer.clip = videoClip[2];
                }
                else if (videoPlayer.clip == videoClip[1])
                {
                    videoPlayer.clip = videoClip[0];
                }
                else if (videoPlayer.clip == videoClip[2])
                {
                    videoPlayer.clip = videoClip[1];
                }
            }
            else if (nextVideoAction.triggered) // next video
            {
                if (videoPlayer.clip == videoClip[0])
                {
                    videoPlayer.clip = videoClip[1];
                }
                else if (videoPlayer.clip == videoClip[1])
                {
                    videoPlayer.clip = videoClip[2];
                }
                else if (videoPlayer.clip == videoClip[2])
                {
                    videoPlayer.clip = videoClip[0];
                }
            }
        }
    }
}