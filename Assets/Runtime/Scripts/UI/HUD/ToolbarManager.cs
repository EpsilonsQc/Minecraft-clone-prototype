using UnityEngine;
using UnityEngine.InputSystem;

namespace Minecraft.UI.HUD
{
    public class ToolbarManager : MonoBehaviour
    {
        public static int block = 1;
        public static Transform blockToPlace;

        [Header("Blocks")]
        [SerializeField] private Transform grassBlock;
        [SerializeField] private Transform dirtBlock;
        [SerializeField] private Transform stoneBlock;
        [SerializeField] private Transform cobbleBlock;
        [SerializeField] private Transform plankBlock;
        [SerializeField] private Transform brickBlock;
        [SerializeField] private Transform glassBlock;
        [SerializeField] private Transform woodBlock;
        [SerializeField] private Transform leaveBlock;

        void Update()
        {
            ToolbarManagement();
        }

        private void ToolbarManagement()
        {
            if (Mouse.current.scroll.ReadValue().y < 0)
            {
                block++;
            }
            else if (Mouse.current.scroll.ReadValue().y > 0)
            {
                block--;
            }

            if (block > 9)
            {
                block = 1;
            }
            else if (block < 1)
            {
                block = 9;
            }

            // block list
            if (block == 1)
            {
                blockToPlace = grassBlock;
            }
            else if (block == 2)
            {
                blockToPlace = dirtBlock;
            }
            else if (block == 3)
            {
                blockToPlace = stoneBlock;
            }
            else if (block == 4)
            {
                blockToPlace = cobbleBlock;
            }
            else if (block == 5)
            {
                blockToPlace = plankBlock;
            }
            else if (block == 6)
            {
                blockToPlace = brickBlock;
            }
            else if (block == 7)
            {
                blockToPlace = glassBlock;
            }
            else if (block == 8)
            {
                blockToPlace = woodBlock;
            }
            else if (block == 9)
            {
                blockToPlace = leaveBlock;
            }
        }

    }
}