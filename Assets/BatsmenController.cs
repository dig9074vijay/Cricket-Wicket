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
    public GameObject bat;
    public GameObject EntryWindow;
    public GameObject ExitWindow;
  //public BallHitWindow ballHitWindow;
    Animator anim;
    private Animator bowlingAnimator;
    public BowlController bowlController;
    public string[] offShots = new string[]{"SquareCut","LateCut","Cover"};
    public string[] legShots = new string[]{ "Hook", "Sweep", "StDrive" };
    int index = 0;
    public CameraShaker cameraShaker;
    public int[] scores = new int[] { 1, 2, 4, 6 };
    public GameObject Boom;

    public GameObject Six;
    public GameObject Four;


    int runIndex;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        bowlingAnimator = bowlController.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
    //  Debug.Log(BallHitWindow.canHit);

    }

    //batsmen shot animations called from the button clicks
    public void PlayLeg() {
        if (bowlController.canSwing)
        {
            if (/*bowlController.transform.position.z < 2.9f */ BallHitWindow.canHit)
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
            bowlController.canSwing = false;
        }
    }

    public void PlayOff()
    {
        if (bowlController.canSwing)
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
            bowlController.canSwing = false;
        }
    }

    //public void PlayCover()
    //{
    //    anim.SetTrigger("Cover");
    //}

    //adding force to the balls
    public void LegShot()
    {
        StartCoroutine(HitWithBoom());
        bowlController.newBall.GetComponent<Rigidbody>().AddForce(new Vector3(-10f, 10f, 12f), ForceMode.Impulse);
        runIndex = Random.Range(0, 3);
        bowlController.score += scores[runIndex];
        bowlController.Score.text = "Score: " + bowlController.score.ToString();
        if (scores[runIndex] == 6)
        {
            StartCoroutine(BoundaryDisplay(Six));


        }
        else if(scores[runIndex]== 4)
        {
            StartCoroutine(BoundaryDisplay(Four));
        }
      
        Debug.Log("Shot!!!");
        BallHitWindow.canHit = false;
    }

    public void OffShot()
    {
        StartCoroutine(HitWithBoom());

        bowlController.newBall.GetComponent<Rigidbody>().AddForce(new Vector3(20f, 10f, 12f), ForceMode.Impulse);
        runIndex = Random.Range(0, 3);
        bowlController.score += scores[runIndex];
        bowlController.Score.text = "Score: " + bowlController.score.ToString();

        Debug.Log("Shot!!!");
        BallHitWindow.canHit = false;

    }

    IEnumerator HitWithBoom()
    {
        bowlController.newBall.transform.parent = bat.transform;
        Boom.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        bowlController.newBall.transform.parent = transform.root;
        Boom.SetActive(false);
        Debug.Log("HitWithBoom");

    }

    IEnumerator BoundaryDisplay(GameObject gO) {
        gO.SetActive(true);

        yield return new WaitForSeconds(1f);
        gO.SetActive(false);

    }

}
