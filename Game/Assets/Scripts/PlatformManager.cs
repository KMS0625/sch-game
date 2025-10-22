using UnityEngine;

public class PlatformManager : MonoBehaviour
{
    [Header("플랫폼 프리팹 설정")]
    public GameObject basicPlatformPrefab;
    public GameObject breakingPlatformPrefab;
    public GameObject respawnPlatformPrefab;

    [Header("생성 설정")]
    public int platformCount = 10;     // 한 번에 생성할 개수
    public float spawnRangeX = 8f;     // 좌우 위치 범위
    public float spawnRangeY = 5f;     // 위아래 위치 범위
    public float minYDistance = 1.5f;  // 발판 간 최소 거리

    [Header("랜덤 확률 설정")]
    [Range(0f, 1f)] public float breakPlatformChance = 0.3f;  // 부서지는 발판 확률
    [Range(0f, 1f)] public float respawnPlatformChance = 0.2f; // 재생성 발판 확률

    void Start()
    {
        SpawnPlatforms();
    }

    void SpawnPlatforms()
    {
        for (int i = 0; i < platformCount; i++)
        {
            // X, Y 랜덤 위치
            float randomX = Random.Range(-spawnRangeX, spawnRangeX);
            float randomY = i * minYDistance + Random.Range(0f, spawnRangeY / platformCount);
            Vector3 spawnPos = new Vector3(randomX, randomY, 0);

            GameObject selectedPrefab = ChooseRandomPlatform();

            if (selectedPrefab != null)
            {
                Instantiate(selectedPrefab, spawnPos, Quaternion.identity);
            }
        }
    }

    GameObject ChooseRandomPlatform()
    {
        float rand = Random.value;

        if (rand < breakPlatformChance)
            return breakingPlatformPrefab;
        else if (rand < breakPlatformChance + respawnPlatformChance)
            return respawnPlatformPrefab;
        else
            return basicPlatformPrefab;
    }
}


//게임 시작 시 랜덤한 위치에 10개의 플랫폼 생성

//30% 확률로 부서지는 발판, 20% 확률로 재생성 발판, 나머지는 기본 발판

//부서지는 발판은 밟자마자 사라지고, 재생성 발판은 몇 초 후 다시 등장

//맵이 매번 달라져서 **“플레이어가 예측 불가능한 플랫폼 패턴”**을 경험할 수 있음 