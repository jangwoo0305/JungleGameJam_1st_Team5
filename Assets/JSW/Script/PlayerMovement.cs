using NUnit.Framework.Constraints;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("����")]
    public float moveSpeed = 10f;

    private Rigidbody2D rb;
    private PlayerState playerState;
    private Animator anim; // 1. �ִϸ����� ���� �߰�

    private Vector2 moveInputMoving;
    private Vector2 lastMoveDirection = Vector2.up; // 마지막 이동 방향 저장

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerState = GetComponent<PlayerState>();
        anim = GetComponentInChildren<Animator>(); // 2. ������Ʈ ��������
    }

    void Start()
    {
        playerState.currentState = PlayerStates.Move;
    }

    public void Move(Vector2 moveInput)
    {
        // �Է¹��� ���͸� ����ȭ�Ͽ� ����
        moveInputMoving = moveInput.normalized;
        
        // 이동 방향이 있으면 마지막 이동 방향 업데이트
        if (moveInputMoving.sqrMagnitude > 0.01f)
        {
            lastMoveDirection = moveInputMoving;
        }
    }
    
    /// <summary>
    /// 현재 이동 방향 반환 (눈덩이 발사 등에 사용)
    /// </summary>
    public Vector2 GetMoveDirection()
    {
        // 이동 방향이 없으면 마지막 이동 방향 반환
        if (moveInputMoving.sqrMagnitude < 0.01f)
        {
            return lastMoveDirection;
        }
        return moveInputMoving;
    }

    void Update() // 3. �ִϸ��̼� ó���� Update���� �ϴ� ���� �ε巴���ϴ�.
    {
        UpdateAnimation();
    }

    void FixedUpdate()
    {
        if (playerState.currentState == PlayerStates.Snowman)
        {
            moveInputMoving = Vector2.zero;
        }

        if (moveInputMoving.x > 0) anim.transform.localScale = new Vector3(10, 10, 1);
        else if (moveInputMoving.x < 0) anim.transform.localScale = new Vector3(-10, 10, 1);

        // ���� �̵� ó��
        rb.MovePosition(rb.position + moveInputMoving * moveSpeed * Time.fixedDeltaTime);
    }

    bool isRight = false;
    // 4. �ִϸ��̼� ���� ������Ʈ �Լ� �߰�
    void UpdateAnimation()
    {
        if (anim == null) return;

        // ����� ������ ���� ������ ���� 0���� �����Ͽ� Idle�� ���� (���û���)
        if (playerState.currentState == PlayerStates.Snowman)
        {
            anim.SetFloat("InputX", 0);
            anim.SetFloat("InputY", 0);
            return;
        }


        // ���� �̵� �Է°��� �ִϸ����� �Ķ���ͷ� ����
        // �������� ���� ��(0,0)�� �ڵ����� ������ Ʈ���� ���(Idle)�� �����
        anim.SetFloat("InputX", moveInputMoving.x);
        anim.SetFloat("InputY", moveInputMoving.y);
    }
}