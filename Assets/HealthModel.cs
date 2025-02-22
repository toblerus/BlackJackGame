using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class HealthModel : MonoBehaviour
{
    public int MaxHealthPlayer = 5;
    public int MaxHealthDealer = 5;

    private int CurrentHealthPlayer = 0;
    private int CurrentHealthDealer = 0;

    public List<GameObject> PlayerHearts;
    public List<GameObject> DealerHearts;

    public Transform PlayerContainer;
    public Transform DealerContainer;

    public GameObject HeartPrefab;
    public GameObject DamageAnimation;

    public void Start()
    {
        CurrentHealthPlayer = MaxHealthPlayer;
        CurrentHealthDealer = MaxHealthDealer;

        for (int i = 0; i < MaxHealthPlayer; i++)
        {
            PlayerHearts.Add(Instantiate(HeartPrefab, PlayerContainer));
        }

        for (int i = 0; i < MaxHealthDealer; i++)
        {
            DealerHearts.Add(Instantiate(HeartPrefab, DealerContainer));
        }
    }

    public void TakeDamage(Actor actor)
    {
        switch (actor)
        {
            case Actor.Player:
                if(CurrentHealthPlayer >= 1)
                {
                    CurrentHealthPlayer--;
                    var heart = PlayerHearts[0];
                    PlayerHearts.Remove(heart);
                    Destroy(heart);
                }
                break;
            case Actor.Dealer:
                if (CurrentHealthDealer >= 1)
                {
                    CurrentHealthDealer--;
                    var heart = DealerHearts[0];
                    DealerHearts.Remove(heart);
                    Destroy(heart);
                    ShowDamage();
                }
                break;
        }
    }

    public bool IsActorDead(Actor actor)
    {
        switch (actor)
        {
            case Actor.Player:
                return CurrentHealthPlayer <= 0;
            case Actor.Dealer:
                return CurrentHealthDealer <= 0;
        }
        return false;
    }

    private async void ShowDamage()
    {
        DamageAnimation.SetActive(true);
        await Task.Delay(500);
        DamageAnimation.SetActive(false);
    }
}