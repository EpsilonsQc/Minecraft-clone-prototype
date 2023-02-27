using UnityEngine;

namespace Minecraft.Cam
{
    public class CameraFollow : MonoBehaviour
    {
        [Header("Camera Follow")]
        [SerializeField] private Transform target;
        [SerializeField] private Vector3 offset;

        private void LateUpdate()
        {
            FollowTarget();
        }

        private void FollowTarget()
        {
            transform.position = target.position + offset; // Follow the target
        }
    }
}