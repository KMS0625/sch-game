using UnityEngine;
using System.Collections;

public class RespawnPlatform : MonoBehaviour
{
    public float disappearTime = 1f;
    public float respawnTime = 3f;

    private SpriteRenderer sr;
    private Collider2D col;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            StartCoroutine(DisappearAndRespawn());
    }

    private IEnumerator DisappearAndRespawn()
    {
        yield return new WaitForSeconds(disappearTime);
        sr.enabled = false;
        col.enabled = false;

        yield return new WaitForSeconds(respawnTime);
        sr.enabled = true;
        col.enabled = true;
    }
}
