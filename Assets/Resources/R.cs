using UnityEngine;

public static partial class R
{
    public static bool isInited = false;
    public static VoiceSO normalVoice;
    public static VoiceSO whiteVoice;

    public static void InitAll()
    {
        isInited = true;
        R.InitAudio();
        R.normalVoice = Resources.Load<VoiceSO>("TinyVoice");
        R.whiteVoice = Resources.Load<VoiceSO>("WhiteVoice");
    }
}
