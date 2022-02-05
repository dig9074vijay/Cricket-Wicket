using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;
    public bool isStart = false;   
    public bool isGamePaused=true;
    public bool isGameSlow = false;

    public int count;
    public Text textCountdown;
    public Canvas GameOver;
    public Canvas GameCanvas;
    public Canvas HowToPlay;
    public GameObject LCD;
    public BowlController Bowler;
    public BatsmenController Batsmen;
    public bool isGameOver = true;
    [SerializeField] GameObject Message2x;
    public TextMeshProUGUI FinalScore;
    public TextMeshProUGUI FinalScoreInTable;
    bool displayed = true;
  //  public TextMeshProUGUI OtherPlayerFinalScore;
    public Camera mainCamera;
  //  public Camera batsmenCamera;
    public Camera stadiumCamera;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        //Invoke("StartGameAfterTimeline", 9.5f);
   

    }

    // Update is called once per frame
    void Update()
    {

        if (Bowler.noOfBalls > 12)
        {
            // Debug.Log("Inside if GameOVER");
            string score = Bowler.score.ToString();
            FinalScore.text = score;
            FinalScoreInTable.text = score;

            gameOver();
        }

        if (Bowler.noOfBalls > 6 && displayed)
        {
            StartCoroutine(Display2x());
            displayed = false;

        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
            //isGamePaused = !isGamePaused;
           
        }
        if(isGamePaused)
        {
            PauseGame();
        }
        else
        {
            ResumeGame();
        }

        if (isGameSlow)
        {
            SlowGame();
        }
        else if(!isGamePaused)
        {
            ResumeGameSpeed();
        }
        // Time.timeScale += 0.5f * Time.unscaledDeltaTime;
    }


    IEnumerator Display2x()
    {
        Batsmen.ScoreMultiplier = 2;
        Message2x.SetActive(true);
        yield return new WaitForSeconds(2f);
        Message2x.SetActive(false);

    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void quitGame()
    {
        Application.Quit();
    }

    public void gameOver()
    {
        GameOver.gameObject.SetActive(true);
        LCD.gameObject.SetActive(false);
        isGameOver = true;
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
        Bowler.gameObject.SetActive(true);
      //Batsmen.SetActive(true);
        mainCamera.enabled = true;
        stadiumCamera.enabled = false;
        mainCamera.gameObject.GetComponent<Animator>().Play("StartCamera");
      //batsmenCamera.enabled = false;
        Batsmen.GetComponent<Animator>().Play("BatsManIdle_01");
        LCD.gameObject.SetActive(true);

        GameCanvas.gameObject.SetActive(true);
       

        Debug.Log("GameStart");
    }


    void SlowGame()
    {
     // Time.timeScale = 0;
     // Time.fixedDeltaTime = 0.02f * Time.timeScale;
        Time.timeScale = 0.5f;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;

    }
   

    void PauseGame()
    {
        Time.timeScale = 0;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
        //Time.timeScale = 0.5f;
        //Time.fixedDeltaTime = Time.timeScale * 0.02f;

    }
    public void ResumeGame()
    {
        Time.timeScale = 1;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;

    }
    public void ResumeGameSpeed()
    {
        Time.timeScale = 1;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;

    }

    //public void SlowGame()
    //{
    //    Debug.Log("Game Slow");
    //    Time.timeScale = 0.1f;

    //    Time.fixedDeltaTime = Time.timeScale * 0.02f;
    //}

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
