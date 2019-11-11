using System;
using UnityEngine;

namespace Assets.Scripts
{
    public class UIController : MonoBehaviour
    {
        [SerializeField]
        private GameController GameController;

        [SerializeField]
        private GameObject MainMenu;

        [SerializeField]
        private GameObject NextLevelMenu;

        [SerializeField]
        private GameObject EndGameMenu;

        /// <summary>
        /// Нажатие на кнопку старта игры
        /// </summary>
        public event Action OnStartLevel;

        /// <summary>
        /// нажатие на кнопку выхода в главное меню
        /// </summary>
        public event Action OnRestartGame;

        private void Start()
        {
            GameController.OnLevelFinished += OnLevelFinished;
        }

        private void OnDestroy()
        {
            GameController.OnLevelFinished -= OnLevelFinished;
        }

        public void OnLevelFinished(bool isLastLevel)
        {
            if (!isLastLevel)
            {
                NextLevelMenu.SetActive(true);
            }
            else
            {
                EndGameMenu.SetActive(true);
            }
        }

        public void StartNextLevel()
        {
            NextLevelMenu.SetActive(false);
            MainMenu.SetActive(false);
            OnStartLevel?.Invoke();
        }

        public void OnMainMenuButtonClicked()
        {
            OnRestartGame?.Invoke();
        }
    }
}
