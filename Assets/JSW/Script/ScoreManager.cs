using UnityEngine;
using UnityEngine.Events;

public class ScoreManager : MonoBehaviour
{
    // 1. ½Ì±ÛÅæ ÀÎ½ºÅÏ½º ¼±¾ğ
    public static ScoreManager Instance { get; private set; }

    [Header("Á¡¼ö ¼³Á¤")]
    // ¿ÜºÎ¿¡¼­ °ªÀ» ÀĞÀ» ¼ö´Â ÀÖÁö¸¸, ¼öÁ¤Àº ÀÌ ½ºÅ©¸³Æ® ³»ºÎ¿¡¼­¸¸ °¡´ÉÇÏµµ·Ï ¼³Á¤
    public int CurrentScore { get; private set; }
    public int BestScore { get; private set; }

    [Header("ÀÌº¥Æ® (UI ¿¬°á¿ë)")]
    // Á¡¼ö°¡ º¯°æµÉ ¶§¸¶´Ù È£ÃâµÉ ÀÌº¥Æ® (Inspector¿¡¼­ UI ÇÔ¼ö ¿¬°á °¡´É)
    public UnityEvent<int> onScoreChanged;

    private const string BestScoreKey = "BestScore"; // ÀúÀå Å°°ª

    void Awake()
    {
        // 2. ½Ì±ÛÅæ ÆĞÅÏ ±¸Çö (Áßº¹ ¹æÁö)
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject); // ¾ÀÀÌ ¹Ù²î¾îµµ Á¡¼ö ¸Å´ÏÀú´Â »ç¶óÁöÁö ¾ÊÀ½
        }
        else
        {
            // ÀÌ¹Ì ¸Å´ÏÀú°¡ Á¸ÀçÇÑ´Ù¸é »õ·Î »ı±ä °Ç ÆÄ±«
            Destroy(gameObject);
        }
    }

    void Start()
    {
        UpdateScoreUI(); // ÃÊ±â UI °»½Å
    }

    // 3. Á¡¼ö Ãß°¡ ¸Ş¼­µå (¿ÜºÎ¿¡¼­ È£Ãâ)
    public void AddScore(int amount)
    {
        CurrentScore += amount;
        Debug.Log("Up : " + CurrentScore);

        // ÃÖ°í Á¡¼ö °»½Å Ã¼Å©
        if (CurrentScore > BestScore)
        {
            BestScore = CurrentScore;
        }

        // Á¡¼ö º¯°æ ¾Ë¸² (UI ¾÷µ¥ÀÌÆ® µî)
        UpdateScoreUI();

        // TimerManagerì— ì„ ë¬¼ ë°°ë‹¬ ì„±ê³µ ì•Œë¦¼
        if (TimerManager.Instance != null)
        {
            TimerManager.Instance.CheckWinCondition();
        }
    }

    // Á¡¼ö ÃÊ±âÈ­ (°ÔÀÓ Àç½ÃÀÛ ½Ã µî)
    public void ResetScore()
    {
        CurrentScore = 0;
        UpdateScoreUI();
    }

    // UI ¾÷µ¥ÀÌÆ® ÀÌº¥Æ® ½ÇÇà
    private void UpdateScoreUI()
    {
        // ÀÌº¥Æ®¿¡ µî·ÏµÈ ¸®½º³Ê°¡ ÀÖ´Ù¸é ÇöÀç Á¡¼ö¸¦ º¸³¿
        onScoreChanged?.Invoke(CurrentScore);

        // ·Î±× È®ÀÎ¿ë
        // Debug.Log($"ÇöÀç Á¡¼ö: {CurrentScore} / ÃÖ°í Á¡¼ö: {BestScore}");
    }

}
