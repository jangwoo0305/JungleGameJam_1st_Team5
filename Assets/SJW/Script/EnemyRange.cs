using UnityEngine;
using UnityEngine.AI;

public class EnemyRange : MonoBehaviour
{
    NavMeshAgent agent;
    FieldOfView fov;
    Vector2 lookDir = Vector2.up;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        fov = GetComponent<FieldOfView>();

        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    void Update()
    {
        UpdateLookDirection();
    }

    void UpdateLookDirection()
    {
        Vector3 velocity = agent.velocity;

        if (velocity.sqrMagnitude < 0.01f)
            return;

        lookDir = velocity.normalized;
        
        if (fov != null)
            fov.SetDirection(lookDir);
    }
}