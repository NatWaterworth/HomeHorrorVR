using UnityEngine;

[CreateAssetMenu(fileName = "LightmapSet", menuName = "Lighting/Lightmap Set")]
public class LightmapSet : ScriptableObject
{
    public Texture2D[] lightmapColorTextures;
    public Texture2D[] lightmapDirTextures;
    public Texture2D[] shadowMasks;
    public ReflectionProbe[] reflectionProbes;
}