using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private UIManager uiManager;

    void Start()
    {
        // 수동 등록 없이 씬 내의 UIManager를 자동으로 찾음
        uiManager = FindFirstObjectByType<UIManager>();
        
        if (uiManager == null)
            Debug.LogError("씬에 UIManager가 없습니다! Canvas에 스크립트를 붙였는지 확인하세요.");
        else
            Debug.Log($"[연결 완료] {gameObject.name}이(가) {uiManager.gameObject.name}를 찾았습니다.");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (uiManager != null) uiManager.HandleHover(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (uiManager != null) uiManager.HandleHover(false);
    }

    public void OnBtnClick()
    {
        if (uiManager != null) uiManager.StartGameSequence();
    }
}