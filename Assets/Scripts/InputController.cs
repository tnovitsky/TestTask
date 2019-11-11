using System;
using UnityEngine;

namespace Assets.Scripts
{
    public class InputController : MonoBehaviour
    {
        public event Action<Vector2Int> OnMove;
        public event Action OnRestart;
        public event Action OnEscape;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                OnMove?.Invoke(Vector2Int.left);
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                OnMove?.Invoke(Vector2Int.right);
            }
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                OnMove?.Invoke(Vector2Int.up);
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                OnMove?.Invoke(Vector2Int.down);
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                OnRestart?.Invoke();
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                OnEscape?.Invoke();
            }
        }
    }
}
