using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WicketController : MonoBehaviour
{
    Animator anim;
    public BowlController bowlController;
    public AudioSource audioSource;
    public bool isBowled = false;
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
            isBowled = true;
       
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
            yield return new WaitForSeconds(1f);
            anim.SetBool("Bowled", false);
            break;
        }
    }
}
