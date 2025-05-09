using System.Collections;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    public GameObject dialoguePanel;
    public float textSpeed = 0.04f;

    public string[] dialogueLines;
    private bool isDialogueActive = false;
     private int currentLine = 0;
    private PlayerMovementHandler playerMovement;
    void Awake()
    {
        dialoguePanel.SetActive(false);
    }

    public void StartDialogue(string[] lines)
    {
        
        StartCoroutine(StartDialogueRoutine(lines));
    }

    public IEnumerator StartDialogueRoutine(string[] lines)
    {
        // reset state
        currentLine = 0;
        dialogueLines = lines;

        if (lines.Length == 0)
            yield break;

        dialoguePanel.SetActive(true);
        isDialogueActive = true;
        playerMovement = FindFirstObjectByType<PlayerMovementHandler>();
        if (playerMovement != null) 
            playerMovement.isDialogueActive = true;

        // Play through all lines
        while (currentLine < dialogueLines.Length)
        {
            yield return StartCoroutine(TypeLine(dialogueLines[currentLine]));
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
            currentLine++;
        }

        // Clean up
        EndDialogue();
        yield break;
    }

    IEnumerator PlayDialogue()
    {
        while (currentLine < dialogueLines.Length)
        {
            yield return StartCoroutine(TypeLine(dialogueLines[currentLine]));
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
            currentLine++;
        }
        EndDialogue();
    }

    IEnumerator TypeLine(string line)
    {
        dialogueText.text = "";
        Debug.Log("Typing line: " + line);
        foreach (char letter in line)
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(textSpeed);
        }
        Debug.Log("Finished line: " + dialogueText.text);
    }

    void EndDialogue()
    {
        isDialogueActive = false;
        dialoguePanel.SetActive(false);
        if (playerMovement != null) playerMovement.isDialogueActive = false;
    }
}