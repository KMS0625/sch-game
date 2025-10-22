//using UnityEngine;
//using UnityEngine.TestTools;

//public class PlayerHealth : MonoBehaviour
//{
//    public int maxHealth = 3;
//    private int currentHealth;

//    void Start()
//    {
//        currentHealth = maxHealth;
//    }

//    public void TakeDamage(int amount)
//    {
//        currentHealth -= amount;
//        Debug.Log("🩸 Player hit! 현재 체력: " + currentHealth);

//        if (currentHealth <= 0)
//        {
//            Debug.Log("💀 Player Died!");
//            // 나중에 GameOver UI 연결 가능
//        }
//    }

//    void Update()
//    {
//        // 좌우 왕복 이동
//        transform.Translate(Vector2.right * direction * moveSpeed * Time.deltaTime);

//        if (Mathf.Abs(transform.position.x - startPosition.x) > moveRange)
//            direction *= -1;

//        // 살짝 위아래로 흔들리는 효과 (날아다니는 느낌)
//        float newY = startPosition.y + Mathf.Sin(Time.time * 2f) * 0.3f;
//        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
//    }

//}


using UnityEngine;
using TMPro;  // ← 추가!

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 3;
    private int currentHealth;

    [Header("UI Elements")]
    public TextMeshProUGUI healthText;  // ← Text 대신 TMP로 변경

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        Debug.Log("🩸 Player hit! 현재 체력: " + currentHealth);
        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            Debug.Log("💀 Player Died!");
            // 나중에 Game Over 처리
        }
    }

    void UpdateHealthUI()
    {
        if (healthText != null)
            healthText.text = "Health : " + currentHealth;
    }
}

