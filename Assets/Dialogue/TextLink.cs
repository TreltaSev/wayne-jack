using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public enum Mode
{
    Transmitter,
    Receiver
}


[ExecuteAlways]
[RequireComponent(typeof(TextMeshProUGUI))]
public class TextLink : MonoBehaviour
{

    // === CONTENT === //
    [SerializeField, Tooltip("Content of the text box")]
    private string content;
    private string previous_content = "";

    // === MODE === //
    [SerializeField, Tooltip("Sets the mode of the text link")]
    private Mode mode;

    // === LISTEN TO === //
    [SerializeField] private TextLink listenTo;
    private TextLink currentlyListeningTo;

    // === TEXT COMPONENT === //
    private TextMeshProUGUI textComponent;

    // --- Events ---
    private readonly UnityEvent<string> e_content_change = new();

    /// <summary>
    /// Updates the content value programmatically.
    /// Also calls the content change event.
    /// </summary>
    /// <param name="new_content"></param>
    public void SetContent(string new_content)
    {
        content = new_content;
        ApplyContentIfChanged();
    }

    /// <summary>
    /// Attempts to update the content value programmatically,
    /// only if this instance is of mode `Transmitter`
    /// </summary>
    /// <param name="new_content"></param>
    public void TrySetTransmitterContent(string new_content)
    {
        if (mode != Mode.Transmitter) return;
        SetContent(new_content);
    }

    /// <summary>
    /// Gets the textComponent reference if there isn't one already
    /// </summary>
    private void Cache()
    {
        if (!textComponent)
            textComponent = GetComponent<TextMeshProUGUI>();
    }

    private void OnValidate()
    {
        Cache();
        SetupListener();
        ApplyContentIfChanged();
    }

    private void OnEnable()
    {
        Cache();
        SetupListener();
        ApplyContentIfChanged();
    }

    /// <summary>
    /// Sets up the listener if this component is in receiver mode
    /// </summary>
    private void SetupListener()
    {

        Debug.Log("First");

        if (mode != Mode.Receiver || listenTo == null) return;

        if (currentlyListeningTo != null)
            currentlyListeningTo.e_content_change.RemoveListener(TransmitterListener);
        Debug.Log("Fourth");

        currentlyListeningTo = listenTo;

        Debug.Log("Added Listener");
        currentlyListeningTo.e_content_change.AddListener(TransmitterListener);
    }

    /// <summary>
    /// Checks the current and previous saved contents and invokes the event if its a transmitter.
    /// </summary>
    private void ApplyContentIfChanged()
    {
        if (previous_content == content) return;

        previous_content = content;

        if (textComponent)
            textComponent.text = content;

        if (mode == Mode.Transmitter)
            e_content_change.Invoke(content);
    }

    public void TransmitterListener(string new_content)
    {
        Debug.Log($"Transmit {this} {new_content}");
        this.content = new_content;
        this.textComponent.text = this.content;
    }

    [CustomEditor(typeof(TextLink))]
    public class TextLinkEditor : Editor
    {
        private TextLink textLink;

        public override void OnInspectorGUI()
        {
            this.textLink = (TextLink)target;

            // Display Editor Options
            this.DisplayEditor();
        }

        private void DisplayEditor()
        {

            EditorGUI.BeginChangeCheck();

            EditorGUILayout.Space();

            // Mode
            this.textLink.mode = (Mode)EditorGUILayout.EnumPopup("Mode", textLink.mode);
            EditorGUILayout.Space();

            // Content ( Dependant on Mode )
            if (this.textLink.mode == Mode.Transmitter)
            {
                EditorGUILayout.LabelField("Content");
                this.textLink.content = EditorGUILayout.TextField(this.textLink.content);
                EditorGUILayout.Space();
            }

            // Transmitter ( Depends on Mode == Mode.Receiver )
            if (textLink.mode == Mode.Receiver)
            {
                textLink.listenTo = (TextLink)EditorGUILayout.ObjectField("Listen To", this.textLink.listenTo, typeof(TextLink), true);
                EditorGUILayout.Space();
            }

            

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(textLink, "TextLink Change");

                textLink.Cache();
                textLink.ApplyContentIfChanged();

                EditorUtility.SetDirty(textLink);
            }
        }
    }
}
