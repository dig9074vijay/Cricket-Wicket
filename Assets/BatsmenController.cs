using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MilkShake;
using TMPro;


public class BatsmenController : MonoBehaviour
{
    //Inspector field for the Shaker component.
 //
    [SerializeField] GameObject bat;
    [SerializeField] GameObject EntryWindow;
    [SerializeField] GameObject ExitWindow;
    AudioSource audioSource;
    [SerializeField] AudioClip thatsHuge;
    [SerializeField] AudioClip ballHitClip;
    [SerializeField] TextMeshProUGUI Ball1Score;
    [SerializeField] TextMeshProUGUI Ball2Score;
    [SerializeField] TextMeshProUGUI Ball3Score;
    [SerializeField] TextMeshProUGUI Ball4Score;
    [SerializeField] TextMeshProUGUI Ball5Score;
    [SerializeField] TextMeshProUGUI Ball6Score;

    [SerializeField] GameObject Umpire;
    public Animator umpireAnim;

    bool displayDistance=false;
    TextMeshPro ballDistance;
    //public BallHitWindow ballHitWindow;
    Animator anim;
    private Animator bowlingAnimator;
    public BowlController bowlController;
    string[] offShots = new string[]{"SquareCut","OnDrive","Cover"};
    string[] legShots = new string[]{ "Helicopter", "Sweep", "StDrive" };
    int index = 0;
    public int ScoreMultiplier = 1;
  //  public CameraShaker cameraShaker;
    int[] scores = new int[] { 1, 2, 3, 4, 6 };
    [SerializeField] GameObject Boom;

    // public Camera mainCamera;
    // public GameObject Bowler;

     public bool displayParticles = false;

    [SerializeField] LookAtBall look;

    Vector3 offForce = new Vector3(20f, 10f, 12f);
    Vector3 legForce = new Vector3(-10f, 10f, 12f);


