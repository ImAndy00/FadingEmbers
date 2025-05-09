using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuHandler : MonoBehaviour
{
    [Header("References")]
    public GameObject startPanel;
    public GameObject optionsPanel;
    void Start()
    {
        optionsPanel.SetActive(false);
        startPanel.SetActive(true);

    }

    public void StartGame()
    {
        PlayerPrefs.DeleteAll();  // fresh-start
        PlayerPrefs.Save();
        SceneManager.LoadScene("StartingDialogue");
    }

    public void OpenOptions()
    {
        startPanel.SetActive(false);
        optionsPanel.SetActive(true);
    }

    public void CloseOptions()
    {
        optionsPanel.SetActive(false);
        startPanel.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
