using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class EndingScoreUI : MonoBehaviour
{
    [SerializeField]
    TMP_Text text;

    private void Start()
    {
        text.text = ScoreManager.Instance.CurrentScore.ToString();
    }
}
