using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening; // Add this for DOTween
using System.Threading.Tasks;
using System;
using Random = UnityEngine.Random;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    public TextMeshPro playerText, dealerText, resultText;
    public Button hitButton, standButton, restartButton;

    private Deck deck;
    private List<Card> playerHand, dealerHand;
    private int playerScore, dealerScore;
    private bool gameOver;

    [SerializeField] private Transform spawnPoint;

    [SerializeField] private Transform dealerCardDisplay;
    [SerializeField] private Transform playerCardDisplay;
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private List<GameObject> spawnedCards;
    [SerializeField] private int minScareDuration, maxScareDuration;
    [SerializeField] private GameObject jumpScare;


    private bool blockInputs = false;

    void Start()
    {
        restartButton.onClick.AddListener(StartWrapper);
        hitButton.onClick.AddListener(PlayerHitWrapper);
        standButton.onClick.AddListener(PlayerStandWrapper);
        _ = StartGameAsync();
    }

    private void StartWrapper()
    {
        if (!blockInputs)
            _ = StartGameAsync();
    }

    async Task StartGameAsync()
    {
        deck = new Deck();
        playerHand = new List<Card>();
        dealerHand = new List<Card>();
        playerScore = 0;
        dealerScore = 0;
        gameOver = false;
        resultText.text = "";

        spawnedCards.ForEach(cardObject => Destroy(cardObject));
        spawnedCards.Clear();

        await AddCard(playerHand, deck.DrawCard(), playerCardDisplay);
        await AddCard(playerHand, deck.DrawCard(), playerCardDisplay);
        await AddCard(dealerHand, deck.DrawCard(), dealerCardDisplay);

        CheckBlackjack();
    }

    private void PlayerHitWrapper()
    {
        if (!blockInputs)
            _ = PlayerHitAsync();
    }

    async Task PlayerHitAsync()
    {
        if (gameOver) return;

        await AddCard(playerHand, deck.DrawCard(), playerCardDisplay);


        if (playerScore > 21)
        {
            resultText.text = "Bust! Dealer Wins!";
            ShowJumpScare();
            EndGame();
        }
    }

    private void PlayerStandWrapper()
    {
        if (!blockInputs)
            _ = PlayerStandAsync();
    }

    async Task PlayerStandAsync()
    {
        if (gameOver) return;

        while (dealerScore < 17)
        {
            await AddCard(dealerHand, deck.DrawCard(), dealerCardDisplay);
        }

        DetermineWinner();
    }

    void UpdateScores()
    {
        playerScore = CalculateScore(playerHand);
        dealerScore = CalculateScore(dealerHand);

        playerText.text = "Player: " + playerScore;
        dealerText.text = "Dealer: " + dealerScore;
    }

    int CalculateScore(List<Card> hand)
    {
        int score = 0;
        int aceCount = 0;

        foreach (var card in hand)
        {
            score += card.Value;
            if (card.Rank == "A") aceCount++;
        }

        while (score > 21 && aceCount > 0)
        {
            score -= 10;
            aceCount--;
        }

        return score;
    }

    void CheckBlackjack()
    {
        if (playerScore == 21)
        {
            resultText.text = "Blackjack! You Win!";
            EndGame();
        }
    }

    void DetermineWinner()
    {
        if (dealerScore > 21 || playerScore > dealerScore)
        {
            resultText.text = "You Win!";
        }
        else if (playerScore < dealerScore)
        {
            resultText.text = "Dealer Wins!";
            ShowJumpScare();
        }
        else
        {
            resultText.text = "Push!";
        }

        EndGame();
    }

    private async void ShowJumpScare()
    {
        jumpScare.SetActive(true);
        await Task.Delay(Random.Range(minScareDuration,maxScareDuration));
        jumpScare.SetActive(false);
    }

    void EndGame()
    {
        gameOver = true;
    }

    private async Task AddCard(List<Card> hand, Card card, Transform display)
    {
        blockInputs = true;
        hand.Add(card);
        var cardInstance = Instantiate(cardPrefab, display);
        spawnedCards.Add(cardInstance);
        cardInstance.GetComponent<CardView>().SetImage(card);

        // Assuming the card's Image component is a child of the cardInstance
        RectTransform cardImage = cardInstance.transform.GetChild(0).GetComponent<RectTransform>(); // Use RectTransform

        // Convert spawnPoint world position to local canvas space
        Canvas canvas = display.GetComponentInParent<Canvas>();
        RectTransform canvasRect = canvas.GetComponent<RectTransform>();

        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, spawnPoint.position);

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            screenPoint,
            canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : Camera.main,
            out Vector2 localPoint
        );

        Vector3 canvasStartPosition = new Vector3(localPoint.x, localPoint.y, 0);

        // Set starting position for the animation
        Vector3 endPosition = cardImage.localPosition;

        // Temporarily move the image to the starting position
        cardImage.anchoredPosition = canvasStartPosition;

        // Animate the card image to its final position
        await cardImage.DOAnchorPos(endPosition, 0.5f).SetEase(Ease.OutQuad).AsyncWaitForCompletion();

        UpdateScores();

        // Delay for 2 seconds after the animation
        await Task.Delay(500);
        blockInputs = false;
    }

}
