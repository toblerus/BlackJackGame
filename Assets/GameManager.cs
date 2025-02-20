using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Text playerText, dealerText, resultText;
    public Button hitButton, standButton, restartButton;

    private Deck deck;
    private List<Card> playerHand, dealerHand;
    private int playerScore, dealerScore;
    private bool gameOver;

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

        playerHand.Add(deck.DrawCard());
        playerHand.Add(deck.DrawCard());
        dealerHand.Add(deck.DrawCard());

        UpdateScores();
        CheckBlackjack();
    }

    void PlayerHit()
    {
        if (gameOver) return;

        playerHand.Add(deck.DrawCard());
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
            dealerHand.Add(deck.DrawCard());
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
}
