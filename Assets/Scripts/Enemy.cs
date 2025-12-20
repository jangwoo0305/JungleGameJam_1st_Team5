using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    NavMeshAgent _agent;
    [SerializeField] float moveRange = 5f; // 이동할 수 있는 랜덤 범위

    void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.updateRotation = false;
        _agent.updateUpAxis = false;
    }

    private void Update()
    {
        // 목적지에 도착했는지 확인
        if (!_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance)
        {
            _agent.SetDestination(MakeDestination());
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
        return transform.position;;
    }
}
