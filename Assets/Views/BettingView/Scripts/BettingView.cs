using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class BettingView : MonoBehaviour
{
    // ! === Game Manager ===
    GameManager gameManager;
    BlackjackRoundManager blackjackRoundManager;
    Player player;

    // ! === Properties ===
    private Chips chips;
    private List<ChipView> chipViews;
    public GameObject chipViewPrefab;

    // * --- Selection ---
    private ChipView selectedChipView;
    public int hoveredIndex = 0;
    [SerializeField] private float pendingBet = 0;

    // * --- Events ---
    [Tooltip("If the user is interacting with this object")]
    [SerializeField] public bool interacting = false; // ? Determines if the user is currently interacting with the object
    public UnityEvent OnEnterInteraction = new(); // ? Should be invoked when the user wants to enter the interaction
    public UnityEvent OnTryLeaveInteraction = new(); // ? Should be invoked when the user wants to leave the interaction.
    public UnityEvent OnSuccessfulLeaveInteraction = new(); // ? Should be invoked when the user leaves the interaction.
    
    // ! === Listeners ===
    public void OnEnter()
    {
        interacting = true;
    }

    public void OnLeave()
    {
        interacting = false;
    }


    // ! === Base Listeners ===
    void Awake()
    {
        // ? --- Game Manager ---
        gameManager = FindFirstObjectByType<GameManager>();
        blackjackRoundManager = FindFirstObjectByType<BlackjackRoundManager>();

        // ? --- Chips ---
        chips = FindFirstObjectByType<Chips>();
        player = FindFirstObjectByType<Player>();

        // ? --- Register Input Listeners ---
        pendingBet = 0;
        gameManager.onSelectPlaying.AddListener(OnSelect);
        gameManager.onMoveLeftPlaying.AddListener(OnMoveLeft);
        gameManager.onMoveRightPlaying.AddListener(OnMoveRight);
        gameManager.onMoveDownPlaying.AddListener(OnMoveDown);
        UpdatePendingBetDisplay();
    }
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        var result = chips.CalculateFrom(2000);
        foreach (KeyValuePair<string, int> items in result)
        {
            this.CreateChipView($"{items.Key}:{items.Value}", items.Key);
        }   

        // ? --- Chips ---
        chipViews = GetComponentsInChildren<ChipView>().ToList();   
    }

    public void CreateChipView(string text, string denomination)
    {
        GameObject chipViewObject = Instantiate(chipViewPrefab, transform);        
        ChipView chipView = chipViewObject.GetComponent<ChipView>();
        chipView.denomination = denomination;
        chipView.UpdateText(text);
    }



    // ! === Inputs ===
    private void OnSelect()
    {
        if (!interacting) return;
        ChipView selectedView = chipViews[hoveredIndex];
        Debug.Log($"Selecting with denomination {selectedView.denomination}");
        Chip selectedChip = chips.GetChip(int.Parse(selectedView.denomination.Replace("chip-", "")));
        Debug.Log($"Selected Chip: {selectedChip.denomination} {selectedChip.Id}");

        if (blackjackRoundManager == null)
        {
            Debug.Log("BlackjackRoundManager not found");
            return;
        }

        if (player == null)
        {
            Debug.Log("Player not found");
            return;
        }

        float potentialBet = pendingBet + selectedChip.denomination;
        if (potentialBet > player.balance)
        {
            Debug.Log($"Insufficient balance to add chip {selectedChip.denomination}. Pending: {pendingBet} Balance: {player.balance}");
            return;
        }

        pendingBet = potentialBet;
        Debug.Log($"Pending bet updated: {pendingBet}");
        UpdatePendingBetDisplay();

    }

    private void OnMoveLeft()
    {
        if (!interacting) return;
        int newIndex = hoveredIndex - 1;
        if (newIndex < 0) return;
        HoverOverView(newIndex);
    }

    private void OnMoveRight()
    {
        if (!interacting) return;
        if (hoveredIndex == (chipViews.Count - 1)) return;
        int newIndex = hoveredIndex + 1;
        if (newIndex > chipViews.Count) return;
        HoverOverView(newIndex);

    }

    private void OnMoveDown()
    {
        if (!interacting) return;

        if (blackjackRoundManager != null && pendingBet > 0)
        {
            object result = player != null ? player.OffsetBalance(-pendingBet) : null;
            if (player != null && result == null)
            {
                Debug.Log("Failed to place bet due to insufficient balance");
                return;
            }

            blackjackRoundManager.SetBetAmount(pendingBet);
            blackjackRoundManager.ConfirmBetAndStartRound();
            pendingBet = 0;
            UpdatePendingBetDisplay();
        }

        this.OnTryLeaveInteraction.Invoke();
        interacting = false;
    }

    private void HoverOverView(int index)
    {
        ChipView newView = chipViews[index];
        HoverOverView(newView);
        hoveredIndex = index;
    }

    private void HoverOverView(ChipView chipView)
    {
        ChipView previous = selectedChipView;
        if (previous) previous.HoverLeave();
        selectedChipView = chipView;
        selectedChipView.HoverEnter();
    }

    private void UpdatePendingBetDisplay()
    {
        if (player == null) return;

        player.pendingBet = (int)pendingBet;
        player.OnPendingBetChange(pendingBet);
    }

    /// <summary>
    /// Should be called when the user successfully leaves the interaction
    /// </summary>
    public void TriggerSuccessfulLeave()
    {
        this.OnSuccessfulLeaveInteraction.Invoke();
    }
}
