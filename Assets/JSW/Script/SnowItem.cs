using UnityEngine;

public class SnowItem : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<SnowballShooter>().AddSnowball();
            Destroy(gameObject);
        }
    }
}
