using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    public TextMeshProUGUI scoreText; // UI 텍스트 연결

    private void Start()
    {
        scoreText = GetComponent<TextMeshProUGUI>();
        ScoreManager.Instance.onScoreChanged.AddListener(UpdateScoreUI);
    }

    // 이 함수를 UnityEvent에 연결할 겁니다.
    // 매개변수로 int를 받아야 UnityEvent<int>와 연결됩니다.
    public void UpdateScoreUI(int newScore)
    {
        scoreText.text = newScore.ToString();
    }

    // [중요] 스크립트가 사라질 때 연결을 끊어줘야 에러가 안 납니다.
    private void OnDestroy()
    {
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.onScoreChanged.RemoveListener(UpdateScoreUI);
        }
    }
}
