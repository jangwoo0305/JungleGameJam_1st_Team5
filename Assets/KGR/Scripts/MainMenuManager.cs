using UnityEngine;
using UnityEngine.SceneManagement;
// using UnityEngine.UI;

/// <summary>
/// 메인 메뉴 씬을 관리하는 스크립트
/// 플레이 버튼과 스코어 버튼을 처리합니다.
/// </summary>
public class MainMenuManager : MonoBehaviour
{
    [Header("UI References")]
    // [SerializeField] private Button playButton;
    // [SerializeField] private Button scoreButton;
    
    [Header("Scene Settings")]
    [SerializeField] private string gameSceneName = "SampleScene"; // 게임 씬 이름
    
    private void Start()
    {
        // 버튼 이벤트 연결
        // if (playButton != null)
        // {
        //     playButton.onClick.AddListener(OnPlayButtonClicked);
        // }
        // else
        // {
        //     Debug.LogWarning("PlayButton이 할당되지 않았습니다!");
        // }
        
        // if (scoreButton != null)
        // {
        //     scoreButton.onClick.AddListener(OnScoreButtonClicked);
        // }
        // else
        // {
        //     Debug.LogWarning("ScoreButton이 할당되지 않았습니다!");
        // }
    }
    
    private void OnDestroy()
    {
        // 메모리 누수 방지: 이벤트 해제
        // if (playButton != null)
        // {
        //     playButton.onClick.RemoveListener(OnPlayButtonClicked);
        // }
        //
        // if (scoreButton != null)
        // {
        //     scoreButton.onClick.RemoveListener(OnScoreButtonClicked);
        // }
    }
    
    /// <summary>
    /// 플레이 버튼 클릭 시 게임 씬으로 이동
    /// </summary>
    public void OnPlayButtonClicked()
    {
        Debug.Log("플레이 버튼 클릭");
        
        // 게임 씬으로 이동
        if (!string.IsNullOrEmpty(gameSceneName))
        {
            SceneManager.LoadScene(gameSceneName);
        }
        else
        {
            Debug.LogError("게임 씬 이름이 설정되지 않았습니다!");
        }
    }
    
    /// <summary>
    /// 스코어 버튼 클릭 (현재는 동작하지 않음)
    /// </summary>
    public void OnScoreButtonClicked()
    {
        Debug.Log("스코어 버튼 클릭 (기능 미구현)");
        // TODO: 나중에 스코어 화면 구현
    }
}
