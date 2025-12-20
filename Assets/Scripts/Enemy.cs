using System;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;


public class Enemy : MonoBehaviour
{
    
    NavMeshAgent _agent;
    [SerializeField] float moveRange = 5f; // 이동할 수 있는 랜덤 범위
    
    Vector2 _lookDir = Vector2.up;
    public FieldOfView fov;
    Animator anim;
    SpriteRenderer spriter;
    
    void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.updateRotation = false;
        _agent.updateUpAxis = false;
        anim =  GetComponent<Animator>();
        spriter = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        // 목적지에 도착했는지 확인
        if (!_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance)
        {
            _agent.SetDestination(MakeDestination());
        }
        
        UpdateLookDirection();
    }

    private void LateUpdate()
    {
        if (_agent.velocity.x != 0)
        {
            spriter.flipX = _agent.velocity.x < 0;
        }
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
}
