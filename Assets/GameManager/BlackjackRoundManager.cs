using UnityEngine;
using UnityEngine.Events;

public class BlackjackRoundManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Game.Dealer dealer;
    [SerializeField] private Player player;
    [SerializeField] private GameManager gameManager;

    [Header("Betting")]
    [SerializeField] private float currentBet = 0;
    [SerializeField] private bool hasConfirmedBet = false;

    [Header("Round State")]
    [SerializeField] private bool roundInProgress = false;
    [SerializeField] private bool playerTurnActive = false;

    public UnityEvent<float> OnBetConfirmed = new();
    public UnityEvent OnRoundStarted = new();
    public UnityEvent OnRoundEnded = new();
    public UnityEvent OnDealerOpeningDealt = new();

    void Awake()
    {
        if (!dealer) dealer = FindFirstObjectByType<Game.Dealer>();
        if (!player) player = FindFirstObjectByType<Player>();
        if (!gameManager) gameManager = FindFirstObjectByType<GameManager>();
    }

    void OnEnable()
    {
        if (gameManager != null)
        {
            gameManager.onLightPlaying.AddListener(HandleHitInput);
            gameManager.onHeavyPlaying.AddListener(HandleStandInput);
        }

        if (player != null)
        {
            player.OnPlayerBust += HandlePlayerBust;
        }
    }

    void OnDisable()
    {
        if (gameManager != null)
        {
            gameManager.onLightPlaying.RemoveListener(HandleHitInput);
            gameManager.onHeavyPlaying.RemoveListener(HandleStandInput);
        }

        if (player != null)
        {
            player.OnPlayerBust -= HandlePlayerBust;
        }
    }

    /// <summary>
    /// Sets the bet value before the round begins.
    /// </summary>
    /// <param name="amount"></param>
    public void SetBetAmount(float amount)
    {
        if (amount <= 0)
        {
            Debug.Log("Bet amount must be greater than 0");
            return;
        }

        currentBet = amount;
        hasConfirmedBet = false;
        Debug.Log($"Bet amount selected: {currentBet}");
    }

    /// <summary>
    /// Triggers the start of a new round after bet selection.
    /// </summary>
    public void ConfirmBetAndStartRound()
    {
        if (currentBet <= 0)
        {
            Debug.Log("Cannot start round without a valid bet");
            return;
        }

        // Deduct the bet from player balance
        player.OffsetBalance(-currentBet);
        
        hasConfirmedBet = true;
        OnBetConfirmed.Invoke(currentBet);
        StartRound();
    }

    /// <summary>
    /// Deals opening cards to player and dealer.
    /// </summary>
    public void StartRound()
    {
        if (!hasConfirmedBet)
        {
            Debug.Log("Bet must be confirmed before starting round");
            return;
        }

        if (dealer == null || player == null)
        {
            Debug.Log("Dealer or Player reference missing on BlackjackRoundManager");
            return;
        }

        // Player receives their opening two cards after bet confirmation.
        player.DealCard();
        player.DealCard();

        roundInProgress = true;
        playerTurnActive = true;
        OnRoundStarted.Invoke();

        Debug.Log($"Round started with bet {currentBet}. Player value: {player.GetHandValue()} Dealer value: {dealer.GetHandValue()}");
    }

    /// <summary>
    /// Called to have the dealer deal their opening hand before the player places a bet.
    /// </summary>
    public void DealerDealOpening()
    {
        if (dealer == null || player == null)
        {
            Debug.Log("Dealer or Player reference missing on BlackjackRoundManager");
            return;
        }

        // Clear any previous round state and hands before dealer deals.
        ResetRound();

        dealer.DealOpeningHand();
        OnDealerOpeningDealt.Invoke();

        Debug.Log($"Dealer dealt opening hand. Dealer value: {dealer.GetHandValue()}");
    }

    /// <summary>
    /// Called by outside scripts to make player hit.
    /// </summary>
    public void Hit()
    {
        if (!roundInProgress || !playerTurnActive) return;
        player.DealCard();
    }

    /// <summary>
    /// Called by outside scripts to make player stand and run dealer turn.
    /// </summary>
    public void Stand()
    {
        if (!roundInProgress || !playerTurnActive) return;

        player.Stand();
        playerTurnActive = false;

        dealer.PlayDealerTurn();
        EndRound();
    }

    /// <summary>
    /// Clears both hands and round state.
    /// </summary>
    public void ResetRound()
    {
        if (dealer != null) {
            dealer.ResetAll();
            dealer.DealOpeningHand();
        }
        if (player != null) player.ResetAll();

        roundInProgress = false;
        playerTurnActive = false;
    }

    /// <summary>
    /// Handles hit input while in playing state.
    /// </summary>
    public void HandleHitInput()
    {
        Hit();
    }

    /// <summary>
    /// Handles stand input while in playing state.
    /// </summary>
    public void HandleStandInput()
    {
        Stand();
    }

    /// <summary>
    /// Ends the current round.
    /// </summary>
    private void EndRound()
    {
        if (!roundInProgress) return;

        

        int playerValue = player.GetHandValue();
        int dealerValue = dealer.GetHandValue();
        bool playerBust = playerValue > 21;
        bool dealerBust = dealerValue > 21;

        if (!playerBust && (dealerBust || playerValue > dealerValue))
        {
            float payout = currentBet * 2f;
            player.OffsetBalance(payout);
            Debug.Log($"Player wins. Payout: {payout} New balance: {player.balance}");
        }
        else if (!playerBust && playerValue == dealerValue)
        {
            float refund = currentBet;
            player.OffsetBalance(refund);
            Debug.Log($"Push. Bet refunded: {refund} New balance: {player.balance}");
        }
        else
        {
        Debug.Log($"Player loses. Bet lost: {currentBet} New balance: {player.balance}");
        }

        roundInProgress = false;
        playerTurnActive = false;
        hasConfirmedBet = false;
        OnRoundEnded.Invoke();

        // Clear the current hands so the next bet starts fresh.
        ResetRound();

        Debug.Log($"Round ended. Player: {playerValue} Dealer: {dealerValue}");
    }

    /// <summary>
    /// Handles player bust event.
    /// </summary>
    /// <param name="handValue"></param>
    private void HandlePlayerBust(int handValue)
    {
        if (!roundInProgress) return;
        Debug.Log($"Player busted at {handValue}");
        EndRound();
    }
}
