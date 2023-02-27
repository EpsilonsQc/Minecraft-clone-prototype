using UnityEngine;
using UnityEngine.InputSystem;
using Minecraft.Audio; // using audio namespace
using Minecraft.UI.HUD; // using HUD namespace

namespace Minecraft.Core
{
    public class BlockManager : MonoBehaviour
    {
        [Header("Layer mask")]
        [SerializeField] private LayerMask layerMaskBlockPlacing;
        [SerializeField] private LayerMask layerMaskBlockDestruction;

        public static Transform blockToDestroy;
        public static Transform xChunkToUpdate;
        public static Transform zChunkToUpdate;

        [Header("Debug")]
        [SerializeField] private bool debug;

        // References
        private Transform cam;
        private GameObject player;
        private BlockSound blockSound;

        // Input
        private PlayerInput playerInput;
        private InputAction mouseLeftClickAction;
        private InputAction mouseRightClickAction;

        private void Awake()
        {
            cam = FindObjectOfType<Camera>().transform;
            player = this.transform.gameObject;
            blockSound = FindObjectOfType<BlockSound>();

            playerInput = GetComponent<PlayerInput>();
            mouseLeftClickAction = playerInput.actions["Mouse Left Click"];
            mouseRightClickAction = playerInput.actions["Mouse Right Click"];
        }

        void Update()
        {
            BlockInteraction();
            DebugDrawRay(); // draw a debug ray in the scene view (if debug is enabled)
        }

        private void BlockInteraction()
        {
            if (mouseRightClickAction.triggered)
            {
                PlaceBlock(ToolbarManager.blockToPlace);
            }

            if (mouseLeftClickAction.triggered)
            {
                DestroyBlock();
            }
        }

        private void PlaceBlock(Transform Block)
        {
            RaycastHit hitInfo;
            Physics.Raycast(cam.transform.position, cam.transform.forward, out hitInfo, 5, layerMaskBlockPlacing);

            if (hitInfo.transform != null)
            {
                PlaySoundPlaceBlock();

                Vector3 parentBlockPos = hitInfo.transform.parent.transform.position;

                if (hitInfo.transform.name == "top_face")
                {
                    Transform block = Instantiate(Block, new Vector3(parentBlockPos.x, parentBlockPos.y + 1, parentBlockPos.z), Quaternion.identity);
                    block.parent = hitInfo.transform.parent.parent;
                    hitInfo.transform.parent.parent.GetComponent<TerrainChunk>().UpdateBlockFacesInChunk();
                }
                else if (hitInfo.transform.name == "bottom_face")
                {
                    Transform block = Instantiate(Block, new Vector3(parentBlockPos.x, parentBlockPos.y - 1, parentBlockPos.z), Quaternion.identity);
                    block.parent = hitInfo.transform.parent.parent;
                    hitInfo.transform.parent.parent.GetComponent<TerrainChunk>().UpdateBlockFacesInChunk();
                }
                else if (hitInfo.transform.name == "front_face")
                {
                    Transform block = Instantiate(Block, new Vector3(parentBlockPos.x, parentBlockPos.y, parentBlockPos.z + 1), Quaternion.identity);
                    block.parent = hitInfo.transform.parent.parent;
                    hitInfo.transform.parent.parent.GetComponent<TerrainChunk>().UpdateBlockFacesInChunk();
                }
                else if (hitInfo.transform.name == "back_face")
                {
                    Transform block = Instantiate(Block, new Vector3(parentBlockPos.x, parentBlockPos.y, parentBlockPos.z - 1), Quaternion.identity);
                    block.parent = hitInfo.transform.parent.parent;
                    hitInfo.transform.parent.parent.GetComponent<TerrainChunk>().UpdateBlockFacesInChunk();
                }
                else if (hitInfo.transform.name == "left_face")
                {
                    Transform block = Instantiate(Block, new Vector3(parentBlockPos.x + 1, parentBlockPos.y, parentBlockPos.z), Quaternion.identity);
                    block.parent = hitInfo.transform.parent.parent;
                    hitInfo.transform.parent.parent.GetComponent<TerrainChunk>().UpdateBlockFacesInChunk();
                }
                else if (hitInfo.transform.name == "right_face")
                {
                    Transform block = Instantiate(Block, new Vector3(parentBlockPos.x - 1, parentBlockPos.y, parentBlockPos.z), Quaternion.identity);
                    block.parent = hitInfo.transform.parent.parent;
                    hitInfo.transform.parent.parent.GetComponent<TerrainChunk>().UpdateBlockFacesInChunk();
                }
            }
        }

