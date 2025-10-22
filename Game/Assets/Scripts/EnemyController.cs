//using UnityEngine;

//public class EnemyController : MonoBehaviour
//{
//    [Header("Movement Settings")]
//    public float moveSpeed = 2f;           // 좌우 이동 속도
//    public float moveRange = 3f;           // 이동 범위
//    public float floatAmplitude = 0.3f;    // 위아래 이동 높이 (날갯짓 느낌)
//    public float floatFrequency = 2f;      // 위아래 이동 속도

//    [Header("Attack Settings")]
//    public int damageAmount = 1;           // 플레이어에게 줄 데미지

//    private Vector3 startPosition;
//    private int direction = 1;

//    void Start()
//    {
//        startPosition = transform.position;
//        Debug.Log("🚀 EnemyController 실행됨!");
//    }

//    void Update()
//    {
//        // 🔹 좌우 왕복 이동
//        transform.Translate(Vector2.right * direction * moveSpeed * Time.deltaTime);

//        if (Mathf.Abs(transform.position.x - startPosition.x) > moveRange)
//            direction *= -1;

//        // 🔹 위아래로 흔들리며 나는 효과 (갈매기 느낌)
//        float newY = startPosition.y + Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
//        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
//    }

//    // 🔹 플레이어와 닿았을 때 데미지 주기
//    void OnTriggerEnter2D(Collider2D collision)
//    {
//        if (collision.CompareTag("Player"))
//        {
//            Debug.Log("💥 Enemy hit Player!");

//            // PlayerHealth 스크립트 찾아서 체력 깎기
//            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
//            if (playerHealth != null)
//            {
//                playerHealth.TakeDamage(damageAmount);
//            }
//        }
//    }
//}

//using UnityEngine;

//public class EnemyController : MonoBehaviour
//{
//    [Header("이동 설정")]
//    public float moveSpeed = 2f;          // 좌우 이동 속도
//    public float moveRange = 3f;          // 좌우 왕복 거리
//    public float floatAmplitude = 0.3f;   // 위아래로 흔들리는 높이
//    public float floatFrequency = 2f;     // 위아래로 움직이는 속도
//    public float flyHeight = 3f;          // 초기 비행 높이

//    [Header("공격 설정")]
//    public int damageAmount = 1;          // 플레이어 데미지량

//    private Vector3 startPosition;
//    private int direction = 1;

//    void Start()
//    {
//        // 시작 위치 지정 (조금 위쪽에서 시작)
//        startPosition = transform.position + Vector3.up * flyHeight;
//        transform.position = startPosition;
//        Debug.Log("🕊️ EnemyController 실행됨!");
//    }

//    void Update()
//    {
//        // 🔹 좌우로 왕복 이동
//        transform.Translate(Vector2.right * direction * moveSpeed * Time.deltaTime);

//        // 이동 범위 제한 → 방향 반전
//        if (Mathf.Abs(transform.position.x - startPosition.x) > moveRange)
//        {
//            direction *= -1;
//        }

//        // 🔹 위아래로 부드럽게 흔들리는 비행 (sin 파형)
//        float offsetY = Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
//        transform.position = new Vector3(transform.position.x, startPosition.y + offsetY, transform.position.z);

//        // 화면 아래로 떨어졌을 경우 자동 제거 (안정성용)
//        if (transform.position.y < -5f)
//        {
//            Destroy(gameObject);
//        }
//    }

//    void OnTriggerEnter2D(Collider2D other)
//    {
//        if (other.CompareTag("Player"))
//        {
//            Debug.Log("💥 Enemy hit Player!");

//            // PlayerHealth 불러와서 데미지 적용
//            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
//            if (playerHealth != null)
//            {
//                playerHealth.TakeDamage(damageAmount);
//            }

//            // 갈매기 사라지기
//            Destroy(gameObject);
//        }
//    }
//}





//using UnityEngine;

//public class EnemyController : MonoBehaviour
//{
//    public float moveSpeed = 2f;
//    public float flyHeight = 3f;
//    private int direction;

//    private float lifetime = 10f; // 10초 뒤 자동 파괴

//    void Start()
//    {
//        // 랜덤 방향 (왼쪽 또는 오른쪽)
//        direction = Random.Range(0, 2) == 0 ? -1 : 1;

//        // 생성될 때 높이 보정
//        transform.position = new Vector3(transform.position.x, flyHeight, 0);

//        Debug.Log("🚀 EnemyController 실행됨! 방향: " + (direction == 1 ? "오른쪽" : "왼쪽"));
//    }

//    void Update()
//    {
//        // 갈매기 이동 (한 방향으로)
//        transform.Translate(Vector2.right * direction * moveSpeed * Time.deltaTime);

