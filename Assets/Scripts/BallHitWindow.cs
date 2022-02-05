using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallHitWindow : MonoBehaviour
{
    
    public static bool canHit;
    // public bool CanHit{ get { return canHit; } }
    // Start is called before the first frame update

    RectTransform distanceTransform;
    void Start()
    {
       canHit = false;
        // transform.GetChild(0).GetComponent<Rigidbody>().freezeRotation = true;
        distanceTransform = this.gameObject.transform.GetChild(0).gameObject.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        //   Debug.Log(canHit);
        distanceTransform.localEulerAngles = new Vector3(
       this.gameObject.transform.eulerAngles.x,
       (this.gameObject.transform.eulerAngles.y + 180),
       this.gameObject.transform.eulerAngles.z);
    }

    private void FixedUpdate()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        if(other.gameObject.name == "EntryWindow")
        {
            canHit = true;
        //    Debug.Log(canHit);
            Debug.Log("Entry");
        }
        if (other.gameObject.name == "ExitWindow")
        {
            //  Time.timeScale = 1f;

            canHit = false;
          //  Debug.Log(canHit);
            Debug.Log("Exit");

        }
    }
}
