using UnityEngine;

[CreateAssetMenu(fileName = "NewVoice", menuName = "MyScriptableObject/Voice")]
public class VoiceSO : ScriptableObject
{
    public AudioClip voice;
    public Material textMaterial;
    public int deltaSound;
    public Color color;
}
