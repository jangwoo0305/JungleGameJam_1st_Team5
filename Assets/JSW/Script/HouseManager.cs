using UnityEngine;
using System.Collections.Generic; // List 사용을 위해 필요
using System.Linq; // 배열을 리스트로 쉽게 바꾸기 위해 필요 (.ToList())

public class HouseManager : MonoBehaviour
{
    public List<House> houseList = new List<House>();
    public House nowTargetHouse = null;
    public RotationArrow rotationArrow = null;

    public static HouseManager Instance { get; private set; }

    void Awake()
    {

        // 2. 싱글톤 패턴 구현 (중복 방지)
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬이 바뀌어도 점수 매니저는 사라지지 않음
        }
        else
        {
            // 이미 매니저가 존재한다면 새로 생긴 건 파괴
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // 1. 씬에 있는 모든 House 컴포넌트를 찾아 배열로 반환
        // (Unity 2023 이상) SortMode.None이 성능이 더 빠름
        var housesArray = FindObjectsByType<House>(FindObjectsSortMode.None);
        rotationArrow = FindAnyObjectByType<RotationArrow>(); ;

        // (Unity 2022 이하 구버전인 경우 아래 코드 사용)
        // var housesArray = FindObjectsOfType<House>();

        // 2. 배열을 리스트로 변환하여 저장
        houseList = housesArray.ToList();

        Debug.Log($"총 {houseList.Count}개의 집을 찾았습니다.");

        ChoseNewHouse();
    }

    public void ChoseNewHouse()
    {
        // 예외 처리 1: 리스트가 비어있으면 아무것도 안 함
        if (houseList.Count == 0) return;

        // 예외 처리 2: 집이 딱 1채밖에 없다면 선택권이 없으므로 그것을 선택
        if (houseList.Count == 1)
        {
            nowTargetHouse = houseList[0];
            return;
        }

        // 1. 현재 집(nowHouse)과 다른 집들만 골라내어 임시 리스트(candidates)를 만듦
        var candidates = houseList.Where(house => house != nowTargetHouse).ToList();

        // 2. 후보군 중에서 랜덤으로 하나 선택
        int randomIndex = Random.Range(0, candidates.Count);

        // 3. 할당
        nowTargetHouse = candidates[randomIndex];

        Debug.Log($"새로운 집으로 {nowTargetHouse.name}이(가) 선택되었습니다.");

        nowTargetHouse.canDelivery = true;
        rotationArrow.homeTrans = nowTargetHouse.GetComponent<Transform>();
        nowTargetHouse.SetActiveInfo();
    }

}
