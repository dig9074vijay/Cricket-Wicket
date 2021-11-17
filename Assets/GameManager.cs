using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    
        
        
    private bool isGamePaused = false;
    public int count;
    public Text textCountdown;
    public Canvas GameOver;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartCount());
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

}
