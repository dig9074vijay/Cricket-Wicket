using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MilkShake;
public class BatsmenController : MonoBehaviour
{
    //Inspector field for the Shaker component.
    public Shaker MyShaker;
    //Inspector field for a Shake Preset to use as the shake parameters.
    public ShakePreset ShakePreset;

    public GameObject EntryWindow;
    public GameObject ExitWindow;
  //  public BallHitWindow ballHitWindow;
    Animator anim;
    private Animator bowlingAnimator;
    public BowlController bowlController;
    public string[] offShots = new string[]{"SquareCut","LateCut","Cover"};
    public string[] legShots = new string[]{ "Hook", "Sweep", "StDrive" };
    int index = 0;
    public CameraShaker cameraShaker;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        bowlingAnimator = bowlController.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
      Debug.Log(BallHitWindow.canHit);

    }

    //batsmen shot animations called from the button clicks
    public void PlayLeg() {
        if (/*bowlController.transform.position.z < 2.9f */ BallHitWindow.canHit )
        {
            index = Random.Range(0, 2);
            anim.SetTrigger(legShots[index]);
            LegShot();
            Debug.Log("Play Leg");
           // MyShaker.Shake(ShakePreset);
            //    StartCoroutine(cameraShaker.Shake(0.15f,0.35f));
            //Invoke("LegShot",0.1f);
        }
        else
        {

            anim.SetTrigger("Hook");
        }
    }

    public void PlayOff()
    {
        if (BallHitWindow.canHit)
        {
            index = Random.Range(0, 2);
            anim.SetTrigger(offShots[index]);
            OffShot();
           // MyShaker.Shake(ShakePreset);
            // StartCoroutine(cameraShaker.Shake(0.15f,0.35f));
        }
        else
        {
            anim.SetTrigger("LateCut");
        }
    }

    //public void PlayCover()
    //{
    //    anim.SetTrigger("Cover");
    //}

    //adding force to the balls
    public void LegShot()
    {
        bowlController.newBall.GetComponent<Rigidbody>().AddForce(new Vector3(-10f, 10f, 12f), ForceMode.Impulse);
        Debug.Log("Shot!!!");
        BallHitWindow.canHit = false;
    }

    public void OffShot()
    {
        bowlController.newBall.GetComponent<Rigidbody>().AddForce(new Vector3(20f, 10f, 12f), ForceMode.Impulse);
        Debug.Log("Shot!!!");
        BallHitWindow.canHit = false;

    }
}
