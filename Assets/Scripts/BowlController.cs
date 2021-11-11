using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BowlController : MonoBehaviour
{
    public GameObject ball;
    Rigidbody rb;
    public Transform TipSpot;
    public float throwingSpeed = 5f;
    public Vector3 throwingDirection = new Vector3(0,-10,-30);
    private int count;
    
    // Start is called before the first frame update
    void Start()
    {
        
        rb =ball.GetComponent<Rigidbody>();
        throwingDirection = TipSpot.position - ball.transform.position;
        count = 0;
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
        if (ball.transform.position.z < -12f)
        {
            Destroy(this.ball);
        }


    }
    public void ThrowBall()
    {
        
        ball.SetActive(true);
        
        rb.AddForce(throwingDirection * throwingSpeed, ForceMode.Impulse);

    }
}
