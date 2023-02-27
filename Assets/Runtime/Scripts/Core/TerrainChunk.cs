using UnityEngine;

namespace Minecraft.Core
{
    public class TerrainChunk : MonoBehaviour
    {
        [Header("Blocks")]
        [SerializeField] protected Transform stoneBlock;
        [SerializeField] protected Transform dirtBlock;
        [SerializeField] protected Transform grassBlock;

        [Header("Props")]
        [SerializeField] protected Transform rose;
        [SerializeField] protected Transform tallGrass;
        [SerializeField] protected Transform tree;

        public static Texture2D heightMap;

        private Transform[] blocksInChunk;
        private int chance;

        private void Start()
        {
            GenerateChunk();
        }

        private void GenerateChunk()
        {
            // loop through all blocks on the X axis
            for (int xOffset = (int)transform.position.x; xOffset <(transform.position.x + 10); xOffset++) 
            {
                // loop through all blocks on the Z axis for each X axis
                for (int zOffset = (int)transform.position.z; zOffset < (transform.position.z + 10); zOffset++)
                {
                    // loop through all the blocks on the Y axis for each Z axis
                    for (int yOffset = 0; yOffset < (heightMap.GetPixel(xOffset, zOffset).r * 100); yOffset++)
                    {
                        // grass
                        if ((int)(heightMap.GetPixel(xOffset, zOffset).r * 100) - yOffset == 0)
                        {
                            Transform block = Instantiate(grassBlock, new Vector3(xOffset, yOffset, zOffset), Quaternion.identity);
                            block.parent = transform;
                            block.name = ("Grass_block");
                            chance = Random.Range(1, 201);

                            if (chance <= 20)
                            {
                                Transform grass = Instantiate(tallGrass, new Vector3(xOffset, yOffset + 1, zOffset), Quaternion.identity);
                                grass.parent = block;
                                grass.name = ("Tall_Grass");
                            }

                            if (chance == 199)
                            {
                                Transform grass = Instantiate(tree, new Vector3(xOffset, yOffset + 1, zOffset), Quaternion.identity);
                                grass.parent = transform;
                                grass.name = ("Tree");
                            }

                            if (chance == 200)
                            {
                                Transform grass = Instantiate(rose, new Vector3(xOffset, yOffset + 1, zOffset), Quaternion.identity);
                                grass.parent = block;
                                grass.name = ("Rose");
                            }
                        }

                        // dirt
                        if ((int)(heightMap.GetPixel(xOffset, zOffset).r * 100) - yOffset < 4 && (heightMap.GetPixel(xOffset, zOffset).r * 100) - yOffset > 1)
                        {
                            Transform block = Instantiate(dirtBlock, new Vector3(xOffset, yOffset, zOffset), Quaternion.identity);
                            block.parent = transform;
                            block.name = ("Dirt_block");
                        }

                        // stone
                        if ((int)(heightMap.GetPixel(xOffset, zOffset).r * 100) - yOffset > 3)
                        {
                            Transform block = Instantiate(stoneBlock, new Vector3(xOffset, yOffset, zOffset), Quaternion.identity);
                            block.parent = transform;
                            block.name = ("Stone_block");
                        }
                    }
                }
            }
        }

        public void UpdateBlockFacesInChunk()
        {
            blocksInChunk = GetComponentsInChildren<Transform>();

            foreach (Transform block in blocksInChunk)
            {
                if (block != null && block.GetComponent<Block>())
                {
                    block.GetComponent<Block>().UpdateBlockFaces();
                }
            }
        }
    }
}
