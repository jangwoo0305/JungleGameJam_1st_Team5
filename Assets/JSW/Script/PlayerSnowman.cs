using UnityEngine;

public class PlayerSnowman : MonoBehaviour
{
    [SerializeField]
    private GameObject body;
    [SerializeField]
    private GameObject snowMan;
    [SerializeField]
    private Sprite snowManUp;
    [SerializeField]
    private Sprite snowManRight;
    [SerializeField]
    private Sprite snowManDown;

    [SerializeField]
    private Animator characterAnimator;


    public void ChangeSnowman(bool isSnowman)
    {
        if (isSnowman)
        {
            this.tag = "Snowman";
            body.SetActive(false);

            snowMan.SetActive(true);
            if(characterAnimator.GetFloat("InputY") == 1)
            {
                snowMan.GetComponent<SpriteRenderer>().sprite = snowManUp;
            }
            else if (characterAnimator.GetFloat("InputY") == -1)
            {
                snowMan.GetComponent<SpriteRenderer>().sprite = snowManDown;
            }
            else if (characterAnimator.GetFloat("InputX") == 1)
            {
                snowMan.GetComponent<SpriteRenderer>().sprite = snowManRight;
                snowMan.GetComponent<SpriteRenderer>().transform.localScale = new Vector3(-5, 5, 1);
            }
            else if (characterAnimator.GetFloat("InputX") == -1)
            {
                snowMan.GetComponent<SpriteRenderer>().sprite = snowManRight;
                snowMan.GetComponent<SpriteRenderer>().transform.localScale = new Vector3(5, 5, 1);
            }
        }
        else
        {

            this.tag = "Player";
            body.SetActive(true);
            snowMan.SetActive(false);
        }
    } 
}
