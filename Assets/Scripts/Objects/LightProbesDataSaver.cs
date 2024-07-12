using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public class LightProbesDataSaver : EditorWindow
{
    private LightProbesData selectedLightProbesData;

    [MenuItem("Tools/Save Baked Light Probes")]
    public static void ShowWindow()
    {
        GetWindow<LightProbesDataSaver>("Save Baked Light Probes");
    }

    private void OnGUI()
    {
        GUILayout.Label("Save Baked Light Probes", EditorStyles.boldLabel);

        selectedLightProbesData = (LightProbesData)EditorGUILayout.ObjectField("Light Probes Data", selectedLightProbesData, typeof(LightProbesData), false);

        if (GUILayout.Button("Save Baked Probes"))
        {
            SaveBakedProbes();
        }
    }

    private void SaveBakedProbes()
    {
        if (selectedLightProbesData == null)
        {
            Debug.LogError("Please assign a LightProbesData object.");
            return;
        }

        // Get the baked light probes from the LightmapSettings
        SphericalHarmonicsL2[] bakedProbes = LightmapSettings.lightProbes.bakedProbes;

        if (bakedProbes == null || bakedProbes.Length == 0)
        {
            Debug.LogError("No baked probes found in LightmapSettings.");
            return;
        }

        // Assign the baked probes to the selected LightProbesData object
        selectedLightProbesData.bakedProbes = bakedProbes;

        // Mark the asset as dirty to ensure it gets saved
        EditorUtility.SetDirty(selectedLightProbesData);
        AssetDatabase.SaveAssets();

        Debug.Log("Baked probes saved successfully to " + selectedLightProbesData.name + ".");
    }
}
