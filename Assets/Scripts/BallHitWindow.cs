using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallHitWindow : MonoBehaviour
{
    public static bool canHit;
   // public bool CanHit{ get { return canHit; } }
    // Start is called before the first frame update
    void Start()
    {
       canHit = false;
    }

    // Update is called once per frame
    void Update()
    {
       //   Debug.Log(canHit);
    }

    private void OnTriggerEnter(Collider other)
    {
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
