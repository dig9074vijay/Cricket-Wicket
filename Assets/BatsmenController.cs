using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatsmenController : MonoBehaviour
{
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();  
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayHook() {
        anim.SetTrigger("Hook");
    }

    public void PlayLateCut()
    {
        anim.SetTrigger("LateCut");
    }

    public void PlayCover()
    {
        anim.SetTrigger("Cover");
    }

}
