using UnityEngine;
using DG.Tweening;

public class CameraBreathing : MonoBehaviour
{
    [Header("Breathing Settings")]
    public float positionAmplitude = 0.03f; // Movement amplitude (position)
    public float rotationAmplitude = 0.5f;  // Rotation amplitude in degrees
    public float duration = 4f;             // Time for a full breath cycle
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
        // Position Tween (subtle 3D movement)
        transform.DOLocalMove(initialPosition + new Vector3(positionAmplitude, positionAmplitude, 0), duration / 2)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(ease);

        // Rotation Tween (adds gentle tilt)
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
