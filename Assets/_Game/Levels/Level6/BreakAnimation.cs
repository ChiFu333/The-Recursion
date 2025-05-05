using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;

public class BreakAnimation : MonoBehaviour
{
    [Header("Stretch Settings")]
    [SerializeField] private int stretchCount = 2;
    [SerializeField] private float stretchDuration = 0.3f;
    [SerializeField] private Vector2 stretchScale = new Vector2(1.2f, 0.8f);
    [SerializeField] private float returnDuration = 0.2f;
    [SerializeField] private int betweenDelay = 10;

    [Header("Break Settings")]
    [SerializeField] private float finalStretchMultiplier = 1.5f;
    [SerializeField] private Vector2 partsInitialScale = new Vector2(0.9f, 1.1f);
    [SerializeField] private float partsReturnDuration = 0.4f;
    [SerializeField] private float fallDuration = 0.8f;
    [SerializeField] private float separationForce = 3f;

    [SerializeField] private Transform leftHalf;
    [SerializeField] private Transform rightHalf;
    [SerializeField] private ParticleSystem _particleSystem;

    private Vector3 originalScale;
    private bool animationStarted;
    
    [Header("Color Effects")]
    [SerializeField] private SpriteRenderer targetSprite1, targetSprite2; // Спрайт для изменения цвета
    [SerializeField] private Color stretchColor = new Color(1f, 0.5f, 0.5f); // Базовый красный
    [SerializeField] private Color maxStretchColor = Color.red; // Максимально красный
    [SerializeField] private float colorChangeSpeed = 2f; // Скорость изменения цвета

    private Color _originalColor;
    private Material _spriteMaterial;
    
    private void Start()
    {
        originalScale = Vector3.one;
        _originalColor = targetSprite1.color;
        originalScale = Vector3.one;
    }

    public async UniTask PlayAnimation()
    {
        if (animationStarted) return;
        animationStarted = true;

        // 1. Анимация растягивания
        await StretchAnimation();

        // 2. Финальное растягивание и разрыв
        await FinalStretch();
        G.AudioManager.PlaySound(R.Audio.BrokenPanel);
        

        // 3. Анимация половинок
        AnimateParts();
    }

    private async UniTask StretchAnimation()
    {
        for (int i = 0; i < stretchCount; i++)
        {
            // Плавное покраснение при растяжении
            targetSprite1.DOColor(stretchColor, stretchDuration / 2)
                .SetEase(Ease.OutQuad);
            targetSprite2.DOColor(stretchColor, stretchDuration / 2)
                .SetEase(Ease.OutQuad);

            // Растягиваем
            await transform.DOScale(
                new Vector3(stretchScale.x, stretchScale.y, originalScale.z), 
                stretchDuration)
                .SetEase(Ease.OutQuad)
                .AsyncWaitForCompletion();

            // Возвращаем цвет и масштаб
            await transform.DOScale(originalScale, returnDuration)
                .SetEase(Ease.InOutQuad)
                .AsyncWaitForCompletion();

            targetSprite1.DOColor(_originalColor, returnDuration / 2);
            targetSprite2.DOColor(_originalColor, returnDuration / 2);

            // Пауза между растягиваниями
            if (i < stretchCount -1)
                await UniTask.Delay(betweenDelay * 100, DelayType.Realtime);
        }
    }

    private async UniTask FinalStretch()
    {
        ChangeColor();
        //ChangeColor();
        // Сильное финальное растягивание
        await transform.DOScale(
            new Vector3(stretchScale.x * finalStretchMultiplier, 
                       stretchScale.y / finalStretchMultiplier, 
                       originalScale.z), 
            stretchDuration)
            .SetEase(Ease.OutQuad)
            .AsyncWaitForCompletion();
        
        //Time.timeScale = 0.8f;
        //DOVirtual.DelayedCall(0.3f, () => Time.timeScale = 1f);
        // Возврат к нормальному scale и цвету
        targetSprite1.DOColor(_originalColor, returnDuration * 1f);
        targetSprite2.DOColor(_originalColor, returnDuration * 1f);
        
        await transform.DOScale(originalScale, returnDuration)
            .SetEase(Ease.InOutQuad)
            .AsyncWaitForCompletion();
        _particleSystem.Play();
        targetSprite1.DOKill();
        targetSprite2.DOKill();
        targetSprite1.DOColor(_originalColor, returnDuration * 0.1f);
        targetSprite2.DOColor(_originalColor, returnDuration * 0.1f);
    }

    private void AnimateParts()
    {
        // Начальный scale для половинок
        leftHalf.localScale = new Vector3(partsInitialScale.x, partsInitialScale.y, originalScale.z);
        rightHalf.localScale = new Vector3(partsInitialScale.x, partsInitialScale.y, originalScale.z);

        // Возврат к нормальному scale с эластичностью
        leftHalf.DOScale(originalScale, partsReturnDuration)
            .SetEase(Ease.OutElastic);
        
        rightHalf.DOScale(originalScale, partsReturnDuration)
            .SetEase(Ease.OutElastic);
        
        // Добавляем физику
        AddPhysics();
    }

    private async void ChangeColor()
    {
        await UniTask.Delay(100);
        // Интенсивное покраснение при финальном растяжении
        targetSprite1.DOColor(maxStretchColor, stretchDuration * 1f)
            .SetEase(Ease.OutQuint);
        targetSprite2.DOColor(maxStretchColor, stretchDuration * 1f)
            .SetEase(Ease.OutQuint);
    }
    private void AddPhysics()
    {
        // Левая половина
        var leftRb = leftHalf.gameObject.AddComponent<Rigidbody2D>();
        leftRb.gravityScale = 1.5f;
        leftRb.AddForce(new Vector2(-separationForce, separationForce * 0.5f), ForceMode2D.Impulse);
        leftRb.AddTorque(separationForce * 7f);

        // Правая половина
        var rightRb = rightHalf.gameObject.AddComponent<Rigidbody2D>();
        rightRb.gravityScale = 1.5f;
        rightRb.AddForce(new Vector2(separationForce, separationForce * 0.5f), ForceMode2D.Impulse);
        rightRb.AddTorque(-separationForce * 7f);
    }
}