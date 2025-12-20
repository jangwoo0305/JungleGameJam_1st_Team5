using UnityEngine;

public class QuitHandler : MonoBehaviour
{
    public void ExitGame()
    {
        // 1. 실제 빌드된 게임 종료
        Application.Quit();

        // 2. 유니티 에디터에서 실행 중일 때 재생 모드 종료 (테스트용)
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif

        Debug.Log("게임이 종료되었습니다.");
    }
}