    [SerializeField] GameObject Six;
    [SerializeField] GameObject Four;
    [SerializeField] GameObject Miss;
    Transform HitTimePosition;
    [SerializeField] GameObject HitTime; //UI element for displaying how later or early the ball is hit
   // public float HitPositionZ;
    int runIndex;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        bowlingAnimator = bowlController.GetComponent<Animator>();
        HitTimePosition = HitTime.GetComponent<RectTransform>();
        audioSource = GetComponent<AudioSource>();
        umpireAnim = Umpire.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //  Debug.Log(BallHitWindow.canHit);
        if(displayDistance == true)
        ballDistance.text = "+" + (Vector3.Distance(bowlController.newBall.transform.position, transform.position)).ToString("0") + "m";
    }

    //batsmen shot animations called from the button clicks
    public void PlayLeg() {
      //  Time.timeScale = 1f;
        //Time.fixedDeltaTime = 0.02f * Time.timeScale;

        HitTimePosition.localPosition = new Vector3(0f,Remap(bowlController.newBall.transform.position.z, -7.23f,-4.2f,-32f, 34f),0f);
        if (bowlController.canSwing)
        {
            if (/*bowlController.transform.position.z < 2.9f */ BallHitWindow.canHit && bowlController.canPlayLeg)
            {
                index = Random.Range(0, 3);
          
                anim.SetTrigger(legShots[index]);
                 LegShot();



                /*Here this lrgshot function is being called by animation*/




                Debug.Log("Play Leg");
                // MyShaker.Shake(ShakePreset);
                //    StartCoroutine(cameraShaker.Shake(0.15f,0.35f));
                //Invoke("LegShot",0.1f);
            }
            else
            {
                index = Random.Range(0, 3);
                anim.SetTrigger(legShots[index]);
                StartCoroutine(BoundaryDisplay(Miss));

            }
            bowlController.canSwing = false;
        }
    }

    public void PlayOff()
    {
      // Time.timeScale = 1f;
        //Time.fixedDeltaTime = 0.02f * Time.timeScale;

        HitTimePosition.localPosition = new Vector3(0f, Remap(bowlController.newBall.transform.position.z, -4.2f, -7.23f, 34f, -32f), 0f);

        if (bowlController.canSwing)
        {
            if (BallHitWindow.canHit && bowlController.canPlayOff)
            {
                index = Random.Range(0, 3);
                Debug.Log(offShots[index]);
                anim.SetTrigger(offShots[index]);
                OffShot();

                /*Here this offshot function is being called by animation*/






                // MyShaker.Shake(ShakePreset);
                // StartCoroutine(cameraShaker.Shake(0.15f,0.35f));
            }
            else
            {
                index = Random.Range(0, 3);
                Debug.Log(offShots[index]);

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
        audioSource.clip = ballHitClip;
        audioSource.Play();
        if (HitTimePosition.localPosition.y <= 12f && HitTimePosition.localPosition.y >= -12f)
        {
            runIndex = Random.Range(3, 5);
            if (runIndex == 3)
            {
                legForce = new Vector3(-6f, 3f, 10f);
                umpireAnim.Play("Four");
            }

            else
            {
                legForce = new Vector3(-7f, 6f, 13f);
                umpireAnim.Play("Six");

            }
        }
        else
        {
            runIndex = Random.Range(0, 3);

            legForce = new Vector3(-3f,4f,7f);
        }
        bowlController.newBall.GetComponent<Rigidbody>().AddForce(legForce, ForceMode.Impulse);
        if (bowlController.noOfBalls == 1 || bowlController.noOfBalls == 7)
        {
            Ball1Score.gameObject.transform.parent.gameObject.SetActive(true);
            Ball1Score.text = (scores[runIndex] * ScoreMultiplier).ToString();
        }
        if (bowlController.noOfBalls == 2 || bowlController.noOfBalls == 8)
        {
            Ball2Score.gameObject.transform.parent.gameObject.SetActive(true);

    
            Ball2Score.text = (scores[runIndex] * ScoreMultiplier).ToString();
        }
        if (bowlController.noOfBalls == 3 || bowlController.noOfBalls == 9)
        {
            Ball3Score.gameObject.transform.parent.gameObject.SetActive(true);

            Ball3Score.text = (scores[runIndex] * ScoreMultiplier).ToString();
        }
        if (bowlController.noOfBalls == 4 || bowlController.noOfBalls == 10)
        {
            Ball4Score.gameObject.transform.parent.gameObject.SetActive(true);

            Ball4Score.text = (scores[runIndex] * ScoreMultiplier).ToString();
        }
        if (bowlController.noOfBalls == 5 || bowlController.noOfBalls == 11)
        {
            Ball5Score.gameObject.transform.parent.gameObject.SetActive(true);

            Ball5Score.text = (scores[runIndex] * ScoreMultiplier).ToString();
        }
        if (bowlController.noOfBalls == 6 || bowlController.noOfBalls == 12)
        {
            Ball6Score.gameObject.transform.parent.gameObject.SetActive(true);

            Ball6Score.text = (scores[runIndex] * ScoreMultiplier).ToString();
        }
        bowlController.score += (scores[runIndex]*ScoreMultiplier);
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
        StartCoroutine("LookBall");

        BallHitWindow.canHit = false;
    }

    public void OffShot()
    {

        StartCoroutine(HitWithBoom());
        audioSource.clip = ballHitClip;

        audioSource.Play();
        if (HitTimePosition.localPosition.y <= 12f && HitTimePosition.localPosition.y >= -12f)
        {
            runIndex = Random.Range(3, 5);
            if (runIndex == 3)
            {
                offForce = new Vector3(6f, 3f, 9f);
                umpireAnim.Play("Four");
            }
            else
            {
                offForce = new Vector3(8f, 8f, 12f);
                umpireAnim.Play("Six");

            }
        }
        else
        {
            runIndex = Random.Range(0, 3);
            offForce = new Vector3(4f, 1.2f, 6f);


        }
        bowlController.newBall.GetComponent<Rigidbody>().AddForce(offForce, ForceMode.Impulse);
        if (bowlController.noOfBalls == 1 || bowlController.noOfBalls == 7)
        {
            Ball1Score.gameObject.transform.parent.gameObject.SetActive(true);
            Ball1Score.text = (scores[runIndex] * ScoreMultiplier).ToString();
        }
        if (bowlController.noOfBalls == 2 || bowlController.noOfBalls == 8)
        {
            Ball2Score.gameObject.transform.parent.gameObject.SetActive(true);


            Ball2Score.text = (scores[runIndex] * ScoreMultiplier).ToString();
        }
        if (bowlController.noOfBalls == 3 || bowlController.noOfBalls == 9)
        {
            Ball3Score.gameObject.transform.parent.gameObject.SetActive(true);

            Ball3Score.text = (scores[runIndex] * ScoreMultiplier).ToString();
        }
        if (bowlController.noOfBalls == 4 || bowlController.noOfBalls == 10)
        {
            Ball4Score.gameObject.transform.parent.gameObject.SetActive(true);

            Ball4Score.text = (scores[runIndex] * ScoreMultiplier).ToString();
        }
        if (bowlController.noOfBalls == 5 || bowlController.noOfBalls == 11)
        {
            Ball5Score.gameObject.transform.parent.gameObject.SetActive(true);

            Ball5Score.text = (scores[runIndex] * ScoreMultiplier).ToString();
        }
        if (bowlController.noOfBalls == 6 || bowlController.noOfBalls == 12)
        {
            Ball6Score.gameObject.transform.parent.gameObject.SetActive(true);

            Ball6Score.text = (scores[runIndex] * ScoreMultiplier).ToString();
        }
        bowlController.score += (scores[runIndex]*ScoreMultiplier);
        bowlController.Score.text = "Score: " + bowlController.score.ToString();
        if (scores[runIndex] == 6)
        {
            StartCoroutine(BoundaryDisplay(Six));


        }
        else if (scores[runIndex] == 4)
        {
            StartCoroutine(BoundaryDisplay(Four));
        }
       
        Debug.Log("Shot!!!");
        StartCoroutine("LookBall");

        BallHitWindow.canHit = false;

    }

    IEnumerator HitWithBoom()
    {
    //    Boom.transform.position = bowlController.newBall.transform.position;
        bowlController.newBall.transform.parent = bat.transform;
        Boom.SetActive(true);
        bowlController.newBall.GetComponent<TrailRenderer>().enabled = false;
        yield return new WaitForSeconds(0.2f);
        // anim.Play("BattingIdle_01 0");
    
       // bowlController.newBall.transform.parent = bat.gameObject.transform;
     //   bowlController.newBall.transform.position = new Vector3(0,0,0);
        Boom.SetActive(false);
        Debug.Log("HitWithBoom");
        yield return new WaitForSeconds(0.5f);
        //  anim.StartPlayback("BattingIdle_01 0");
        anim.enabled = false;
        bowlController.newBall.GetComponent<TrailRenderer>().enabled = true;

        yield return new WaitForSeconds(1.5f);
        anim.enabled = true;

        anim.Play("BattingIdle_01 0");

    }

    IEnumerator BoundaryDisplay(GameObject gO) {
        // gO.SetActive(true);
        displayParticles = true;
        yield return new WaitForSeconds(1.5f);
        gO.SetActive(false);
        displayParticles = false;
    }

    public static float Remap(float input, float oldLow, float oldHigh, float newLow, float newHigh)
    {
        float t = Mathf.InverseLerp(oldLow, oldHigh, input);
        return Mathf.Lerp(newLow, newHigh, t);
    }


    

    IEnumerator LookBall()
    {
        yield return new WaitForSeconds(0.4f);
        look.FollowBall();
        ballDistance = bowlController.newBall.transform.GetChild(0).gameObject.GetComponent<TextMeshPro>();
        bowlController.newBall.transform.GetChild(0).gameObject.SetActive(true);
        displayDistance = true;
        Invoke("BackPitch", 1.5f);

    }

    void BackPitch()
    {
        displayDistance = false;
    }
}
