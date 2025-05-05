using UnityEngine;
using UnityEngine;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;

public class LevelOneContoller : MonoBehaviour
{
    [SerializeField] private GameObject winArt;
    [SerializeField] private List<GameObject> buttons;
    [Header("Ubi")]
    [SerializeField] private GameObject _UbiStand;
    [Header("Bed")]
    [SerializeField] private SpriteRenderer _bed;
    [SerializeField] private Sprite _normalBed, _bedWithUbi;
    [Header("Text")]
    [SerializeField] private GameObject _ubiText;
    [Header("Song")]
    [SerializeField] private GameObject _ubiSing;

    [Header("LevelsToWin")] 
    [SerializeField] private VoiceSO voice;
    [SerializeField] private TextThrower _textThrower;
    public List<Level> levels;
    
    //----------------
    [System.Serializable]
    public class Level
    {
        public LocString textLine;
    }
    //----------------
    private PopUpAnim _popAnim;
    public int currentLevel = 0;
    void Start()
    {
        _popAnim = GetComponent<PopUpAnim>();
        _textThrower.ThrowText(levels[currentLevel].textLine,voice);
    }

    public bool CheckLastGame()
    {
        return currentLevel + 3 > levels.Count;
    }
    public async UniTask NextLevel(int id)
    {
        var chechers = new List<int>() { 0, 0, 0, 1, 1, 2, 2, 2, 1, 0, 4 };

        if (chechers[currentLevel + 1]  != id)
            return;
        if (currentLevel + 1 >= levels.Count)
        {
            
        }
        else
        {
            currentLevel++;
            await _textThrower.ThrowText(levels[currentLevel].textLine,voice);
        }
    }
    public async void SelectOption(int id)
    {
        foreach (var VARIABLE in buttons)
        {
            VARIABLE.SetActive(false);
        }
        _UbiStand.SetActive(false);
        if (id == 0)
        {
            _bed.sprite = _bedWithUbi;
            _bed.transform.DOPunchScale(Vector3.one * 0.5f, 0.3f, 1, 0.5f)
                .SetEase(Ease.OutQuad);
        }

        if (id == 1)
        {
            _ubiText.SetActive(true);
            _ubiText.transform.DOPunchScale(Vector3.one * 0.5f, 0.3f, 1, 0.5f)
                .SetEase(Ease.OutQuad);
        }
        if (id == 2)
        {
            _ubiSing.SetActive(true);
            _ubiSing.transform.DOPunchScale(Vector3.one * 0.5f, 0.3f, 1, 0.5f)
                .SetEase(Ease.OutQuad);
        }

        await UniTask.Delay(100);
        await _popAnim.ShowMyPops(id);
        ReturnAll();
    }

    public async UniTask ReMoveUp()
    {
        await winArt.transform.DOLocalMoveY(0, 0.7f)
            .SetEase(Ease.OutCubic)
            .AsyncWaitForCompletion();
    }
    private void ReturnAll()
    {
        _ubiText.SetActive(false);
        _ubiSing.SetActive(false);
        _UbiStand.SetActive(true);
        _bed.sprite = _normalBed;
    }
}
