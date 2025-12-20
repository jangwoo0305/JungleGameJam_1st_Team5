using UnityEngine;
using UnityEngine.Events;

public class TimerManager : MonoBehaviour
{
    // 싱글톤 인스턴스
    public static TimerManager Instance { get; private set; }

    [Header("게임 설정")]
    [SerializeField] private float finishTime = 60f; // 타이머 시간 (초)
    [SerializeField] private int goal = 5; // 목표 선물 배달 개수

    [Header("타이머 UI 이벤트")]
    public UnityEvent<float> onTimerChanged; // 타이머 값 변경 시 호출 (남은 시간 전달)

    [Header("게임 상태 이벤트")]
    public UnityEvent onGameWin; // 게임 승리 시 호출
    public UnityEvent onGameLose; // 게임 실패 시 호출

    // 게임 상태
    public bool isGameActive { get; private set; }
    public bool isGameWon { get; private set; }
    public bool isGameLost { get; private set; }

    // 타이머 관련
    private float currentTime;
    private bool timerStarted = false;

    void Awake()
    {
        // 싱글톤 설정
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // 스테이지 시작 시 타이머 시작
        StartTimer();
    }

    void Update()
    {
        if (!isGameActive || !timerStarted) return;

        // 타이머 카운트다운
        currentTime -= Time.deltaTime;

        // 타이머 UI 업데이트
        onTimerChanged?.Invoke(currentTime);

        // 타이머가 0 이하가 되면 실패 체크
        if (currentTime <= 0f)
        {
            currentTime = 0f;
            CheckLoseCondition();
        }
    }

    /// <summary>
    /// 타이머 시작
    /// </summary>
    public void StartTimer()
    {
        if (timerStarted) return;

        currentTime = finishTime;
        isGameActive = true;
        isGameWon = false;
        isGameLost = false;
        timerStarted = true;

        Debug.Log($"타이머 시작! 목표 시간: {finishTime}초, 목표 배달: {goal}개");
        onTimerChanged?.Invoke(currentTime);
    }

    /// <summary>
    /// 타이머 중지
    /// </summary>
    public void StopTimer()
    {
        isGameActive = false;
        timerStarted = false;
    }

    /// <summary>
    /// 승리 조건 체크 (선물 배달 성공 시 호출)
    /// </summary>
    public void CheckWinCondition()
    {
        if (!isGameActive || isGameWon || isGameLost) return;

        if (ScoreManager.Instance == null)
        {
            Debug.LogWarning("ScoreManager를 찾을 수 없습니다!");
            return;
        }

        int currentScore = ScoreManager.Instance.CurrentScore;

        // 목표 달성 시 승리
        if (currentScore >= goal && currentTime > 0f)
        {
            WinGame();
        }
    }

    /// <summary>
    /// 실패 조건 체크 (타이머 종료 시 호출)
    /// </summary>
    private void CheckLoseCondition()
    {
        if (!isGameActive || isGameWon || isGameLost) return;

        if (ScoreManager.Instance == null)
        {
            Debug.LogWarning("ScoreManager를 찾을 수 없습니다!");
            return;
        }

        int currentScore = ScoreManager.Instance.CurrentScore;

        // 타이머가 끝났는데 목표를 달성하지 못한 경우 실패
        if (currentScore < goal)
        {
            LoseGame("시간 초과");
        }
    }

    /// <summary>
    /// 적에게 발각되었을 때 호출
    /// </summary>
    public void OnPlayerDetected()
    {
        if (!isGameActive || isGameWon || isGameLost) return;

        LoseGame("적에게 발각됨");
    }

    /// <summary>
    /// 게임 승리 처리
    /// </summary>
    private void WinGame()
    {
        if (isGameWon || isGameLost) return;

        isGameWon = true;
        isGameActive = false;
        StopTimer();

        Debug.Log($"게임 승리! 배달 성공: {ScoreManager.Instance.CurrentScore}/{goal}개, 남은 시간: {currentTime:F2}초");
        
        onGameWin?.Invoke();
        
        if (GameManager.Instance != null)
        {
            GameManager.Instance.WinGame();
        }
    }

    /// <summary>
    /// 게임 실패 처리
    /// </summary>
    private void LoseGame(string reason = "알 수 없는 이유")
    {
        if (isGameWon || isGameLost) return;

        isGameLost = true;
        isGameActive = false;
        StopTimer();

        int currentScore = ScoreManager.Instance != null ? ScoreManager.Instance.CurrentScore : 0;
        Debug.Log($"게임 실패! 이유: {reason}, 배달 성공: {currentScore}/{goal}개, 남은 시간: {currentTime:F2}초");
        
        onGameLose?.Invoke();
        
        if (GameManager.Instance != null)
        {
            GameManager.Instance.LoseGame();
        }
    }

    /// <summary>
    /// 현재 남은 시간 반환
    /// </summary>
    public float GetRemainingTime()
    {
        return currentTime;
    }

    /// <summary>
    /// 목표 개수 반환
    /// </summary>
    public int GetGoal()
    {
        return goal;
    }
}

