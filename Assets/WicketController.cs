using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WicketController : MonoBehaviour
{
    Animator anim;
    [SerializeField] BowlController bowlController;
    [SerializeField] AudioSource audioSource;
  
    //  public GameObject outDisplay;
    [SerializeField] BatsmenController batsmenController;
   // BowlController bowlControllerWicket;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.name == "Ball(Clone)")
        {
         
            bowlController.score -= 2;
            bowlController.Score.text = "Score: " + bowlController.score.ToString();
            bowlController.GetComponent<Animator>().Play("Celebration_11");
            bowlController.destroyBall();
            audioSource.Play();
            StartCoroutine(PlaceWickets());
            //Deduct runs for getting bowled
        }
    }

    IEnumerator PlaceWickets()
    {
        while (true)
        {
            anim.SetBool("Bowled", true);
          //  outDisplay.SetActive(true);
            batsmenController.umpireAnim.Play("Out");

            yield return new WaitForSeconds(1f);
          //  outDisplay.SetActive(false);
            anim.SetBool("Bowled", false);
            break;
        }
    }
}
