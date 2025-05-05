using UnityEngine;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine.UI;

public class LevelTwoController : MonoBehaviour
{
    [Header("Texters")]
    [SerializeField] private TextThrower normalTexter, darkTexter;
    [Header("GameobjectsStart")]
    [SerializeField] private GameObject leftUbi, rightWindow;
    [SerializeField] private GameObject buttonToCloseEyes;
    
    private bool triggerToCloseEyers = false;
    private bool triggerToMove = false;
    [Header("Fade")]
    [SerializeField] private GameObject blackFade;
    [SerializeField] private GameObject standInNowhere;
    [Header("Moving")] 
    [SerializeField] private GameObject first;
    [SerializeField] private GameObject second;
    [SerializeField] private GameObject third;
    [SerializeField] private GameObject buttonToMoveOne;
    [SerializeField] private GameObject buttonToMoveTwo;
    [Header("End")] 
    [SerializeField] private GameObject coolBack;
    [SerializeField] private GameObject endUbi;
    async void Start()
    {
        leftUbi.transform.DOLocalMoveX(-5, 2);
        rightWindow.transform.DOLocalMoveX(6, 2);
        await normalTexter.ThrowText(new LocString("In secret, in a whisper, I say:", "По секрету, по шёпоту я говорю:"), R.normalVoice);
        await UniTask.Delay(900 *2);
        await normalTexter.ThrowText(new LocString("The world exists only when I look.", "Мир существует только, когда я смотрю."), R.normalVoice);
        
        buttonToCloseEyes.SetActive(true);
        await UniTask.WaitUntil(() => triggerToCloseEyers);
        
        G.AudioManager.PlayWithRandomPitch(R.Audio.lampOut, 0.1f);
        await blackFade.transform.DOScale(Vector3.one * 1.6f,0.5f)
            .AsyncWaitForCompletion();
        await normalTexter.ThrowText(new LocString("", ""), R.normalVoice);
        await darkTexter.ThrowText(new LocString("Op, I squeezed my eyes shut, and nothing happened.", "Оп-п-п, зажмурился, и ничего не стало"), R.whiteVoice);
        await UniTask.Delay(900*2);
        
        leftUbi.SetActive(false);
        rightWindow.SetActive(false);
        
        standInNowhere.SetActive(true);
        await darkTexter.ThrowText(new LocString("(Everything is gone)", "(Всё пропало)"), R.whiteVoice);
        await UniTask.Delay(900*2);
        
        standInNowhere.SetActive(false);
        first.SetActive(true);
        await darkTexter.ThrowText(new LocString("No people...", "Ни людей..."), R.whiteVoice);
        buttonToMoveOne.SetActive(true);

        triggerToMove = false;
        await UniTask.WaitUntil(() => triggerToMove);
        
        first.SetActive(false);
        second.SetActive(true);
        await darkTexter.ThrowText(new LocString("No equipment...", "Ни техники..."), R.whiteVoice);
        buttonToMoveTwo.SetActive(true);

        triggerToMove = false;
        await UniTask.WaitUntil(() => triggerToMove);
        //--------
        
        second.SetActive(false);
        third.SetActive(true);
        await darkTexter.ThrowText(new LocString("No architecture...", "Ни архитектуры..."), R.whiteVoice);
        buttonToMoveTwo.SetActive(false);
        await UniTask.Delay(900*2);
        //---
        
        third.SetActive(false);
        coolBack.SetActive(true);
        endUbi.SetActive(true);
        
        await darkTexter.ThrowText(new LocString("I lifted my eyelids...", "Поднял веки..."), R.whiteVoice);
        await UniTask.Delay(700*2);
        await darkTexter.ThrowText(new LocString("", ""), R.whiteVoice);
        G.AudioManager.PlayWithRandomPitch(R.Audio.lampOut, 0.1f);
        await blackFade.transform.DOScale(Vector3.one * 0f,0.5f)
            .AsyncWaitForCompletion();
        await normalTexter.ThrowText(new LocString("And the textures are loaded again!", "И по новой прогружаются текстуры!"), R.normalVoice);
        await UniTask.Delay(1000*2);
        FindFirstObjectByType<LevelsContoller>().NextLevel();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TriggerCloseEyes()
    {
        triggerToCloseEyers = true;
    }

    public void TriggerToMove()
    {
        triggerToMove = true;
    }
}
