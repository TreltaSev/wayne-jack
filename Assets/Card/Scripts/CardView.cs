using System;
using UnityEngine;


[RequireComponent(typeof(Card))]
public class CardView : MonoBehaviour
{

    // === Sprites === //
    public Sprite[] glyphs;
    public Sprite[] faces;

    /// <summary>
    /// Gets a sprite of a specified name from given sprites
    /// </summary>
    /// <param name="_from"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    private static Sprite GetSprite(Sprite[] _from, string name)
    {
        Sprite sprite = Array.Find(_from, element => element.name == name);
        return sprite;
    }

    public Sprite GetGlyph(string name)
    {
        return GetSprite(glyphs, name);
    }

    public Sprite GetFace(string name)
    {
        return GetSprite(faces, name);
    }

    /// <summary>
    /// Reset the cards scale back to 1
    /// </summary>
    public void ScaleDown()
    {
        transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
    }

    /// <summary>
    /// Set the cards scale to 1.1
    /// </summary>
    public void ScaleUp() {
        transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
    }
}
