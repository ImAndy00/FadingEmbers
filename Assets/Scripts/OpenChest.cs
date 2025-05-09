using UnityEngine;
using System.Collections;

public class OpenChest : MonoBehaviour
{
    [Header("Part References")]
    public Transform lid;
    public float openAngle = 70f;
    public float duration  = 0.5f;

    private bool   isOpen      = false;
    private bool   playerInRange = false;
    private Quaternion closedRotation;
    private Quaternion openRotation;
    private Coroutine currentRoutine;
    public AudioClip laserUnlock;
    public AudioClip chestUnlock;
    public GameObject lootPrefab;
    public Transform lootSpawnPoint;
    public float lootFloatSpeed = 1f;
    public float lootFloatDuration = 2f;
    public string[] openDialogue;
    GameObject loot;

    private DialogueManager dialogueManager;
    

    void Awake()
    {
        dialogueManager = FindFirstObjectByType<DialogueManager>();
        if (dialogueManager == null)
            Debug.LogError("No DialogueManager found! Make sure one is in the scene.");
        closedRotation = lid.localRotation;
        openRotation   = closedRotation * Quaternion.Euler(-openAngle, 0f, 0f);
    }

    void Update()
    {
        if (!playerInRange || !Input.GetKeyDown(KeyCode.E))
            return;

        // If this is the WandChest, only allow opening if visited==1
        if (CompareTag("WandChest"))
        {
            int visited = PlayerPrefs.GetInt("VisitedTown", 0);
            if (visited == 1){
                StartCoroutine(Toggle());
            }else
                Debug.Log("You should visit the town before opening this chest!");
        }
        else
        {
            // Any other chest opens immediately
            StartCoroutine(Toggle());
        }
    }

    private IEnumerator Toggle()
    {
        if (CompareTag("WandChest"))
        {
            PlayerPrefs.SetInt("PlayerHasWand", 1);
            PlayerPrefs.Save();
        }
        if (currentRoutine != null)
            StopCoroutine(currentRoutine);
        AudioSource audioSource = GetComponent<AudioSource>();
        if(audioSource != null && chestUnlock != null)
            {
                audioSource.PlayOneShot(chestUnlock);
        }
        Quaternion target = isOpen ? closedRotation : openRotation;
        currentRoutine = StartCoroutine(AnimateLid(target));
        isOpen = !isOpen;
        SpawnLoot();
        if (dialogueManager != null && openDialogue.Length > 0)
            yield return StartCoroutine(dialogueManager.StartDialogueRoutine(openDialogue));
        Destroy(loot);
        UnlockTags();
    }

    private IEnumerator AnimateLid(Quaternion targetRot)
    {
        Quaternion startRot = lid.localRotation;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, elapsed / duration);
            lid.localRotation = Quaternion.Slerp(startRot, targetRot, t);
            yield return null;
        }

        lid.localRotation = targetRot;
        currentRoutine = null;
    }

    private void UnlockTags()
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        // Only run this for the MapChest
        if (CompareTag("MapChest")){
            GameObject[] lasers = GameObject.FindGameObjectsWithTag("MapLaser");
            if (audioSource != null && laserUnlock != null)
            {
                audioSource.PlayOneShot(laserUnlock, 0.3f);
            }
            foreach (GameObject laser in lasers)
            {
                Destroy(laser);
            }
            PlayerPrefs.SetInt("Unlocked_IceBall", 1);
            PlayerPrefs.Save();
        }
        if (CompareTag("IceBeamChest")){
            GameObject[] lasers = GameObject.FindGameObjectsWithTag("Room9ChestLaser");
            if (audioSource != null && laserUnlock != null)
            {
                audioSource.PlayOneShot(laserUnlock, 0.3f);
            }
            foreach (GameObject laser in lasers)
            {
                Destroy(laser);
            }
            PlayerPrefs.SetInt("Unlocked_IceBeam", 1);
            PlayerPrefs.Save();
        }
    }

    private void SpawnLoot()
    {
        if (lootPrefab == null || lootSpawnPoint == null) return;

        loot = Instantiate(
            lootPrefab,
            lootSpawnPoint.position,
            Quaternion.identity
        );

        Collider lootCol = loot.GetComponent<Collider>();
        if (lootCol != null)
            lootCol.enabled = false;

        // Start floating coroutine on the loot
        loot.AddComponent<LootFloater>()
            .Initialize(lootFloatSpeed, lootFloatDuration);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInRange = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInRange = false;
    }

}