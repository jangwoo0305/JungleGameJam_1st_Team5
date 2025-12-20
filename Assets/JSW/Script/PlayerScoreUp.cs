using System.Linq.Expressions;
using UnityEngine;

public class PlayerScoreUp : MonoBehaviour
{
    // ���� ��ȣ�ۿ� ������(�浹 ����) ������Ʈ�� ������ ����
    public GameObject nearObject;
    [SerializeField]
    private GameObject ThrowBox;
    [SerializeField]
    private GameObject funKeyUI;

    private SnowballShooter snowballShooter;

    private void Start()
    {
        snowballShooter = GetComponent<SnowballShooter>();
    }

    void Update()
    {

        if (nearObject == null)
        {
            if (Input.GetKeyDown(KeyCode.F)) snowballShooter.TryShootSnowball();

            return;
        }

        House houseScript = nearObject.GetComponent<House>();

        // 1. FŰ�� ������ + ��ȣ�ۿ��� ��ü�� �����Ѵٸ�
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (HouseManager.Instance.nowTargetHouse == houseScript)
            {
                if (houseScript != null)
                {
                    GameObject throwBoxing = Instantiate(ThrowBox);
                    throwBoxing.GetComponent<DeliveryBox>().FlyToTarget(transform.position, houseScript.transform.position, 0.6f);
                    houseScript.ScoreUp();
                    funKeyUI.SetActive(false);
                }
            }
            else
            {
                snowballShooter.TryShootSnowball();
            }
        }
    }

    // ������ ������ ��: ����� ������ ���� (Lock On)
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("House")) // �±� Ȯ��
        {
            nearObject = other.gameObject;

            if (HouseManager.Instance.nowTargetHouse == nearObject.GetComponent<House>())
            {
                funKeyUI.SetActive(true);
            }
        }
    }

    // �������� ������ ��: ��� ���� �ʱ�ȭ (Lock Off)
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("House"))
        {
            // ���� ���� ���� �ִ� �� �������� �´��� Ȯ�� �� ����
            if (nearObject == other.gameObject)
            {
                funKeyUI.SetActive(false);
                nearObject = null;
            }
        }
    }
    
    /// <summary>
    /// 선물을 던질 수 있는지 확인 (SnowballShooter에서 우선순위 확인용)
    /// </summary>
    public bool CanThrowGift()
    {
        if (nearObject == null) return false;
        
        House houseScript = nearObject.GetComponent<House>();
        if (houseScript == null) return false;


        Debug.Log(HouseManager.Instance.nowTargetHouse.gameObject);
        Debug.Log(nearObject);
        return HouseManager.Instance != null && HouseManager.Instance.nowTargetHouse.gameObject == nearObject;
    }

}
