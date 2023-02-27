using UnityEngine;
using UnityEngine.UI;

namespace Minecraft.UI.HUD
{
    public class Toolbar : MonoBehaviour
    {
        [Header("Parameters")]
        [SerializeField] private int blockNum;
        [SerializeField] private Transform Select;

        private void Start()
        {
            Select = transform.GetChild(0); // get the first child of the toolbar
        }

        private void Update()
        {
            if (ToolbarManager.block == blockNum)
            {
                Select.GetComponent<RawImage>().enabled = true; // show the selected block
            }
            if (ToolbarManager.block != blockNum)
            {
                Select.GetComponent<RawImage>().enabled = false; // hide the selected block
            }
        }
    }
}
