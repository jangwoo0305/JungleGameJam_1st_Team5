using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class FieldOfView : MonoBehaviour
{
    [Range(0, 360)] public float fov = 90f;     // 시야각
    public int rayCount = 90;                   // 레이 수
    public float viewDistance = 5f;             // 시야 거리
    public LayerMask layerMask;                 // 벽 레이어

    private Mesh mesh;
    private Vector2 lookDir = Vector2.up;       // 외부에서 받는 방향

    void Start()
    {
        mesh = new Mesh();
        mesh.name = "FOV_Mesh";
        GetComponent<MeshFilter>().mesh = mesh;
    }

    void LateUpdate()
    {
        DrawFOV();
    }

    // 🔥 EnemyRange(SJW)에서 호출
    public void SetDirection(Vector2 dir)
    {
        if (dir.sqrMagnitude < 0.01f)
            return;

        lookDir = dir.normalized;
    }

    void DrawFOV()
    {
        float halfFOV = fov * 0.5f;
        float angleIncrease = fov / rayCount;

        Vector3[] vertices = new Vector3[rayCount + 2];
        int[] triangles = new int[rayCount * 3];

        vertices[0] = Vector3.zero;

        int vertexIndex = 1;
        int triangleIndex = 0;

        float startAngle = GetAngleFromVector(lookDir) + halfFOV;

        for (int i = 0; i <= rayCount; i++)
        {
            float angle = startAngle - angleIncrease * i;
            Vector3 dir = GetVectorFromAngle(angle);

            RaycastHit2D hit = Physics2D.Raycast(
                transform.position,
                dir,
                viewDistance,
                layerMask
            );

            if (hit.collider == null)
            {
                vertices[vertexIndex] = dir * viewDistance;
            }
            else
            {
                vertices[vertexIndex] = dir * hit.distance;
            }

            if (i > 0)
            {
                triangles[triangleIndex + 0] = 0;
                triangles[triangleIndex + 1] = vertexIndex - 1;
                triangles[triangleIndex + 2] = vertexIndex;
                triangleIndex += 3;
            }

            vertexIndex++;
        }

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateBounds();
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
}