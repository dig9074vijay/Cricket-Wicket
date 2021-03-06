using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class BowlController : MonoBehaviour
{
    [SerializeField] GameObject ball;   //ball prefab
 //   [SerializeField] GameObject bowledTimeline;
    public GameObject newBall; //Instantiated GameObject
    public int score = 0;
    [SerializeField] WicketController wicketController;
    public int noOfBalls = 0;
    int ballTypeIndex = 0;
   // public static BowlController instance;
    [SerializeField] Transform TipSpot;
     [SerializeField] AudioClip BatsmenRhythmClip;
    [SerializeField] AudioSource audioSource;

    [SerializeField] float throwingSpeed = 5f;
    [SerializeField] Vector3 throwingDirection = new Vector3(0,-10,-30);
    [SerializeField] Vector3 spinDirection = new Vector3(0, 0, 0);
    [SerializeField] float spinIntensity = 3f;
    float temp;
    float tempOver;
    [SerializeField] Text Over;
    public Text Score;
    [SerializeField] Text p1Score; 
    Animator bowlingAnimator;
    [SerializeField] string[] ballTypes = { "ARM BALL", "FAST", "LEG SPIN", "OFF SPIN" };
    [SerializeField] GameManager gameManager;
    [SerializeField] Vector3 error = new Vector3(0, -2f, 0);
    public bool canSwing = false;
    [SerializeField] GameObject EarlyLateBar;
    //bool offBall = false;
    //bool armBall = false;
    //bool fast = false;
    //bool legBall = false;
    float tipSpotLeftPosition = 0.27f;
    float tipSpotRightPosition = 1.36f;

    public bool  canPlayOff = false;
    public bool  canPlayLeg = false;
    [SerializeField] GameObject lcd_arm;
    [SerializeField] GameObject lcd_fast;

    [SerializeField] GameObject lcd_off;

    [SerializeField] GameObject lcd_leg;
    [SerializeField] BatsmenController batsmenController;

    //public GameObject ballDistance;
    // Start is called before the first frame update
    void Start()
    {
   //     instance = this;
        StartCoroutine(PlayBatsmenRhythmClip());
        bowlingAnimator = GetComponent<Animator>();
        Over.text = "Over: "+ noOfBalls.ToString() + "/2";
        Score.text = "Score: " + score.ToString();

        newBall = NewBallCreated();

        ChooseBallType();

        throwingDirection = TipSpot.position - ball.transform.position;
     
    }

    // Update is called once per frame
    void Update()
    {
        if (newBall.transform.position.z < -50f)
        {
            destroyBall();
        }

        p1Score.text = score.ToString();
        // ballDistance.GetComponent<TextMesh>().text = Vector3.Distance(ball.transform.position, new Vector3(0, 0, -7.61f)).ToString();
    }

    private void FixedUpdate()
    {
        if (newBall.transform.position.z >= (TipSpot.position.z - 0.1f) && newBall.transform.position.z <= (TipSpot.position.z + 0.1f))
        {
            newBall.GetComponent<Rigidbody>().AddForce(spinDirection * spinIntensity, ForceMode.Impulse);
           
        }
    }

    public void destroyBall() {
        GameManager.instance.isGameSlow = false;
        Destroy(newBall);
        EarlyLateBar.SetActive(true);
        
        bowlingAnimator.SetBool("Finished", true);
        tempOver = noOfBalls / 10f;
        Debug.Log(tempOver);
        if (tempOver < 0.6f)
            Over.text = "Over: " + tempOver.ToString() + "/2";
        else if (tempOver == 0.6f)
        {
            Over.text = "Over: " + "1/2";
            batsmenController.DisableAllScoreElements();
        }
        else if (tempOver > 0.6f && tempOver != 0.12f)
        {
            temp = 1f + tempOver - 0.6f;
            Debug.Log(temp);

            Over.text = "Over: " + temp.ToString() + "/2";

        }
        else if (tempOver == 0.12f)
        {
            Over.text = "Over: " + "2/2";

        }

        Random.Range(0, 4);
        NextBall();
    }


    public void NextBall()
    {
        newBall = NewBallCreated();
        ChooseBallType();

        randomizeTip();
        throwingDirection = TipSpot.position - newBall.transform.position;  
    }

    void randomizeTip() {
        TipSpot.position = new Vector3(Random.Range(tipSpotLeftPosition, tipSpotRightPosition), TipSpot.position.y, Random.Range(-3.2f, -5.04f)); //TipSpot position randomized
    }

    //Invoked by the end of bowling animation
    public void ThrowBall()
    {
        EarlyLateBar.SetActive(false);
        newBall.SetActive(true);
        GameManager.instance.isGameSlow = true;
        canSwing = true;
        //newBall.GetComponent<Rigidbody>().AddForce(new Vector3(0,-10f,30f) * throwingSpeed, ForceMode.Impulse);
        // Debug.Log("ThrowBall Invoked");
       // gameManager.SlowGame();
        //Time.fixedDeltaTime = 0.02f * Time.timeScale;
        Debug.Log(Time.timeScale);
     //   Time.fixedDeltaTime
        newBall.GetComponent<Rigidbody>().AddForce(throwingDirection * throwingSpeed - error, ForceMode.Impulse );
      //  newBall.GetComponent<Rigidbody>().velocity = throwingDirection * throwingSpeed - error;
    }

    public GameObject NewBallCreated()
    {
        noOfBalls++;
        batsmenController.DisplayScoreOnDotBall();
        return Instantiate(ball);
    }

    void ChooseBallType()
    {
        ballTypeIndex = Random.Range(0, 4);
        if (ballTypes[ballTypeIndex] == "ARM BALL")
        {
            lcd_arm.SetActive(true);
            lcd_fast.SetActive(false);
            lcd_off.SetActive(false);
            lcd_leg.SetActive(false);
            tipSpotLeftPosition = 0.27f;
            tipSpotRightPosition = 0.9f;
            spinDirection = new Vector3(0,0,0);
            canPlayLeg = true;
            canPlayOff = true;
            //armBall = true;
            //offBall = false;
            //legBall = false;
            //fast = false;
        }
        else if (ballTypes[ballTypeIndex] == "FAST")
        {
            lcd_arm.SetActive(false);
            lcd_fast.SetActive(true);
            lcd_off.SetActive(false);
            lcd_leg.SetActive(false);
            tipSpotLeftPosition = 0.27f;
            tipSpotRightPosition = 1.1f;
            spinDirection = new Vector3(0, 0, 0);
            canPlayLeg = true;
            canPlayOff = true;
            //armBall = false;
            //offBall = false;
            //legBall = false;
            //fast = true;
        }
        else if (ballTypes[ballTypeIndex] == "LEG SPIN")
        {
            lcd_arm.SetActive(false);
            lcd_fast.SetActive(false);
            lcd_off.SetActive(false);
            lcd_leg.SetActive(true);
            tipSpotLeftPosition = 0.27f;
            tipSpotRightPosition = 0.7f;
            spinDirection = new Vector3(0.5f, 0, 0);
            canPlayLeg = true;
            canPlayOff = false;
            //armBall = false;
            //offBall = false;
            //legBall = true;
            //fast = false;
        }
        else if (ballTypes[ballTypeIndex] == "OFF SPIN")
        {
            lcd_arm.SetActive(false);
            lcd_fast.SetActive(false);
            lcd_off.SetActive(true);
            lcd_leg.SetActive(false);
            tipSpotLeftPosition = 0.9f;
            tipSpotRightPosition = 1.36f;
            spinDirection = new Vector3(-0.5f, 0, 0);
            canPlayLeg = false;
            canPlayOff = true;
            //armBall = false;
            //offBall = true;
            //legBall = false;
            //fast = false;
        }
    }


    IEnumerator PlayBatsmenRhythmClip()
    {
        yield return new WaitForSeconds(2f);
        audioSource.clip = BatsmenRhythmClip;
        audioSource.Play();
    }

}
