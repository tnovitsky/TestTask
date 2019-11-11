using Assets.Scripts.LevelConstructor;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor
{
    [CustomEditor(typeof(LevelConstructor))]
    public class LevelConstructorEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            var levelConstructor = (LevelConstructor)target;

            if (GUILayout.Button("Save level"))
            {
                SaveLevel(levelConstructor);
            }

            if (GUILayout.Button("Clear level"))
            {
                ClearLevel(levelConstructor);
            }
        }

        static void SaveLevel(LevelConstructor levelConstructor)
        {
            var data = AssetDatabase.LoadAssetAtPath<LevelData>($"Assets/Resources/Levels/Level-{levelConstructor.LevelNumber}.asset");
            if (data != null)
            {
                //Если уже есть уровень с таким номером, то спрашиваем заменить его или нет
                if (!EditorUtility.DisplayDialog("Save level",
                    $"Level with number {levelConstructor.LevelNumber} already exist.\n Replace?", "Replace", "Cancel"))
                {
                    return;
                }
            }
            if (levelConstructor.SaveLevel())
                EditorUtility.DisplayDialog("Save level", "Level saved successfuly", "OK");
            else
                EditorUtility.DisplayDialog("Save level", "Oops.. Something goes wrong while saving level. See console", "OK");
        }

        static void ClearLevel(LevelConstructor levelConstructor)
        {
            if (EditorUtility.DisplayDialog("Clear level", "Do you realy want to clear this level?", "Yes", "No"))
            {
                levelConstructor.ClearLevel();
            }
        }

        #region Menu item
        [MenuItem("LevelConstructor/Load level")]
        public static void LoadLevelMenu()
        {
            var levelConstructor = FindObjectOfType<LevelConstructor>();
            if (levelConstructor != null)
            {
                var path = EditorUtility.OpenFilePanel("Load file", "Assets/Resources/Levels", "asset");
                if (!string.IsNullOrEmpty(path))
                {

                    if (path.StartsWith(Application.dataPath))
                    {
                        path = "Assets" + path.Substring(Application.dataPath.Length);
                    }
                    levelConstructor.LoadLevel(path);
                }
            }
            else
            {
                Debug.LogWarning("Must open construcotr scene first");
            }
        }

        [MenuItem("LevelConstructor/Save level")]
        public static void SaveLeveMenu()
        {
            var levelConstructor = FindObjectOfType<LevelConstructor>();
            if (levelConstructor != null)
            {
                SaveLevel(levelConstructor);
            }
            else
            {
                Debug.LogWarning("Must open construcotr scene first");
            }
        }

        [MenuItem("LevelConstructor/Clear level")]
        public static void ClearLevelMenu()
        {
            var levelConstructor = FindObjectOfType<LevelConstructor>();
            if (levelConstructor != null)
            {
                ClearLevel(levelConstructor);
            }
            else
            {
                Debug.LogWarning("Must open construcotr scene first");
            }
        }
        #endregion
    }
}
