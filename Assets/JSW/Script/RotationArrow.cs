using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationArrow : MonoBehaviour
{
    [Header("Targets")]
    public Transform playerTrans; // 플레이어 (기준점)
    public Transform homeTrans;   // 집 (목표지점)

    [Header("Arrow")]
    public Transform arrowTrans;  // 회전시킬 화살표 본체

    void Update()
    {
        // 1. 방어 코드: 대상이 없으면 실행하지 않음
        if (playerTrans == null || homeTrans == null || arrowTrans == null)
        {
            return;
        }

        // 2. 방향 벡터 계산 (목표지점 - 기준점)
        // 집의 위치에서 플레이어의 위치를 빼면, 플레이어 -> 집으로 향하는 벡터가 나옵니다.
        Vector3 direction = homeTrans.position - playerTrans.position;

        // [중요] 2D 게임이라면 Z축 깊이 차이로 인해 화살표가 이상하게 기울어질 수 있습니다.
        // Z축을 0으로 만들어 평면상에서의 방향만 계산하도록 합니다.
        direction.z = 0;

        // 3. 회전 적용
        // 화살표 이미지가 기본적으로 '위쪽(Vector3.up)'을 가리키고 있다고 가정합니다.
        // 현재 화살표의 위쪽 방향을 계산된 direction 쪽으로 회전시킵니다.
        arrowTrans.rotation = Quaternion.FromToRotation(Vector3.up, direction);

        // (선택 사항) 화살표를 항상 플레이어 위치로 따라다니게 하려면 아래 주석 해제
        // arrowTrans.position = playerTrans.position + new Vector3(0, 1.5f, 0); // 머리 위
    }
}
