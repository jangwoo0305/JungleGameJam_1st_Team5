using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.AI;
using UnityEngine.Tilemaps;

public enum SpawnType
{
    Enemy,
    Snow
}

public class Spawner : MonoBehaviour
{
    [Header("Enemy Settings")]
    public GameObject enemyPrefab;
    [SerializeField] int spawnCountOnStart = 2;
    [SerializeField] int spawnSec = 30;
    [SerializeField] int maxSpawnCount = 20;
    int _curSpawnCount = 0;
    
    [Header("Snow Settings")]
    public GameObject snowPrefab;
    [SerializeField] int spawnSnowOnStart = 2;
    [SerializeField] int spawnSnowSec = 10;
    [SerializeField] int maxSnowCount = 20;
    int _curSnowCount = 0;
    
    public Tilemap groundTilemap;
    
    void Start()
    {
        for (int i = 0; i < spawnCountOnStart || i < spawnSnowOnStart; i++)
        {
            if (i < spawnCountOnStart) SpawnEnemy();
            if (i < spawnSnowOnStart) SpawnSnow();
        }
        // 코루틴
        StartCoroutine(SpawnTimer(spawnSec, SpawnType.Enemy));
        StartCoroutine(SpawnTimer(spawnSnowSec, SpawnType.Snow));
    }

    IEnumerator SpawnTimer(int sec, SpawnType type)
    {
        while (!GameManager.Instance.isGameOver)
        {
            yield return new WaitForSeconds(sec);

            switch (type)
            {
                case SpawnType.Enemy:
                    if (_curSpawnCount < maxSpawnCount) SpawnEnemy();
                    else yield break;
                    break;
                case SpawnType.Snow:
                    if (_curSnowCount < maxSnowCount) SpawnSnow();
                    else yield break;
                    break;
            }
        }
    }
    
    void SpawnSnow()
    {
        BoundsInt bounds = groundTilemap.cellBounds;

        for (int attempt = 0; attempt < 5; attempt++)
        {
            // Tilemap 범위 안 랜덤 좌표
            int x = Random.Range(bounds.xMin, bounds.xMax);
            int y = Random.Range(bounds.yMin, bounds.yMax);
            Vector3Int cellPos = new Vector3Int(x, y, 0);

            // 월드 좌표로
            Vector3 spawnPos = groundTilemap.CellToWorld(cellPos) + groundTilemap.tileAnchor;

            NavMeshHit hit;
            if (NavMesh.SamplePosition(spawnPos, out hit, 2f, NavMesh.AllAreas))
            {
                Instantiate(snowPrefab, hit.position, Quaternion.identity, transform);
                _curSnowCount++;
                return;
            }
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
                Instantiate(enemyPrefab, hit.position, Quaternion.identity, transform);
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

