using UnityEngine;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;

public class Level6Controller : MonoBehaviour
{
    [SerializeField] private TextThrower normalThrower;
    [SerializeField] private GameObject coolPanel;
    [SerializeField] private GameObject buttonToClose;

    private bool panelBool = false;
    async void Start()
    {
        await normalThrower.ThrowText(new LocString("And they go cycle after cycle, it will always be like this.", "И они ходят цикл за циклом, вечно будет так"), R.normalVoice);
        
        await UniTask.WaitUntil(() => panelBool);
        G.AudioManager.PlaySound(R.Audio.CloudShow);
        await coolPanel.transform.DOScale(Vector3.one * 1f,1f)
            .SetEase(Ease.OutBack)
            .AsyncWaitForCompletion();
        await normalThrower.ThrowText(new LocString("This is not a feature, but an ancient bug.", "Это не фича, а древний баг"), R.normalVoice);
        buttonToClose.SetActive(true);

        panelBool = false;
        await UniTask.WaitUntil(() => panelBool);

        await coolPanel.GetComponent<BreakAnimation>().PlayAnimation();
        FindFirstObjectByType<LevelsContoller>().NextLevel();
    }

    public void ActivatePanel()
    {
        panelBool = true;
    }
}
