using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance { get; private set; }

    public bool IsPaused { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;
    }

    public void TogglePause()
    {
        if (IsPaused)
            ResumeGame();
        else
            PauseGame();
    }

    public void PauseGame()
    {
        IsPaused = true;
        Time.timeScale = 0f;

        PauseUI.Instance.Open();
    }

    public void ResumeGame()
    {
        IsPaused = false;
        Time.timeScale = 1f;

        PauseUI.Instance.Close();
    }

    public void SaveGame()
    {
        SaveManager.Instance.SaveGame();
        ResumeGame();
    }

    public void LoadGame()
    {
        SaveManager.Instance.LoadGame();
        ResumeGame();
    }

    public void ExitGame()
    {
        Time.timeScale = 1f;
        Application.Quit();
    }
}
