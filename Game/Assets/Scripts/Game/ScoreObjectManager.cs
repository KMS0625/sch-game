using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class ScoreObjectManager : MonoBehaviour
{
    [SerializeField] private int score;
    [SerializeField] private AudioSource audioSource;

    private bool collected;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        collected = false;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // ⭐️ 중복 충돌 방지 체크 추가
        if (collision.CompareTag("Player") && !collected)
        {
            collected = true;
            GameManager.instance.AddScore(score);
            StartCoroutine(CollectAndDestroyRoutine());
        }
    }

    // ⭐️ 코루틴: 수집 시각적 처리 및 파괴 담당
    private IEnumerator CollectAndDestroyRoutine()
    {
        Collider2D col = GetComponent<Collider2D>();
        SpriteRenderer rend = GetComponent<SpriteRenderer>();

        if (col != null) col.enabled = false;
        if (rend != null) rend.enabled = false;

        float clipLength = 0f;

        if (audioSource != null && audioSource.clip != null)
        {
            audioSource.Play();
            clipLength = audioSource.clip.length;
        }

        if (clipLength > 0.01f)
        {
            yield return new WaitForSeconds(clipLength);
        }
        else
        {
            yield return new WaitForSeconds(0.1f);
        }
        Destroy(gameObject);
    }
}

