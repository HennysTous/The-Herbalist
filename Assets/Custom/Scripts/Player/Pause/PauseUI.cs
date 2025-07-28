using UnityEngine;
using UnityEngine.UI;

public class PauseUI : MonoBehaviour
{
    public static PauseUI Instance { get; private set; }

    [SerializeField] private GameObject panel;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button saveButton;
    [SerializeField] private Button loadButton;
    [SerializeField] private Button exitButton;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;
    }

    private void Start()
    {
        resumeButton.onClick.AddListener(() => PauseManager.Instance.ResumeGame());
        saveButton.onClick.AddListener(() => PauseManager.Instance.SaveGame());
        loadButton.onClick.AddListener(() => PauseManager.Instance.LoadGame());
        exitButton.onClick.AddListener(() => PauseManager.Instance.ExitGame());

        panel.SetActive(false);
    }

    public void Open()
    {
        panel.SetActive(true);
    }

    public void Close()
    {
        panel.SetActive(false);
    }
}
