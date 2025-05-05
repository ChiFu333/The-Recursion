using UnityEngine;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;

public class LastLevelController : MonoBehaviour
{
    [SerializeField] private GameObject _blackBack;
    [SerializeField] private TextThrower _textThrower;
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private Sprite _endSprite;
    async void Start()
    {
        G.AudioManager.StopMusic();
        await _textThrower.ThrowText(new LocString("What was my story about? Decide for yourself.","О чём была моя сказка? Решайте сами."),R.normalVoice);
        await UniTask.Delay(2000*2);
        await _textThrower.ThrowText(new LocString("I hope this stays between us...","Надеюсь, это останется между нами..."),R.normalVoice);
        await UniTask.Delay(1500*2);
        _textThrower.gameObject.SetActive(false);
        _renderer.sprite = _endSprite;
        G.AudioManager.PlaySound(R.Audio.lampOut);
        await UniTask.Delay(150);
        await _blackBack.transform.DOScale(Vector3.one * 1.6f,0.5f)
            .AsyncWaitForCompletion();
        await UniTask.Delay(1000);
        G.SceneLoader.Load("End");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
