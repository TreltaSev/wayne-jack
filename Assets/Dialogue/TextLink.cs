using NUnit.Framework.Constraints;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Mode dropdown selection
/// </summary>
public enum Mode
{
    Transmitter,
    Receiver
}


public class TextLink : MonoBehaviour
{

    // === CONTENT === //
    [Tooltip("Content of the text box")]
    private string content;


    // === MODE === //
    [Tooltip("Sets the mode of the text link")]
    private Mode mode;

    // --- Events ---
    private UnityEvent<string> e_content_change;

    public void SetContent(string new_content)
    {
        this.content = new_content;
        this.e_content_change.Invoke(new_content);
    }

    [CustomEditor(typeof(TextLink))]
    public class TextLinkEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            TextLink textLink = (TextLink)target;

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Mode");
            textLink.mode = (Mode)EditorGUILayout.EnumPopup("Mode", textLink.mode);
            EditorGUILayout.BeginHorizontal();

            
            GameObject transmitter = EditorGUILayout.ObjectField("Some Field", null, typeof(GameObject), true) as GameObject;

            EditorGUILayout.EndHorizontal();


        }
    }
}
