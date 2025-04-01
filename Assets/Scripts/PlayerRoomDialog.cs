using System.Collections;
using UnityEngine;

public class PlayerRoomDialog : MonoBehaviour
{
    public AudioSource knockSound;
    public DialogueManager dialogueManager;
    public DialogueIntro introInfo;
    private string[] wakeUpDialogue;
    private PlayerMovementHandler playerMovement;

    void Start()
    {
        playerMovement = FindFirstObjectByType<PlayerMovementHandler>();
        if (PlayerPrefs.GetInt("HasEnteredPlayersRoom", 0) == 0) // Check if it's the first time
        {
            PlayerPrefs.SetInt("HasEnteredPlayersRoom", 1);
            PlayerPrefs.Save(); 
            StartCoroutine(FindDialogueIntro());
            StartCoroutine(DelayBeforeKnock());
        }
    }


    IEnumerator DelayBeforeKnock()
    {
        playerMovement.isDialogueActive = true;
        yield return new WaitForSeconds(1);
        knockSound.Play();
        dialogueManager.StartDialogue(wakeUpDialogue);
    }

    private IEnumerator FindDialogueIntro()
    {
        // Wait until the player object is instantiated
        while (introInfo == null)
        {
            introInfo = FindFirstObjectByType<DialogueIntro>();;
            yield return null;
        }

        // Once the player is found, continue
        Debug.Log("Dialog Found");
        wakeUpDialogue = new string[]
        {
            "*Knock Knock*",
            introInfo.PlayerName() + "!!!",
            "Wake up! I need to talk to you.",
            "It's important! Please come outside."
        };
    }
}
