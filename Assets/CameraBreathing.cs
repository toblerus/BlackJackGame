using UnityEngine;
using DG.Tweening;

public class CameraBreathing : MonoBehaviour
{
    public float positionAmplitude = 0.03f; 
    public float rotationAmplitude = 0.5f;  
    public float duration = 4f;             
    public Ease ease = Ease.InOutSine;

    private Vector3 initialPosition;
    private Vector3 initialRotation;

    void Start()
    {
        initialPosition = transform.localPosition;
        initialRotation = transform.localEulerAngles;
        StartBreathing();
    }

    void StartBreathing()
    {
        transform.DOLocalMove(initialPosition + new Vector3(positionAmplitude, positionAmplitude, 0), duration / 2)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(ease);

        transform.DOLocalRotate(initialRotation + new Vector3(rotationAmplitude, 0, rotationAmplitude), duration / 2)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(ease);
    }

    void OnDisable()
    {
        DOTween.Kill(transform);
        transform.localPosition = initialPosition;
        transform.localEulerAngles = initialRotation;
    }
}
