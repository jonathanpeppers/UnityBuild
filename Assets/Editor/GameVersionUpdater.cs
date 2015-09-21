using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;

/// <summary>
/// This script updates GameVersion.cs so that GameVersion.Version matches PlayerSettings.bundleVersion from the Unity Editor
/// </summary>
[InitializeOnLoad]
public class GameVersionUpdater
{
    private const string GameVersionPath = "Assets/GameVersion.cs";

    static GameVersionUpdater()
    {
        Update();
    }

    public static void Update()
    {
        if (GameVersion.Version != PlayerSettings.bundleVersion)
        {
            Debug.Log("Updating GameVersion.cs");

            if (!File.Exists(GameVersionPath))
            {
                Debug.LogError("Could not locate GameVersion.cs!");
                return;
            }

            //Inception!
            var regex = new Regex(@"public const string Version = ""(\d+(?:\.\d+)+)""");
            string existingCode = File.ReadAllText(GameVersionPath);

            var match = regex.Match(existingCode);
            if (match.Success)
            {
                string replacement = match.Value.Replace(match.Groups[1].Value, PlayerSettings.bundleVersion);
                existingCode = regex.Replace(existingCode, replacement);
                File.WriteAllText(GameVersionPath, existingCode);
            }
            else
            {
                Debug.LogError("No Regex match for GameVersion.cs!");
            }
        }
    }
}
