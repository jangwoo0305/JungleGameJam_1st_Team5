using TMPro;
using UnityEngine;

public class SnowballShooter : MonoBehaviour
{
    [Header("눈덩이 설정")]
    [SerializeField] private GameObject snowballPrefab;
    [SerializeField] private float snowballSpeed = 10f;
    [SerializeField] private float snowballRange = 5f;
    [SerializeField] private float snowballArea = 0.5f; // Collider 크기
    
    [Header("눈덩이 개수")]
    [SerializeField] private int maxSnowballs = 10;

    [SerializeField] private TMP_Text snowText;
    private int currentSnowballs = 0;
    
    private PlayerMovement playerMovement;
    private PlayerScoreUp playerScoreUp;
    
    void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerScoreUp = GetComponent<PlayerScoreUp>();
    }
    
    void Start()
    {
        // 최초 눈덩이 개수를 최대 개수로 설정
        currentSnowballs = maxSnowballs;
        snowText.text = currentSnowballs.ToString();
        Debug.Log($"눈덩이 초기화 완료! 현재 개수: {currentSnowballs}");
    }
    
    //void Update()
    //{
    //    // F키 입력 처리
    //    if (Input.GetKeyDown(KeyCode.F))
    //    {
    //        // 선물 던지기 우선순위 확인
    //        bool canThrowGift = true;
            
    //        if (canThrowGift)
    //        {
    //            // 선물을 던질 수 있으면 눈덩이 발사하지 않음
    //            return;
    //        }
    //        // 눈덩이 발사
    //        TryShootSnowball();
    //    }
    //}
    
    /// <summary>
    /// 선물을 던질 수 있는지 확인
    /// </summary>
    private bool CanThrowGift()
    {
        if (playerScoreUp == null) return false;

        // PlayerScoreUp의 CanThrowGift 메서드 사용
        return playerScoreUp.CanThrowGift();
    }
    
    /// <summary>
    /// 눈덩이 발사 시도
    /// </summary>
    public void TryShootSnowball()
    {
        // 눈덩이 개수 확인
        if (currentSnowballs <= 0)
        {
            Debug.Log("눈덩이가 없습니다!");
            return;
        }
        
        // 이동 방향 가져오기
        Vector2 direction = GetShootDirection();
        
        // 이동 방향이 없으면 발사하지 않음
        if (direction.sqrMagnitude < 0.01f)
        {
            Debug.Log("발사 방향이 없습니다!");
            return;
        }
        
        // 눈덩이 발사
        ShootSnowball(direction);
        
        // 눈덩이 개수 감소
        currentSnowballs--;
        snowText.text = currentSnowballs.ToString();
        Debug.Log($"눈덩이 발사! 남은 개수: {currentSnowballs}");
        
        // TODO: UI 업데이트
    }
    
    /// <summary>
    /// 발사 방향 가져오기 (플레이어의 현재 이동 방향)
    /// </summary>
    private Vector2 GetShootDirection()
    {
        if (playerMovement == null) return Vector2.up;
        
        // PlayerMovement에서 이동 방향 가져오기
        return playerMovement.GetMoveDirection();
    }
    
    /// <summary>
    /// 눈덩이 발사
    /// </summary>
    private void ShootSnowball(Vector2 direction)
    {
        if (snowballPrefab == null)
        {
            Debug.LogError("눈덩이 프리팹이 설정되지 않았습니다!");
            return;
        }
        
        // 눈덩이 인스턴스 생성
        GameObject snowball = Instantiate(snowballPrefab, transform.position, Quaternion.identity);
        
        // Snowball 컴포넌트에 발사 정보 전달
        Snowball snowballScript = snowball.GetComponent<Snowball>();
        
        if (snowballScript != null)
        {
            snowballScript.FlyInDirection(transform.position, direction, snowballSpeed, snowballRange, snowballArea);
        }
        else
        {
            Debug.LogError("Snowball 컴포넌트를 찾을 수 없습니다!");
        }
    }
    
    /// <summary>
    /// 눈덩이 개수 추가 (TODO: 나중에 아이템 획득 시 호출)
    /// </summary>
    public void AddSnowball(int amount = 1)
    {
        currentSnowballs = Mathf.Min(currentSnowballs + amount, maxSnowballs);
        Debug.Log($"눈덩이 획득! 현재 개수: {currentSnowballs}");

        snowText.text = currentSnowballs.ToString();
        // TODO: UI 업데이트
    }
    
    /// <summary>
    /// 현재 눈덩이 개수 반환
    /// </summary>
    public int GetCurrentSnowballs()
    {
        return currentSnowballs;
    }
}

