using UnityEngine;
using UnityEngine.UI;

public class CardView : MonoBehaviour
{
    public Image cardImage;
    public Sprite[] heartSprites;
    public Sprite[] diamondSprites;
    public Sprite[] clubSprites;
    public Sprite[] spadeSprites;

    public WorldSpaceCanvasButton button;
    private CardHighlightComponent cardPreview;

    private void Awake()
    {
        if (heartSprites.Length == 0 || diamondSprites.Length == 0 || clubSprites.Length == 0 || spadeSprites.Length == 0)
        {
            Debug.LogWarning("Some suit sprite arrays are empty. Assign them manually in the Inspector.");
        }
    }

    private void Start()
    {
        cardPreview = FindObjectOfType<CardHighlightComponent>();

        button.onHoverEnter.AddListener(() => cardPreview.SetImage(cardImage.sprite));
        button.onHoverExit.AddListener(() => cardPreview.Hide());
    }


    public void SetImage(Card card)
    {
        int rankIndex = GetRankIndex(card.Rank);
        if (rankIndex < 0)
        {
            Debug.LogError("Invalid Rank");
            return;
        }

        Sprite selectedSprite = null;

        switch (card.Suit.ToLower())
        {
            case "hearts":
                if (rankIndex < heartSprites.Length)
                    selectedSprite = heartSprites[rankIndex];
                break;
            case "diamonds":
                if (rankIndex < diamondSprites.Length)
                    selectedSprite = diamondSprites[rankIndex];
                break;
            case "clubs":
                if (rankIndex < clubSprites.Length)
                    selectedSprite = clubSprites[rankIndex];
                break;
            case "spades":
                if (rankIndex < spadeSprites.Length)
                    selectedSprite = spadeSprites[rankIndex];
                break;
            default:
                Debug.LogError("Invalid Suit");
                return;
        }

        if (selectedSprite != null)
        {
            cardImage.sprite = selectedSprite;
        }
        else
        {
            Debug.LogError("Selected sprite is null or index out of range");
        }
    }

    private int GetRankIndex(string rank)
    {
        switch (rank)
        {
            case "A": return 0;
            case "2": return 1;
            case "3": return 2;
            case "4": return 3;
            case "5": return 4;
            case "6": return 5;
            case "7": return 6;
            case "8": return 7;
            case "9": return 8;
            case "10": return 9;
            case "J": return 10;
            case "Q": return 11;
            case "K": return 12;
            case "Special1": return 13;
            case "Special2": return 14;
            default: return -1;
        }
    }
}
