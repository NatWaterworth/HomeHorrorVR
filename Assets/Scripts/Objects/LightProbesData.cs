using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(fileName = "LightProbesData", menuName = "Lighting/Light Probes Data")]
public class LightProbesData : ScriptableObject
{
    public SphericalHarmonicsL2[] bakedProbes;
}
