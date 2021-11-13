using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BowlController : MonoBehaviour
{
    public GameObject ball;   //ball prefab
    private GameObject newBall; //Instantiated GameObject
    public int noOfBalls = 0;
    public Transform TipSpot; 
    public float throwingSpeed = 5f;
    public Vector3 throwingDirection = new Vector3(0,-10,-30); 
    public Text Over;

    
    // Start is called before the first frame update
    void Start()
    {
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
            DestroyImmediate(newBall,true);
            Over.text = "Over: "+ noOfBalls.ToString() + "/12";
        }


    }

    //Invoked by the end of bowling animation
    public void ThrowBall()
    {

        newBall.SetActive(true); 
        newBall.GetComponent<Rigidbody>().AddForce(throwingDirection * throwingSpeed, ForceMode.Impulse);

    }


    public GameObject NewBallCreated()
    {
        noOfBalls++;
        return Instantiate(ball, ball.transform);
    }
}
