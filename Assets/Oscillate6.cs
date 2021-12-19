using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscillate6 : MonoBehaviour
{
    float zPosition;
  //  float t;
  //  Transform transformPoster;
    // Start is called before the first frame update
    void Start()
    {
        zPosition = transform.position.z;
       // StartCoroutine("Animate");
        //transformPoster = this.gameObject.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(0f,0f, Mathf.PingPong(zPosition,0.5f));
    }
}
