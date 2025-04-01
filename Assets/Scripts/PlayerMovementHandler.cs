using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovementHandler : MonoBehaviour
{
    public Player player;
    public Transform cameraTransform;
    public bool isDialogueActive = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
#pragma warning disable CS0618 // Type or member is obsolete
        if (FindObjectsOfType<PlayerMovementHandler>().Length > 1)
        {
            Destroy(gameObject); // Destroy this duplicate instance
            return;
        }
#pragma warning restore CS0618 // Type or member is obsolete
        DontDestroyOnLoad(gameObject);
        StartCoroutine(FindPlayer());
        UpdateCameraReference();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private IEnumerator FindPlayer()
    {
        // Wait until the player object is instantiated
        while (player == null)
        {
            player = FindFirstObjectByType<Player>();
            yield return null;
        }
        Debug.Log("Player found!");
    }


    // Update is called once per frame
    void Update()
    {
        if (player == null || isDialogueActive) return;
        Vector3 cameraForward = cameraTransform.forward;
        cameraForward.y = 0;

        Vector3 cameraRight = cameraTransform.right;
        cameraRight.y = 0;

        Vector3 finalMovement = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
        {
            finalMovement += cameraForward;
        }
        if (Input.GetKey(KeyCode.S))
        {
            finalMovement -= cameraForward;
        }

        if (Input.GetKey(KeyCode.A)){
            finalMovement -= cameraRight;
        }

        if(Input.GetKey(KeyCode.D)){
            finalMovement += cameraRight;
        }

        finalMovement.Normalize();
        player.MoveWithCC(finalMovement);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        UpdateCameraReference();
    }

    // This method will find the main camera in the current scene
    private void UpdateCameraReference()
    {
        if (cameraTransform == null)
        {
            // Try to find the camera in the current scene
            cameraTransform = Camera.main?.transform;
        }
    }

    // Clean up event registration when the object is destroyed
    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
