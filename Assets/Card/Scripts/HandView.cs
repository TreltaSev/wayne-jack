using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;

public class HandView : MonoBehaviour
{
    // ! === Game Manager ===
    GameManager gameManager;
    
    // ! === Cards ===
    private List<Card> cards;
    private List<Transform> transforms;
    
    // * --- Card Events ---
    private UnityEvent TriggerLoad = new(); // ? Should be Invoked when you want to load the cards

    // * --- Selection ---
    public Card selectedCard;
    public int selectedIndex;
    public bool canInteract = false;

    void Awake()
    {
        // ? --- Game Manager ---
        gameManager = FindFirstObjectByType<GameManager>();

        // ? --- Register Listeners ---
        TriggerLoad.AddListener(LoadCards);        

        gameManager.onMoveRightPlaying.AddListener(OnMoveRight);
        gameManager.onMoveLeftPlaying.AddListener(OnMoveLeft);
    }

    /// <summary>
    /// Gets all the available cards and transformations
    /// </summary>
    private void LoadCards()
    {
        cards = GetComponentsInChildren<Card>().ToList();
        // ? Get transforms from cards
        transforms = new List<Transform>();
        foreach (var item in cards) transforms.Add(item.transform);
    }

    private void OnMoveRight()
    {
        int newIndex = selectedIndex + 1;
        if (newIndex > cards.Count) return;
        //
        Card previousCard = selectedCard;
        previousCard.cardView.ScaleDown();
        selectedCard = cards[newIndex];
        selectedIndex = newIndex;
        selectedCard.cardView.ScaleUp();
        
    }

    private void OnMoveLeft()
    {
        int newIndex = selectedIndex - 1;
        if (newIndex < 0) return;
        //
        Card previousCard = selectedCard;
        previousCard.cardView.ScaleDown();
        selectedCard = cards[newIndex];
        selectedIndex = newIndex;
        selectedCard.cardView.ScaleUp();        
    }

    /// <summary>
    /// Returns true if the current selected index is the last card
    /// </summary>
    /// <returns></returns>
    private bool LastSelected()
    {
        return selectedIndex == (cards.Count - 1);
    }



}
