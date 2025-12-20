using UnityEngine;

public class PlayerState : MonoBehaviour
{
    // 현재 상태를 담는 변수 (Enum 사용)
    public PlayerStates currentState;

}

public enum PlayerStates
{
    Idle,   // 대기
    Move,   // 이동
    Snowman,    // 얼음
    Dead    // 사망
}