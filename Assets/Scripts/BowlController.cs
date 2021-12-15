using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BowlController : MonoBehaviour
{
    public GameObject ball;   //ball prefab
    public GameObject newBall; //Instantiated GameObject
    public int score = 0;
    public WicketController wicketController;
    int noOfBalls = 0;
    int ballTypeIndex = 0;
    public Transform TipSpot; 
    public float throwingSpeed = 5f;
    public Vector3 throwingDirection = new Vector3(0,-10,-30);
    public Vector3 spinDirection = new Vector3(0, 0, 0);
    public float spinIntensity = 3f;

    public Text Over;
    public Text Score;
    public Text FinalScore;
    Animator bowlingAnimator;
    public string[] ballTypes = { "ARM BALL", "FAST", "LEG SPIN", "OFF SPIN" };
    public GameObject gameManager;
    public Vector3 error = new Vector3(0, -2f, 0);
    public bool canSwing = false;
    public GameObject EarlyLateBar;
    //bool offBall = false;
    //bool armBall = false;
    //bool fast = false;
    //bool legBall = false;
    float tipSpotLeftPosition = 0.27f;
    float tipSpotRightPosition = 1.36f;

    public bool  canPlayOff = false;
    public bool  canPlayLeg = false;
    public GameObject lcd_arm;
    public GameObject lcd_fast;

    public GameObject lcd_off;

    public GameObject lcd_leg;


    //public GameObject ballDistance;
    // Start is called before the first frame update
    void Start()
    {
        bowlingAnimator = GetComponent<Animator>();
        Over.text = "Over: "+ noOfBalls.ToString() + "/12";
        Score.text = "Score: " + score.ToString();

        newBall = NewBallCreated();

        ChooseBallType();

        throwingDirection = TipSpot.position - ball.transform.position;
     
    }

    // Update is called once per frame
    void Update()
    {
        if(noOfBalls > 12)
        {
            Debug.Log("Inside if GameOVER");
            FinalScore.text = "Your Score: " + score.ToString();
           

            gameManager.GetComponent<GameManager>().gameOver();
        }
        if (newBall.transform.position.z < -50f)
        {
            destroyBall();
        }


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
        Destroy(newBall);
        EarlyLateBar.SetActive(true);
        if (wicketController.isBowled)
        {
            bowlingAnimator.SetBool("Finished", false);

            bowlingAnimator.SetBool("Bowled", true);
            wicketController.isBowled = false;
        }
        else
        {
            bowlingAnimator.SetBool("Finished", true);

            bowlingAnimator.SetBool("Bowled", false);
        }
        bowlingAnimator.SetBool("Finished", true);
        
        Over.text = "Over: " + noOfBalls.ToString() + "/12";
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
        canSwing = true;
        //newBall.GetComponent<Rigidbody>().AddForce(new Vector3(0,-10f,30f) * throwingSpeed, ForceMode.Impulse);
       // Debug.Log("ThrowBall Invoked");
        newBall.GetComponent<Rigidbody>().AddForce(throwingDirection * throwingSpeed - error, ForceMode.Impulse);
    }

    public GameObject NewBallCreated()
    {
        noOfBalls++;
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
}
