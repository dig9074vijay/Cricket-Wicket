using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FPSCounter : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI fps;
    float currentFPS;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currentFPS = 1 / Time.unscaledDeltaTime;
        fps.text = currentFPS.ToString("0");
    }
}
