using UnityEngine;
using Cysharp.Threading.Tasks;
using DG.Tweening;

public class Level3Controller : MonoBehaviour
{
    [SerializeField] private TextThrower normalTexter;
    [SerializeField] private GameObject renderObject;
    [SerializeField] private SpriteRenderer render;
    [SerializeField] private GameObject buttonOne;
    [SerializeField] private Sprite second, third;
    private bool buttonTrigger = false;
    private Vector3 myScale;

    private void Awake()
    {
        myScale = renderObject.transform.localScale;
    }

    private async void Start()
    {
        await normalTexter.ThrowText(new LocString("Any one of us...", "Любой из нас..."), R.normalVoice);
        await UniTask.Delay(800*2);

        buttonOne.SetActive(true);
        buttonTrigger = false;
        await UniTask.WaitUntil(() => buttonTrigger);
        
        await IncreaseAndChange(second);
        
        await normalTexter.ThrowText(new LocString("It's just a set of...", "Всего лишь набор..."), R.normalVoice);
        await UniTask.Delay(800*2);

        buttonOne.SetActive(true);
        buttonTrigger = false;
        await UniTask.WaitUntil(() => buttonTrigger);
        
        await IncreaseAndChange(third);
        
        await normalTexter.ThrowText(new LocString("Colored dots", "Разноцветных точек"), R.normalVoice);
        await UniTask.Delay(1200*2);
        await normalTexter.ThrowText(new LocString("The developer didn't expect anything less.", "На меньшее не рассчитывал разработчик"), R.normalVoice);
        await UniTask.Delay(2000*2);
        FindFirstObjectByType<LevelsContoller>().NextLevel();
    }

    public async UniTask IncreaseAndChange(Sprite s)
    {
        G.AudioManager.PlayWithRandomPitch(R.Audio.CloudShow, 0.15f);
        await renderObject.transform.DOScale(Vector3.one * 75, 0.8f)
            .SetEase(Ease.OutQuint)
            .AsyncWaitForCompletion();
        render.sprite = s;
        renderObject.transform.localScale = myScale;
    }

public void ButtonTrigger()
    {
        buttonTrigger = true;
    }
}
