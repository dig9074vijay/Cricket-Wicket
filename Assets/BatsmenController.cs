using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatsmenController : MonoBehaviour
{
    Animator anim;
    private Animator bowlingAnimator;
    public BowlController bowlController;
    public string[] offShots = new string[]{"SquareCut","LateCut","Cover"};
    public string[] legShots = new string[]{ "Hook", "Sweep", "StDrive" };
    int index = 0;

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
    public void PlayLeg() {
        if (/*bowlController.transform.position.z < 2.9f */ true )
        {
            index = Random.Range(0, 2);
            anim.SetTrigger(legShots[index]);
            Invoke("LegShot",0.1f);
           
        }
        else
        {

            anim.SetTrigger("Hook");
       }
    }

    public void PlayOff()
    {
        if (true)
        {
            index = Random.Range(0, 2);
            anim.SetTrigger(offShots[index]);
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


    //adding force to the balls
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
