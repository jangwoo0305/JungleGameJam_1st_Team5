using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("설정")]
    public float moveSpeed = 5f; // 이동 속도

    private Rigidbody2D rb;
    private PlayerState playerState;
    private Vector2 moveInputMoving;

    void Awake()
    {
        // 컴포넌트 가져오기
        rb = GetComponent<Rigidbody2D>();
        playerState = GetComponent<PlayerState>();
    }

    void Start()
    {
        playerState.currentState = PlayerStates.Move;
    }

    public void Move(Vector2 moveInput)
    {
        // 대각선 이동 시 속도가 빨라지는 것을 방지 (정규화)
        moveInputMoving = moveInput.normalized;
    }

    void FixedUpdate()
    {
        if (playerState.currentState == PlayerStates.Snowman)
        {
            moveInputMoving = Vector2.zero;
        }

        // 2. 물리 이동 처리 (일정한 시간 간격으로 실행)
        // 현재 위치 + (입력 방향 * 속도 * 시간)
        rb.MovePosition(rb.position + moveInputMoving * moveSpeed * Time.fixedDeltaTime);
    }
}
