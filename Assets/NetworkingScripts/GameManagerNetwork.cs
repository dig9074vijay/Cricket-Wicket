using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManagerNetwork : MonoBehaviour
{
    

    [SerializeField] GameObject Timeline;
    [SerializeField] GameObject HowToPlayCanvas;
    [SerializeField] GameObject Matchmaking;
    //  [SerializeField] GameObject TableRow2;



    [SerializeField] Image endGameTimer;

   // [SerializeField] Text scoreTextHolder;
     public Text OpponentScoreTextHolder;
    [SerializeField] TextMeshProUGUI WinnerNameText;
    [SerializeField] TextMeshProUGUI scoreOnScoreBoard;
    [SerializeField] TextMeshProUGUI opponentScoreOnScoreBoard;
    [SerializeField] TextMeshProUGUI LosserNameText;
    [SerializeField] TextMeshProUGUI thisText;
    [SerializeField] TextMeshProUGUI otherText;
    [SerializeField] TextMeshProUGUI displayScoreTextAtEnd;
    [SerializeField] TextMeshProUGUI displayWinnerOrLosserText;
    //int levelCounter = 1;
    //string currentTag;
    // public GameObject knifeImage;
    public bool isDataSend = false;
    int score;
    [SerializeField]TextMeshProUGUI StageText;
    public bool isGameOver;
    [SerializeField] GameObject findingPlayerScreen;
    [SerializeField] GameObject StartGameOverScreen;
    [SerializeField] GameManager gameManager;
    //Networking Variables

    public NetworkingPlayer otherPlayer;
    //public string serverURL = "http://localhost:3000/";
    public string serverURL = "https://gamejoyproserver1v1.herokuapp.com/";
    //  public string serverURL = "http://3.109.122.170:4000/";

    public string sendDataURL = "http://52.66.182.199/api/gameplay";
    public SendData1 sendThisPlayerData;
    public WinningDetails winningDetails;
    string sendWinningDetailsData;
    string sendNewData1;
    public NetworkingPlayer thisPlayer;
    public bool foundOtherPlayer = false;
    public bool canStartGame;
    public bool foundWinner;
    string myRoomId;
    public static GameManagerNetwork instance;
    public BaseAppData baseAppData;
    [SerializeField] Image thisPlayerImage;
    [SerializeField] Image otherPlayerImage;
    private void Awake()
    {
        instance = this;
        thisText.text = baseAppData.user_name;
        thisPlayer.playerName = baseAppData.user_name;
        //thisPlayer.playerId = baseAppData.androidJsonData.player_id;



    }
    private void Start()
    {
        //thisText.text = baseAppData.jsonString;
        StartCoroutine(startOnlinePlay());
    }

    private void Update()
    {
        
    }

    public void startReloadGameTime()
    {
        StartCoroutine(EndGameTimeFill());
    }

    IEnumerator EndGameTimeFill()
    {
        while (true)
        {
            endGameTimer.fillAmount += 0.01f;
            yield return new WaitForSecondsRealtime(0.2f);
            if (endGameTimer.fillAmount == 1)
            {
                SceneManager.LoadScene(0);

            }
        }
    }

    IEnumerator startOnlinePlay()
    {
        bool isConnected=true;
        try
        {
            thisPlayer.imageURL = baseAppData.profile_image;
            WebRequestHandler.Instance.DownloadSprite(thisPlayer.imageURL, (sprite) =>
            {
           //     Debug.Log("hitthis");
                thisPlayerImage.sprite = sprite;
            });
            string mydata = JsonUtility.ToJson(thisPlayer);
            WebRequestHandler.Instance.Post(serverURL + "startRoom", mydata, (response, status) => {

                NetworkingPlayer player = JsonUtility.FromJson<NetworkingPlayer>(response);
                thisPlayer.RoomID = player.RoomID;
                thisPlayer.playerId = player.playerId;
                thisPlayer.playerName = player.playerName;
                myRoomId = player.RoomID;
                Debug.Log("My Room is " + thisPlayer.RoomID);
            });
        }
        catch {

            isConnected = false;
        }

        if (isConnected == false) yield break;

        thisPlayer.score = gameManager.Bowler.score;
        thisText.text = thisPlayer.playerName;




        while(foundOtherPlayer == false)
        {
            yield return new WaitForSecondsRealtime(0.5f);
         //   scoreTextHolder.text = gameManager.Bowler.score.ToString();
            WebRequestHandler.Instance.Get(serverURL + "fetchOtherPlayerData/" +myRoomId+"/"+thisPlayer.playerId, (response, status) => {

                otherPlayer = JsonUtility.FromJson<NetworkingPlayer>(response);
                Debug.Log(response);
                if (otherPlayer.playerId != null && otherPlayer.playerId != "")
                {
                    foundOtherPlayer = true;
                   
                    otherText.text=otherPlayer.playerName;
                    if(otherPlayer.isBot)
                    {
                        otherPlayer.imageURL = "https://picsum.photos/400";
                    }
                    //else
                    //{
                    //    otherText.text = "RoboGUEST";
                    //}
                    WebRequestHandler.Instance.DownloadSprite(otherPlayer.imageURL, (sprite) =>
                    {
                        otherPlayerImage.sprite = sprite;
                    });
                }
                //otherPlayer = JsonUtility.FromJson<NetworkingPlayer>(response);
   
            });
        }

        yield return new WaitForSecondsRealtime(0.5f);



        HowToPlayCanvas.SetActive(true);
        Matchmaking.SetActive(false);
        gameManager.gameObject.SetActive(true);
        Timeline.SetActive(true);




        canStartGame = true;
            


       



        

        //SpawnFirstLog();
        //spawndagger();
       // StageUIManager.instance.ChangeSprite();

        while (gameManager.isGameOver == false)
        {
            thisPlayer.score = gameManager.Bowler.score;
           yield return new WaitForSecondsRealtime(0.5f);

            WebRequestHandler.Instance.Get(serverURL + "fetchscore/" + myRoomId + "/" + thisPlayer.playerId +"/"+ otherPlayer.playerId + "/" + thisPlayer.score + "/" + thisPlayer.incrementFactor, (response, status) => {

                otherPlayer.score = Int32.Parse(response);
                OpponentScoreTextHolder.text = response;
                winningDetails.thisplayerScore = gameManager.Bowler.score;

                Debug.Log(response);
                
                //otherPlayer = JsonUtility.FromJson<NetworkingPlayer>(response);

            });
        }

        thisPlayer.finishedPlaying = true;
     //   StartGameOverScreen.SetActive(true);

        while (foundWinner == false)
        {
            yield return new WaitForSecondsRealtime(0.5f);
            thisPlayer.score = gameManager.Bowler.score;
            //if(otherPlayer.isBot)
            //otherPlayer.score = BatsmenController.instance.totalBotScore;

            WebRequestHandler.Instance.Get(serverURL + "getWinner/" + myRoomId + "/" + 
                thisPlayer.playerId + "/" + otherPlayer.playerId + "/" + thisPlayer.score + "/" + thisPlayer.finishedPlaying + "/" +
                foundWinner,
                (response, status) => {

                    if(response != "false" && foundWinner==false)
                    {
                      //  TableRow2.SetActive(false);
                        foundWinner = true;
                        NetworkingPlayer winner = JsonUtility.FromJson<NetworkingPlayer>(response);
                        if (winner.playerId == thisPlayer.playerId)
                        {
                            thisPlayer.iWon = true;
                            Debug.Log(thisPlayer.playerName + " won with score " + thisPlayer.score);
                            WinnerNameText.text = thisPlayer.playerName;
                            LosserNameText.text = otherPlayer.playerName;
                            StageText.text = "1";
                          //  displayWinnerOrLosserText.text = "YOU WON";
                           // displayScoreTextAtEnd.text = thisPlayer.score.ToString();
                            scoreOnScoreBoard.text = gameManager.Bowler.score.ToString();
                            opponentScoreOnScoreBoard.text = otherPlayer.score.ToString();

                        } 
                        else if(winner.playerId == otherPlayer.playerId)
                        {
                            otherPlayer.iWon = true;
                            otherPlayer = winner;
                            Debug.Log(otherPlayer.playerName + " won with score " + otherPlayer.score);
                            WinnerNameText.text = otherPlayer.playerName;
                            LosserNameText.text = thisPlayer.playerName;
                            StageText.text = "2";

                            //  displayWinnerOrLosserText.text = "YOU LOSE";
                            //   displayScoreTextAtEnd.text = thisPlayer.score.ToString();
                            scoreOnScoreBoard.text = otherPlayer.score.ToString();
                            opponentScoreOnScoreBoard.text = gameManager.Bowler.score.ToString();
                        }

                        startReloadGameTime();
                        // StartCoroutine(CallLeaveRoom());
                        sendWinningDetailsData = JsonUtility.ToJson(winningDetails);
                        sendNewData1 = JsonUtility.ToJson(sendThisPlayerData);
                        //Debug.Log(sendNewData+"sendNewData");
                        WebRequestHandler.Instance.Post(sendDataURL, sendNewData1, (response, status) =>
                        {
                            Debug.Log(response + "HitNewApi");
                        });

                        WebRequestHandler.Instance.Post(sendDataURL, sendWinningDetailsData, (response, status) =>
                        {
                            Debug.Log(response + "For Winning Details");
                        });

                        isDataSend = true;
                    }
                    else
                    {
                        Debug.Log("Waiting for other player to finish playing");
                    }
    


            });
        }
    }

    //IEnumerator CallLeaveRoom()
    //{
    //    yield return new WaitForSecondsRealtime(0.5f);
    //    LeaveRoom();
    //}

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
    ////    currentLog.GetComponent<rotateAuto>().enabled = true;
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
    // //   currentLog.GetComponent<rotateAuto>().enabled = true;
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
    ////    currentLog.GetComponent<rotateAuto>().enabled = true;
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
    //  //  currentLog.GetComponent<rotateAuto>().enabled = true;
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

    //  //  knife.GetComponent<ThrowKnife>().KnifeThrow();
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
    ////    breakEffect.startColor = ParticleColor;
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
        
    // //   StageUIManager.instance.ChangeSprite();
    //    Invoke("SetTouchBlockInActive",0.5f);
    //}

    //void SetTouchBlockInActive(){

    //    TouchBlockerWhenLogIsDestroyed.SetActive(false);
    //}

    // int index=0;
    //private bool hasLeftRoom;

    //public void KnifeImageUI()
    //{
    //    if (index > knifeImage.transform.childCount - 1) return;

    //        Image knife = knifeImage.transform.GetChild(index).GetComponent<Image>();
        
    //        knife.fillAmount = 0;
    //        index++;
    //        Debug.Log("the current images is" + knife);
    //}

 

  //  void DisplayFunc(string pn, Color cl)
  //  {
  //      GameObject tempPoints = Instantiate(pointsPrefab, canvas);
  ////      tempPoints.GetComponent<PointManager>().DisplayPoints(pn, cl);

  //  }

  //  public void LogTookDamage()
  //  {
  //      DisplayFunc("+" + points.ToString(), Color.white);
  //      score = score + points;
  //      Debug.Log("score is" + score);
  //      scoreTextHolder.text = score.ToString();
  //  }
  //  public void CancelApplication()
  //  {
  //      //hasLeftRoom = true;
  //      Application.Quit();
  //  }

  //  public void ReloadScene()
  //  {
  //      Debug.Log("ChangeScene");
  //      SceneManager.LoadScene(0);
        
  //  }

   //public void startReloadGameTime()
   // {
   //     StartCoroutine(EndGameTimeFill());
   // }

   // IEnumerator EndGameTimeFill()
   // {
   //     while (true)
   //     {
   //         endGameTimer.fillAmount += 0.01f;
   //         yield return new WaitForSecondsRealtime(0.2f);
   //         if (endGameTimer.fillAmount == 1)
   //         {
   //             SceneManager.LoadScene(0);
                
   //         }
   //     }
   // }

    //private void OnDestroy()
    //{
    //    if (hasLeftRoom == false)
    //    {
    //        LeaveRoom();
    //    }
    //}

    //public void LeaveRoom()
    //{
    //    Debug.Log("leaving Room");
    //    WebRequestHandler.Instance.Get("http://localhost:3000/removeFromRooms/" + myRoomId + "/" + thisPlayer.playerName, (response, status) => {
    //        thisPlayer.RoomID = "";
    //        myRoomId = "";
    //        //otherPlayer = JsonUtility.FromJson<NetworkingPlayer>(response);
    //        //Debug.Log(response);
    //        //if (otherPlayer.playerName != null && otherPlayer.playerName != "")
    //        //{
    //        //    foundOtherPlayer = true;
    //        //    otherText.text = otherPlayer.playerName;
    //        //}
    //        //otherPlayer = JsonUtility.FromJson<NetworkingPlayer>(response);

    //    });

    //}
}
