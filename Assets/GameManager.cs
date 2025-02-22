using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening; // Add this for DOTween

public class GameManager : MonoBehaviour
{
    public TextMeshPro playerText, dealerText, resultText;
    public ThreeDButton hitButton, standButton, restartButton;

    private Deck deck;
    private List<Card> playerHand, dealerHand;
    private int playerScore, dealerScore;
    private bool gameOver;

    [SerializeField] private Transform spawnPoint;

    [SerializeField] private Transform dealerCardDisplay;
    [SerializeField] private Transform playerCardDisplay;
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private List<GameObject> spawnedCards;

    void Start()
    {
        restartButton.onClick.AddListener(StartGame);
        hitButton.onClick.AddListener(PlayerHit);
        standButton.onClick.AddListener(PlayerStand);
        StartGame();
    }

    void StartGame()
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

        AddCard(playerHand, deck.DrawCard(), playerCardDisplay);
        AddCard(playerHand, deck.DrawCard(), playerCardDisplay);
        AddCard(dealerHand, deck.DrawCard(), dealerCardDisplay);

        UpdateScores();
        CheckBlackjack();
    }

    void PlayerHit()
    {
        if (gameOver) return;

        AddCard(playerHand, deck.DrawCard(), playerCardDisplay);

        UpdateScores();

        if (playerScore > 21)
        {
            resultText.text = "Bust! Dealer Wins!";
            EndGame();
        }
    }

    void PlayerStand()
    {
        if (gameOver) return;

        while (dealerScore < 17)
        {
            AddCard(dealerHand, deck.DrawCard(), dealerCardDisplay);
            UpdateScores();
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
        }
        else
        {
            resultText.text = "Push!";
        }

        EndGame();
    }

    void EndGame()
    {
        gameOver = true;
    }

    private void AddCard(List<Card> hand, Card card, Transform display)
    {
        hand.Add(card);
        var cardInstance = Instantiate(cardPrefab, display);
        spawnedCards.Add(cardInstance);
        cardInstance.GetComponent<CardView>().SetImage(card);

        // Assuming the card's Image component is a child of the cardInstance
        Transform cardImage = cardInstance.transform.GetChild(0); // Adjust if needed based on your hierarchy

        // Set starting position for the animation
        Vector3 endPosition = cardImage.localPosition; // Local position within parent (layout group)

        // Temporarily move the image to the starting position
        cardImage.localPosition = spawnPoint.position;

        // Animate the card image to its final position
        cardImage.DOLocalMove(endPosition, 0.5f).SetEase(Ease.OutQuad);
    }
}