        private void DestroyBlock()
        {
            RaycastHit hitInfo;
            Physics.Raycast(cam.transform.position, cam.transform.forward, out hitInfo, 5, layerMaskBlockDestruction);

            if (hitInfo.transform != null)
            {
                if (hitInfo.transform.tag == "Block")
                {
                    PlaySoundDestroyBlock(hitInfo);

                    if (hitInfo.transform.position.x - hitInfo.transform.parent.position.x == 9)
                    {
                        // 1 right
                        RaycastHit chunkChecker;
                        Physics.Raycast(new Vector3(hitInfo.transform.position.x + 0.5f, hitInfo.transform.position.y, hitInfo.transform.position.z), Vector3.right, out chunkChecker, 0.3f);
                        if (chunkChecker.collider != null)
                        {
                            Debug.DrawRay(new Vector3(hitInfo.transform.position.x + 0.5f, hitInfo.transform.position.y, hitInfo.transform.position.z), Vector3.right, Color.red, 1);
                            // 2 right
                            xChunkToUpdate = chunkChecker.transform.parent;
                        }
                    }

                    if(hitInfo.transform.position.x - hitInfo.transform.parent.position.x == 0)
                    {
                        // 1 left
                        RaycastHit chunkChecker;
                        Physics.Raycast(new Vector3(hitInfo.transform.position.x - 0.5f, hitInfo.transform.position.y, hitInfo.transform.position.z), Vector3.left, out chunkChecker, 0.3f);
                        if (chunkChecker.collider != null)
                        {
                            Debug.DrawRay(new Vector3(hitInfo.transform.position.x - 0.5f, hitInfo.transform.position.y, hitInfo.transform.position.z), Vector3.left, Color.blue, 1);
                            // 2 left
                            xChunkToUpdate = chunkChecker.transform.parent;
                        }
                    }

                    if (hitInfo.transform.position.z - hitInfo.transform.parent.position.z == 9)
                    {
                        // 1 forward
                        RaycastHit chunkChecker;
                        Physics.Raycast(new Vector3(hitInfo.transform.position.x, hitInfo.transform.position.y, hitInfo.transform.position.z + 0.5f), Vector3.forward, out chunkChecker, 0.3f);
                        if (chunkChecker.collider != null)
                        {
                            Debug.DrawRay(new Vector3(hitInfo.transform.position.x, hitInfo.transform.position.y, hitInfo.transform.position.z + 0.5f), Vector3.forward, Color.green, 1);
                            // 2 forward
                            zChunkToUpdate = chunkChecker.transform.parent;
                        }
                    }

                    if (hitInfo.transform.position.z - hitInfo.transform.parent.position.z == 0)
                    {
                        // 1 back
                        RaycastHit chunkChecker;
                        Physics.Raycast(new Vector3(hitInfo.transform.position.x, hitInfo.transform.position.y, hitInfo.transform.position.z - 0.5f), Vector3.back, out chunkChecker, 0.3f);

                        if (chunkChecker.collider != null && chunkChecker.transform != player)
                        {
                            Debug.DrawRay(new Vector3(hitInfo.transform.position.x, hitInfo.transform.position.y, hitInfo.transform.position.z - 0.5f), Vector3.back, Color.yellow, 1);
                            // 2 back
                            zChunkToUpdate = chunkChecker.transform.parent;
                        }
                    }

                    blockToDestroy = hitInfo.transform;
                    Destroy(hitInfo.transform.gameObject);
                }
            }
        }

        private void PlaySoundPlaceBlock()
        {
            if ( ToolbarManager.blockToPlace.name == "Grass" || 
                 ToolbarManager.blockToPlace.name == "Leaves" )
            {
                blockSound.Play(blockSound.grassAudio);
            }
            else if ( ToolbarManager.blockToPlace.name == "Dirt" )
            {
                blockSound.Play(blockSound.dirtAudio);
            }
            else if ( ToolbarManager.blockToPlace.name == "Stone" ||
                      ToolbarManager.blockToPlace.name == "Cobble" ||
                      ToolbarManager.blockToPlace.name == "Brick" ||
                      ToolbarManager.blockToPlace.name == "Glass" )
            {
                blockSound.Play(blockSound.stoneAudio);
            }
            else if ( ToolbarManager.blockToPlace.name == "Wood" ||
                      ToolbarManager.blockToPlace.name == "Planks" )
            {
                blockSound.Play(blockSound.woodAudio);
            }
        }

        private void PlaySoundDestroyBlock(RaycastHit hitInfo)
        {
            if ( hitInfo.transform.name == "Grass_block" ||
                 hitInfo.transform.name == "Grass(Clone)" ||
                 hitInfo.transform.name == "Leaves(Clone)" )
            {
                blockSound.Play(blockSound.grassAudio);
            }
            else if ( hitInfo.transform.name == "Dirt_block" ||
                      hitInfo.transform.name == "Dirt(Clone)" )
            {
                blockSound.Play(blockSound.dirtAudio);
            }
            else if ( hitInfo.transform.name == "Stone_block" ||
                      hitInfo.transform.name == "Stone(Clone)" ||
                      hitInfo.transform.name == "Cobble(Clone)" ||
                      hitInfo.transform.name == "Brick(Clone)" ||
                      hitInfo.transform.name == "Glass(Clone)" )
            {
                blockSound.Play(blockSound.stoneAudio);
            }
            else if ( hitInfo.transform.name == "Wood(Clone)" ||
                      hitInfo.transform.name == "Planks(Clone)" )
            {
                blockSound.Play(blockSound.woodAudio);
            }
        }

        private void DebugDrawRay()
        {
            if(debug)
            {
                Debug.DrawRay(cam.transform.position, cam.transform.forward, Color.red);
            }
        }
    }
}
