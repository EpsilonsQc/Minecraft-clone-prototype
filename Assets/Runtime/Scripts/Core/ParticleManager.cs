using UnityEngine;
using UnityEngine.InputSystem;

namespace Minecraft.Core
{
    public enum AudioSourceName { Music, UI, FX };

    public class ParticleManager : MonoBehaviour
    {
        [Header("Parameter")]
        [SerializeField] private float maxInteractionDistance = 2.0f;

        [Header("Audio Settings")]
        [SerializeField] private AudioSourceName audioSourceName; // the name of the audio source to play the sound from
        [SerializeField] private AudioClip audioClip; // the audio clip to play when the particle system is activated

        private ParticleSystem ps;
        private AudioSource audioSource;
        private GameObject player;

        // Input
        private PlayerInput playerInput;
        private InputAction useAction;

        private void Awake()
        {
            ps = GetComponentInChildren<ParticleSystem>();
            ps.Stop();

            audioSource = GameObject.Find(audioSourceName.ToString()).GetComponent<AudioSource>();
            player = GameObject.Find("Player");

            playerInput = GetComponent<PlayerInput>();
            useAction = playerInput.actions["Use"];
        }

        private void Update()
        {
            ManageParticleSystem(); // activate or deactivate the particle system
        }

        private void ManageParticleSystem()
        {
            if (useAction.triggered && !ps.isPlaying && Vector3.Distance(player.transform.position, this.transform.position) <= maxInteractionDistance)
            {
                ps.Play();
                audioSource.clip = audioClip;
                audioSource.Play();
            }
            else if (useAction.triggered && ps.isPlaying && Vector3.Distance(player.transform.position, this.transform.position) <= maxInteractionDistance)
            {
                ps.Stop();
                audioSource.Stop();
            }
        }
    }
}
