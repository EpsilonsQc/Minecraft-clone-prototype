using UnityEngine;
using Minecraft.Player; // using player namespace

namespace Minecraft.Core
{
    public class Block : MonoBehaviour
    {
        [Header("Block face")]
        [SerializeField] private Transform topBlockFace;
        [SerializeField] private Transform bottomBlockFace;
        [SerializeField] private Transform leftBlockFace;
        [SerializeField] private Transform rightBlockFace;
        [SerializeField] private Transform frontBlockFace;
        [SerializeField] private Transform backBlockFace;

        [Header("Layer mask")]
        [SerializeField] private LayerMask layerMask;

        [Header("Player transform")]
        [SerializeField] private Transform player;

        private bool isExploded = false;

        private Transform xChunkToUpdate;
        private Transform zChunkToUpdate;

        private void Awake()
        {
            TerrainGenerator.BlockFaces = 6; // 6 faces per block
        }

        private void Start()
        {
            UpdateBlockFaces();

            backBlockFace = transform.GetChild(0);
            frontBlockFace = transform.GetChild(1);
            rightBlockFace = transform.GetChild(2);
            leftBlockFace = transform.GetChild(3);
            topBlockFace = transform.GetChild(4);
            bottomBlockFace = transform.GetChild(5);

            if (transform.position.y == 0)
            {
                bottomBlockFace.GetComponent<Renderer>().enabled = false;
            }

            // handles the case of tree prefabs
            if (name == "Wood(Clone)" && transform.parent.name == "Tree"|| name == "Leaves(Clone)" && transform.parent.name == "Tree")
            {
                transform.parent = transform.parent.parent;
            }
        }

