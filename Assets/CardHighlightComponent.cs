using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardHighlightComponent : MonoBehaviour
{
    [SerializeField] private Image cardImage;
    public void SetImage(Sprite sprite)
    {
        cardImage.sprite = sprite;
        cardImage.enabled = true;
    }

    public void Hide()
    {
        cardImage.enabled = false;
    }

    public void Start()
    {
        cardImage.enabled = false;
    }
}
