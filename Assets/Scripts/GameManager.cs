#if UNITY_EDITOR
    using UnityEditor;
#endif
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    
    public static GameManager Instance { get 
    {
        if (_instance == null)
            _instance = FindFirstObjectByType<GameManager>();
        return _instance;
    } }
    
    public  bool isGameOver = false;
    
    public void EndGame()
    {
        if (isGameOver) return;
        isGameOver = true;
        
        Debug.Log("게임 종료!");

#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
