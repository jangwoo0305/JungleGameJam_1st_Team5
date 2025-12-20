using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class FieldOfView : MonoBehaviour
{
    [Range(0, 360)] public float fov = 70f;       // 시야각
    public int rayCount = 300;                     // 시야 시각화 레이 수
    public float viewDistance = 3f;               // 시야 거리
    public float fovVisualizationDistance = 1f; // DrawFOV 시각화 거리

    public LayerMask targetLayerMask;             // Player가 속한 레이어
    public LayerMask wallLayerMask;               // 벽 레이어
    public string targetTag = "Player";           // 감지할 객체 태그

    private Mesh mesh;
    private Vector2 lookDir = Vector2.up;         // 바라보는 방향
    private bool gameEnded = false;

    Vector2 GetFOVOrigin()
    {
        return (Vector2)transform.position;
    }

    void Start()
    {
        mesh = new Mesh();
        mesh.name = "FOV_Mesh";
        GetComponent<MeshFilter>().mesh = mesh;

        GetComponent<MeshRenderer>().sortingOrder = 5;
    }

    void LateUpdate()
    {
        if (!gameEnded)
        { 
            DrawFOV();
            CheckForTargets();
        }
    }

    // 외부에서 방향 세팅
    public void SetDirection(Vector2 dir)
    {
        if (dir.sqrMagnitude < 0.01f) return;
        lookDir = dir.normalized;
    }

    // 🔹 시야 시각화
    void DrawFOV()
    {
        float halfFOV = fov * 0.5f;
        float angleStep = fov / rayCount;

        Vector3[] vertices = new Vector3[rayCount + 2];
        int[] triangles = new int[rayCount * 3];
        vertices[0] = Vector3.zero;

        int vertexIndex = 1;
        int triangleIndex = 0;
        float startAngle = GetAngleFromVector(lookDir) + halfFOV;

        Vector2 origin = GetFOVOrigin() + lookDir * 0.05f;

        for (int i = 0; i <= rayCount; i++)
        {
            float angle = startAngle - angleStep * i;
            Vector3 dir = GetVectorFromAngle(angle);

            RaycastHit2D wallHit = Physics2D.Raycast(origin, dir, fovVisualizationDistance, wallLayerMask);
            float distance = wallHit.collider != null ? Mathf.Min(wallHit.distance, fovVisualizationDistance) : fovVisualizationDistance;
            distance = Mathf.Max(0, distance - 0.02f);
            vertices[vertexIndex] = dir * distance;

            if (i > 0)
            {
                triangles[triangleIndex++] = 0;
                triangles[triangleIndex++] = vertexIndex - 1;
                triangles[triangleIndex++] = vertexIndex;
            }
            vertexIndex++;
        }

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateBounds();
    }

    // 🔹 FOV 내 Player 감지 (DrawFOV mesh와 동일한 rayCount 방식)
    void CheckForTargets()
    {
        Vector2 origin = GetFOVOrigin();
        float halfFOV = fov * 0.5f;
        float angleStep = fov / rayCount;
        float startAngle = GetAngleFromVector(lookDir) + halfFOV;

        for (int i = 0; i <= rayCount; i++)
        {
            float angle = startAngle - angleStep * i;
            Vector2 dir = GetVectorFromAngle(angle);

            // 1️⃣ 먼저 wall 체크
            RaycastHit2D wallHit =
                Physics2D.Raycast(origin, dir, viewDistance, wallLayerMask);

            float maxDistance = wallHit.collider != null
                ? wallHit.distance
                : viewDistance;

            // 2️⃣ wall 앞까지만 Player 체크
            RaycastHit2D playerHit =
                Physics2D.Raycast(origin, dir, maxDistance, targetLayerMask);

            if (playerHit.collider != null &&
                playerHit.collider.CompareTag(targetTag))
            {
                EndGame();
                return;
            }
        }
    }

    Vector3 GetVectorFromAngle(float angle)
    {
        float rad = angle * Mathf.Deg2Rad;
        return new Vector3(Mathf.Cos(rad), Mathf.Sin(rad));
    }

    float GetAngleFromVector(Vector2 dir)
    {
        dir.Normalize();
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (angle < 0) angle += 360;
        return angle;
    }

    void EndGame()
    {
        if (gameEnded) return;
        gameEnded = true;

        Debug.Log("게임 종료: Player가 FOV 내에 감지됨!");
        

#if UNITY_EDITOR
        
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        Vector2 origin = Application.isPlaying ? GetFOVOrigin() : (Vector2)transform.position;

        // 실제 감지 범위 표시
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(origin, viewDistance);
    }
#endif
}