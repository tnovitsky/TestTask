#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Scripts.LevelConstructor
{
    public class LevelConstructor : MonoBehaviour
    {
        public int LevelNumber;

        public Level Level;

        public TileBase StartTile;
        public TileBase FinishTile;

        public void LoadLevel(string path)
        {
            var levelData = AssetDatabase.LoadAssetAtPath<LevelData>(path);
            if (levelData != null)
            {
                Level.LoadLevel(levelData);
                LevelNumber = levelData.LevelNumber;
            }
        }

        public bool SaveLevel()
        {
            var levelData = ScriptableObject.CreateInstance<LevelData>();
            try
            {
                AssetDatabase.CreateAsset(levelData, $"Assets/Resources/Levels/Level-{LevelNumber}.asset");
                levelData.LevelNumber = LevelNumber;
                levelData.InitFloorData(Level.Floor, StartTile, FinishTile);
                levelData.InitObstacles(Level.Obstacles);
                levelData.InitMovableObjects(Level.MovableObjects);
                EditorUtility.SetDirty(levelData);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
                return false;
            }
        }

        public void ClearLevel()
        {
            Level.ClearLevel();
        }
    }
}
#endif