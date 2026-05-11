using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;

public class HandView : MonoBehaviour
{
    // ! === Game Manager ===
    GameManager gameManager;
    BlackjackRoundManager blackjackRoundManager;

    // ! === Cards ===
    private List<Card> cards;
    private List<Transform> transforms;
    
    // * --- Card Events ---
    private UnityEvent TriggerLoad = new(); // ? Should be Invoked when you want to load the cards


    [Tooltip("If the user is interacting with this hand")]
    [SerializeField] public bool interacting = false; // ? Determines if the user is currently interacting with the object
    public UnityEvent OnEnterInteraction = new(); // ? Should be invoked when the user wants to enter the interaction
    public UnityEvent OnTryLeaveInteraction = new(); // ? Should be invoked when the user wants to leave the interaction.
    public UnityEvent OnSuccessfulLeaveInteraction = new(); // ? Should be invoked when the user leaves the interaction.
    public UnityEvent OnReset = new(); // ? Invoked when the view is reset between rounds


    // * --- Selection ---
    public Card selectedCard;
    public int selectedIndex = 0;

    void Awake()
    {
        // ? --- Game Manager ---
        gameManager = FindFirstObjectByType<GameManager>();
        blackjackRoundManager = FindFirstObjectByType<BlackjackRoundManager>();

        // ? --- Load Cards & Register Listeners ---
        TriggerLoad.AddListener(LoadCards);
        LoadCards();
        
        // ? --- Register Input Listeners ---
        gameManager.onMoveRightPlaying.AddListener(OnMoveRight);
        gameManager.onMoveLeftPlaying.AddListener(OnMoveLeft);
        gameManager.onMoveUpPlaying.AddListener(OnMoveUp);

        // ? --- Register Interaction Listeners ---
        OnEnterInteraction.AddListener(SelectCurrentCard);
        OnEnterInteraction.AddListener(OnEnter);
        OnSuccessfulLeaveInteraction.AddListener(OnLeave);

        if (blackjackRoundManager != null)
        {
            blackjackRoundManager.OnDealerOpeningDealt.AddListener(ResetViewState);
        }
    }

    // ! === Loading Logic ===

    /// <summary>
    /// Gets all the available cards and transformations
    /// </summary>
    public void LoadCards()
    {
        cards = GetComponentsInChildren<Card>().ToList();
        // ? Get transforms from cards
        transforms = new List<Transform>();
        foreach (var item in cards) transforms.Add(item.transform);
    }

    /// <summary>
    /// Clears cached hand-view state so the next round starts fresh.
    /// </summary>
    public void ResetViewState()
    {
        interacting = false;
        selectedIndex = 0;

        if (selectedCard != null && selectedCard.cardView != null)
        {
            selectedCard.cardView.ScaleDown();
        }

        // Destroy any child Card GameObjects so the visual hand is fully cleared.
        Card[] childCards = GetComponentsInChildren<Card>();
        foreach (var c in childCards)
        {
            if (c == null) continue;
            if (c.gameObject == this.gameObject) continue;

            if (Application.isPlaying)
            {
                Destroy(c.gameObject);
            }
            else
            {
                DestroyImmediate(c.gameObject);
            }
        }

        selectedCard = null;
        cards = new List<Card>();
        transforms = new List<Transform>();

        // Notify external listeners (e.g., BettingView) that the hand view was reset
        OnReset.Invoke();
    }

    // ! === Listeners ===

    /// <summary>
    /// Should be called when the user successfully leaves the interaction
    /// </summary>
    public void TriggerSuccessfulLeave()
    {
        this.OnSuccessfulLeaveInteraction.Invoke();
    }

    /// <summary>
    ///  Invokes the onenterinteraction
    /// </summary>
    public void EnterInteraction()
    {
        this.OnEnterInteraction.Invoke();
    }

    /// <summary>
    /// Invokes the ontryleaveinteraction
    /// </summary>
    public void LeaveInteraction()
    {
        this.OnTryLeaveInteraction.Invoke();
    }

    /// <summary>
    /// Ran whenever the user enters this interaction
    /// </summary>
    private void OnEnter()
    {
        interacting = true;
    }

    /// <summary>
    /// Ran whenever the user leaves this interaction
    /// </summary>
    public void OnLeave()
    {
        interacting = false;
        this.DeselectCurrentCard();        
    }

    void OnDisable()
    {
        if (blackjackRoundManager != null)
        {
            blackjackRoundManager.OnDealerOpeningDealt.RemoveListener(ResetViewState);
        }
    }

    // ! === Select / Deselect Logic ===

    // Selects the current selectedIndex as the new card
    // This is called when the user enters the interaction.
    private void SelectCurrentCard()
    {
        this.SelectCard(this.selectedIndex);
    }

    /// <summary>
    /// Ran when wanting to reset the cardView of the current card.
    /// </summary>
    private void DeselectCurrentCard()
    {
        this.selectedCard.cardView.ScaleDown();
    }

    /// <summary>
    /// Select a card with a given cardIndex
    /// </summary>
    /// <param name="cardIndex"></param>
    private void SelectCard(int cardIndex)
    {
        Card newCard = cards[cardIndex];
        SelectCard(newCard);
        selectedIndex = cardIndex;
    }

    /// <summary>
    /// Select a given card
    /// </summary>
    /// <param name="card"></param>
    private void SelectCard(Card card)
    {
        
        Card previousCard = selectedCard;
        if (previousCard)
        {
            previousCard.cardView.ScaleDown();            
        }

        card.cardView.ScaleUp();
        selectedCard = card;
    } 

    /// <summary>
    /// Returns true if the current selected index is the last card
    /// </summary>
    /// <returns></returns>
    private bool LastSelected()
    {
        return selectedIndex == (cards.Count - 1);
    }

    /// <summary>
    /// Returns true if the current selected index is 0
    /// </summary>
    /// <returns></returns>
    private bool FirstSelected()
    {
        return selectedIndex == 0;
    }

    private void OnMoveUp()
    {
        if (!interacting) return;
        Debug.Log("Setting interacting to false");
        this.OnTryLeaveInteraction.Invoke();
        interacting = false;
    }

    // ! === Inputs ===
    private void OnMoveLeft()
    {
        if (!interacting) return;
        int newIndex = selectedIndex - 1;
        if (newIndex < 0) return;
        SelectCard(newIndex);     
    }

    private void OnMoveRight()
    {
        if (!interacting) return;
        if (LastSelected())
        {
            return;
        }
        int newIndex = selectedIndex + 1;
        if (newIndex > cards.Count) return;
        SelectCard(newIndex);
    }
}
