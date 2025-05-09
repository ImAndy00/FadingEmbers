using System.Collections;
using UnityEngine;
public class TownManager : MonoBehaviour
{
    [Header("Old Man Setup")]
    public GameObject oldManPrefab;
    public GameObject fire;
    public Transform fireSpawn;
    public Transform firstVisitSpawn;
    public Transform returnVisitSpawn;
    
    [Header("Intro Dialogue")]
    [TextArea]
    public string[] introLines;

    private DialogueManager dialogueManager;
    private const string PrefKey = "VisitedTown";

    void Start()
    {
        bool notBeatDungeon = PlayerPrefs.GetInt("PlayerBeatDungeon", 0) == 0;
        if (!notBeatDungeon)
            Instantiate(fire, fireSpawn.position, fireSpawn.rotation);
        dialogueManager = FindFirstObjectByType<DialogueManager>();
        if (dialogueManager == null)
        {
            Debug.LogError("No DialogueManager found in scene!");
            return;
        }

        bool firstTime = PlayerPrefs.GetInt(PrefKey, 0) == 0;
        bool hasWand = PlayerPrefs.GetInt("PlayerHasWand", 0) == 1;
        Vector3 spawnPos = firstTime 
            ? firstVisitSpawn.position 
            : returnVisitSpawn.position;

        if (firstTime)
        {
            GameObject oldMan = Instantiate(oldManPrefab, spawnPos, firstVisitSpawn.rotation);
            // Run the intro dialogue
            StartCoroutine(Dialog());

            // Mark as visited so next time is not "first"
            PlayerPrefs.SetInt(PrefKey, 1);
            PlayerPrefs.Save();
        } else if(!hasWand) {
            GameObject oldMan = Instantiate(oldManPrefab, firstVisitSpawn.position, firstVisitSpawn.rotation);
        } else {
            GameObject oldMan = Instantiate(oldManPrefab, spawnPos, returnVisitSpawn.rotation);
        }
    }

    private IEnumerator Dialog(){
        yield return StartCoroutine(FindPlayerObject());
        if (dialogueManager != null && introLines.Length > 0)
            yield return StartCoroutine(dialogueManager.StartDialogueRoutine(introLines));
    }

    private IEnumerator FindPlayerObject()
    {
        GameObject player = null;
        // Wait until the player object is found
        while (player == null)
        {
            player = GameObject.FindWithTag("Player");
            yield return null;
        }
        Debug.Log("Player object found!");
    }

    
}