        public void UpdateBlockFaces()
        {
            //up
            if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), Vector3.up, 0.3f, layerMask))
            {
                topBlockFace.GetComponent<Renderer>().enabled = false;
                topBlockFace.GetComponent<Collider>().enabled = false;
                TerrainGenerator.BlockFaces -= 1;
            }
            else
            {
                topBlockFace.GetComponent<Renderer>().enabled = true;
                topBlockFace.GetComponent<Collider>().enabled = true;
                TerrainGenerator.BlockFaces += 1;
            }

            //down
            if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z), Vector3.down, 0.3f, layerMask))
            {
                bottomBlockFace.GetComponent<Renderer>().enabled = false;
                bottomBlockFace.GetComponent<Collider>().enabled = false;
                TerrainGenerator.BlockFaces -= 1;
            }
            else if(transform.position.y != 0)
            {
                bottomBlockFace.GetComponent<Renderer>().enabled = true;
                bottomBlockFace.GetComponent<Collider>().enabled = true;
                TerrainGenerator.BlockFaces += 1;
            }

            //right
            if (Physics.Raycast(new Vector3(transform.position.x - 0.5f, transform.position.y, transform.position.z), Vector3.left, 0.3f, layerMask))
            {
                rightBlockFace.GetComponent<Renderer>().enabled = false;
                rightBlockFace.GetComponent<Collider>().enabled = false;
                TerrainGenerator.BlockFaces -= 1;
            }
            else
            {
                rightBlockFace.GetComponent<Renderer>().enabled = true;
                rightBlockFace.GetComponent<Collider>().enabled = true;
                TerrainGenerator.BlockFaces += 1;
            }

            //left
            if (Physics.Raycast(new Vector3(transform.position.x + 0.5f, transform.position.y, transform.position.z), Vector3.right, 0.3f, layerMask))
            {
                leftBlockFace.GetComponent<Renderer>().enabled = false;
                leftBlockFace.GetComponent<Collider>().enabled = false;
                TerrainGenerator.BlockFaces -= 1;
            }
            else
            {
                leftBlockFace.GetComponent<Renderer>().enabled = true;
                leftBlockFace.GetComponent<Collider>().enabled = true;
                TerrainGenerator.BlockFaces += 1;
            }

            //front
            if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y, transform.position.z + 0.5f), Vector3.forward, 0.3f, layerMask))
            {
                frontBlockFace.GetComponent<Renderer>().enabled = false;
                frontBlockFace.GetComponent<Collider>().enabled = false;
                TerrainGenerator.BlockFaces -= 1;
            }
            else
            {
                frontBlockFace.GetComponent<Renderer>().enabled = true;
                frontBlockFace.GetComponent<Collider>().enabled = true;
                TerrainGenerator.BlockFaces += 1;
            }

            //back
            if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y, transform.position.z - 0.5f), Vector3.back, 0.3f, layerMask))
            {
                backBlockFace.GetComponent<Renderer>().enabled = false;
                backBlockFace.GetComponent<Collider>().enabled = false;
                TerrainGenerator.BlockFaces -= 1;
            }
            else
            {
                backBlockFace.GetComponent<Renderer>().enabled = true;
                backBlockFace.GetComponent<Collider>().enabled = true;
                TerrainGenerator.BlockFaces += 1;
            }
        }

        private void OnDestroy()
        {
            if (isExploded)
            {
                transform.parent.GetComponent<TerrainChunk>().UpdateBlockFacesInChunk();
                if (transform.position.x - transform.parent.position.x == 9)
                {
                    // 1 right
                    RaycastHit chunkChecker;
                    Physics.Raycast(new Vector3(transform.position.x + 0.5f, transform.position.y, transform.position.z), Vector3.right, out chunkChecker, 0.3f);

                    if (chunkChecker.collider != null)
                    {
                        Debug.DrawRay(new Vector3(transform.position.x + 0.5f, transform.position.y, transform.position.z), Vector3.right, Color.red, 1);
                        // 2 right
                        chunkChecker.transform.parent.GetComponent<TerrainChunk>().UpdateBlockFacesInChunk();
                    }
                }

                if (transform.position.x - transform.parent.position.x == 0)
                {
                    // 1 left
                    RaycastHit chunkChecker;
                    Physics.Raycast(new Vector3(transform.position.x - 0.5f, transform.position.y, transform.position.z), Vector3.left, out chunkChecker, 0.3f);

                    if (chunkChecker.collider != null)
                    {
                        Debug.DrawRay(new Vector3(transform.position.x - 0.5f, transform.position.y, transform.position.z), Vector3.left, Color.blue, 1);
                        // 2 left
                        chunkChecker.transform.parent.GetComponent<TerrainChunk>().UpdateBlockFacesInChunk();
                    }
                }

                if (transform.position.z - transform.parent.position.z == 9)
                {
                    // 1 forward
                    RaycastHit chunkChecker;
                    Physics.Raycast(new Vector3(transform.position.x, transform.position.y, transform.position.z + 0.5f), Vector3.forward, out chunkChecker, 0.3f);

                    if (chunkChecker.collider != null)
                    {
                        Debug.DrawRay(new Vector3(transform.position.x, transform.position.y, transform.position.z + 0.5f), Vector3.forward, Color.green, 1);
                        // 2 forward
                        chunkChecker.transform.parent.GetComponent<TerrainChunk>().UpdateBlockFacesInChunk();
                    }
                }

                if (transform.position.z - transform.parent.position.z == 0)
                {
                    // 1 back
                    RaycastHit chunkChecker;
                    Physics.Raycast(new Vector3(transform.position.x, transform.position.y, transform.position.z - 0.5f), Vector3.back, out chunkChecker, 0.3f);

                    if (chunkChecker.collider != null && chunkChecker.transform != player)
                    {
                        Debug.DrawRay(new Vector3(transform.position.x, transform.position.y, transform.position.z - 0.5f), Vector3.back, Color.yellow, 1);
                        // 2 back
                        chunkChecker.transform.parent.GetComponent<TerrainChunk>().UpdateBlockFacesInChunk();
                    }
                }
            }

            if(transform == BlockManager.blockToDestroy)
            {
                if (BlockManager.xChunkToUpdate != null)
                {
                    BlockManager.xChunkToUpdate.GetComponent<TerrainChunk>().UpdateBlockFacesInChunk();
                    BlockManager.xChunkToUpdate = null;
                }

                if (BlockManager.zChunkToUpdate != null)
                {
                    BlockManager.zChunkToUpdate.GetComponent<TerrainChunk>().UpdateBlockFacesInChunk();
                    BlockManager.zChunkToUpdate = null;
                }

                transform.parent.GetComponent<TerrainChunk>().UpdateBlockFacesInChunk();
            }
        }
    }
}
