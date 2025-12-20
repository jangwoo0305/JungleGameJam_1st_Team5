using UnityEngine;

public class PlayerScoreUp : MonoBehaviour
{
    // 현재 상호작용 가능한(충돌 중인) 오브젝트를 저장할 변수
    private GameObject nearObject;
    [SerializeField]
    private GameObject ThrowBox;

    void Update()
    {
        if (nearObject == null) return;

        House houseScript = nearObject.GetComponent<House>();

        // 1. F키를 눌렀고 + 상호작용할 물체가 존재한다면
        if (Input.GetKeyDown(KeyCode.F) && HouseManager.Instance.nowTargetHouse == houseScript)
        {

            if (houseScript != null)
            {
                GameObject throwBoxing = Instantiate(ThrowBox);
                throwBoxing.GetComponent<DeliveryBox>().FlyToTarget(transform.position, houseScript.transform.position, 0.6f);
                houseScript.ScoreUp();
            }
        }
    }

    // 영역에 들어왔을 때: 대상을 변수에 저장 (Lock On)
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("House")) // 태그 확인
        {
            nearObject = other.gameObject;
            Debug.Log("집 발견! F키를 눌러 획득하세요.");
        }
    }

    // 영역에서 나갔을 때: 대상 변수 초기화 (Lock Off)
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("House"))
        {
            // 내가 지금 보고 있던 그 아이템이 맞는지 확인 후 해제
            if (nearObject == other.gameObject)
            {
                nearObject = null;
            }
        }
    }


}
