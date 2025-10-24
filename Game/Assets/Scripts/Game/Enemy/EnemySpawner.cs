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

    [Header("부모 오브젝트 설정")]
    public GameObject parentObject;

    void Start()
    {
        InvokeRepeating(nameof(SpawnEnemy), 2f, spawnInterval);
    }

    void SpawnEnemy()
    {
        if (!GameManager.instance.checkGameRun())
        {
            return;
        }

        if (enemyPrefab == null)
        {
            Debug.LogWarning("⚠️ Enemy Prefab이 지정되지 않았어요!");
            return;
        }

        bool spawnOnLeft = Random.Range(0, 2) == 0;

        float spawnX = spawnOnLeft ? -spawnXRange : spawnXRange;
        float spawnY = Random.Range(-4f, 3.5f); // 살짝 랜덤 높이
        Vector3 spawnPos = new Vector3(spawnX, spawnY, 0);

        GameObject enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
        enemy.transform.SetParent(parentObject.transform, true);

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
