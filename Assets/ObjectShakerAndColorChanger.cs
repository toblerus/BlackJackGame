using UnityEngine;
using DG.Tweening;

public class ObjectShakerAndColorChanger : MonoBehaviour
{
    public Transform objectA;
    public SpriteRenderer objectB;

    public float shakeStrength = 0.5f;
    public float shakeFrequency = 0.02f;
    public float colorChangeInterval = 0.05f;

    private Vector3 originalPosition;
    private Tween colorTween;
    private float shakeTimer;

    void Start()
    {
        if (objectA != null)
        {
            originalPosition = objectA.localPosition;
            InvokeRepeating(nameof(ShakeObject), 0f, shakeFrequency);
        }

        if (objectB != null)
        {
            colorTween = DOTween.To(() => objectB.color, x => objectB.color = x, Color.white, colorChangeInterval)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.Linear);
        }
    }

    void ShakeObject()
    {
        if (objectA != null)
        {
            Vector3 randomOffset = new Vector3(
                Random.Range(-shakeStrength, shakeStrength),
                Random.Range(-shakeStrength, shakeStrength),
                0f
            );
            objectA.localPosition = originalPosition + randomOffset;
        }
    }

    void OnDestroy()
    {
        if (colorTween != null && colorTween.IsActive())
        {
            colorTween.Kill();
        }

        CancelInvoke(nameof(ShakeObject));

        if (objectA != null)
        {
            objectA.localPosition = originalPosition;
        }
    }
}
