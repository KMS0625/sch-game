using UnityEngine;

public class PlatformGenerator : MonoBehaviour
{
    public GameObject[] platformPrefabs; // 기본, 부서짐, 재생성 프리팹 배열
    public Transform player;
    public float generateDistance = 10f; // 카메라 위쪽에 생성 거리
    public float minX = -7f;
    public float maxX = 7f;
    public float yGap = 1.5f;

    private float highestY;

    void Start()
    {
        highestY = player.position.y;
    }

    void Update()
    {
        if (player.position.y + generateDistance > highestY)
        {
            GeneratePlatform();
        }
    }

    void GeneratePlatform()
    {
        for (int i = 0; i < 3; i++)
        {
            float randomX = Random.Range(minX, maxX);
            highestY += yGap;
            Vector3 spawnPos = new Vector3(randomX, highestY, 0);

            int index = Random.Range(0, platformPrefabs.Length);
            Instantiate(platformPrefabs[index], spawnPos, Quaternion.identity);
        }
    }
}
