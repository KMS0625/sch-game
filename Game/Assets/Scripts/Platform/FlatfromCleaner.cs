using System.Collections;
using UnityEngine;
using UnityEngine.Video;
public class PlatformCleaner : MonoBehaviour
{
    public GameObject platformParent; // 부모 오브젝트

    public void CleanPlatforms(float disappearTime)
    {

        foreach (Transform child in platformParent.transform)
        {
            StartCoroutine(BlinkAndDestroy(child.gameObject, disappearTime));
        }
    }

    IEnumerator BlinkAndDestroy(GameObject platform, float blinkTime)
    {
        SpriteRenderer sr = platform.GetComponent<SpriteRenderer>();
        if (sr == null) yield break;

        float elapsed = 0f;
        while (elapsed < blinkTime)
        {
            sr.color = new Color(1f, 1f, 1f, 0.5f);
            yield return new WaitForSeconds(0.3f);
            sr.color = new Color(1f, 1f, 1f, 1f);
            yield return new WaitForSeconds(0.3f);
            elapsed += 0.6f;
        }
        if (sr != null)
        {
            sr.color = new Color(1f, 1f, 1f, 1f);
        }
        if (platform != null)
        {
            Destroy(platform);
        }
    }
}