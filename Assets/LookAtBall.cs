using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtBall : MonoBehaviour
{
    [SerializeField] float smoothSpeed = 10f;

    [SerializeField] Transform target;
    Quaternion initialRot;
    [SerializeField] Vector3 offset = new Vector3(0f, 0f, -5f);
    Vector3 initialPos;
    bool look = false;
    // public BatsmenController batsmenController;
    [SerializeField] BowlController bowlController;
    [SerializeField] BatsmenController batController;

    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        initialPos = new Vector3(0.19f, 1.58f, -9.78f);
        initialRot = new Quaternion(0.0503314f, 0f, -0.00244f, 0.9987296f);
        anim = GetComponent<Animator>();
       // Invoke("PlayCameraAnimation", 8.4f);
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
      //  initialRot = transform.rotation;
        look = true;
        if (batController.displayParticles)
        {
            transform.GetChild(0).gameObject.SetActive(true);
            transform.GetChild(1).gameObject.SetActive(true);

        }
        Invoke("BackToPitch", 3f);
    }

    public void BackToPitch()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(false);
        look = false;

        transform.position = initialPos;
        transform.rotation = initialRot;

        bowlController.destroyBall();
        batController.displayParticles = false;

    }

    //   public Transform target;


    //public Vector3 offset;

    void PlayCameraAnimation()
    {
        anim.Play("StartCamera");
    }
}
