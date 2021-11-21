using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatsmenController : MonoBehaviour
{
    Animator anim;
    private Animator bowlingAnimator;
    public BowlController bowlController;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        bowlingAnimator = bowlController.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    //batsmen shot animations called from the button clicks
    public void PlayHook() {
        if (/*bowlController.transform.position.z < 2.9f */ true )
        {
            anim.SetTrigger("Hook");
            Invoke("LegShot",0.1f);
           
        }
        else
        {

            anim.SetTrigger("Hook");
       }
    }

    public void PlayLateCut()
    {
        if (true)
        {
            anim.SetTrigger("LateCut");
            Invoke("OffShot",0.1f);
        }
        else
        {
            anim.SetTrigger("LateCut");

        }
    }

    public void PlayCover()
    {
        anim.SetTrigger("Cover");
    }

    public void LegShot()
    {
        bowlController.newBall.GetComponent<Rigidbody>().AddForce(new Vector3(-10f, 10f, 12f), ForceMode.Impulse);
        Debug.Log("Shot!!!");
    }

    public void OffShot()
    {
        bowlController.newBall.GetComponent<Rigidbody>().AddForce(new Vector3(20f, 10f, 12f), ForceMode.Impulse);
        Debug.Log("Shot!!!");
    }
}
