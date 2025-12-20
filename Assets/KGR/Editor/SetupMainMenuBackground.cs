using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

namespace KGR.Editor
{
    public class SetupMainMenuBackground
    {
        [MenuItem("KGR/Setup Main Menu Background")]
        public static void SetupBackground()
        {
            // Background GameObject 찾기
            GameObject background = GameObject.Find("Background");
            if (background == null)
            {
                Debug.LogError("Background GameObject를 찾을 수 없습니다!");
                return;
            }

            // Image 컴포넌트 추가 (없으면)
            Image image = background.GetComponent<Image>();
            if (image == null)
            {
                image = background.AddComponent<Image>();
            }

            // 배경 이미지 로드
            string imagePath = "Assets/KGR/assets/png/game-main-background.png";
            Sprite backgroundSprite = AssetDatabase.LoadAssetAtPath<Sprite>(imagePath);
            
            if (backgroundSprite == null)
            {
                // Sprite가 아닌 Texture2D로 로드 시도
                Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D>(imagePath);
                if (texture != null)
                {
                    // Texture2D를 Sprite로 변환
                    backgroundSprite = Sprite.Create(
                        texture,
                        new Rect(0, 0, texture.width, texture.height),
                        new Vector2(0.5f, 0.5f)
                    );
                }
            }

            if (backgroundSprite != null)
            {
                image.sprite = backgroundSprite;
                image.preserveAspect = true;
                image.type = Image.Type.Simple;
                
                // RectTransform 설정 (전체 화면)
                RectTransform rectTransform = background.GetComponent<RectTransform>();
                if (rectTransform != null)
                {
                    rectTransform.anchorMin = Vector2.zero;
                    rectTransform.anchorMax = Vector2.one;
                    rectTransform.sizeDelta = Vector2.zero;
                    rectTransform.anchoredPosition = Vector2.zero;
                }
                
                Debug.Log($"배경 이미지가 적용되었습니다: {imagePath}");
            }
            else
            {
                Debug.LogError($"이미지를 찾을 수 없습니다: {imagePath}");
            }
        }
    }
}
