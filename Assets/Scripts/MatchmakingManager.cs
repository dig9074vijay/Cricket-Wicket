using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MatchmakingManager : MonoBehaviour
{
    public static MatchmakingManager instance;

   
    public float time = 0f;

    
    public TextMeshProUGUI OppNo;

  

    void Start()
    {
        instance = this;

      
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        if (!GameManagerNetwork.instance.foundOtherPlayer)
        OppNo.text = string.Format("{0}*****{1}", UnityEngine.Random.Range(80, 99), UnityEngine.Random.Range(800, 999));

       

    }
}
