using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public enum State
{
    Menu,
    Dialogue,
    Playing
}

public class GameManager : MonoBehaviour
{

    // ! === INSTANCE HANDLING === //
    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }

    private void Awake()
    {
        Singleton();
        LoadInputs();
    }

    /// <summary>
    /// Force only one instance
    /// </summary>
    private void Singleton()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    // ! === Properties === //
    public State state;

    public void SetMenu()
    {
        this.state = State.Menu;
    }

    public void SetDialogue()
    {
        this.state = State.Dialogue;
    }

    public void SetPlaying()
    {
        this.state = State.Playing;
    }

    // ! === Functions === //

    /// <summary>
    /// Load a specified scene
    /// </summary>
    /// <param name="newSceneName"></param>
    public static void LoadScene(string newSceneName)
    {
        SceneManager.LoadScene(newSceneName);
    }

    /// <summary>
    /// Quit the app
    /// </summary>
    public static void Quit()
    {
        Application.Quit();
    }

    // ! === INPUT ACTIONS === //

    // --- Setup --- //
    public InputActionAsset InputActions;

    private void OnEnable()
    {
        InputActions.FindActionMap("Generic").Enable();
    }

    private void OnDisable()
    {
        InputActions.FindActionMap("Generic").Disable();
    }

    // ! === GENERICS === //

    
    // ! === INPUTS === //

    /// <summary>
    /// Loads all available input action buttons
    /// </summary>
    private void LoadInputs()
    {
        m_selectAction = InputActions.FindAction("Generic/Select");
        m_moveUpAction = InputActions.FindAction("Generic/MoveUp");
        m_moveRightAction = InputActions.FindAction("Generic/MoveRight");
        m_moveDownAction = InputActions.FindAction("Generic/MoveDown");
        m_moveLeftAction = InputActions.FindAction("Generic/MoveLeft");
    }

    // ? --- Select --- //
    private InputAction m_selectAction;
    public UnityEvent onSelect = new();
    public UnityEvent onSelectMenu = new();
    public UnityEvent onSelectDialogue = new();
    public UnityEvent onSelectPlaying = new();

    private void HandleSelect()
    {
        if (!m_selectAction.WasPressedThisFrame()) return;
        onSelect.Invoke();
        if (state == State.Menu) onSelectMenu.Invoke();
        if (state == State.Dialogue) onSelectDialogue.Invoke();
        if (state == State.Playing) onSelectPlaying.Invoke();
    }

    // ? --- MoveUp --- //
    private InputAction m_moveUpAction;
    public UnityEvent onMoveUp = new();
    public UnityEvent onMoveUpMenu = new();
    public UnityEvent onMoveUpDialogue = new();
    public UnityEvent onMoveUpPlaying = new();

    private void HandleMoveUp()
    {
        if (!m_moveUpAction.WasPressedThisFrame()) return;
        onMoveUp.Invoke();
        if (state == State.Menu) onMoveUpMenu.Invoke();
        if (state == State.Dialogue) onMoveUpDialogue.Invoke();
        if (state == State.Playing) onMoveUpPlaying.Invoke();
    }

    // ? --- MoveRight --- //
    private InputAction m_moveRightAction;
    public UnityEvent onMoveRight = new();
    public UnityEvent onMoveRightMenu = new();
    public UnityEvent onMoveRightDialogue = new();
    public UnityEvent onMoveRightPlaying = new();

    private void HandleMoveRight()
    {
        if (!m_moveRightAction.WasPressedThisFrame()) return;
        onMoveRight.Invoke();
        if (state == State.Menu) onMoveRightMenu.Invoke();
        if (state == State.Dialogue) onMoveRightDialogue.Invoke();
        if (state == State.Playing) onMoveRightPlaying.Invoke();
    }

    // ? --- MoveDown --- //
    private InputAction m_moveDownAction;
    public UnityEvent onMoveDown = new();
    public UnityEvent onMoveDownMenu = new();
    public UnityEvent onMoveDownDialogue = new();
    public UnityEvent onMoveDownPlaying = new();

    private void HandleMoveDown()
    {
        if (!m_moveDownAction.WasPressedThisFrame()) return;
        onMoveDown.Invoke();
        if (state == State.Menu) onMoveDownMenu.Invoke();
        if (state == State.Dialogue) onMoveDownDialogue.Invoke();
        if (state == State.Playing) onMoveDownPlaying.Invoke();
    }

    // ? --- MoveLeft --- //
    private InputAction m_moveLeftAction;
    public UnityEvent onMoveLeft = new();
    public UnityEvent onMoveLeftMenu = new();
    public UnityEvent onMoveLeftDialogue = new();
    public UnityEvent onMoveLeftPlaying = new();

    private void HandleMoveLeft()
    {
        if (!m_moveLeftAction.WasPressedThisFrame()) return;
        onMoveLeft.Invoke();
        if (state == State.Menu) onMoveLeftMenu.Invoke();
        if (state == State.Dialogue) onMoveLeftDialogue.Invoke();
        if (state == State.Playing) onMoveLeftPlaying.Invoke();
    }

    private void Update()
    {
        HandleSelect();
        HandleMoveUp();
        HandleMoveRight();
        HandleMoveDown();
        HandleMoveLeft();
    }

    public void FooSelect()
    {
        Debug.Log("Testing...");
    }

    public void FooMoveUp()
    {
        Debug.Log("MoveUp...");
    }

    public void FooMoveRight()
    {
        Debug.Log("MoveRight...");
    }

    public void FooMoveDown()
    {
        Debug.Log("MoveDown...");
    }

    public void FooMoveLeft()
    {
        Debug.Log("MoveLeft...");
    }

}