using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.AI;

public class Spawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    [SerializeField] int spawnCountOnStart = 2;
    [SerializeField] int spawnSec = 30;
    [SerializeField] int maxSpawnCount = 20;
    int _curSpawnCount = 0;
    
    void Start()
    {
        for (int i = 0; i < spawnCountOnStart; i++)
        {
            SpawnEnemy();
        }
        // 코루틴
        StartCoroutine(SpawnTimer());
    }

    IEnumerator SpawnTimer()
    {
        while (!GameManager.Instance.isGameOver && _curSpawnCount < maxSpawnCount)
        {
            yield return new WaitForSeconds(spawnSec);
            
            SpawnEnemy();
        }
    }

    void SpawnEnemy()
    {
        for (int i = 0; i < 5; i++)
        {
            Vector3 spawnPos = GetPositionOutsideCamera();
            NavMeshHit hit;
        
            if (NavMesh.SamplePosition(spawnPos, out hit, 10f, NavMesh.AllAreas))
            {
                Instantiate(enemyPrefab, hit.position, Quaternion.identity);
                _curSpawnCount++;
                return;
            }
        }
    }

    Vector3 GetPositionOutsideCamera()
    {
        Camera cam = Camera.main;
        if (cam == null) return Vector3.zero;

        // 0: 왼쪽, 1: 오른쪽, 2: 아래, 3: 위
        int side = Random.Range(0, 4);
        Vector3 viewportPos = Vector3.zero;

        switch (side)
        {
            case 0:
                viewportPos = new Vector3(Random.Range(-0.2f, -0.1f), Random.Range(0f, 1f), 0);
                break;
            case 1:
                viewportPos = new Vector3(Random.Range(1.1f, 1.2f), Random.Range(0f, 1f), 0);
                break;
            case 2:
                viewportPos = new Vector3(Random.Range(0f, 1f), Random.Range(-0.2f, -0.1f), 0);
                break;
            case 3:
                viewportPos = new Vector3(Random.Range(0f, 1f), Random.Range(1.1f, 1.2f), 0);
                break;
        }

        // 월드 좌표로 변환
        Vector3 worldPos = cam.ViewportToWorldPoint(viewportPos);
        worldPos.z = 0;
        
        return worldPos;
    }
}

