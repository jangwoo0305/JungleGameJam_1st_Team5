using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private PlayerState playerState;
    private PlayerSnowman playerSnowman;
    private Vector2 moveInput;

    void Awake()
    {
        // 컴포넌트 가져오기
        playerMovement = GetComponent<PlayerMovement>();
        playerState = GetComponent<PlayerState>();
        playerSnowman = GetComponent<PlayerSnowman>();
    }

    void Update()
    {
        // 1. 입력 감지 (매 프레임 실행)
        // GetAxisRaw는 즉각적인 반응을 위해 사용 (부드러운 이동 원하면 GetAxis 사용)
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        // 대각선 이동 시 속도가 빨라지는 것을 방지 (정규화)
        moveInput = moveInput.normalized;

        playerMovement.Move(moveInput);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            // 눈사람으로 변해
            if (playerState.currentState == PlayerStates.Move)
            {
                playerState.currentState = PlayerStates.Snowman;
                playerSnowman.ChangeSnowman(true);
            }
            else if (playerState.currentState == PlayerStates.Snowman)
            {
                playerState.currentState = PlayerStates.Move;
                playerSnowman.ChangeSnowman(false);
            }
        }
    }
}
