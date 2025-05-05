using UnityEngine;
using Cysharp.Threading.Tasks;
using DG.Tweening;

public class Level5Controller : MonoBehaviour
{
    [SerializeField] private float deltaX, time;
    [SerializeField] private TextThrower normalTexter;
    [SerializeField] private GameObject poleHolder;
    [SerializeField] private SpriteRenderer background;
    [SerializeField] private GameObject ButtonHolder;
    
    private bool buttonTriggered = false;
    private int fromCenter = 0;
    private int inputValue = -1;
    private async void Start()
    {
        await normalTexter.ThrowText(new LocString("And no matter how much you walk around the field in the rye...", "И сколько не гуляй по полю во ржи..."), R.normalVoice);

        while (true)
        {
            ButtonHolder.SetActive(true);
            buttonTriggered = false;
            await UniTask.WaitUntil(() => buttonTriggered);
            ButtonHolder.SetActive(false);
            G.AudioManager.PlayWithRandomPitch(R.Audio.CloudShow,0.1f);
            if (inputValue == 0)
            {
                fromCenter -= 1;
                UpdateBackgroundAlpha();
                await poleHolder.transform.DOLocalMoveX(transform.position.x - deltaX, time)
                    .SetEase(Ease.OutCubic)
                    .AsyncWaitForCompletion();
                
            }
            else if(inputValue == 1)
            {
                fromCenter += 1;
                UpdateBackgroundAlpha();
                await poleHolder.transform.DOLocalMoveX(transform.position.x + deltaX, time)
                    .SetEase(Ease.OutCubic)
                    .AsyncWaitForCompletion();
            }

            poleHolder.transform.localPosition = Vector3.zero;
            if (fromCenter == 5 || fromCenter == -5)
            {
                await normalTexter.ThrowText(new LocString("The PNG-like background will always load somewhere.", "Всегда где-то прогрузится фон как у PNG"), R.normalVoice);
                await UniTask.Delay(1500*2);
                break;
            }
        }
        FindFirstObjectByType<LevelsContoller>().NextLevel();
    }
    private void UpdateBackgroundAlpha()
    {
        float targetAlpha = Mathf.Abs(fromCenter) / 5f; // 0 → 1 (чем дальше от центра, тем ярче)
        background.DOFade(targetAlpha, 0.3f); // Плавное изменение за 0.3 секунды
    }
    public void ClickedButton(int pos)
    {
        inputValue = pos;
        buttonTriggered = true;
    }
}
