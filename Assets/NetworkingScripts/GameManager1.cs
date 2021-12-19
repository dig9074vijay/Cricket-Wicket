using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameManager1 : MonoBehaviour
{
    
    [SerializeField] Text scoreTextHolder;
    [SerializeField]Text OppenentScoreTextHolder;
    [SerializeField] Text WinnerNameText;
    [SerializeField] Text LosserNameText;
    [SerializeField] Text thisText;
    [SerializeField] Text otherText;
    [SerializeField] Text displayScoreTextAtEnd;
    [SerializeField] Text displayWinnerOrLosserText;
 //  int levelCounter = 1;
   // string currentTag;
 //   public GameObject knifeImage;
    int score;
//    [SerializeField]Text StageText;
    public bool isGameOver;
    [SerializeField] GameObject findingPlayerScreen;
    [SerializeField] GameObject StartGameOverScreen;
    
    //Networking Variables

    NetworkingPlayer otherPlayer;
    public NetworkingPlayer thisPlayer;
    public bool foundOtherPlayer = false;
    public bool canStartGame;
    public bool foundWinner;

    string myRoomId;
    public static GameManager1 instance;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        StartCoroutine(startOnlinePlay());
    }

    IEnumerator startOnlinePlay()
    {
        bool isConnected=true;
        try
        {
            string mydata = JsonUtility.ToJson(thisPlayer);
            WebRequestHandler.Instance.Post("http://localhost:3000/startRoom", mydata, (response, status) => {

                NetworkingPlayer player = JsonUtility.FromJson<NetworkingPlayer>(response);
                thisPlayer.RoomID = player.RoomID;
                myRoomId = player.RoomID;
                Debug.Log("My Room is " + myRoomId);
            });
        }
        catch {

            isConnected = false;
        }

        if (isConnected == false) yield break;

        thisPlayer.score = score;
        thisText.text = thisPlayer.playerName;




        while(foundOtherPlayer == false)
        {
            yield return new WaitForSecondsRealtime(0.5f);

            WebRequestHandler.Instance.Get("http://localhost:3000/fetchOtherPlayerData/"+myRoomId+"/"+thisPlayer.playerName, (response, status) => {

                otherPlayer = JsonUtility.FromJson<NetworkingPlayer>(response);
                Debug.Log(response);
                if (otherPlayer.playerName != null && otherPlayer.playerName != "")
                {
                    foundOtherPlayer = true;
                    otherText.text=otherPlayer.playerName;
                }
                //otherPlayer = JsonUtility.FromJson<NetworkingPlayer>(response);
   
            });
        }
        yield return new WaitForSecondsRealtime(0.5f);
        findingPlayerScreen.SetActive(false);
        canStartGame = true;
            


        //knifeHolder = GameObject.Find("KnifeHolder").transform;
        //TouchBlocker = GameObject.Find("TouchBlocker");
        //Flash = GameObject.Find("Flash");
        //Flash.SetActive(false);
        //TouchBlocker.SetActive(false);
        //TouchBlockerWhenLogIsDestroyed.SetActive(false);



        //spawnPosition = new Vector3(0, 5.03f, 0);
        //daggerSpawnPosition = new Vector3(0f, -8.65f, 0);
        //currentEasyLogs = new List<GameObject>();
        //currentHardLogs = new List<GameObject>();
        //currentBossLogs = new List<GameObject>();

        //SpawnFirstLog();
        //spawndagger();
        //StageUIManager.instance.ChangeSprite();

        while (isGameOver == false)
        {
            thisPlayer.score = score;
           yield return new WaitForSecondsRealtime(0.5f);

            WebRequestHandler.Instance.Get("http://localhost:3000/fetchscore/" + myRoomId + "/" + thisPlayer.playerName +"/" + thisPlayer.score, (response, status) => {

                otherPlayer.score = Int32.Parse(response);
                OppenentScoreTextHolder.text = response;
                Debug.Log(response);
                
                //otherPlayer = JsonUtility.FromJson<NetworkingPlayer>(response);

            });
        }

        thisPlayer.finishedPlaying = true;
        StartGameOverScreen.SetActive(true);

        while (foundWinner == false)
        {
            yield return new WaitForSecondsRealtime(0.5f);
            thisPlayer.score = score;
            WebRequestHandler.Instance.Get("http://localhost:3000/getWinner/" + myRoomId + "/" + 
                thisPlayer.playerName + "/" + thisPlayer.score + "/" + thisPlayer.finishedPlaying + "/" +
                foundWinner,
                (response, status) => {

                    if(response != "false")
                    {
                        StartGameOverScreen.transform.GetChild(1).gameObject.SetActive(false);
                        foundWinner = true;
                        NetworkingPlayer winner = JsonUtility.FromJson<NetworkingPlayer>(response);
                        if (winner.playerName == thisPlayer.playerName)
                        {
                            thisPlayer.iWon = true;
                            Debug.Log(thisPlayer.playerName + " won with score " + thisPlayer.score);
                            WinnerNameText.text = thisPlayer.playerName;
                            LosserNameText.text = otherPlayer.playerName;
                            displayWinnerOrLosserText.text = "YOU WON";
                            displayScoreTextAtEnd.text = score.ToString("SCORE :" + thisPlayer.score);

                        } 
                        else if(winner.playerName == otherPlayer.playerName)
                        {
                            otherPlayer.iWon = true;
                            otherPlayer = winner;
                            Debug.Log(otherPlayer.playerName + " won with score " + otherPlayer.score);
                            WinnerNameText.text = otherPlayer.playerName;
                            LosserNameText.text = thisPlayer.playerName;
                            displayWinnerOrLosserText.text = "YOU LOSE";
                            displayScoreTextAtEnd.text = score.ToString("SCORE :" + thisPlayer.score);
                        }

                    }
                    else
                    {
                        Debug.Log("Waiting for other player to finish playing");
                    }
    


            });
        }
    }

    //void SpawnFirstLog()
    //{
    //    if (currentEasyLogs.Count == 0)
    //    {
    //        currentEasyLogs = easyLogs;

    //    }
    //    int index = 0;
    //    currentLog = Instantiate(currentEasyLogs[index], this.transform);
    //    currentLog.transform.localPosition = spawnPosition;
    //    // = currentHinge.transform.GetChild(0).gameObject;
    //    currentLog.transform.SetParent(this.transform);
    //    currentLog.GetComponent<rotateAuto>().enabled = true;
    //    currentEasyLogs.RemoveAt(index);
    //}

    //private void SpawnEasyLogs()
    //{
    //    if (currentEasyLogs.Count == 0)
    //    {
    //        currentEasyLogs =easyLogs;

    //    }
    //    int index =UnityEngine.Random.Range(0, currentEasyLogs.Count);
    //    currentLog = Instantiate(currentEasyLogs[index],this.transform);
    //    currentLog.transform.localPosition = spawnPosition;
    //    // = currentHinge.transform.GetChild(0).gameObject;
    //    currentLog.transform.SetParent(this.transform);
    //    currentLog.GetComponent<rotateAuto>().enabled = true;
    //    currentEasyLogs.RemoveAt(index);
    //} 
    //public void SpawnHardLogs()
    //{
    //    if (currentHardLogs.Count == 0)
    //    {
    //        currentHardLogs = hardLogs;

    //    }
    //    int index = UnityEngine. Random.Range(0, currentHardLogs.Count);
    //    currentLog = Instantiate(currentHardLogs[index],this.transform);
    //    currentLog.transform.localPosition = spawnPosition;
    //    currentLog.transform.SetParent(this.transform);
    //    currentLog.GetComponent<rotateAuto>().enabled = true;
    //    //currentLog.GetComponent<Animator>().Play("Reverse");
    //    currentHardLogs.RemoveAt(index);
    //}
    //public void SpawnBossLogs()
    //{
    //    if (currentBossLogs.Count == 0)
    //    {
    //        currentBossLogs = bossLogs;

    //    }
    //    int index =UnityEngine. Random.Range(0, currentBossLogs.Count);
    //    currentLog = Instantiate(currentBossLogs[index],this.transform);
    //    currentLog.transform.localPosition = spawnPosition;
    //   // currentLog = currentHinge.transform.GetChild(0).gameObject;
    //    currentLog.transform.SetParent(this.transform);
    //    currentLog.GetComponent<rotateAuto>().enabled = true;
    //    currentBossLogs.RemoveAt(index);
    //}

    //public void spawndagger()
    //{
    //    knife = Instantiate(knifePrefab, this.transform);
    //    knife.transform.localPosition = daggerSpawnPosition;

    //}

    //public void throwKnife()
    //{
    //    if (isGameOver == true) return;

    //    knife.GetComponent<ThrowKnife>().KnifeThrow();
    //    KnifeImageUI();
    //    Debug.Log("button");
    //    knife = null;
    //    Invoke("spawndagger", 0.05f);
    //   spawndagger();

    //}

    //public void BlockTouch()
    //{
    //    TouchBlocker.SetActive(true);
    //    inGameTime = 3;
    //    StartCoroutine(StartTimeout());
    //    index--;
    //    Debug.Log(index +"index");
    //    Image knife = knifeImage.transform.GetChild(index).GetComponent<Image>();
    //    knife.fillAmount = 1;
    //    DisplayFunc("-" + points.ToString(), Color.red);
    //    score = score - points;
    //    if (score < 0)
    //    {
    //        score = 0;
    //    }
    //    scoreTextHolder.text = score.ToString();


    //}
    //IEnumerator StartTimeout()
    //{
    //    while(inGameTime != 0)
    //    {
    //        inGameTimer.text = inGameTime.ToString();
    //        yield return new WaitForSecondsRealtime(1f);
    //        inGameTime--;
    //    }
    //    UnBlockTouch();
    //}
 


    //public void UnBlockTouch()
    //{
    //    TouchBlocker.SetActive(false);
    //}

    //public void LevelFinishedStartFlash(Color32 ParticleColor)
    //{
    //    Flash.SetActive(true);
    //    breakEffect.startColor = ParticleColor;
    //    breakEffect.Play();
    //}

    //public void LevelFinishedStopFlash()
    //{

    //    Flash.SetActive(false);

    //}

    //public void LogManagerInvoker(string incomingTag)
    //{
    //    currentTag = incomingTag;
    //    Invoke("LogManager", 1.2f);
    //}

    //public void LogManager()
    //{
    //    StageText.text= "STAGE"+(levelCounter+1);
    //    levelCounter++;
    //    points += 5;
    //    Debug.Log(currentTag);
    //    if (currentTag == "boss")
    //    {
    //        SpawnEasyLogs();
    //        Debug.Log("Spawning easy Stage");
    //    }
    //    else
    //    {
    //        if(levelCounter%4==0)
    //        {
    //            Invoke("SpawnBossLogs",1f);
    //            StageText.text="BOSS STAGE";
    //             Debug.Log("Spawning Boss Stage");
    //        }
    //        else
    //        {
    //            SpawnHardLogs();
    //             Debug.Log("Spawning Hard Stage");
    //        }
    //    }

    //    index = 0;
    //    foreach(Transform child in knifeImage.transform)
    //    {
    //        child.GetComponent<Image>().fillAmount = 1;
    //    }
        
    //    StageUIManager.instance.ChangeSprite();
    //    Invoke("SetTouchBlockInActive",0.5f);
    //}

    //void SetTouchBlockInActive(){

    //    TouchBlockerWhenLogIsDestroyed.SetActive(false);
    //}

    // int index=0;
    //public void KnifeImageUI()
    //{
    //    if (index > knifeImage.transform.childCount - 1) return;

    //        Image knife = knifeImage.transform.GetChild(index).GetComponent<Image>();
        
    //        knife.fillAmount = 0;
    //        index++;
    //        Debug.Log("the current images is" + knife);
    //}

 

    //void DisplayFunc(string pn, Color cl)
    //{
    //    GameObject tempPoints = Instantiate(pointsPrefab, canvas);
    //    tempPoints.GetComponent<PointManager>().DisplayPoints(pn, cl);

    //}

    //public void LogTookDamage()
    //{
    //    DisplayFunc("+" + points.ToString(), Color.white);
    //    score = score + points;
    //    Debug.Log("score is" + score);
    //    scoreTextHolder.text = score.ToString();
    //}
    public void CancelApplication()
    {
        Application.Quit();
    }

    public void ReloadScene()
    {
        Debug.Log("ChangeScene");
        SceneManager.LoadScene(0);
    }

   public void startReloadGameTime()
    {
   //     StartCoroutine(EndGameTimeFill());
    }

    //IEnumerator EndGameTimeFill()
    //{
    //    while (true)
    //    {
    //        endGameTimer.fillAmount += 0.01f;
    //        yield return new WaitForSecondsRealtime(0.2f);
    //        if (endGameTimer.fillAmount == 1)
    //        {
    //            SceneManager.LoadScene(0);
    //        }
    //    }
    //}
}
