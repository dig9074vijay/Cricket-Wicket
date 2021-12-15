using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{


    public bool isStart = false;   
    private bool isGamePaused = true;
    public int count;
    public Text textCountdown;
    public Canvas GameOver;
    public Canvas GameCanvas;
    public Canvas HowToPlay;
    public GameObject LCD;
    public GameObject Bowler;
  //  public GameObject Batsmen;

    public Camera mainCamera;
  //  public Camera batsmenCamera;
    public Camera stadiumCamera;
    // Start is called before the first frame update
    void Start()
    {

        //Invoke("StartGameAfterTimeline", 9.5f);
   

    }

    // Update is called once per frame
    void Update()
    {
  
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isGamePaused = !isGamePaused;
           
        }
        if(isGamePaused)
        {
            PauseGame();
        }
        else
        {
            ResumeGame();
        } 
    }

    public void gameOver()
    {
        GameOver.gameObject.SetActive(true);

        isGamePaused = true;
        Debug.Log("GameOver");
    }

    public void StartGame()
    {
        isGamePaused = false;
        HowToPlay.gameObject.SetActive(false);

        Invoke("StartGameAfterTimeline", 8.4f);
    }
    

    void StartGameAfterTimeline()
    {
        StartCoroutine(StartCount());
        Bowler.SetActive(true);
        //Batsmen.SetActive(true);
        mainCamera.enabled = true;
        stadiumCamera.enabled = false;

      //  batsmenCamera.enabled = false;

        LCD.gameObject.SetActive(true);

        GameCanvas.gameObject.SetActive(true);
       

        Debug.Log("GameStart");
    }


    void PauseGame()
    {
        Time.timeScale = 0;
    }

    void ResumeGame()
    {
        Time.timeScale = 1;
    }

    IEnumerator StartCount() {

       textCountdown.gameObject.SetActive(true);
        while (count > 0)
        {
            textCountdown.text = count.ToString();
            yield return new WaitForSeconds(1f);
            count--;
        }
 
        textCountdown.text = "Go!";
        yield return new WaitForSeconds(1f);
        textCountdown.gameObject.SetActive(false);
    }
    //COROUTINE 
}
