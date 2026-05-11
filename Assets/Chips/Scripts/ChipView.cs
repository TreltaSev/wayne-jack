using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ChipView : MonoBehaviour
{
    public Image idleChipImage;
    public Image hoverChipImage;
    public Image selectChipImage;

    public TextMeshProUGUI chipText;

    /// <summary>
    /// Object containing the chip image
    /// </summary>
    public GameObject chipImageObject;
    public Image chipImageImage;

    public string denomination;


    public bool isHovered = false;
    public bool isSelected = false;

    private readonly UnityEvent RefreshEvent = new();

    void Awake()
    {

        // ? --- Setup Listeners ---
        RefreshEvent.AddListener(Refresh);

        // ? --- Get References ---
        chipImageImage = chipImageObject.GetComponent<Image>();
    }

    public void HoverEnter()
    {
        this.isHovered = true;
        RefreshEvent.Invoke();
    }

    public void HoverLeave()
    {
        this.isHovered = false;
        this.Deselect();
        RefreshEvent.Invoke();
    }

    public void Select()
    {
        this.isSelected = true;
        RefreshEvent.Invoke();
    }

    public void Deselect()
    {
        this.isSelected = false;
        RefreshEvent.Invoke();
    }

    /// <summary>
    /// Refreshes the image displayed depending on isHovered
    /// </summary>
    private void Refresh()
    {
        if (isHovered)
        {
            chipImageImage.sprite = hoverChipImage.sprite;
        } else if (isSelected)
        {
            chipImageImage.sprite = selectChipImage.sprite;
        } else
        {
            chipImageImage.sprite = idleChipImage.sprite;
        }
    }


    public void UpdateText(string new_text)
    {
        chipText.text = new_text;
    }
}
