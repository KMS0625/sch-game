using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RankPrefabManager : MonoBehaviour
{
    public TextMeshProUGUI rankText;
    public TextMeshProUGUI idText;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI departmentText;
    public TextMeshProUGUI scoreText;
    public GameObject Tropy;
    [SerializeField] private Sprite gold;
    [SerializeField] private Sprite silver;
    [SerializeField] private Sprite bronze;
    [SerializeField] private Sprite egg;

    public void setTropy(string troypyColor)
    {
        UnityEngine.UI.Image img = Tropy.GetComponent<UnityEngine.UI.Image>();

        if (troypyColor == "gold")
        {
            img.sprite = gold;
        }
        else if (troypyColor == "silver")
        {
            img.sprite = silver;
        }
        else if (troypyColor == "bronze")
        {
            img.sprite = bronze;
        }
        else
        {
            img.sprite = egg;
        }
    }

    public void setRankPrefab(int rank, string id, string name, string department, int score, string troypyColor)
    {
        setRankText(rank);
        setIDText(id);
        setNameText(name);
        setDepartmentText(department);
        setScoreText(score);
        setTropy(troypyColor);
    }

    public void setRankText(int rank)
    {
        rankText.color = new Color(0f, 0f, 0f, 1f);
        rankText.text = rank.ToString();
    }

    public void setIDText(string text)
    {
        idText.text = text;
    }

    public void setNameText(string text)
    {
        nameText.text = text;
    }

    public void setDepartmentText(string text)
    {
        departmentText.text = text;
    }

    public void setScoreText(int score)
    {
        scoreText.text = score.ToString();
    }
}
