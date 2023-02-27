using System.Collections;
using UnityEngine;
using Minecraft.Player;

namespace Minecraft.Audio
{
    public class BlockSound : MonoBehaviour
    {
        [Header("Audio source")]
        [SerializeField] private AudioSource audioSource;

        [Header("Audio clip")]
        [SerializeField] public AudioClip grassAudio;
        [SerializeField] public AudioClip dirtAudio;
        [SerializeField] public AudioClip stoneAudio;
        [SerializeField] public AudioClip woodAudio;
        
        private bool stepAudioIsPlaying = false;
        private PlayerMovements playerMovements;

        private void Awake()
        {
            playerMovements = FindObjectOfType<PlayerMovements>();
        }

        void Update()
        {
            PlayStepAudio();
        }

        private void PlayStepAudio()
        {
            if (playerMovements.isGrounded)
            {
                if (playerMovements.IsMoving())
                {
                    if (!stepAudioIsPlaying)
                    {
                        if ( playerMovements.blockBeneath.transform.name == "Grass_block" ||
                             playerMovements.blockBeneath.transform.name == "Leaves(Clone)" )
                        {
                            StartCoroutine(PlayStep(grassAudio));
                        }
                        else if (playerMovements.blockBeneath.transform.name == "Dirt_block")
                        {
                            StartCoroutine(PlayStep(dirtAudio));
                        }
                        else if ( playerMovements.blockBeneath.transform.name == "Stone_block" ||
                                  playerMovements.blockBeneath.transform.name == "Glass(Clone)" ||
                                  playerMovements.blockBeneath.transform.name == "Cobble(Clone)" ||
                                  playerMovements.blockBeneath.transform.name == "Stone(Clone)" ||
                                  playerMovements.blockBeneath.transform.name == "Brick(Clone)" )
                        {
                            StartCoroutine(PlayStep(stoneAudio));
                        }
                        else if ( playerMovements.blockBeneath.transform.name == "Wood(Clone)" ||
                                  playerMovements.blockBeneath.transform.name == "Planks(Clone)" )
                        {
                            StartCoroutine(PlayStep(woodAudio));
                        }
                    }
                }
            }
        }

        private IEnumerator PlayStep(AudioClip audioClip)
        {
            stepAudioIsPlaying = true;
            audioSource.PlayOneShot(audioClip, 0.1f);
            yield return new WaitForSecondsRealtime(0.5f);
            stepAudioIsPlaying = false;
        }

        public void Play(AudioClip audioClip)
        {
            audioSource.PlayOneShot(audioClip);
        }
    }
}