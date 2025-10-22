using UnityEngine;

public class PlatformCleaner : MonoBehaviour
{
    public Transform player;
    public float distanceBelow = 8f;

    void Update()
    {
        foreach (GameObject platform in GameObject.FindGameObjectsWithTag("Platform"))
        {
            if (platform.transform.position.y < player.position.y - distanceBelow)
            {
                Destroy(platform);
            }
        }
    }
}
