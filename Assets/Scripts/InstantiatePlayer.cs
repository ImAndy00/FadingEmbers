using System.Collections;
using UnityEngine;

public class InstantiatePlayer : MonoBehaviour
{
    public GameObject boyPrefab;
    public GameObject girlPrefab;
    public DialogueIntro dialogueIntro;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {   
        StartCoroutine(FindDialogueIntro());
        if (dialogueIntro != null && Player.Instance == null)
        {
            if (dialogueIntro.PlayerGender() == "Male")
            {
                Instantiate(boyPrefab, new Vector3(-6.57f, 1.76346791f, 1.97000003f), Quaternion.Euler(-90, 0, 180));
            }
            else if (dialogueIntro.PlayerGender() == "Female")
            {
                Instantiate(girlPrefab, new Vector3(-6.57f, 1.76346791f, 1.97000003f), Quaternion.Euler(-90, 0, 180));
            }
        }
        
    }

    private IEnumerator FindDialogueIntro()
    {
        // Wait until the player object is instantiated
        while (dialogueIntro == null)
        {
            dialogueIntro = FindFirstObjectByType<DialogueIntro>();;
            yield return null; 
        }
    }

}
