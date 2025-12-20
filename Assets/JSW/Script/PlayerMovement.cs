using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("설정")]
    public float moveSpeed = 10f;

    private Rigidbody2D rb;
    private PlayerState playerState;
    private Animator anim; // 1. 애니메이터 변수 추가

    private Vector2 moveInputMoving;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerState = GetComponent<PlayerState>();
        anim = GetComponentInChildren<Animator>(); // 2. 컴포넌트 가져오기
    }

    void Start()
    {
        playerState.currentState = PlayerStates.Move;
    }

    public void Move(Vector2 moveInput)
    {
        // 입력받은 벡터를 정규화하여 저장
        moveInputMoving = moveInput.normalized;
    }

    void Update() // 3. 애니메이션 처리는 Update에서 하는 것이 부드럽습니다.
    {
        UpdateAnimation();
    }

    void FixedUpdate()
    {
        if (playerState.currentState == PlayerStates.Snowman)
        {
            moveInputMoving = Vector2.zero;
        }

        // 물리 이동 처리
        rb.MovePosition(rb.position + moveInputMoving * moveSpeed * Time.fixedDeltaTime);
    }

    bool isRight = false;
    // 4. 애니메이션 상태 업데이트 함수 추가
    void UpdateAnimation()
    {
        if (anim == null) return;

        // 눈사람 상태일 때는 움직임 값을 0으로 강제하여 Idle로 보냄 (선택사항)
        if (playerState.currentState == PlayerStates.Snowman)
        {
            anim.SetFloat("InputX", 0);
            anim.SetFloat("InputY", 0);
            return;
        }


        // 현재 이동 입력값을 애니메이터 파라미터로 전달
        // 움직이지 않을 때(0,0)는 자동으로 블렌드 트리의 가운데(Idle)가 실행됨
        anim.SetFloat("InputX", moveInputMoving.x);
        anim.SetFloat("InputY", moveInputMoving.y);
    }
}