using TMPro;
using UnityEngine;

public class TimerUI : MonoBehaviour
{
    public TextMeshProUGUI timerText; // UI 텍스트 컴포넌트

    private void Start()
    {
        // TextMeshProUGUI 컴포넌트 자동 찾기 (없으면 수동 할당)
        if (timerText == null)
        {
            timerText = GetComponent<TextMeshProUGUI>();
        }

        // TimerManager의 이벤트 구독
        if (TimerManager.Instance != null)
        {
            TimerManager.Instance.onTimerChanged.AddListener(UpdateTimerUI);
        }
    }

    // 이 메서드는 UnityEvent<float>의 시그니처와 일치해야 합니다.
    // 매개변수로 float(남은 시간)를 받습니다.
    public void UpdateTimerUI(float remainingTime)
    {
        if (timerText != null)
        {
            // 초를 분:초 형식으로 표시 (예: 60.5초 → "01:00")
            int minutes = Mathf.FloorToInt(remainingTime / 60f);
            int seconds = Mathf.FloorToInt(remainingTime % 60f);
            timerText.text = $"{minutes:00}:{seconds:00}";
            
            // 또는 단순히 초만 표시하려면:
            // timerText.text = Mathf.CeilToInt(remainingTime).ToString();
        }
    }

    // 컴포넌트가 파괴될 때 이벤트 구독 해제
    private void OnDestroy()
    {
        if (TimerManager.Instance != null)
        {
            TimerManager.Instance.onTimerChanged.RemoveListener(UpdateTimerUI);
        }
    }
}