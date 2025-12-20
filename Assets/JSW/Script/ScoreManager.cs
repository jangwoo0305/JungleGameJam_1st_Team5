using UnityEngine;
using UnityEngine.Events;

public class ScoreManager : MonoBehaviour
{
    // 1. 싱글톤 인스턴스 선언
    public static ScoreManager Instance { get; private set; }

    [Header("점수 설정")]
    // 외부에서 값을 읽을 수는 있지만, 수정은 이 스크립트 내부에서만 가능하도록 설정
    public int CurrentScore { get; private set; }
    public int BestScore { get; private set; }

    [Header("이벤트 (UI 연결용)")]
    // 점수가 변경될 때마다 호출될 이벤트 (Inspector에서 UI 함수 연결 가능)
    public UnityEvent<int> onScoreChanged;

    private const string BestScoreKey = "BestScore"; // 저장 키값

    void Awake()
    {
        // 2. 싱글톤 패턴 구현 (중복 방지)
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬이 바뀌어도 점수 매니저는 사라지지 않음
        }
        else
        {
            // 이미 매니저가 존재한다면 새로 생긴 건 파괴
            Destroy(gameObject);
        }
    }

    void Start()
    {
        UpdateScoreUI(); // 초기 UI 갱신
    }

    // 3. 점수 추가 메서드 (외부에서 호출)
    public void AddScore(int amount)
    {
        CurrentScore += amount;
        Debug.Log("Up : " + CurrentScore);

        // 최고 점수 갱신 체크
        if (CurrentScore > BestScore)
        {
            BestScore = CurrentScore;
        }

        // 점수 변경 알림 (UI 업데이트 등)
        UpdateScoreUI();
    }

    // 점수 초기화 (게임 재시작 시 등)
    public void ResetScore()
    {
        CurrentScore = 0;
        UpdateScoreUI();
    }

    // UI 업데이트 이벤트 실행
    private void UpdateScoreUI()
    {
        // 이벤트에 등록된 리스너가 있다면 현재 점수를 보냄
        onScoreChanged?.Invoke(CurrentScore);

        // 로그 확인용
        // Debug.Log($"현재 점수: {CurrentScore} / 최고 점수: {BestScore}");
    }

}
