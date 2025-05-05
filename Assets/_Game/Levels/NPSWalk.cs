using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using System.Threading;

public class NPSWalk : MonoBehaviour, IPointerClickHandler
{
    public UnityEvent clickEvent;
    
    [SerializeField] private float timeToGo;
    [SerializeField] private float DeltaLeft;
    [SerializeField] private Sprite up, down;
    private SpriteRenderer _renderer;
    private float baseXPos; 
    private CancellationTokenSource _cancellationTokenSource;
    
    private void Awake()
    {
        _cancellationTokenSource = new CancellationTokenSource();
    }
    
    private async UniTaskVoid Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
        baseXPos = transform.position.x;
        
        MoveMe(_cancellationTokenSource.Token).Forget();
        await UniTask.Delay(Random.Range(200, 2000), cancellationToken: _cancellationTokenSource.Token);
        ChangeSprite(_cancellationTokenSource.Token).Forget();
    }

    private async UniTaskVoid MoveMe(CancellationToken token)
    {
        while (!token.IsCancellationRequested && gameObject.activeSelf)
        {
            bool moveCompleted = false;
            var tween = transform.DOLocalMoveX(transform.position.x + DeltaLeft, timeToGo)
                .SetEase(Ease.Linear)
                .OnComplete(() => moveCompleted = true);

            // Ожидаем либо завершение твина, либо отмену через токен
            await UniTask.WaitUntil(() => moveCompleted || token.IsCancellationRequested);

            if (token.IsCancellationRequested || !gameObject.activeSelf) 
            {
                tween?.Kill(); // Принудительно убиваем твин, если задача отменена
                return;
            }
            
            transform.position = new Vector3(baseXPos, transform.position.y, transform.position.z);
        }
    }

    private async UniTaskVoid ChangeSprite(CancellationToken token)
    {
        while (!token.IsCancellationRequested && gameObject.activeSelf)
        {
            _renderer.sprite = up;
            await UniTask.Delay(2000, cancellationToken: token);
            
            if (token.IsCancellationRequested || !gameObject.activeSelf) 
                return;
            
            _renderer.sprite = down;
            await UniTask.Delay(2000, cancellationToken: token);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        DOTween.Kill(2, true);
        transform.DOPunchScale(Vector3.one * 0.5f, 0.3f, 1, 0.5f)
            .SetId(2)
            .SetEase(Ease.OutQuad);
        
        clickEvent.Invoke();
        GetComponent<BoxCollider2D>().enabled = false;
    }

    private void OnDestroy()
    {
        // Отменяем все UniTask
        _cancellationTokenSource?.Cancel();
        _cancellationTokenSource?.Dispose();
        
        // Убиваем все DOTween анимации этого объекта
        DOTween.Kill(transform);
    }
}
