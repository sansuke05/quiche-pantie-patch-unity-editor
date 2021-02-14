using UnityEditor;


public static class UnityPackageExporter
{
    [MenuItem("Tools/Export Unitypackage")]
    public static void Export()
    {
        var exportDirs = new []
        {
            "Assets/AliceLaboratory/Editor",
            "Assets/Plugins/UniTask"
        };
        var exportPath = "./QuichePantiePatchEditor.unitypackage";

        AssetDatabase.ExportPackage(exportDirs, exportPath, ExportPackageOptions.Recurse);        
    }
}
