using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 10f;
    public float gravity = -9.8f;
    CharacterController characterController;
    public static Player Instance;
    public GameObject IceBall;
    public GameObject IceBeam;
    public GameObject Wand;
    public Transform wandPivot;
    public Camera mainCamera;
    private string lastUsedTag = "";
    private bool wandSpawned = false;
    public string[] lines;

    [Header("Stats")]
    public int maxHealth = 15;
    private int currentHealth;
    public int CurrentHealth => currentHealth;
    public int MaxHealth     => maxHealth;
    private bool canTakeDamage = true;
    public float damageCooldown = 1f; // seconds of invincibility

    private DialogueManager dialogueManager;
    private bool hitFlame = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        
    }
    void Awake()
    {
        currentHealth = maxHealth;
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist the player across scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate player
        }
        
        characterController = GetComponent<CharacterController>();
        if (characterController == null)
        {
            Debug.LogError("CharacterController component not found on Player object. Check the Player prefab!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        ApplyGravityToCC();
        if (!wandSpawned && PlayerPrefs.GetInt("PlayerHasWand", 0) == 1)
        {
            TrySpawnWand();
            wandSpawned = true;
        }
    }

    private void TrySpawnWand()
    {
        // Check PlayerPrefs: 0 = false, 1 = true
        bool hasWand = PlayerPrefs.GetInt("PlayerHasWand", 0) == 1;
        if (!hasWand || Wand == null || wandPivot == null)
            return;

        // Instantiate the wand and parent it with local transform reset
        GameObject wandInstance = Instantiate(Wand);
        wandInstance.transform.SetParent(wandPivot, false); // false = don't keep world position
        wandInstance.transform.localPosition = Vector3.zero;
        wandInstance.transform.localRotation = Quaternion.identity;
        wandInstance.transform.localScale = Vector3.one * 5f;
    }

    void OnTriggerEnter(Collider other)
    {
        SceneHandler sceneHandler = FindFirstObjectByType<SceneHandler>();

        if (other.CompareTag("PlayerDoorInside"))
        {
            lastUsedTag = "PlayerDoorInside"; 
            sceneHandler.LoadSceneAndSpawnPlayer("Town", lastUsedTag);
        }
        else if (other.CompareTag("PlayerDoorOutside"))
        {
            lastUsedTag = "PlayerDoorOutside"; 
            sceneHandler.LoadSceneAndSpawnPlayer("PlayersRoom", lastUsedTag);
        }
        else if (other.CompareTag("TownExit"))
        {
            lastUsedTag = "TownExit"; 
            sceneHandler.LoadSceneAndSpawnPlayer("DungeonEntrance", lastUsedTag);
        }
        else if (other.CompareTag("DungeonEntranceOutside"))
        {
            lastUsedTag = "DungeonEntranceOutside"; 
            sceneHandler.LoadSceneAndSpawnPlayer("DungeonInside", lastUsedTag);
        }
        else if (other.CompareTag("ExitEntrance"))
        {
            lastUsedTag = "ExitEntrance"; 
            sceneHandler.LoadSceneAndSpawnPlayer("Town", lastUsedTag);
        }
        else if (other.CompareTag("DungeonExit"))
        {
            lastUsedTag = "DungeonExit"; 
            sceneHandler.LoadSceneAndSpawnPlayer("DungeonEntrance", lastUsedTag);
        }
        SceneManager.sceneLoaded += OnSceneLoaded;

        //Dungeon Handler
        //Room1
        if (other.CompareTag("Room1toRoom2")){
            Debug.Log("Collided!");
            characterController.enabled = false;
            transform.position = new Vector3(0,0,36);
            mainCamera.transform.position = new Vector3(0,20,20);
            characterController.enabled = true;
        }

        //Room2
        if (other.CompareTag("Room2toRoom1")){
            Debug.Log("Collided!");
            characterController.enabled = false;
            transform.position = new Vector3(0,0,16);
            mainCamera.transform.position = new Vector3(0,20,-21);
            characterController.enabled = true;
        } else if (other.CompareTag("Room2toRoom3")){
            Debug.Log("Collided!");
            characterController.enabled = false;
            transform.position = new Vector3(0,0,75);
            mainCamera.transform.position = new Vector3(0,20,60);
            characterController.enabled = true;
        } else if (other.CompareTag("Room2toRoom5")){
            Debug.Log("Collided!");
            characterController.enabled = false;
            transform.position = new Vector3(34,0,44);
            mainCamera.transform.position = new Vector3(50,20,20);
            characterController.enabled = true;
        }
        if (other.CompareTag("ResetSpace"))
        {
            Debug.Log("Reset");
            GameObject blueCube = GameObject.FindWithTag("BlueCube");
            GameObject redCube = GameObject.FindWithTag("RedCube");
            blueCube.transform.position = new Vector3(-3.63000011f,1.5f,45.6500015f);
            redCube.transform.position = new Vector3(4.30000019f,1.5f,45.6500015f);
        }
        if (other.CompareTag("BigCubeSpace"))
        {
            Debug.Log("Collided!");
            characterController.enabled = false;
            transform.position = new Vector3(0,17,75);
            characterController.enabled = true;
        }

        //Room3
        if (other.CompareTag("Room3toRoom2")){
            Debug.Log("Collided!");
            characterController.enabled = false;
            transform.position = new Vector3(0,0,55);
            mainCamera.transform.position = new Vector3(0,20,20);
            characterController.enabled = true;
        } else if (other.CompareTag("Room3toRoom4")){
            Debug.Log("Collided!");
            characterController.enabled = false;
            transform.position = new Vector3(0,0,115);
            mainCamera.transform.position = new Vector3(0,20,100);
            characterController.enabled = true;
        } else if (other.CompareTag("Room3toRoom8")){
            Debug.Log("Collided!");
            characterController.enabled = false;
            transform.position = new Vector3(34,0,84);
            mainCamera.transform.position = new Vector3(50,20,60);
            characterController.enabled = true;
        }

        //Room4
        if (other.CompareTag("Room4toRoom3")){
            Debug.Log("Collided!");
            characterController.enabled = false;
            transform.position = new Vector3(0,0,96);
            mainCamera.transform.position = new Vector3(0,20,60);
            characterController.enabled = true;
        }

        //Room5
        if (other.CompareTag("Room5toRoom2")){
            Debug.Log("Collided!");
            characterController.enabled = false;
            transform.position = new Vector3(15,0,44);
            mainCamera.transform.position = new Vector3(0,20,20);
            characterController.enabled = true;
        } else if (other.CompareTag("Room5toRoom8")){
            Debug.Log("Collided!");
            characterController.enabled = false;
            transform.position = new Vector3(50,0,75);
            mainCamera.transform.position = new Vector3(50,20,60);
            characterController.enabled = true;
        } else if (other.CompareTag("Room5toRoom6")){
            Debug.Log("Collided!");
            characterController.enabled = false;
            transform.position = new Vector3(84,0,44);
            mainCamera.transform.position = new Vector3(100,20,20);
            characterController.enabled = true;
        }

        //Room 6
        if (other.CompareTag("Room6toRoom5")){
            Debug.Log("Collided!");
            characterController.enabled = false;
            transform.position = new Vector3(65,0,44);
            mainCamera.transform.position = new Vector3(50,20,20);
            characterController.enabled = true;
        } else if (other.CompareTag("Room6toRoom7")){
            Debug.Log("Collided!");
            characterController.enabled = false;
            transform.position = new Vector3(100,0,15);
            mainCamera.transform.position = new Vector3(100,20,-20);
            characterController.enabled = true;
        } else if (other.CompareTag("Room6toRoom9")){
            Debug.Log("Collided!");
            characterController.enabled = false;
            transform.position = new Vector3(100,0,74);
            mainCamera.transform.position = new Vector3(100,20,60);
            characterController.enabled = true;
        }

        //Room 7
        if (other.CompareTag("Room7toRoom6")){
            Debug.Log("Collided!");
            characterController.enabled = false;
            transform.position = new Vector3(100,0,34);
            mainCamera.transform.position = new Vector3(100,20,20);
            characterController.enabled = true;
        }

        //Room 8
        if (other.CompareTag("Room8toRoom3")){
            Debug.Log("Collided!");
            characterController.enabled = false;
            transform.position = new Vector3(15,0,84);
            mainCamera.transform.position = new Vector3(0,20,60);
            characterController.enabled = true;
        } else if (other.CompareTag("Room8ToRoom5")){
            Debug.Log("Collided!");
            characterController.enabled = false;
            transform.position = new Vector3(50,0,55);
            mainCamera.transform.position = new Vector3(50,20,20);
            characterController.enabled = true;
        } else if (other.CompareTag("Room8toRoom9")){
            Debug.Log("Collided!");
            characterController.enabled = false;
            transform.position = new Vector3(85,0,84);
            mainCamera.transform.position = new Vector3(100,20,60);
            characterController.enabled = true;
        }

        //Room 9
        if (other.CompareTag("Room9toRoom8")){
            Debug.Log("Collided!");
            characterController.enabled = false;
            transform.position = new Vector3(65,0,85);
            mainCamera.transform.position = new Vector3(50,20,60);
            characterController.enabled = true;
        } else if (other.CompareTag("Room9toRoom10")){
            Debug.Log("Collided!");
            characterController.enabled = false;
            transform.position = new Vector3(135,0,85);
            mainCamera.transform.position = new Vector3(150,20,60);
            characterController.enabled = true;
        } else if (other.CompareTag("Room9toRoom6")){
            Debug.Log("Collided!");
            characterController.enabled = false;
            transform.position = new Vector3(100,0,55);
            mainCamera.transform.position = new Vector3(100,20,20);
            characterController.enabled = true;
        }

        //Room 10
        if (other.CompareTag("Room10toRoom9")){
            Debug.Log("Collided!");
            characterController.enabled = false;
            transform.position = new Vector3(115,0,85);
            mainCamera.transform.position = new Vector3(100,20,60);
            characterController.enabled = true;
        } else if (other.CompareTag("Room10toRoom11")){
            Debug.Log("Collided!");
            characterController.enabled = false;
            transform.position = new Vector3(150,0,114);
            mainCamera.transform.position = new Vector3(150,20,100);
            characterController.enabled = true;
        }

        //Room 11
        if (other.CompareTag("Room11ToRoom10")){
            Debug.Log("Collided!");
            characterController.enabled = false;
            transform.position = new Vector3(150,0,95);
            mainCamera.transform.position = new Vector3(150,20,60);
            characterController.enabled = true;
        }
        if (other.CompareTag("Fire")){
                
            Debug.Log("Collided!");
            hitFlame=true;
            StartCoroutine(Dialog(lines));
            PlayerPrefs.SetInt("PlayerBeatDungeon", 1);
            PlayerPrefs.Save();
            maxHealth += 5;
            currentHealth = maxHealth;
            FindFirstObjectByType<HealthDisplay>()?.UpdateText();
            GameObject[] toDestroy = GameObject.FindGameObjectsWithTag("Room11FlameLaser");
            foreach (GameObject obj in toDestroy)
            {
                Destroy(obj);
            }
        }
        if (other.CompareTag("IceEnemy") && canTakeDamage)
        {
            TakeDamage(1);
        }
        if (other.CompareTag("IceBoss") && canTakeDamage)
        {
            TakeDamage(5);
        }
    }

    private void TakeDamage(int amount)
    {
        currentHealth -= amount;
        Debug.Log($"Player hit! HP: {currentHealth}/{maxHealth}");

        // trigger the red flash
        var flash = FindFirstObjectByType<DamageFlashUI>();
        if (flash != null) flash.FlashDamage();

        // update the on‐screen health text
        var hud = FindFirstObjectByType<HealthDisplay>();
        if (hud != null) hud.UpdateText();

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            // start invincibility
            canTakeDamage = false;
            StartCoroutine(ResetDamageCooldown());
        }
    }

    private IEnumerator ResetDamageCooldown()
    {
        yield return new WaitForSeconds(damageCooldown);
        canTakeDamage = true;
    }

    private void Die()
    {
        Debug.Log("Player has died!");

        // SLOW WHITE FLASH
        FindFirstObjectByType<DamageFlashUI>()?.FlashDeath();

        // ** Restore to full health on death **
        currentHealth = maxHealth;
        FindFirstObjectByType<HealthDisplay>()?.UpdateText();

        characterController.enabled = false;
        transform.position = new Vector3(0,0,-4);
        mainCamera.transform.position = new Vector3(0,20,-21);
        characterController.enabled = true;

        string[] deathLines = {
            "You awaken on the cold dungeon floor...",
            "You get up and feel your head pounding... What happened?",
            "You must press on... You feel... determined."
        };
        StartCoroutine(Dialog(deathLines));
    }

    private IEnumerator Dialog(string[] dLines){
        dialogueManager = FindFirstObjectByType<DialogueManager>();
        if (dialogueManager != null && dLines.Length > 0){
            yield return StartCoroutine(dialogueManager.StartDialogueRoutine(dLines));
            if (hitFlame){
                Destroy(GameObject.FindGameObjectWithTag("Fire"));
            }
        }
    }

    public void MoveWithCC(Vector3 direction){
        characterController.Move(direction * speed * Time.deltaTime);
        //transform.LookAt(transform.position + direction);
        if (direction.sqrMagnitude > 0.001f)
        {
            // Compute the target facing rotation
            Quaternion targetRot = Quaternion.LookRotation(direction);

            // Interpolate from current to target
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRot,
                10 * Time.deltaTime
            );
        }
    }



    Vector3 gravityVelocity = Vector3.zero;
    void ApplyGravityToCC(){
        if(characterController.isGrounded && gravityVelocity.y < 0){
            gravityVelocity = Vector3.zero;
            return;
        }
        gravityVelocity.y += gravity * Time.deltaTime;
        characterController.Move(gravityVelocity * Time.deltaTime);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
    SceneHandler sceneHandler = FindFirstObjectByType<SceneHandler>();
    Transform spawnPoint = null;

    if (scene.name == "PlayersRoom" && lastUsedTag == "PlayerDoorOutside")
    {
        spawnPoint = sceneHandler.spawnHouse;
    }
    else if (scene.name == "Town")
    {
        if (lastUsedTag == "PlayerDoorInside")
        {
            spawnPoint = sceneHandler.spawnOutsideHouse;
        }
        else if (lastUsedTag == "ExitEntrance")
        {
            spawnPoint = sceneHandler.spawnReturnDungeon;
        }
    }
    else if (scene.name == "DungeonEntrance")
    {
        if (lastUsedTag == "TownExit")
        {
            spawnPoint = sceneHandler.spawnDungeonEntrance;
        }
        else if (lastUsedTag == "DungeonExit")
        {
            spawnPoint = sceneHandler.spawnExitDungeon;
        }
    }
    else if (scene.name == "DungeonInside")
    {
        spawnPoint = sceneHandler.spawnInsideDungeon;
    }

    if (spawnPoint != null)
    {
        StartCoroutine(SetPlayerPositionDelayed(spawnPoint.position));
    }
    Camera newCam = Camera.main;
    if (newCam != null)
        mainCamera = newCam;
    SceneManager.sceneLoaded -= OnSceneLoaded;
}
    private IEnumerator SetPlayerPositionDelayed(Vector3 position)
    {
        // Wait for a frame to ensure the scene is fully initialized
        yield return null; 
        transform.position = position;
        Debug.Log("Player Spawned at: " + transform.position);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;
        if (body == null || body.isKinematic) return;

        // push only horizontally
        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0f, hit.moveDirection.z).normalized;

        // set its lateral velocity
        float pushSpeed = 5f; // tweak down if it still tunnels
        Vector3 desiredVelocity = pushDir * pushSpeed;
        // preserve any existing vertical velocity
        desiredVelocity.y = body.linearVelocity.y;
        body.linearVelocity = desiredVelocity;
    }

    public void shootIceBall(){
        Vector3 spawnOffset = transform.forward * 2f + Vector3.up * 2f;
        Vector3 spawnPosition = transform.position + spawnOffset;
        GameObject thrownBall = Instantiate(IceBall, spawnPosition, quaternion.identity);
        thrownBall.GetComponent<Rigidbody>().linearVelocity = transform.forward * 25;

    }

    public void shootIceBeam(){
         Vector3 spawnOffset = transform.forward * 1f + Vector3.up * 1f;
        Vector3 spawnPosition = transform.position + spawnOffset;
        GameObject shotBeam = Instantiate(IceBeam, spawnPosition, Quaternion.LookRotation(transform.forward));
        shotBeam.transform.SetParent(transform);
        Destroy(shotBeam, 3f);
    }
}
