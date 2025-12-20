using UnityEngine;
using System.Collections;

public class DeliveryBox : MonoBehaviour
{
    [Header("설정")]
    public float rotateSpeed = 720f; // 1초에 720도 회전 (2바퀴)

    public void FlyToTarget(Vector3 startPos, Vector3 endPos, float duration)
    {
        StartCoroutine(CurveCoroutine(startPos, endPos, duration));
    }

    IEnumerator CurveCoroutine(Vector3 p0, Vector3 p2, float duration)
    {
        float time = 0f;

        // 제어점(P1) 계산 (중간 지점에서 위로 띄움)
        float height = 2.0f;
        Vector3 p1 = (p0 + p2) / 2 + (Vector3.up * height);

        while (time < 1f)
        {
            time += Time.deltaTime / duration;

            // --- 베지에 곡선 이동 ---
            float t = time;
            float oneMinusT = 1f - t;

            Vector3 position =
                (oneMinusT * oneMinusT * p0) +
                (2f * oneMinusT * t * p1) +
                (t * t * p2);

            transform.position = position;

            // --- [추가됨] 회전 로직 ---
            // Z축을 기준으로 뺑글뺑글 돌립니다. (2D 게임 기준)
            // 3D라면 Vector3.up이나 Vector3.right 등을 섞어서 쓰면 입체적으로 돕니다.
            transform.Rotate(0, 0, rotateSpeed * Time.deltaTime);

            yield return null;
        }

        // 도착 보장
        transform.position = p2;

        // --- [추가됨] 도착 후 삭제 ---
        Destroy(gameObject);
    }
}
