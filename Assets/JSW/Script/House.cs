using UnityEngine;

public class House : MonoBehaviour
{
    public int limitTimer=15;
    private float currentTimer = 0;
    private bool canDelivery;

    void Start()
    {
        canDelivery = true;
    }

    void Update()
    {
        if (!canDelivery) currentTimer += Time.deltaTime;

        if(limitTimer < currentTimer)
        {
            canDelivery = true;
            currentTimer = 0;
        }
    }

    public void ScoreUp()
    {
        if (canDelivery)
        {
            Debug.Log("배달 성공!");
            canDelivery = false;
            ScoreManager.Instance.AddScore(1);
        }
        else
        {
            Debug.Log("배달 대기! 남은 시간: " + currentTimer);
        }
    }
    
}
