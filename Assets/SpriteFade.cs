using UnityEngine;
using DG.Tweening;

public class SpriteFade : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float fadeDuration = 3f;

    private void Start()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        Color originalColor = spriteRenderer.color;

        Color transparentColor = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);

        Sequence fadeSequence = DOTween.Sequence();

        fadeSequence.Append(spriteRenderer.DOColor(transparentColor, 0))
                    .Append(spriteRenderer.DOColor(originalColor, fadeDuration / 2f))
                    .SetEase(Ease.Linear);
    }
}
