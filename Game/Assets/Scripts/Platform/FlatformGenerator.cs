using Unity.VisualScripting;
using UnityEngine;

public class PlatformGenerator : MonoBehaviour
{
    public GameObject stage;      // Platforms_1 ~ Platforms_8이 들어있는 부모 오브젝트
    public GameObject platform;   // 생성할 프리팹
    public GameObject platformParent;

    public GameObject score;
    public GameObject highScore;

    GameObject newObject;

    public void GeneratePlatform()
    {
        int rand = Random.Range(0, 9);
        Transform targetGroup = stage.transform.Find("Platforms_" + rand);
        if (targetGroup == null)
        {
            Debug.LogWarning($"Platforms_{rand} 오브젝트를 찾을 수 없습니다.");
            return;
        }


        Debug.Log("Platforms_" + rand);
        foreach (Transform child in targetGroup)
        {
            if (child.name == "Platform_Position")
            {
                newObject = Instantiate(platform, child.position, Quaternion.identity);
            }

            if (child.name == "Fish_Position")
            {
                newObject = Instantiate(score, child.position, Quaternion.identity);
            }

            if (child.name == "GoldFish_Position")
            {
                newObject = Instantiate(highScore, child.position, Quaternion.identity);
            }

            newObject.transform.SetParent(platformParent.transform.transform);
        }
    }
}
