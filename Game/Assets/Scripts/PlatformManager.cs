using System.Collections.Generic;
using UnityEngine;

public class PlatformManager : MonoBehaviour
{
    [Header("플랫폼 프리팹")]
    public GameObject basicPlatformPrefab;
    public GameObject breakingPlatformPrefab;
    public GameObject respawnPlatformPrefab;

    [Header("플랫폼 위치 리스트 (3단계)")]
    public List<Vector3> phase1Positions = new List<Vector3>();
    public List<Vector3> phase2Positions = new List<Vector3>();
    public List<Vector3> phase3Positions = new List<Vector3>();

    [Header("시간 / 난이도 설정")]
    public float totalGameTime = 120f; // 2분
    public float spawnInterval = 2f;   // 초기 생성 간격
    private float nextPhaseTime;       // 다음 페이즈로 바뀌는 시간
    private int currentPhase = 1;

    private List<GameObject> currentPlatforms = new List<GameObject>();

    void Start()
    {
        // 2분 동안 3단계로 나누면, 40초마다 변화
        nextPhaseTime = totalGameTime / 3f;

        GeneratePlatforms(phase1Positions);
        InvokeRepeating(nameof(SpawnPlatformCycle), spawnInterval, spawnInterval);
        Invoke(nameof(ChangeToPhase2), nextPhaseTime);
        Invoke(nameof(ChangeToPhase3), nextPhaseTime * 2);
    }

    void SpawnPlatformCycle()
    {
        // 주기적으로 재생성 효과 — 플랫폼 재배치 전 삭제
        CleanUpPlatforms();
        GenerateCurrentPhasePlatforms();
    }

    void ChangeToPhase2()
    {
        currentPhase = 2;
        spawnInterval *= 0.8f; // 속도 살짝 증가
        CancelInvoke(nameof(SpawnPlatformCycle));
        InvokeRepeating(nameof(SpawnPlatformCycle), spawnInterval, spawnInterval);
        Debug.Log("⚡ 2단계 진입! 플랫폼 위치 변경 및 속도 증가");
    }

    void ChangeToPhase3()
    {
        currentPhase = 3;
        spawnInterval *= 0.7f; // 더 빠르게
        CancelInvoke(nameof(SpawnPlatformCycle));
        InvokeRepeating(nameof(SpawnPlatformCycle), spawnInterval, spawnInterval);
        Debug.Log("🔥 3단계 진입! 난이도 최고");
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
