using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BowlController : MonoBehaviour
{
    public GameObject ball;   //ball prefab
    private GameObject newBall; //Instantiated GameObject
    public WicketController wicketController;
    public int noOfBalls = 0;
    public Transform TipSpot; 
    public float throwingSpeed = 5f;
    public Vector3 throwingDirection = new Vector3(0,-10,-30); 
    public Text Over;
    public Animator bowlingAnimator;
    public string[] ballTypes = { "ARM BALL", "FAST", "LEG SPIN", "OFF SPIN" };
    public GameObject gameManager;
    public Vector3 error = new Vector3(0, -2f, 0);

    // Start is called before the first frame update
    void Start()
    {
        bowlingAnimator = GetComponent<Animator>();
        Over.text = "Over: "+ noOfBalls.ToString() + "/12";
        newBall = NewBallCreated();
        throwingDirection = TipSpot.position - newBall.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        /* if(ball.transform.position.z < -12f && count <= 1)
         {
             count++;
             Debug.Log(count);
             SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
         } */

        //if the ball goes behind the wicket destroy the ball and update the over
        //can be replaced by a function
        if (newBall.transform.position.z < -12f)
        {
            destroyBall();
        }

        if(noOfBalls > 12)
        {
            Debug.Log("Inside if");
            gameManager.GetComponent<GameManager>().gameOver();
        }
    }

    public void destroyBall() {
        Destroy(newBall);
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
        randomizeTip();
        throwingDirection = TipSpot.position - newBall.transform.position;
       
    }

    void randomizeTip() {
        TipSpot.position = new Vector3(Random.Range(0.27f, 1.36f), TipSpot.position.y, Random.Range(-3.2f, -5.04f)); //TipSpot position randomized

    }


    //Invoked by the end of bowling animation
    public void ThrowBall()
    {

        newBall.SetActive(true); 
        newBall.GetComponent<Rigidbody>().AddForce(throwingDirection * throwingSpeed - error, ForceMode.Impulse);

    }


    public GameObject NewBallCreated()
    {
        noOfBalls++;
        return Instantiate(ball, ball.transform);
    }
}
