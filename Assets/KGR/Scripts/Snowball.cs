using UnityEngine;
using System.Collections;

public class Snowball : MonoBehaviour
{
    [Header("발사 설정")]
    private Vector3 startPos;
    private Vector2 direction;
    private float speed;
    private float range;
    private float area;
    
    private float traveledDistance = 0f;
    private bool hasHit = false;
    
    [Header("포물선 설정")]
    [SerializeField] private float arcHeight = 1.5f; // 포물선 높이
    
    /// <summary>
    /// 방향으로 눈덩이 발사 (포물선 이동)
    /// </summary>
    public void FlyInDirection(Vector3 startPos, Vector2 direction, float speed, float range, float area)
    {
        this.startPos = startPos;
        this.direction = direction.normalized;
        this.speed = speed;
        this.range = range;
        this.area = area;
        
        transform.position = startPos;
        
        // Collider2D 설정
        CircleCollider2D collider = GetComponent<CircleCollider2D>();
        if (collider == null)
        {
            collider = gameObject.AddComponent<CircleCollider2D>();
        }
        collider.radius = area;
        collider.isTrigger = true;
        
        // 포물선 이동 시작
        StartCoroutine(FlyCoroutine());
    }
    
    IEnumerator FlyCoroutine()
    {
        Vector3 endPos = startPos + (Vector3)(direction * range);
        
        // 포물선의 중간점 (최고점)
        Vector3 midPoint = (startPos + endPos) / 2f + Vector3.up * arcHeight;
        
        float distance = 0f;
        float totalDistance = range;
        float duration = totalDistance / speed;
        float elapsedTime = 0f;
        
        while (distance < totalDistance && !hasHit)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            
            if (t > 1f) t = 1f;
            
            // 베지어 곡선을 사용한 포물선 이동
            Vector3 position = CalculateBezierPoint(startPos, midPoint, endPos, t);
            transform.position = position;
            
            // 이동 거리 계산
            distance = Vector3.Distance(startPos, position);
            traveledDistance = distance;
            
            yield return null;
        }
        
        // 범위를 벗어나거나 도착했으면 제거
        if (!hasHit)
        {
            Destroy(gameObject);
        }
    }
    
    /// <summary>
    /// 베지어 곡선의 점 계산 (포물선)
    /// </summary>
    private Vector3 CalculateBezierPoint(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
        float oneMinusT = 1f - t;
        return (oneMinusT * oneMinusT * p0) + (2f * oneMinusT * t * p1) + (t * t * p2);
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        // 이미 맞았으면 무시
        if (hasHit)
        {
            return;
        }
        
        // Enemy 태그 확인
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            
            if (enemy != null)
            {
                // 적을 기절시킴
                enemy.Stun(3f);
                hasHit = true;
                
                // 눈덩이 제거
                Destroy(gameObject);
            }
        }
    }
}

