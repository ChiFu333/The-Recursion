using UnityEngine;
using Cysharp.Threading.Tasks;
using DG.Tweening;

public class Level4Controller : MonoBehaviour
{
    [SerializeField] private TextThrower normalTexter;
    public int clickedNPC = 0;
    private async void Start()
    {
        await normalTexter.ThrowText(new LocString("And no matter how much you walk down the street...", "И сколько не гуляй по улице..."), R.normalVoice);

        while (true)
        {
            if (clickedNPC == 5)
            {
                await normalTexter.ThrowText(new LocString("There will always be familiar NPCs", "Всегда встретятся знакомые NPC"), R.normalVoice);
                await UniTask.Delay(1500);
                break;
            }
            else
            {
                await UniTask.Yield();

            }
        }
        FindFirstObjectByType<LevelsContoller>().NextLevel();
    }

    public void plusCount()
    {
        clickedNPC++;
    }
}
