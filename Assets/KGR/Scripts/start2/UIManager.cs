using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class UIManager : MonoBehaviour
{
    [Header("UI Elements")]
    public TMP_Text buttonText;
    public CanvasGroup hoverImageGroup;
    public CanvasGroup startImageGroup;
    public CanvasGroup fadeImageGroup;

    public float transitionSpeed = 0.5f;
    public Color normalColor = Color.white;
    public Color hoverColor = Color.black;

    private Coroutine hoverCoroutine;

    void Awake()
    {
        // 씬 내에 UIManager가 하나만 있는지 확인
        var managers = FindObjectsByType<UIManager>(FindObjectsSortMode.None);
        if (managers.Length > 1)
        {
            Debug.LogError($"[경고] 현재 씬에 UIManager가 {managers.Length}개 있습니다! 중복된 매니저를 삭제하세요.");
        }
    }

    void Start()
    {
        // 초기화 시 강제로 Alpha를 0으로 설정
        ResetUI(hoverImageGroup);
        ResetUI(startImageGroup);
        ResetUI(fadeImageGroup);
        if (buttonText != null) buttonText.color = normalColor;
    }

    private void ResetUI(CanvasGroup cg)
    {
        if (cg != null) { cg.alpha = 0; cg.blocksRaycasts = false; }
    }

    public void HandleHover(bool isHovering)
    {
        if (hoverCoroutine != null) StopCoroutine(hoverCoroutine);
        hoverCoroutine = StartCoroutine(FadeHover(isHovering ? 1f : 0f, isHovering ? hoverColor : normalColor));
    }

    IEnumerator FadeHover(float targetAlpha, Color targetColor)
    {
        float time = 0;
        float startAlpha = hoverImageGroup.alpha;
        Color startColor = buttonText.color;

        while (time < transitionSpeed)
        {
            time += Time.deltaTime;
            float t = time / transitionSpeed;
            
            // 이 로그가 인스펙터 숫자 대신 콘솔에 찍히는지 보세요
            hoverImageGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, t);
            buttonText.color = Color.Lerp(startColor, targetColor, t);
            yield return null;
        }

        hoverImageGroup.alpha = targetAlpha;
        buttonText.color = targetColor;
    }

    public void StartGameSequence()
    {
        StopAllCoroutines();
        StartCoroutine(ClickSequence());
    }

    IEnumerator ClickSequence()
    {
        float time = 0;
        while (time < transitionSpeed)
        {
            time += Time.deltaTime;
            startImageGroup.alpha = Mathf.Lerp(0, 1, time / transitionSpeed);
            yield return null;
        }
        startImageGroup.alpha = 1;

        yield return new WaitForSeconds(0.3f);

        time = 0;
        fadeImageGroup.blocksRaycasts = true;
        while (time < transitionSpeed)
        {
            time += Time.deltaTime;
            fadeImageGroup.alpha = Mathf.Lerp(0, 1, time / transitionSpeed);
            yield return null;
        }
        // SceneManager.LoadScene("Scene2");
    }
}