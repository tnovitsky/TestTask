using System;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private LevelDataLoader LevelLoader;

    [SerializeField]
    private InputController InputController;

    [SerializeField]
    private UIController UIController;

    [SerializeField]
    private Player Player;

    [SerializeField]
    private Level Level;

    public event Action<bool> OnLevelFinished;

    private bool _isGameActive;

    private void Start()
    {
        InputController.OnMove += InputControllerMovePlayer;
        InputController.OnRestart += InputControllerRestartLevel;
        InputController.OnEscape += RestartGame;

        UIController.OnStartLevel += StartNextLevel;
        UIController.OnRestartGame += RestartGame;
    }

    private void OnDestroy()
    {
        InputController.OnMove -= InputControllerMovePlayer;
        InputController.OnRestart -= InputControllerRestartLevel;
        InputController.OnEscape -= RestartGame;

        UIController.OnStartLevel -= StartNextLevel;
        UIController.OnRestartGame -= RestartGame;
    }

    public void StartNextLevel()
    {
        if (LevelLoader.HasMoveLevels())
        {
            Player.gameObject.SetActive(true);
            Level.LoadLevel(LevelLoader.GetNextLevelData());
            Player.Move(Level.PlayerStartPosition);
            _isGameActive = true;
        }
    }

    public void InputControllerRestartLevel()
    {
        if (!_isGameActive) return;
        Level.RestartLevel();
        Player.Move(Level.PlayerStartPosition);
    }

    public void InputControllerMovePlayer(Vector2Int direction)
    {
        if (!_isGameActive) return;
        Player.Move(Level.NextPlayerPosition(Player.Position, (Vector3Int)direction));
        if (Level.IsOnFinishCell(Player.Position)) //проверяем на конец уровня
        {
            _isGameActive = false;
            OnLevelFinished?.Invoke(!LevelLoader.HasMoveLevels());
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }
}
