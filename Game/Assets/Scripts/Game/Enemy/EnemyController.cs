using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class EnemyController : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float flyHeight = 3f;
    public float floatAmplitude = 0.3f;
    public float floatFrequency = 2f;
    public float knockbackForce = 50f; // 💥 플레이어 튕겨내는 힘

    private int direction = 1;
    private float lifetime = 10f;
    private float baseY;

    private SpriteRenderer sprite;

    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        transform.position = new Vector3(transform.position.x, flyHeight, 0);
        baseY = transform.position.y;

    }

    void Update()
    {
        if (!GameManager.instance.checkGameRun())
        {
            return;
        }

        transform.Translate(Vector2.right * direction * moveSpeed * Time.deltaTime);

        float offsetY = Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
        transform.position = new Vector3(transform.position.x, baseY + offsetY, transform.position.z);

        lifetime -= Time.deltaTime;
        if (lifetime <= 0f) Destroy(gameObject);
    }

    public void SetDirection(int dir)
    {
        direction = dir;

        if (dir < 0)
        {
            sprite.flipX = false;
        }
        else
        {
            sprite.flipX = true;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("💥 Enemy hit Player!");
            GameManager.instance.AddScore(-10);

            // 💥 플레이어 튕겨내기
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            PlayerController controller = other.GetComponent<PlayerController>();
            controller.Stop();
            rb.velocity = Vector2.zero;
            if (rb != null)
            {
                float directionX = (other.transform.position.x > transform.position.x) ? 1f : -1f;
                Vector2 knockbackDirection = new Vector2(directionX, 0.5f);
                rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
            }
            Destroy(gameObject);
        }
    }
}



