using UnityEngine;

public class TrackPlayerRoom : MonoBehaviour
{
   public string currentRoom;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Room1"))
        {
            currentRoom = other.tag;
        }
        if (other.CompareTag("Room2"))
        {
            currentRoom = other.tag;
        }
        if (other.CompareTag("Room3"))
        {
            currentRoom = other.tag;
        }
        if (other.CompareTag("Room4"))
        {
            currentRoom = other.tag;
        }
        if (other.CompareTag("Room5"))
        {
            currentRoom = other.tag;
        }
        if (other.CompareTag("Room6"))
        {
            currentRoom = other.tag;
        }
        if (other.CompareTag("Room7"))
        {
            currentRoom = other.tag;
        }
        if (other.CompareTag("Room8"))
        {
            currentRoom = other.tag;
        }
        if (other.CompareTag("Room9"))
        {
            currentRoom = other.tag;
        }
        if (other.CompareTag("Room10"))
        {
            currentRoom = other.tag;
        }
        if (other.CompareTag("Room11"))
        {
            currentRoom = other.tag;
        }
    }
}
