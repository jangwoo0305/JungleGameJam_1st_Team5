using UnityEngine;

public class House : MonoBehaviour
{
    public int limitTimer=15;
    public bool canDelivery;
    private float currentTimer = 0;

    public void ScoreUp()
    {
        if (canDelivery)
        {
            Debug.Log("배달 성공!");
            canDelivery = false;
            HouseManager.Instance.ChoseNewHouse();
            ScoreManager.Instance.AddScore(1);
        }
        else
        {
            Debug.Log("배달 대기! 남은 시간: " + currentTimer);
        }
    }
    
}
