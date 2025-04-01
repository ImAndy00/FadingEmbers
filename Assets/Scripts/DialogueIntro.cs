using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DialogueIntro : MonoBehaviour
{
    public Image backgroundImage; 
    
    // CanvasGroup on the dialogue panel that holds your dialogue text
    public CanvasGroup dialoguePanel; 
    
    // The dialogue text element (using TextMeshPro)
    public TextMeshProUGUI dialogueText; 
    
    // UI elements for name input and gender selection
    public TMP_InputField nameInputField;         // InputField for name
    public Button maleButton;                 // Button for male gender
    public Button femaleButton;               // Button for female gender
    public GameObject boyModel;
    public GameObject girlModel;
    
    public float fadeDuration = 2f;           // Duration for fade transitions
    public float waitAfterDialogue = 1f;      // Wait time after dialogue before fading to white
    public float typingSpeed = 0.04f;

    // Your dialogue lines
    private int currentLine = 0;

    // Player info
    private string playerName;
    private string playerGender;

    // Public properties to access and modify the fields
    public void SetPlayerGender(string gender)
    {
        playerGender = gender;
    }
    public void SetPlayerName(string name)
    {
        playerGender = name;
    }

    // Getter for PlayerGender
    public string PlayerGender()
    {
        return playerGender;
    }
    public string PlayerName()
    {
        return playerName;
    }

    

    private string[] dialogueLines = {
        "Long ago, the light of the world began to fade...",
        "Only a few embers remain, flickering in the darkness.",
        "But hope is not lost...",
        "What is your name, brave soul?",
        "It's also pretty dark in here... are you a boy or a girl?",
        "{playerName}, are you ready to embark on your journey?",
        "The world needs you.",
        "It's time to wake up...",
        "{playerName}...",
        "{playerName}...",
        "{playerName}!"
    };

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        if (SceneManager.GetActiveScene().name == "StartingDialogue")
        {
            // Ensure the background starts as black
            if (backgroundImage != null)
                backgroundImage.color = Color.black;

            // Start with the dialogue panel hidden
            if (dialoguePanel != null)
                dialoguePanel.alpha = 0;

            if (nameInputField != null)
                nameInputField.gameObject.SetActive(false);

            if (maleButton != null)
                maleButton.gameObject.SetActive(false);
            
            if (femaleButton != null)
                femaleButton.gameObject.SetActive(false);

            if (boyModel != null)
                boyModel.SetActive(false);

            if (girlModel != null)
                girlModel.SetActive(false);

            // Set up gender buttons
            maleButton.onClick.AddListener(() => SetGender("Male"));
            femaleButton.onClick.AddListener(() => SetGender("Female"));

            // Start the intro sequence
            StartCoroutine(IntroSequence());
        }
    
    }

    IEnumerator IntroSequence()
    {
        // Fade in the dialogue panel from transparent to fully visible
        yield return StartCoroutine(FadeCanvasGroup(dialoguePanel, 0, 1, fadeDuration));

        // Start displaying dialogue with typing effect
        yield return StartCoroutine(PlayDialogue());

        // Wait a moment after dialogue is finished
        yield return new WaitForSeconds(waitAfterDialogue);

        // Fade out the dialogue panel
        yield return StartCoroutine(FadeCanvasGroup(dialoguePanel, 1, 0, fadeDuration / 2));

        // Fade the background from black to white to simulate waking up
        yield return StartCoroutine(FadeBackgroundToWhite());

        SceneManager.LoadScene("PlayersRoom");
    }

    IEnumerator PlayDialogue()
    {
        while (currentLine < dialogueLines.Length)
        {
            if (dialogueLines[currentLine].Contains("{playerName}") && !string.IsNullOrEmpty(playerName))
        {
            dialogueLines[currentLine] = dialogueLines[currentLine].Replace("{playerName}", playerName);
        }
            yield return StartCoroutine(TypeLine(dialogueLines[currentLine]));

            // Handle different dialogues for name and gender
            if (currentLine == 3)
            {
                if (nameInputField != null)
                {
                    nameInputField.gameObject.SetActive(true);
                    nameInputField.Select();
                }

                yield return new WaitUntil(() => !string.IsNullOrEmpty(nameInputField.text) && Input.GetKeyDown(KeyCode.Return)); 

                playerName = nameInputField.text;
                if (nameInputField != null)
                    nameInputField.gameObject.SetActive(false);

            }
            else if (currentLine == 4) 
            {
                if (maleButton != null)
                    maleButton.gameObject.SetActive(true);

                if (femaleButton != null)
                    femaleButton.gameObject.SetActive(true);

                if (boyModel != null)
                    boyModel.SetActive(true);

                if (girlModel != null)
                    girlModel.SetActive(true);

                // Wait for gender selection
                yield return new WaitUntil(() => !string.IsNullOrEmpty(playerGender));
            }
            else
            {
                yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
            }
            currentLine++;
        }
    }

    IEnumerator TypeLine(string line)
    {
        dialogueText.text = "";
        foreach (char letter in line)
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    // Fades a CanvasGroup's alpha from start to end over duration seconds
    IEnumerator FadeCanvasGroup(CanvasGroup cg, float start, float end, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            cg.alpha = Mathf.Lerp(start, end, elapsed / duration);
            yield return null;
        }
        cg.alpha = end;
    }

    // Fades the background image from black to white
    IEnumerator FadeBackgroundToWhite()
    {
        if (playerGender == "Male")
        {
            if (boyModel != null)
                boyModel.SetActive(false);
        }
        if (playerGender == "Female")
        {
            if (girlModel != null)
                girlModel.SetActive(false);
        }
        float elapsed = 0f;
        Color startColor = Color.black;
        Color endColor = Color.white;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            backgroundImage.color = Color.Lerp(startColor, endColor, elapsed / fadeDuration);
            yield return null;
        }
        backgroundImage.color = endColor;
    }

    // Set gender when a button is clicked
    void SetGender(string gender)
    {
        if (maleButton != null)
            maleButton.gameObject.SetActive(false);

        if (femaleButton != null)
            femaleButton.gameObject.SetActive(false);

        if (gender == "Male")
        {
            if (girlModel != null)
                girlModel.SetActive(false);

            boyModel.transform.position = new Vector3(0,0,0);
        }
        else if (gender == "Female")
        {
            if (boyModel != null)
                boyModel.SetActive(false);

            girlModel.transform.position = new Vector3(0,0,0);
        }

        playerGender = gender;
    }
}