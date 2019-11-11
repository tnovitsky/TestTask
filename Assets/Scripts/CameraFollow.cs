using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(Camera))]
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField]
        private Transform Target;

        [SerializeField]
        private float Speed;

        private void Update()
        {
            if (Target == null) return;
            transform.position = Vector3.Lerp(transform.position, Target.position, Speed * Time.deltaTime);
        }
    }
}
