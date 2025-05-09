using UnityEngine;

public class OldManInteraction : MonoBehaviour
{
    [Header("Dialogue Lines")]
    [TextArea] public string[] noWandLines;        
    [TextArea] public string[] hasWandLines;       
    [TextArea] public string[] postDungeonLines;   

    private bool playerInRange = false;
    private DialogueManager dm;

    void Awake()
    {
        dm = FindFirstObjectByType<DialogueManager>();
        if (dm == null)
            Debug.LogError("OldManInteraction: No DialogueManager found in scene!");
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E) && dm != null)
        {
            bool hasWand       = PlayerPrefs.GetInt("PlayerHasWand", 0) == 1;
            bool beatDungeon   = PlayerPrefs.GetInt("PlayerBeatDungeon", 0) == 1;

            if (beatDungeon)
            {
                dm.StartDialogue(postDungeonLines);
            }
            else if (hasWand)
            {
                dm.StartDialogue(hasWandLines);
            }
            else
            {
                dm.StartDialogue(noWandLines);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInRange = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInRange = false;
    }
}