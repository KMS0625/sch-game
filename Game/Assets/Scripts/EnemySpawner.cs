//using UnityEngine;

//public class EnemySpawner : MonoBehaviour
//{
//    public GameObject enemyPrefab;  // 갈매기 프리팹
//    public float spawnInterval = 3f; // 생성 주기
//    public float spawnHeight = 3f;   // 하늘 높이 (플레이어 위 위치)
//    public float spawnRangeX = 8f;   // 좌우 랜덤 생성 범위

//    void Start()
//    {
//        InvokeRepeating(nameof(SpawnEnemy), 2f, spawnInterval);
//    }

//    void SpawnEnemy()
//    {
//        float randomX = Random.Range(-spawnRangeX, spawnRangeX);
//        Vector3 spawnPos = new Vector3(randomX, spawnHeight, 0);
//        Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
//    }
//}




// 그냥 랜덤생성 완성본
//using UnityEngine;

//public class EnemySpawner : MonoBehaviour
//{
//    [Header("Spawn 설정")]
//    public GameObject enemyPrefab;    // 갈매기 프리팹
//    public float spawnInterval = 3f;  // 생성 주기
//    public float spawnHeight = 3f;    // 기본 비행 높이
//    public float spawnRangeX = 8f;    // 좌우 생성 범위

//    [Header("랜덤 설정")]
//    public float minSpeed = 1.5f;     // 최소 속도
//    public float maxSpeed = 3.5f;     // 최대 속도
//    public float minHeightOffset = 0f; // 최소 높이 편차
//    public float maxHeightOffset = 2f; // 최대 높이 편차

//    void Start()
//    {
//        InvokeRepeating(nameof(SpawnEnemy), 2f, spawnInterval);
//    }

//    void SpawnEnemy()
//    {
//        if (enemyPrefab == null)
//        {
//            Debug.LogWarning("⚠️ Enemy Prefab이 지정되지 않았어요!");
//            return;
//        }

//        // 랜덤 위치 (화면 좌우 랜덤)
//        float randomX = Random.Range(-spawnRangeX, spawnRangeX);
//        float randomY = spawnHeight + Random.Range(minHeightOffset, maxHeightOffset);
//        Vector3 spawnPos = new Vector3(randomX, randomY, 0);

//        // 랜덤한 회전 (항상 똑바로)
//        Quaternion spawnRot = Quaternion.identity;

//        // 갈매기 생성
//        GameObject enemy = Instantiate(enemyPrefab, spawnPos, spawnRot);

//        // EnemyController 속성에 랜덤 값 적용
//        EnemyController controller = enemy.GetComponent<EnemyController>();
//        if (controller != null)
//        {
//            controller.moveSpeed = Random.Range(minSpeed, maxSpeed);
//            controller.flyHeight = randomY; // 각 갈매기별로 높이 다르게
//        }

//        Debug.Log("🕊️ 갈매기 소환됨! 속도: " + controller.moveSpeed.ToString("F1"));
//    }
//}





// 양쪽에서 랜덤생성
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn 설정")]
    public GameObject enemyPrefab;       // 갈매기 프리팹
    public float spawnInterval = 3f;     // 생성 주기
    public float spawnHeight = 3f;       // 비행 높이 (Y 위치)
    public float spawnXRange = 9f;       // 양쪽 생성 위치 거리 (예: 9면 -9~+9 사이)

    [Header("랜덤 설정")]
    public float minSpeed = 1.5f;        // 최소 속도
    public float maxSpeed = 3.5f;        // 최대 속도

    void Start()
    {
        InvokeRepeating(nameof(SpawnEnemy), 2f, spawnInterval);
    }

    void SpawnEnemy()
    {
        if (enemyPrefab == null)
        {
            Debug.LogWarning("⚠️ Enemy Prefab이 지정되지 않았어요!");
            return;
        }

        // 👇 1️⃣ 왼쪽 or 오른쪽 중 랜덤 선택
        bool spawnOnLeft = Random.Range(0, 2) == 0;

        float spawnX = spawnOnLeft ? -spawnXRange : spawnXRange;
        float spawnY = spawnHeight + Random.Range(-1f, 1f); // 살짝 랜덤 높이
        Vector3 spawnPos = new Vector3(spawnX, spawnY, 0);

        // 갈매기 생성
        GameObject enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);

        // 👇 2️⃣ EnemyController 가져오기
        EnemyController controller = enemy.GetComponent<EnemyController>();

        if (controller != null)
        {
            // 왼쪽에서 나오면 오른쪽으로, 오른쪽에서 나오면 왼쪽으로
            controller.moveSpeed = Random.Range(minSpeed, maxSpeed);
            controller.flyHeight = spawnY;

            // 방향 반대로 설정
            controller.SetDirection(spawnOnLeft ? 1 : -1);
        }

        Debug.Log("🕊️ 갈매기 생성됨 (" + (spawnOnLeft ? "왼쪽" : "오른쪽") + ")");
    }
}
