using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 10f;
    public float gravity = -9.8f;
    CharacterController characterController;
    public static Player Instance;
    private string lastUsedTag = "";
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
    void Awake()
    {
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
    }

    public void MoveWithCC(Vector3 direction){
        characterController.Move(direction * speed * Time.deltaTime);
        transform.LookAt(transform.position + direction);
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
    SceneManager.sceneLoaded -= OnSceneLoaded;
}
    private IEnumerator SetPlayerPositionDelayed(Vector3 position)
    {
        // Wait for a frame to ensure the scene is fully initialized
        yield return null; 
        transform.position = position;
        Debug.Log("Player Spawned at: " + transform.position);
    }
}
