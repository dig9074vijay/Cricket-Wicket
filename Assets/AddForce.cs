using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddForce : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody>().AddForce(new Vector3(-10f, 5f, 8f), ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
