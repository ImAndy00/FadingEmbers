using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuHandler : MonoBehaviour
{
   public void StartGame(){
        PlayerPrefs.DeleteAll(); // Clears all stored preferences (fresh start)
        PlayerPrefs.Save();
        SceneManager.LoadScene("StartingDialogue");
    }
}
