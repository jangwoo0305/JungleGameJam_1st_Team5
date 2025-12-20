using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    NavMeshAgent _agent;
    [SerializeField] float moveRange = 5f; // 이동할 수 있는 랜덤 범위
    
    Vector2 _lookDir = Vector2.up;
    public FieldOfView fov;
    
    // 기절 시스템
    private bool isStunned = false;
    private float stunTimer = 0f;
    
    void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.updateRotation = false;
        _agent.updateUpAxis = false;
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

        if (velocity.sqrMagnitude < 0.01f)
            return;

        _lookDir = velocity.normalized;
        
        if (fov != null)
            fov.SetDirection(_lookDir);
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
