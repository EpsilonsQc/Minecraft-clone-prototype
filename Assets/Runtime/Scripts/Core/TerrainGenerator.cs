using UnityEngine;

namespace Minecraft.Core
{
    public class TerrainGenerator : MonoBehaviour
    {
        public static int BlockFaces = 0; // 6 faces per block

        private int textureWidth = 200; // width of the texture
        private int textureHeight = 200; // height of the texture

        private float scale1 = 1f; // scale of the first noise
        private float scale2 = 10f; // scale of the second noise
        private float scale3 = 20f; // scale of the third noise

        private float offsetX; // X-Axis offset
        private float offsetY; // Y-Axis offset

        private void Awake()
        {
            offsetX = Random.Range(0, 99999); // random X-Axis offset
            offsetY = Random.Range(0, 99999); // random Y-Axis offset

            TerrainChunk.heightMap = GenerateHeightMap(); // generate the height map
            if (GetComponent<MeshRenderer>() != null)
            {
                GetComponent<MeshRenderer>().material.mainTexture = TerrainChunk.heightMap; // set the height map as the main texture
            }
        }

        private Texture2D GenerateHeightMap()
        {
            Texture2D heightMap = new Texture2D(textureWidth, textureHeight); // create a new texture

            for (int x = 0; x < textureWidth; x++)
            {
                for (int y = 0; y < textureHeight; y++)
                {
                    Color color = CalculateColor(x, y); // calculate the color of the pixel
                    heightMap.SetPixel(x, y, color); // set the pixel color
                }
            }

            heightMap.Apply(); // apply the changes to the texture

            return heightMap; // return the texture
        }

        private Color CalculateColor(int x, int y)
        {
            float xCoord1 = (float)x / textureWidth * scale1 + offsetX; // calculate the first x coordinate
            float yCoord1 = (float)y / textureHeight * scale1 + offsetY; // calculate the first y coordinate

            float xCoord2 = (float)x / textureWidth * scale2 + offsetX; // calculate the second x coordinate
            float yCoord2 = (float)y / textureHeight * scale2 + offsetY; // calculate the second y coordinate

            float xCoord3 = (float)x / textureWidth * scale3 + offsetX; // calculate the third x coordinate
            float yCoord3 = (float)y / textureHeight * scale3 + offsetY; // calculate the third y coordinate

            float sample1 = Mathf.PerlinNoise(xCoord1, yCoord1) / 15; // calculate the first sample
            float sample2 = Mathf.PerlinNoise(xCoord2, yCoord2) / 15; // calculate the second sample
            float sample3 = Mathf.PerlinNoise(xCoord3, yCoord3) / 15; // calculate the third sample

            return new Color(sample1 + sample2 + sample3, sample1 + sample2 + sample3, sample1 + sample2 + sample3); // return the color
        }
    }
}
