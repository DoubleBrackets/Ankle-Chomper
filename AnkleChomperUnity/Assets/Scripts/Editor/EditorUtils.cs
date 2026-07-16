using System;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class EditorUtils : MonoBehaviour
    {
        // These need to match the build player settings
        public const string CompanyName = "TLV";
        public const string ProductName = "TheLastVideogame";

        [MenuItem("AnkleChomper/Persistent Data Folder")]
        public static void OpenPersistentDataFolder()
        {
            Debug.Log($"Opening {Application.persistentDataPath}");
            Application.OpenURL(Application.persistentDataPath);
        }

        [MenuItem("AnkleChomper/Player Logs")]
        public static void OpenBuildLogsFolder()
        {
            try
            {
#if UNITY_EDITOR_WIN
                Application.OpenURL(Application.persistentDataPath);
#elif UNITY_EDITOR_OSX
                Application.OpenURL($"~/Library/Logs/{CompanyName}/{ProductName}");
#endif
            }
            catch (Exception e)
            {
                Debug.LogError($"Unable to open logs folder: {e.Message}");
            }
        }
    }
}