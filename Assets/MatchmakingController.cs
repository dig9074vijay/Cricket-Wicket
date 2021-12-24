using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchmakingController : MonoBehaviour
{

    [SerializeField] GameObject Timeline;
    [SerializeField] GameObject HowToPlayCanvas;
    [SerializeField] GameObject Matchmaking;
    [SerializeField] GameObject gameManager;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(startMatch());
    }

    

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator startMatch()
    {
        yield return new WaitForSeconds(3f);

        HowToPlayCanvas.SetActive(true);
        Matchmaking.SetActive(false);
        gameManager.SetActive(true);

        Timeline.SetActive(true);

    }
}
