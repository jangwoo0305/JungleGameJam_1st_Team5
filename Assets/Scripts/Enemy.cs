using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    NavMeshAgent _agent;
    [SerializeField] float moveRange = 5f; // 이동할 수 있는 랜덤 범위
    
    Vector2 _lookDir = Vector2.up;
    public FieldOfView fov;
    
    // 스프라이트 방향 이미지
    [SerializeField] private Sprite enemyUp;
    [SerializeField] private Sprite enemyRight;
    [SerializeField] private Sprite enemyDown;
    
    // 스프라이트 렌더러 (Body 자식 오브젝트)
    private SpriteRenderer spriteRenderer;
    private Transform bodyTransform;
    private Vector2 lastMoveDirection = Vector2.up; // 마지막 이동 방향 저장
    
    // Animator (Body 자식 오브젝트)
    private Animator animator;
    
    // 기절 시스템
    private bool isStunned = false;
    private float stunTimer = 0f;
    
    void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.updateRotation = false;
        _agent.updateUpAxis = false;
        
        // Body 자식 오브젝트 찾기
        bodyTransform = transform.Find("Body");
        if (bodyTransform == null)
        {
            Debug.LogWarning("Enemy: 'Body' 자식 오브젝트를 찾을 수 없습니다.");
            return;
        }
        
        // Body 오브젝트에서 SpriteRenderer 컴포넌트 가져오기
        spriteRenderer = bodyTransform.GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogWarning("Enemy: Body 오브젝트에 SpriteRenderer 컴포넌트를 찾을 수 없습니다.");
        }
        
        // Body 오브젝트에서 Animator 컴포넌트 가져오기
        animator = bodyTransform.GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogWarning("Enemy: Body 오브젝트에 Animator 컴포넌트를 찾을 수 없습니다. Animator를 사용하려면 추가해주세요.");
        }
    }

    private void Update()
    {
        // 기절 상태 처리
        if (isStunned)
        {
            stunTimer -= Time.deltaTime;
            if (stunTimer <= 0f)
            {
                // 기절 해제
                isStunned = false;
                _agent.enabled = true;
            }
            return; // 기절 중에는 이동하지 않음
        }
        
        // 목적지에 도착했는지 확인
        if (!_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance)
        {
            _agent.SetDestination(MakeDestination());
        }
        
        UpdateLookDirection();
    }

    Vector3 MakeDestination()
    {
        // 현재 위치 기준으로 랜덤 좌표 생성
        float randomX = Random.Range(-moveRange, moveRange);
        float randomY = Random.Range(-moveRange, moveRange);
        Vector3 randomPos = new Vector3(transform.position.x + randomX, transform.position.y + randomY, 0f);
        
        // 가장 가까운 유효한 좌표를 찾기
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPos, out hit, moveRange, NavMesh.AllAreas))
        {
            return hit.position;
        }
        
        // 새로운 목적지 계산
        return transform.position;
    }
    
    void UpdateLookDirection()
    {
        Vector3 velocity = _agent.velocity;
        Vector2 velocity2D = new Vector2(velocity.x, velocity.y);

        // 이동 방향이 있으면 마지막 이동 방향 업데이트 및 스프라이트 교체
        if (velocity2D.sqrMagnitude > 0.01f)
        {
            _lookDir = velocity2D.normalized;
            lastMoveDirection = _lookDir;
            UpdateSpriteDirection(_lookDir);
            UpdateAnimation(_lookDir);
        }
        else
        {
            // 이동하지 않을 때는 마지막 방향 유지
            UpdateSpriteDirection(lastMoveDirection);
            UpdateAnimation(Vector2.zero);
        }
        
        if (fov != null)
            fov.SetDirection(_lookDir);
    }
    
    /// <summary>
    /// 이동 방향에 따라 스프라이트를 교체합니다.
    /// </summary>
    void UpdateSpriteDirection(Vector2 direction)
    {
        if (spriteRenderer == null) return;
        
        // Y축 이동이 더 크면 Up 또는 Down
        if (Mathf.Abs(direction.y) > Mathf.Abs(direction.x))
        {
            if (direction.y > 0)
            {
                // 위쪽
                if (enemyUp != null)
                {
                    spriteRenderer.sprite = enemyUp;
                    spriteRenderer.flipX = false;
                }
            }
            else
            {
                // 아래쪽
                if (enemyDown != null)
                {
                    spriteRenderer.sprite = enemyDown;
                    spriteRenderer.flipX = false;
                }
            }
        }
        else
        {
            // X축 이동이 더 크면 Right 또는 Left
            if (enemyRight != null)
            {
                spriteRenderer.sprite = enemyRight;
                // 왼쪽이면 flip, 오른쪽이면 정상
                spriteRenderer.flipX = direction.x < 0;
            }
        }
    }
    
    /// <summary>
    /// Animator를 사용하여 애니메이션을 업데이트합니다.
    /// </summary>
    void UpdateAnimation(Vector2 direction)
    {
        if (animator == null) return;
        
        // 기절 중이면 애니메이션 정지
        if (isStunned)
        {
            animator.SetFloat("InputX", 0);
            animator.SetFloat("InputY", 0);
            return;
        }
        
        // 이동 방향을 Animator 파라미터로 전달
        animator.SetFloat("InputX", direction.x);
        animator.SetFloat("InputY", direction.y);
    }
    
    /// <summary>
    /// 적을 기절시킴
    /// </summary>
    public void Stun(float duration)
    {
        if (isStunned) return; // 이미 기절 중이면 무시
        
        isStunned = true;
        stunTimer = duration;
        
        // NavMeshAgent 비활성화하여 이동 중지
        _agent.enabled = false;
        
        Debug.Log($"Enemy 기절! {duration}초 동안 기절합니다.");
    }
    
    /// <summary>
    /// 기절 상태 확인
    /// </summary>
    public bool IsStunned()
    {
        return isStunned;
    }
}
