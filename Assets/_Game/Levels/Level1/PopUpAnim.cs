using System;
using UnityEngine;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;

public class PopUpAnim : MonoBehaviour
{
    [SerializeField] private float timeToShow = 0.7f;
    [SerializeField] private float timeToMoveUp = 1.2f;
    [SerializeField] private float yLen = 1f;
    [SerializeField] private GameObject upCloud;
    [SerializeField] private List<GameObject> sleepClouds;
    [SerializeField] private List<GameObject> writingClouds;
    [SerializeField] private List<GameObject> singClouds;
    [SerializeField] private List<GameObject> buttons;

    private float startYUpCloudPos;

    private void Awake()
    {
        startYUpCloudPos = upCloud.transform.localPosition.y;
    }

    async void Start()
    {
        DiactivateAll();
    }

    public async UniTask ShowMyPops(int id)
    {
        if (id == 0)
        {
            await ShowPops(sleepClouds, R.Audio.pop_fx);
        }
        if (id == 1)
        {
            await ShowPops(writingClouds, R.Audio.pencil_writing);
        }
        if (id == 2)
        {
            await ShowPops(singClouds, R.Audio.Lia);
        }

        if (GetComponent<LevelOneContoller>().CheckLastGame())
        {
            GetComponent<LevelOneContoller>().NextLevel(id);
            GetComponent<LevelOneContoller>().ReMoveUp();
            var realPlace = transform.GetChild(0);
            realPlace.DOLocalMoveY(realPlace.transform.localPosition.y - 10, timeToMoveUp)
                .SetEase(Ease.OutCubic);
            await UniTask.Delay(2050);
            FindFirstObjectByType<LevelsContoller>().NextLevel();
        }
        else
        {
            await MoveUp();
            GetComponent<LevelOneContoller>().NextLevel(id);
            DiactivateAll();
            foreach (var VARIABLE in buttons)
            {
                VARIABLE.SetActive(true);
            }
        }
    }
    private async UniTask ShowPops(List<GameObject> clouds, AudioClip sound)
    {
        var listToIterate = clouds;
        foreach (var cloud in listToIterate)
        {
            var realScale = cloud.transform.localScale.x;
            cloud.transform.localScale = Vector3.zero;
            cloud.SetActive(true);
            G.AudioManager.PlayWithRandomPitch(sound, 0.1f);
            _ = cloud.transform.DOScale(Vector3.one * realScale, timeToShow * 1.5f)
                .SetEase(Ease.OutElastic)
                .AsyncWaitForCompletion();
            await UniTask.Delay((int)(1000 * timeToShow));
        }
        //await UniTask.Delay((int)(timeToShow * 800));
        G.AudioManager.PlayWithRandomPitch(R.Audio.CloudShow, 0.1f);
        upCloud.SetActive(true);
        _ = upCloud.transform.DOLocalMoveY(upCloud.transform.localPosition.y + yLen, timeToShow / 1f)
            .AsyncWaitForCompletion();
        await UniTask.Delay(250);
    }

    public async UniTask MoveUp()
    {
        var realPlace = transform.GetChild(0);
        var fakePlace = transform.GetChild(1);

        realPlace.DOLocalMoveY(realPlace.transform.localPosition.y - 10, timeToMoveUp)
            .SetEase(Ease.OutCubic);
        await fakePlace.DOLocalMoveY(fakePlace.transform.localPosition.y - 10, timeToMoveUp)
            .SetEase(Ease.OutCubic)
            .AsyncWaitForCompletion();
        DiactivateAll();
        realPlace.transform.localPosition = Vector3.zero;
        fakePlace.transform.localPosition = new Vector3(0, 10, 0);
        upCloud.transform.localPosition =
            new Vector3(upCloud.transform.localPosition.x, startYUpCloudPos, upCloud.transform.localPosition.z);
    }

    private void DiactivateAll()
    {
        upCloud.SetActive(false);
        foreach (var VARIABLE in sleepClouds)
        {
            VARIABLE.SetActive(false);
        }
        foreach (var VARIABLE in singClouds)
        {
            VARIABLE.SetActive(false);
        }
        foreach (var VARIABLE in writingClouds)
        {
            VARIABLE.SetActive(false);
        }
    }
}
