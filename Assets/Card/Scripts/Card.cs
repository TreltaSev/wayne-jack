using System.Linq;
using UnityEngine;
using UnityEngine.UI;


public enum Face
{
    Club,
    Diamond,
    Heart,
    Spade
}

public enum Glyph
{
    N0,
    N1,
    N2,
    N3,
    N4,
    N5,
    N6,
    N7,
    N8,
    N9,
    N10,
    J,
    Q,
    K,
    A
}

[RequireComponent(typeof(CardView))]
public class Card : MonoBehaviour
{

    public Image glyphImage;
    public Image faceSmallImage;
    public Image faceBigImage;
    
    public Face face = Face.Club;
    public Glyph glyph = Glyph.N1;

    public CardView cardView;
    void Awake()
    {
        cardView = GetComponent<CardView>();
        this.RefreshSelf();
    }

    void OnValidate()
    {
        cardView = GetComponent<CardView>();
        this.RefreshSelf();
    }

    /// <summary>
    /// Updates the sprite values in the glyph and face
    /// dependant on current component values
    /// </summary>
    public void RefreshSelf()
    {
        string glyph_string = glyph.ToString().Replace("N", "").ToLower();
        string face_string = face.ToString().ToLower();
        glyphImage.sprite = cardView.GetGlyph(glyph_string);
        faceSmallImage.sprite = cardView.GetFace(face_string);
        faceBigImage.sprite = cardView.GetFace(face_string);
    }

    /// <summary>
    /// Sets an image of a component to a given sprite
    /// </summary>
    /// <param name="of"></param>
    /// <param name="to"></param>
    private void SetImageTo(Image of, Sprite to)
    {
        of.sprite = to;
    }

    /// <summary>
    /// Returns the value for this card.
    /// </summary>
    /// <returns></returns>
    public int GetValue()
    {
        string glyph_string = glyph.ToString().Replace("N", "").ToLower();
        if (int.TryParse(glyph_string, out int result)) return result;
        string[] unimportant_face_glyphs = { "j", "q", "k" };
        if (unimportant_face_glyphs.Contains(glyph_string)) return 10;
        if (glyph_string == "a") return 1;
        return 0;
    }

    /// <summary>
    /// Returns true if the card is an ACE
    /// </summary>
    /// <returns></returns>
    public bool IsAce()
    {
        string glyph_string = glyph.ToString().Replace("N", "").ToLower();
        return glyph_string == "a";
    }
}
