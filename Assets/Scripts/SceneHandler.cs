using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
    public Transform spawnHouse;
    public Transform spawnOutsideHouse;
    public Transform spawnDungeonEntrance;
    public Transform spawnInsideDungeon;
    public Transform spawnReturnDungeon;
    public Transform spawnExitDungeon;
    public GameObject player;
    
    void Start()
    {
#pragma warning disable CS0618 // Type or member is obsolete
        if (FindObjectsOfType<SceneHandler>().Length > 1)
    {
        Destroy(gameObject);
        return;
    }
#pragma warning restore CS0618 // Type or member is obsolete
        DontDestroyOnLoad(gameObject);
    }
    public void LoadSceneAndSpawnPlayer(string sceneName, string tagName)
    {
        StartCoroutine(LoadSceneAndWaitForSpawn(sceneName, tagName));
    }
    private IEnumerator LoadSceneAndWaitForSpawn(string sceneName, string tagName)
    {
        // Load the scene asynchronously
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        Debug.Log("Scene Loaded: " + sceneName);
        // Wait until the scene has finished loading
        while (!asyncLoad.isDone)
        {
            yield return null;
            Debug.Log("Waiting");
        }
        Debug.Log("Scene Loaded: " + sceneName);

        yield return StartCoroutine(FindPlayerObject());
        // Once the scene is loaded, instantiate the player at the correct spawn point
        if (sceneName == "PlayersRoom")
        {
            Debug.Log("Spawning player at position: " + spawnHouse.position);
            CharacterController cc = player.GetComponent<CharacterController>();
            cc.enabled = false;
            player.transform.position = spawnHouse.position;
            cc.enabled = true;
        }
        else if (sceneName == "Town")
        {
            if (tagName == "PlayerDoorInside")
            {
                Debug.Log("Spawning player at position: " + spawnOutsideHouse.position);
                CharacterController cc = player.GetComponent<CharacterController>();
                cc.enabled = false;
                player.transform.position = spawnOutsideHouse.position;
                cc.enabled = true;
            }
            else if(tagName == "ExitEntrance"){
                Debug.Log("Spawning player at position: " + spawnReturnDungeon.position);
                CharacterController cc = player.GetComponent<CharacterController>();
                cc.enabled = false;
                player.transform.position = spawnReturnDungeon.position;
                cc.enabled = true;
            }
        }
        else if (sceneName == "DungeonEntrance")
        {
            if(tagName == "TownExit")
            {
                Debug.Log("Spawning player at position: " + spawnDungeonEntrance.position);
                CharacterController cc = player.GetComponent<CharacterController>();
                cc.enabled = false;
                player.transform.position = spawnDungeonEntrance.position;
                cc.enabled = true;
            }
            if(tagName == "DungeonExit")
            {
                Debug.Log("Spawning player at position: " + spawnExitDungeon.position);
                CharacterController cc = player.GetComponent<CharacterController>();
                cc.enabled = false;
                player.transform.position = spawnExitDungeon.position;
                cc.enabled = true;
            }
        }
        else if (sceneName == "DungeonInside")
        {
            Debug.Log("Spawning player at position: " + spawnInsideDungeon.position);
            CharacterController cc = player.GetComponent<CharacterController>();
            cc.enabled = false;
            player.transform.position = spawnInsideDungeon.position;
            cc.enabled = true;
        }
        Debug.Log("Player Spawned: " + player.transform.position);
    }

    private IEnumerator FindPlayerObject()
    {
        // Wait until the player object is found
        while (player == null)
        {
            player = GameObject.FindWithTag("Player");
            yield return null;
        }
        Debug.Log("Player object found!");
    }
}
