using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class planeBounds : MonoBehaviour
{
    GameObject _plane ;

    // Start is called before the first frame update
    void Start()
    {
         _plane = this.gameObject;

        Mesh planeMesh = _plane.GetComponent<MeshFilter>().mesh;
        Bounds bounds = planeMesh.bounds;
        // size in pixels
        float boundsX =  bounds.size.x;
        float boundsY =  bounds.size.y;
        float boundsZ =  bounds.size.z;
        Debug.Log(boundsX);
        Debug.Log( boundsY  );
        Debug.Log(boundsZ);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
