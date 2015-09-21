using System;
using System.IO;
using System.Linq;
using UnityEditor;

public class BuildScript
{
    private static readonly string _versionNumber;
    private static readonly string _buildNumber;

    static BuildScript()
    {
        _versionNumber = Environment.GetEnvironmentVariable("VERSION_NUMBER");
        if (string.IsNullOrEmpty(_versionNumber))
            _versionNumber = "1.0.0.0";

        _buildNumber = Environment.GetEnvironmentVariable("BUILD_NUMBER");
        if (string.IsNullOrEmpty(_buildNumber))
            _buildNumber = "1";

        PlayerSettings.bundleVersion = _versionNumber;
        
        //We have to call this explicitly because [InitializeOnLoad] won't trigger again during a build
        GameVersionUpdater.Update();
    }

    static void CheckDir(string dir)
    {
        if (!Directory.Exists(dir))
            Directory.CreateDirectory(dir);
    }

    static string[] GetScenes()
    {
        return EditorBuildSettings.scenes
            .Where(s => s.enabled)
            .Select(s => s.path)
            .ToArray();
    }

    static void Android()
    {
        CheckDir("build");
        int versionCode;
        int.TryParse(_buildNumber, out versionCode);
        PlayerSettings.Android.bundleVersionCode = versionCode;
        //PlayerSettings.Android.keyaliasName = "StickmanEPIC";
        //PlayerSettings.Android.keyaliasPass =
        //    PlayerSettings.Android.keystorePass = "hcents3@";
        //PlayerSettings.Android.keystoreName = Path.GetFullPath(@"NuGet\Android\Epic2.keystore").Replace('\\', '/');
        BuildPipeline.BuildPlayer(GetScenes(), "build/UnityBuild.apk", BuildTarget.Android, BuildOptions.None);
    }

    static void iOS()
	{
        CheckDir("Scratch/Xcode");
        BuildPipeline.BuildPlayer(GetScenes(), "Scratch/Xcode", BuildTarget.iOS, BuildOptions.None);
	}
}
