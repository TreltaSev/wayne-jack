using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HoverController : MonoBehaviour
{
    public List<Game.Hoverable> items = new List<Game.Hoverable>();
    public Game.Hoverable hovered { get; private set; }
    public GameObject hoveredObject => hovered != null ? hovered.gameObject : null;
    public int index { get; private set; } = 0;

    public event Action<int, Game.Hoverable> OnHoverChanged;
    public UnityEvent onHoverChanged = new();
    
    public event Action<int, Game.Hoverable> OnSelect;
    public UnityEvent onSelect = new();
    
    public event Action<int, Game.Hoverable> OnLight;
    public UnityEvent onLight = new();
    
    public event Action<int, Game.Hoverable> OnHeavy;
    public UnityEvent onHeavy = new();

    public event Action<int, Game.Hoverable> OnUp;
    public UnityEvent onUp = new();

    public event Action<int, Game.Hoverable> OnDown;
    public UnityEvent onDown = new();

    void Awake()
    {
        LoadItems();
        if (items.Count > 0) HoverTo(0);
    }

    public void LoadItems()
    {
        items.Clear();
        for (int i = 0; i < transform.childCount; i++)
        {
            var h = transform.GetChild(i).GetComponent<Game.Hoverable>();
            if (h != null) items.Add(h);
        }
    }

    public bool HoverTo(int targetIndex)
    {
        if (targetIndex < 0 || targetIndex >= items.Count) return false;

        if (hovered != null) hovered.Leave();

        hovered = items[targetIndex];
        index = targetIndex;
        hovered.Enter();
        OnHoverChanged?.Invoke(index, hovered);
        onHoverChanged?.Invoke();
        return true;
    }

    public GameObject GetHoveredObject(int targetIndex)
    {
        if (targetIndex < 0 || targetIndex >= items.Count) return null;
        return items[targetIndex] != null ? items[targetIndex].gameObject : null;
    }

    public bool MoveRight()
    {
        if (items.Count == 0) return false;
        int target = Mathf.Min(index + 1, items.Count - 1);
        return HoverTo(target);
    }

    public bool MoveLeft()
    {
        if (items.Count == 0) return false;
        int target = Mathf.Max(0, index - 1);
        return HoverTo(target);
    }

    public void MoveUp()
    {
        OnUp?.Invoke(index, hovered);
        onUp?.Invoke();
    }

    public void MoveDown()
    {
        OnDown?.Invoke(index, hovered);
        onDown?.Invoke();
    }

    public void InvokeSelect()
    {
        OnSelect?.Invoke(index, hovered);
        onSelect?.Invoke();
    }

    public void InvokeLight()
    {
        OnLight?.Invoke(index, hovered);
        onLight?.Invoke();
    }

    public void InvokeHeavy()
    {
        OnHeavy?.Invoke(index, hovered);
        onHeavy?.Invoke();
    }
}
