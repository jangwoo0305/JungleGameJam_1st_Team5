using UnityEngine;

public class PlayerSnowman : MonoBehaviour
{
    [SerializeField]
    private GameObject body;
    [SerializeField]
    private GameObject snowMan;

    public void ChangeSnowman(bool isSnowman)
    {
        if (isSnowman)
        {
            body.SetActive(false);
            snowMan.SetActive(true);
        }
        else
        {
            body.SetActive(true);
            snowMan.SetActive(false);
        }
    } 
}
