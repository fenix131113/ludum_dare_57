using System;
using UnityEditor;
using UnityEngine;

public static class ManualConsole
{
    [MenuItem("Tools/Run GC Collect")]
    public static void RunGC()
    {
        EditorUtility.UnloadUnusedAssetsImmediate();
        GC.Collect();
        Debug.Log("Память очищена!");
    }
}
