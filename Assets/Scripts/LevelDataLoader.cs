using Assets.Scripts.LevelConstructor;
using UnityEngine;

namespace Assets.Scripts
{
    public class LevelDataLoader : MonoBehaviour
    {
        public int LevelsCount => _levels?.Length ?? 0;

        private LevelData[] _levels;
        private int _currentLevelIndx;

        private void Start()
        {
            _levels = Resources.LoadAll<LevelData>("Levels");
        }

        public bool HasMoveLevels()
        {
            if (_currentLevelIndx > LevelsCount - 1) return false;
            var levelData = _levels[_currentLevelIndx];
            return levelData != null && levelData.IsDataOk(); //проверка на целостность данных
        }

        public LevelData GetNextLevelData()
        {
            return _levels[_currentLevelIndx++];
        }
    }
}
