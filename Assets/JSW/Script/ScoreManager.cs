using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    // 1. �̱��� �ν��Ͻ� ����
    public static ScoreManager Instance { get; private set; }

    [Header("���� ����")]
    // �ܺο��� ���� ���� ���� ������, ������ �� ��ũ��Ʈ ���ο����� �����ϵ��� ����
    public int CurrentScore { get; private set; }
    public int BestScore { get; private set; }

    [Header("�̺�Ʈ (UI �����)")]
    // ������ ����� ������ ȣ��� �̺�Ʈ (Inspector���� UI �Լ� ���� ����)
    public UnityEvent<int> onScoreChanged;

    private const string BestScoreKey = "BestScore"; // ���� Ű��

    void Awake()
    {
        // 2. �̱��� ���� ���� (�ߺ� ����)
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // ���� �ٲ� ���� �Ŵ����� ������� ����
        }
        else
        {
            // �̹� �Ŵ����� �����Ѵٸ� ���� ���� �� �ı�
            Destroy(gameObject);
        }
    }

    void Start()
    {
        UpdateScoreUI(); // �ʱ� UI ����
    }

    // 3. ���� �߰� �޼��� (�ܺο��� ȣ��)
    public void AddScore(int amount)
    {
        CurrentScore += amount;
        Debug.Log("Up : " + CurrentScore);

        // �ְ� ���� ���� üũ
        if (CurrentScore > BestScore)
        {
            BestScore = CurrentScore;
        }

        // ���� ���� �˸� (UI ������Ʈ ��)
        UpdateScoreUI();

        // TimerManager에 선물 배달 성공 알림
        if (TimerManager.Instance != null)
        {
            TimerManager.Instance.CheckWinCondition();
        }
    }

    // ���� �ʱ�ȭ (���� ����� �� ��)
    public void ResetScore()
    {
        CurrentScore = 0;
        UpdateScoreUI();
    }

    // UI ������Ʈ �̺�Ʈ ����
    private void UpdateScoreUI()
    {
        // �̺�Ʈ�� ��ϵ� �����ʰ� �ִٸ� ���� ������ ����
        onScoreChanged?.Invoke(CurrentScore);

        // �α� Ȯ�ο�
        // Debug.Log($"���� ����: {CurrentScore} / �ְ� ����: {BestScore}");
    }

}
