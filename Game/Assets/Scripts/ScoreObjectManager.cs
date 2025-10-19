using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreObjectManager : MonoBehaviour
{
    private int score = 1000;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            GameManager.instance.AddScore(score);
            Destroy(gameObject);
        }
    }
}
