using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class PlatformManager : MonoBehaviour
{
    [Header("플랫폼 프리팹")]
    public GameObject basicPlatformPrefab;
    public GameObject breakingPlatformPrefab;
    public GameObject respawnPlatformPrefab;

    [Header("스크립트")]
    public PlatformGenerator generator;
    public PlatformCleaner cleaner;


    [Header("플랫폼 위치 리스트 (3단계)")]
    public List<Vector3> phase1Positions = new List<Vector3>();
    public List<Vector3> phase2Positions = new List<Vector3>();
    public List<Vector3> phase3Positions = new List<Vector3>();

    [Header("시간 / 난이도 설정")]
    public float spawnTime = 10;   // 초기 생성 간격
    public float minspawnTime = 3f;
    public float disappearTime = 3f;
    public float minsappearTime = 0.8f;
    private float spawnTimeCounter = 0f;
    private float currentPhase = 1;
    bool isSpawning = false;


    private List<GameObject> currentPlatforms = new List<GameObject>();

    void Start()
    {
        generator = GetComponent<PlatformGenerator>();
        cleaner = GetComponent<PlatformCleaner>();

        generator.GeneratePlatform();
        spawnTimeCounter = spawnTime;
        currentPhase = 1;
    }


    void Update()
    {
        if (GameManager.instance.checkGameRun())
        {
            spawnTimeCounter -= Time.deltaTime;

            if (!isSpawning && spawnTimeCounter <= 0)
                StartCoroutine(SpawnOnce());
        }
    }

    IEnumerator SpawnOnce()
    {
        isSpawning = true;
        SpawnPlatformCycle();

        spawnTimeCounter = spawnTime * 0.95f;
        if (spawnTimeCounter < minspawnTime)
        {
            spawnTimeCounter = minspawnTime;
        }

        disappearTime = 0.95f;
        if (disappearTime < minsappearTime)
        {
            disappearTime = minsappearTime;
        }

        // 다음 프레임까지 대기
        yield return null;
        isSpawning = false;
    }

    void SpawnPlatformCycle()
    {
        StartCoroutine(wait());
    }

    IEnumerator wait()
    {
        cleaner.CleanPlatforms(disappearTime);
        yield return new WaitForSeconds(0.5f);
        generator.GeneratePlatform();
    }

    void GenerateCurrentPhasePlatforms()
    {
        if (currentPhase == 1)
            GeneratePlatforms(phase1Positions);
        else if (currentPhase == 2)
            GeneratePlatforms(phase2Positions);
        else
            GeneratePlatforms(phase3Positions);
    }

    void GeneratePlatforms(List<Vector3> positions)
    {
        foreach (Vector3 pos in positions)
        {
            GameObject prefab = ChoosePlatformPrefab();
            if (prefab != null)
            {
                GameObject platformInstance = Instantiate(prefab, pos, Quaternion.identity);
                currentPlatforms.Add(platformInstance);
            }
        }
    }

    GameObject ChoosePlatformPrefab()
    {
        float r = Random.value;
        if (r < 0.3f)
            return breakingPlatformPrefab;
        else if (r < 0.5f)
            return respawnPlatformPrefab;
        else
            return basicPlatformPrefab;
    }

    void CleanUpPlatforms()
    {
        for (int i = 0; i < currentPlatforms.Count; i++)
        {
            if (currentPlatforms[i] != null)
                Destroy(currentPlatforms[i]);
        }

        currentPlatforms.Clear();
        Resources.UnloadUnusedAssets(); // 🔹 메모리 캐시 정리 (Missing 방지)
    }
}
