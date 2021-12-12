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
    public GameObject Miss;
    Transform HitTimePosition;
    public GameObject HitTime; //UI element for displaying how later or early the ball is hit
   // public float HitPositionZ;
    int runIndex;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        bowlingAnimator = bowlController.GetComponent<Animator>();
        HitTimePosition = HitTime.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
    //  Debug.Log(BallHitWindow.canHit);

    }

    //batsmen shot animations called from the button clicks
    public void PlayLeg() {

        HitTimePosition.localPosition = new Vector3(0f,Remap(bowlController.newBall.transform.position.z, -7.23f,-4.2f,-32f, 34f),0f);
        if (bowlController.canSwing)
        {
            if (/*bowlController.transform.position.z < 2.9f */ BallHitWindow.canHit && bowlController.canPlayLeg)
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
                index = Random.Range(0, 2);
                anim.SetTrigger(legShots[index]);
                StartCoroutine(BoundaryDisplay(Miss));

            }
            bowlController.canSwing = false;
        }
    }

    public void PlayOff()
    {
        HitTimePosition.localPosition = new Vector3(0f, Remap(bowlController.newBall.transform.position.z, -4.2f, -7.23f, 34f, -32f), 0f);

        if (bowlController.canSwing)
        {
            if (BallHitWindow.canHit && bowlController.canPlayOff)
            {
                index = Random.Range(0, 2);
                anim.SetTrigger(offShots[index]);
                OffShot();
                // MyShaker.Shake(ShakePreset);
                // StartCoroutine(cameraShaker.Shake(0.15f,0.35f));
            }
            else
            {
                index = Random.Range(0, 2);

                anim.SetTrigger(offShots[index]);
                StartCoroutine(BoundaryDisplay(Miss));
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
        yield return new WaitForSeconds(2f);
        gO.SetActive(false);
    }

    public static float Remap(float input, float oldLow, float oldHigh, float newLow, float newHigh)
    {
        float t = Mathf.InverseLerp(oldLow, oldHigh, input);
        return Mathf.Lerp(newLow, newHigh, t);
    }

}
