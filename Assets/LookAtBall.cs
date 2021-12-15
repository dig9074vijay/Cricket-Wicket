using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtBall : MonoBehaviour
{
    public float smoothSpeed = 10f;

    public Transform target;
    Quaternion initialRot;
    public Vector3 offset = new Vector3(0f, 0f, -5f);
    Vector3 initialPos;
    bool look = false;
    // public BatsmenController batsmenController;
    public BowlController bowlController;
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        initialPos = new Vector3(0.19f, 2.3f, -10.47f);
        
        anim = GetComponent<Animator>();
        Invoke("PlayCameraAnimation", 8.4f);
    }

    // Update is called once per frame
    void Update()
    {
        //if (look)
        //{
        //    transform.position = target.position - offset;
        //}
    }

    void FixedUpdate()
    {
        if (look)
        {
            Vector3 desiredPosition = target.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
            transform.position = smoothedPosition;

            transform.LookAt(target);
        }
    }

    public void FollowBall()
    {
        target = bowlController.newBall.GetComponent<Transform>();
        anim.enabled = false;
        initialRot = transform.rotation;
        look = true;
        Invoke("BackToPitch", 3f);
    }

    public void BackToPitch()
    {
        look = false;
        transform.position = initialPos;
        transform.rotation = initialRot;
        bowlController.destroyBall();
    }

    //   public Transform target;

   
    //public Vector3 offset;

    void PlayCameraAnimation()
    {
        anim.Play("StartCamera");
    }
}