//        // lifetime 지나면 자동 제거
//        lifetime -= Time.deltaTime;
//        if (lifetime <= 0f)
//        {
//            Destroy(gameObject);
//        }
//    }

//    void OnTriggerEnter2D(Collider2D other)
//    {
//        if (other.CompareTag("Player"))
//        {
//            Debug.Log("💥 Enemy hit Player!");

//            PlayerHealth player = other.GetComponent<PlayerHealth>();
//            if (player != null)
//            {
//                player.TakeDamage(1); // 체력 1 깎기
//            }

//            Destroy(gameObject); // 충돌 후 갈매기 제거
//        }
//    }
//}


//using UnityEngine;

//public class EnemyController : MonoBehaviour
//{
//    [Header("이동 설정")]
//    public float moveSpeed = 2f;           // 좌우 이동 속도
//    public float flyHeight = 3f;           // 기본 비행 높이
//    public float floatAmplitude = 0.3f;    // 위아래로 흔들리는 높이
//    public float floatFrequency = 2f;      // 흔들림 속도

//    private int direction;                 // 이동 방향 (-1: 왼쪽, 1: 오른쪽)
//    private float lifetime = 10f;          // 일정 시간 후 자동 파괴
//    private float baseY;                   // 기준 높이
//    private SpriteRenderer spriteRenderer; // 갈매기 이미지 제어용

//    void Start()
//    {
//        // SpriteRenderer 가져오기
//        spriteRenderer = GetComponent<SpriteRenderer>();

//        // 랜덤 방향 설정 (왼쪽 또는 오른쪽)
//        direction = Random.Range(0, 2) == 0 ? -1 : 1;

//        // 시작 높이 설정
//        transform.position = new Vector3(transform.position.x, flyHeight, 0);
//        baseY = transform.position.y;

//        // 처음 방향에 따라 스프라이트 방향 결정
//        spriteRenderer.flipX = (direction == -1);

//        Debug.Log("🕊️ EnemyController 실행됨! 방향: " + (direction == 1 ? "오른쪽" : "왼쪽"));
//    }

//    void Update()
//    {
//        // 1️⃣ 좌우 이동
//        transform.Translate(Vector2.right * direction * moveSpeed * Time.deltaTime);

//        // 2️⃣ 위아래로 부드럽게 흔들리는 비행 (sin 파형)
//        float offsetY = Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
//        transform.position = new Vector3(transform.position.x, baseY + offsetY, transform.position.z);

//        // 3️⃣ lifetime이 지나면 자동 제거
//        lifetime -= Time.deltaTime;
//        if (lifetime <= 0f || transform.position.y < -5f)
//        {
//            Destroy(gameObject);
//        }

//        // 4️⃣ 이동 방향에 따라 스프라이트 좌우 반전
//        if (direction == -1)
//            spriteRenderer.flipX = true;
//        else
//            spriteRenderer.flipX = false;
//    }

//    void OnTriggerEnter2D(Collider2D other)
//    {
//        if (other.CompareTag("Player"))
//        {
//            Debug.Log("💥 Enemy hit Player!");

//            PlayerHealth player = other.GetComponent<PlayerHealth>();
//            if (player != null)
//            {
//                player.TakeDamage(1); // 체력 1 깎기
//            }

//            Destroy(gameObject); // 충돌 후 갈매기 제거
//        }
//    }
//}



//양쪽에서 랜덤생성
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("이동 설정")]
    public float moveSpeed = 2f;
    public float flyHeight = 3f;
    public float floatAmplitude = 0.3f;
    public float floatFrequency = 2f;

    private int direction = 1;
    private float lifetime = 10f;
    private float baseY;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        transform.position = new Vector3(transform.position.x, flyHeight, 0);
        baseY = transform.position.y;

        // 방향에 따라 스프라이트 반전
        spriteRenderer.flipX = (direction == -1);
    }

    void Update()
    {
        // 좌우 이동
        transform.Translate(Vector2.right * direction * moveSpeed * Time.deltaTime);

        // 위아래로 부드럽게 흔들림
        float offsetY = Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
        transform.position = new Vector3(transform.position.x, baseY + offsetY, transform.position.z);

        // Lifetime 종료 시 제거
        lifetime -= Time.deltaTime;
        if (lifetime <= 0f || transform.position.y < -5f)
            Destroy(gameObject);
    }

    // ✅ 외부에서 방향 지정할 수 있게 추가
    public void SetDirection(int dir)
    {
        direction = dir;
        if (spriteRenderer != null)
            spriteRenderer.flipX = (direction == -1);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("💥 Enemy hit Player!");
            PlayerHealth player = other.GetComponent<PlayerHealth>();
            if (player != null)
            {
                player.TakeDamage(1);
            }
            Destroy(gameObject);
        }
    }
}

