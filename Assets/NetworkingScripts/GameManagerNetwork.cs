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


    //game over screen
    [SerializeField] Image endGameTimer;
     public Text OpponentScoreTextHolder;
    [SerializeField] TextMeshProUGUI WinnerNameText;
    [SerializeField] TextMeshProUGUI scoreOnScoreBoard;
    [SerializeField] TextMeshProUGUI opponentScoreOnScoreBoard;
    [SerializeField] TextMeshProUGUI LosserNameText;
    [SerializeField] TextMeshProUGUI thisText;
    [SerializeField] TextMeshProUGUI otherText;
    [SerializeField] TextMeshProUGUI displayScoreTextAtEnd;
    [SerializeField] TextMeshProUGUI displayWinnerOrLosserText;


    [SerializeField] TextMeshProUGUI ChanceLeft;

    public bool isDataSend = false;
    int score;
    [SerializeField]TextMeshProUGUI StageText;
    public bool isGameOver;
    [SerializeField] GameObject findingPlayerScreen;
    [SerializeField] GameObject StartGameOverScreen;
    [SerializeField] GameManager gameManager;


    [SerializeField] GameObject ReplayBtn;
    //Networking Variables

    //public NetworkingPlayer otherPlayer;
    ////public string serverURL = "http://localhost:3000/";
    //public string serverURL = "https://gamejoyproserver1v1.herokuapp.com/";
    ////  public string serverURL = "http://3.109.122.170:4000/";
    [SerializeField] public GameObject matchmaking;
    //public string sendDataURL = "http://52.66.182.199/api/gameplay";
    //public SendData1 sendThisPlayerData;
    //public WinningDetails winningDetails;
    //string sendWinningDetailsData;
    //string sendNewData1;
    //public NetworkingPlayer thisPlayer;
    //public bool foundOtherPlayer = false;
    //public bool canStartGame;
    public bool foundWinner;
   
    string myRoomId;
    public static GameManagerNetwork instance;


    public bool canRestart = false;
    public bool LocalConnect = false;
    public string serverURL = "https://gameserver1v1.herokuapp.com/";
    public string sendDataURL = "http://52.66.182.199/api/gameplay";
    public string walletInfoURL = "http://52.66.182.199/api/wallet";
    public string walletUpdateURL = "http://52.66.182.199/api/walletdeduction";
    public string getTournAttemptURL = "https://admin.gamejoypro.com/api/getattempts";
    public NetworkingPlayer thisPlayer;
    public NetworkingPlayer otherPlayer;
    public SendData1 sendThisPlayerData;
    public WinningDetails winning_details;
    public WalletInfo walletInfo;
    public WallUpdate walletUpdate;
    public bool foundOtherPlayer = false;
    public bool canStartGame;
    public string sendWinningDetailsData;
    public string sendNewData1;
    private string walletInfoData;
    private string walletUpdateData;


    //matchmaking Variable
    [SerializeField] Image thisPlayerImage;
    [SerializeField] Image otherPlayerImage;


    [SerializeField] Image thisPlayerImageTopStrip;
    [SerializeField] Image otherPlayerImageTopStrip;

    [SerializeField] Image thisPlayerImageEnd;
    [SerializeField] Image otherPlayerImageEnd;




  //  [SerializeField] Text OppNo;

    private bool isReEntryPaid;
    private bool isSingleEntry;


    [SerializeField] Text PlayerNameText;
    [SerializeField] Text OpponentPlayerNameText;




    [SerializeField] GameObject NoBalPop;
    [SerializeField] GameObject P2;
    [SerializeField] GameObject Footer_1;


    [SerializeField] TextMeshProUGUI ReloadPrice;
//    [SerializeField] public Image goScreen;

    private void Awake()
    {
        instance = this;
        thisText.text = AndroidtoUnityJSON.instance.user_name;
        thisPlayer.playerName = AndroidtoUnityJSON.instance.user_name;
        PlayerNameText.text = thisPlayer.playerName;
        if (LocalConnect)
        {
            serverURL = "http://localhost:4000/";
        }


    }
    private void Start()
    {

        Debug.Log("android data => " +
            AndroidtoUnityJSON.instance.player_id + ", " + AndroidtoUnityJSON.instance.token + ", " + AndroidtoUnityJSON.instance.user_name + ", " +
            AndroidtoUnityJSON.instance.game_id + ", " + AndroidtoUnityJSON.instance.profile_image + ", " + AndroidtoUnityJSON.instance.game_fee + ", " +
            AndroidtoUnityJSON.instance.game_mode + ", " + AndroidtoUnityJSON.instance.battle_id + ", " + AndroidtoUnityJSON.instance.tour_id + ", " +
            AndroidtoUnityJSON.instance.tour_mode + ", " + AndroidtoUnityJSON.instance.tour_name + ", " + AndroidtoUnityJSON.instance.no_of_attempts + ", " +
            AndroidtoUnityJSON.instance.mm_player + ", " + AndroidtoUnityJSON.instance.entry_type);
        thisPlayer.gameName = thisPlayer.gameName + AndroidtoUnityJSON.instance.game_fee;
        if (AndroidtoUnityJSON.instance.game_mode == "tour")
        {
            if (AndroidtoUnityJSON.instance.mm_player == "2")
            {
                //VS.SetActive(true);
                P2.SetActive(true);

                if (AndroidtoUnityJSON.instance.entry_type == "re entry paid")
                {
                    isReEntryPaid = true;
                }
                else if (AndroidtoUnityJSON.instance.entry_type == "re entry")
                {
                    isReEntryPaid = false;
                }
                else if (AndroidtoUnityJSON.instance.entry_type == "single entry")
                {
                    isSingleEntry = true;
                }

                StartCoroutine(StartOnlinePlay());
            }
            else if (AndroidtoUnityJSON.instance.mm_player == "1")
            {
                //VS.SetActive(false);
                P2.SetActive(false);
                HowToPlayCanvas.SetActive(true);
                Matchmaking.SetActive(false);
                gameManager.gameObject.SetActive(true);
                Timeline.SetActive(true);
                if (AndroidtoUnityJSON.instance.entry_type == "re entry paid")
                {
                    isReEntryPaid = true;
                }
                else if (AndroidtoUnityJSON.instance.entry_type == "re entry")
                {
                    isReEntryPaid = false;
                }
                else if (AndroidtoUnityJSON.instance.entry_type == "single entry")
                {
                    isSingleEntry = true;
                }

                StartCoroutine(StartOfflinePlay());
            }
        }
        else if (AndroidtoUnityJSON.instance.game_mode == "battle")
        {
            StartCoroutine(StartOnlinePlay());
        }
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
                ReloadScene();

            }
        }
    }

    IEnumerator StartOnlinePlay()
    {
        





            ///-----------



            thisPlayer.playerName = AndroidtoUnityJSON.instance.user_name;
            thisPlayer.imageURL = AndroidtoUnityJSON.instance.profile_image;

            WebRequestHandler.Instance.DownloadSprite(thisPlayer.imageURL, (sprite) => { thisPlayerImage.sprite = sprite; thisPlayerImageTopStrip.sprite = sprite; });
            //WebRequestHandler.Instance.DownloadSprite(thisPlayer.imageURL, (sprite) => { playerGameImg.sprite = sprite; });



            string mydata = JsonUtility.ToJson(thisPlayer);
            WebRequestHandler.Instance.Post(serverURL + "startRoom", mydata, (response, status) => {

                NetworkingPlayer player = JsonUtility.FromJson<NetworkingPlayer>(response);
                thisPlayer.RoomID = player.RoomID;
                thisPlayer.playerId = player.playerId;
                sendThisPlayerData.player_id = AndroidtoUnityJSON.instance.player_id;
                thisPlayer.playerName = player.playerName;
                myRoomId = player.RoomID;
                sendThisPlayerData.player_id = thisPlayer.playerIdToBeSentorReceived;
                sendThisPlayerData.room_id = myRoomId;
                Debug.Log("My Room is " + thisPlayer.RoomID);
            });

            thisPlayer.score = gameManager.Bowler.score;
            thisText.text = thisPlayer.playerName;


            while (foundOtherPlayer == false)
            {
                yield return new WaitForSecondsRealtime(0.5f);

                WebRequestHandler.Instance.Get(serverURL + "fetchOtherPlayerData/" + myRoomId + "/" + thisPlayer.playerId, (response, status) => {

                    otherPlayer = JsonUtility.FromJson<NetworkingPlayer>(response);
                    //Debug.Log(response);
                    if (otherPlayer.playerId != null && otherPlayer.playerId != "")
                    {
                        foundOtherPlayer = true;
                        otherText.text = otherPlayer.playerName;

                        if (otherPlayer.playerName.Contains("Guest"))
                        {
                            otherPlayer.imageURL = "https://picsum.photos/400";
                        }

                        //WebRequestHandler.Instance.DownloadSprite(otherPlayer.imageURL, (sprite) => { thisPlayerImage.sprite = sprite; });
                        //WebRequestHandler.Instance.DownloadSprite(otherPlayer.imageURL, (sprite) => { oppGameImg.sprite = sprite; });
                        WebRequestHandler.Instance.DownloadSprite(otherPlayer.imageURL, (sprite) =>
                        {
                            otherPlayerImage.sprite = sprite;
                            otherPlayerImageTopStrip.sprite = sprite;
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


        //goScreen.gameObject.SetActive(true);

        canStartGame = true;



        PlayerNameText.text = thisPlayer.playerName;
        OpponentPlayerNameText.text = otherPlayer.playerName;



        var attemptNo = 0;

   

        //attempt check
        WebRequestHandler.Instance.Get(getTournAttemptURL + "/" + AndroidtoUnityJSON.instance.tour_id, (response, status) =>
        {
            GetAttempt attempt = JsonUtility.FromJson<GetAttempt>(response);

            Debug.Log("Attempt(s) remaining: " + attempt.data.remainAttemp);

            attemptNo = int.Parse(attempt.data.remainAttemp);

            if (attempt.data.remainAttemp == AndroidtoUnityJSON.instance.no_of_attempts)
            {
                AndroidtoUnityJSON.instance.isFirst = true;
            }
            else
            {
                AndroidtoUnityJSON.instance.isFirst = false;
            }
        });

        if (AndroidtoUnityJSON.instance.game_mode == "tour")
        {
            if (AndroidtoUnityJSON.instance.entry_type == "re entry" && AndroidtoUnityJSON.instance.isFirst ||
                AndroidtoUnityJSON.instance.entry_type == "re entry paid" || AndroidtoUnityJSON.instance.entry_type == "single entry")
            {
                DeductWallet();
            }
        }
        else
        {
            DeductWallet();
        }

        while (gameManager.isGameOver == false)
        {
            thisPlayer.score = gameManager.Bowler.score;
           yield return new WaitForSecondsRealtime(0.5f);

            WebRequestHandler.Instance.Get(serverURL + "fetchscore/" + myRoomId + "/" + thisPlayer.playerId +"/"+ otherPlayer.playerId + "/" + thisPlayer.score + "/" + thisPlayer.incrementFactor, (response, status) => {

                otherPlayer.score = Int32.Parse(response);
                OpponentScoreTextHolder.text = response;
                //winningDetails.thisplayerScore = gameManager.Bowler.score;

                //Debug.Log(response);
                
                //otherPlayer = JsonUtility.FromJson<NetworkingPlayer>(response);

            });
        }

        thisPlayer.finishedPlaying = true;
        //   StartGameOverScreen.SetActive(true);



        if (AndroidtoUnityJSON.instance.game_mode == "tour" && AndroidtoUnityJSON.instance.entry_type == "single entry")
        {
            ChanceLeft.gameObject.SetActive(false);
            ReplayBtn.transform.parent.gameObject.SetActive(false);
            Footer_1.SetActive(true);
        }
        else if (AndroidtoUnityJSON.instance.game_mode == "tour")
        {
            if (attemptNo <= 0)
            {
                ReplayBtn.transform.parent.gameObject.SetActive(false);
                Footer_1.SetActive(true);
            }

            ChanceLeft.gameObject.SetActive(true);
            ChanceLeft.text = "Chances Left: " + attemptNo;
        }
        else if (AndroidtoUnityJSON.instance.game_mode == "battle")
        {
            ChanceLeft.gameObject.SetActive(false);
        }



        if (AndroidtoUnityJSON.instance.mm_player == "1")
        {
            LosserNameText.transform.parent.gameObject.SetActive(false);
        }
        else
        {
            ReloadPrice.text = /* "?" +*/ AndroidtoUnityJSON.instance.game_fee;
        }



        if (float.Parse(AndroidtoUnityJSON.instance.game_fee) <= 0 || AndroidtoUnityJSON.instance.entry_type == "re entry" && AndroidtoUnityJSON.instance.game_mode == "tour")
        {
            ReloadPrice.text = "FREE";
        }

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
                        startReloadGameTime();
                        NetworkingPlayer winner = JsonUtility.FromJson<NetworkingPlayer>(response);
                        if (winner.playerId == thisPlayer.playerId)
                        {
                            thisPlayer.iWon = true;

                            sendThisPlayerData.winning_details.winningPlayerScore = gameManager.Bowler.score.ToString();
                            sendThisPlayerData.winning_details.winningPlayerID = thisPlayer.playerId;
                            sendThisPlayerData.winning_details.lossingPlayerID = otherPlayer.playerId;
                            sendThisPlayerData.winning_details.lossingPlayerScore = otherPlayer.score.ToString();

                            Debug.Log(thisPlayer.playerName + " won with score " + thisPlayer.score);
                            WinnerNameText.text = thisPlayer.playerName;
                            LosserNameText.text = otherPlayer.playerName;
                            thisPlayerImageEnd.sprite = thisPlayerImage.sprite;
                            otherPlayerImageEnd.sprite = otherPlayerImage.sprite;
                            StageText.text = "1";
                            sendThisPlayerData.game_status = "WIN";
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

                            sendThisPlayerData.winning_details.winningPlayerScore = otherPlayer.score.ToString();
                            sendThisPlayerData.winning_details.winningPlayerID = otherPlayer.playerId;
                            sendThisPlayerData.winning_details.lossingPlayerScore = gameManager.Bowler.score.ToString();
                            sendThisPlayerData.winning_details.lossingPlayerID = thisPlayer.playerId;

                            WinnerNameText.text = otherPlayer.playerName;
                            LosserNameText.text = thisPlayer.playerName;
                            StageText.text = "2";
                            sendThisPlayerData.game_status = "LOST";
                            thisPlayerImageEnd.sprite = otherPlayerImage.sprite;
                            otherPlayerImageEnd.sprite = thisPlayerImage.sprite;

                            //  displayWinnerOrLosserText.text = "YOU LOSE";
                            //   displayScoreTextAtEnd.text = thisPlayer.score.ToString();
                            scoreOnScoreBoard.text = otherPlayer.score.ToString();
                            opponentScoreOnScoreBoard.text = gameManager.Bowler.score.ToString();
                        }

                        sendThisPlayerData.player_id = otherPlayer.playerId;
                        sendThisPlayerData.winning_details.thisplayerScore = gameManager.Bowler.score;
                        sendThisPlayerData.wallet_amt = AndroidtoUnityJSON.instance.game_fee.ToString();
                        sendThisPlayerData.game_mode = AndroidtoUnityJSON.instance.game_mode;
                        sendThisPlayerData.game_id = AndroidtoUnityJSON.instance.game_id;

                        if (AndroidtoUnityJSON.instance.game_mode == "tour")
                            sendThisPlayerData.battle_tournament_id = AndroidtoUnityJSON.instance.tour_id;
                        else if (AndroidtoUnityJSON.instance.game_mode == "battle")
                            sendThisPlayerData.battle_tournament_id = AndroidtoUnityJSON.instance.battle_id;



                        sendThisPlayerData.game_end_time = GetSystemTime();
//sendThisPlayerData.game_status = "FINISHED";

                        string sendWinningDetailsData = JsonUtility.ToJson(winning_details);
                        string sendNewData = JsonUtility.ToJson(sendThisPlayerData);

                        Debug.Log(sendNewData + " <= sendNewData");
                        WebRequestHandler.Instance.Post(sendDataURL, sendNewData, (response, status) =>
                        {
                            Debug.Log(response + "HitNewApi");
                        });

                        isDataSend = true;
                        // StartCoroutine(CallLeaveRoom());
                    }
                    else
                    {
                        // Debug.Log("Waiting for other player to finish playing");
                    }

                });
        }
        //sendThisPlayerData.player_id = thisPlayer.playerId;
        //sendThisPlayerData.room_id = thisPlayer.RoomID;

    }



    IEnumerator StartOfflinePlay() //SERVERLESS GAME 
    {
        thisPlayer.playerName = AndroidtoUnityJSON.instance.user_name;
        thisPlayer.imageURL = AndroidtoUnityJSON.instance.profile_image;

        //OpponentText.SetActive(false);
        //oppGameImg.enabled = false;
        //OppenentScoreTextHolder.enabled = true;

        WebRequestHandler.Instance.DownloadSprite(thisPlayer.imageURL, (sprite) =>
        {

            thisPlayerImage.sprite = sprite; thisPlayerImageTopStrip.sprite = sprite;

        });

        //thisPlayer.score = int.Parse(score.text);
        thisText.text = thisPlayer.playerName; //matchamking player name
        //PlayerNameTextHolder.text = thisPlayer.playerName;

       

      //  goScreen.gameObject.SetActive(true);

        canStartGame = true;
        foundOtherPlayer = true;
        //Debug.Log("android data => " + AndroidtoUnityJSON.instance.player_id + ", " + AndroidtoUnityJSON.instance.token + ", " + AndroidtoUnityJSON.instance.user_name + ", " +
        //    AndroidtoUnityJSON.instance.game_id + ", " + AndroidtoUnityJSON.instance.profile_image + ", " + AndroidtoUnityJSON.instance.game_fee + ", " +
        //    AndroidtoUnityJSON.instance.game_mode + ", " + AndroidtoUnityJSON.instance.battle_id);

        PlayerNameText.text = thisPlayer.playerName;

        var attemptNo = 0;

        //attempt check
        WebRequestHandler.Instance.Get(getTournAttemptURL + "/" + AndroidtoUnityJSON.instance.tour_id, (response, status) =>
        {
            GetAttempt attempt = JsonUtility.FromJson<GetAttempt>(response);

            Debug.Log("Attempt(s) remaining: " + attempt.data.remainAttemp);

            attemptNo = int.Parse(attempt.data.remainAttemp);

            if (attempt.data.remainAttemp == AndroidtoUnityJSON.instance.no_of_attempts)
            {
                AndroidtoUnityJSON.instance.isFirst = true;
            }
            else
            {
                AndroidtoUnityJSON.instance.isFirst = false;
            }
        });

        if (AndroidtoUnityJSON.instance.game_mode == "tour")
        {
            if (AndroidtoUnityJSON.instance.entry_type == "re entry" && AndroidtoUnityJSON.instance.isFirst ||
                AndroidtoUnityJSON.instance.entry_type == "re entry paid" || AndroidtoUnityJSON.instance.entry_type == "single entry")
            {
                DeductWallet();
            }
        }
        else
        {
            DeductWallet();
        }

        yield return new WaitUntil(() => gameManager.isGameOver);

        thisPlayer.finishedPlaying = true;


        if (AndroidtoUnityJSON.instance.game_mode == "tour" && AndroidtoUnityJSON.instance.entry_type == "single entry")
        {
            ChanceLeft.gameObject.SetActive(false);
            ReplayBtn.transform.parent.gameObject.SetActive(false);
            Footer_1.SetActive(true);
        }
        else if (AndroidtoUnityJSON.instance.game_mode == "tour")
        {
            if (attemptNo <= 0)
            {
                ReplayBtn.transform.parent.gameObject.SetActive(false);
                Footer_1.SetActive(true);
            }

            ChanceLeft.gameObject.SetActive(true);
            ChanceLeft.text = "Chances Left: " + attemptNo;
        }
        else if (AndroidtoUnityJSON.instance.game_mode == "battle")
        {
            ChanceLeft.gameObject.SetActive(false);
        }



        if (AndroidtoUnityJSON.instance.mm_player == "1")
        {
            LosserNameText.transform.parent.gameObject.SetActive(false);
        }



        if (float.Parse(AndroidtoUnityJSON.instance.game_fee) <= 0 || AndroidtoUnityJSON.instance.entry_type == "re entry" && AndroidtoUnityJSON.instance.game_mode == "tour")
        {
            ReloadPrice.text = "FREE";
        }
        else
        {
            ReloadPrice.text = /* "?" +*/ AndroidtoUnityJSON.instance.game_fee;
        }




        while (foundWinner == false)
        {
            // Debug.Log("entered");
            yield return new WaitForSecondsRealtime(0.5f);
            thisPlayer.score = gameManager.Bowler.score; 

            if (foundWinner == false)
            {
                foundWinner = true;
                thisPlayer.iWon = true;
             //   sendThisPlayerData.player_id = AndroidtoUnityJSON.instance.player_id;
                sendThisPlayerData.room_id = "0";

                sendThisPlayerData.winning_details.winningPlayerScore = gameManager.Bowler.score.ToString();
                sendThisPlayerData.winning_details.winningPlayerID = AndroidtoUnityJSON.instance.player_id;
                sendThisPlayerData.winning_details.lossingPlayerID = "0";
                sendThisPlayerData.winning_details.lossingPlayerScore = "0";
                WinnerNameText.text = thisPlayer.playerName;
                //LosserNameText.text = otherPlayer.playerName;
             //   displayWinnerOrLosserText.text = "YOU SCORE";
            //    displayScoreTextAtEnd.text = thisPlayer.score.ToString();
               // scoreOnScoreBoard.text = gameManager.Bowler.score.ToString();
                scoreOnScoreBoard.text = thisPlayer.score.ToString();
                //opponentScoreOnScoreBoard.text = otherPlayer.score.ToString();

                StageText.text = "1";
                sendThisPlayerData.game_status = "WIN";

                //send tournament data through api


                sendThisPlayerData.player_id = "0";
                sendThisPlayerData.winning_details.thisplayerScore = gameManager.Bowler.score;
                sendThisPlayerData.wallet_amt = AndroidtoUnityJSON.instance.game_fee;
                sendThisPlayerData.game_mode = AndroidtoUnityJSON.instance.game_mode;
                sendThisPlayerData.game_id = AndroidtoUnityJSON.instance.game_id;

                if (AndroidtoUnityJSON.instance.game_mode == "tour")
                    sendThisPlayerData.battle_tournament_id = AndroidtoUnityJSON.instance.tour_id;
                else if (AndroidtoUnityJSON.instance.game_mode == "battle")
                    sendThisPlayerData.battle_tournament_id = AndroidtoUnityJSON.instance.battle_id;


                sendThisPlayerData.game_end_time = GetSystemTime();

                string sendWinningDetailsData = JsonUtility.ToJson(winning_details);
                string sendNewData = JsonUtility.ToJson(sendThisPlayerData);

                //Debug.Log(sendNewData + "sendNewData");
                WebRequestHandler.Instance.Post(sendDataURL, sendNewData, (response, status) =>
                {
                    //Debug.Log(response + "HitNewApi");
                });

                isDataSend = true;
                //StartCoroutine(CallLeaveRoom());
            }
        }
    }

    public string GetSystemTime()
    {
        int hr = System.DateTime.Now.Hour;
        int min = System.DateTime.Now.Minute;
        int sec = System.DateTime.Now.Second;

        int year = System.DateTime.Now.Year;
        int month = System.DateTime.Now.Month;
        int day = System.DateTime.Now.Day;

        string format = string.Format("{0}:{1}:{2} {3}:{4}:{5}", year, month, day, hr, min, sec);

        return format;
    }

    public void DeductWallet()
    {
        walletUpdate.amount = AndroidtoUnityJSON.instance.game_fee;
        walletUpdate.game_id = AndroidtoUnityJSON.instance.game_id;
        walletUpdate.type = AndroidtoUnityJSON.instance.game_mode;
        string mydata = JsonUtility.ToJson(walletUpdate);
        WebRequestHandler.Instance.Post(walletUpdateURL, mydata, (response, status) =>
        {
            Debug.Log(response + " sent wallet update");
        });


    }

   

    public void CancelApplication()
    {
        Debug.Log("Quit");
        //hasLeftRoom = true;
        Application.Quit();
    }

    public void ReloadScene()
    {
        //wallet check
        float balance = 0f;

        walletInfoData = JsonUtility.ToJson(walletInfo);
        WebRequestHandler.Instance.Post(walletInfoURL, walletInfoData, (response, status) =>
        {
            WalletInfo walletInfoResponse = JsonUtility.FromJson<WalletInfo>(response);
            balance = float.Parse(walletInfoResponse.data.cash_balance);
            Debug.Log(balance + " <= replay check balance");

            if (balance >= float.Parse(AndroidtoUnityJSON.instance.game_fee))
                canRestart = true;
            else
                canRestart = false;

            if (canRestart)
                SceneManager.LoadScene(0, LoadSceneMode.Single);
            else
                NoBalPop.SetActive(true);

            //Debug.Log("CANT START, NOT ENOUGH BALANCE!"); //show no bal popup
        });

    }

    

    int pauseCounter = 0;
    private void OnApplicationPause(bool pause)
    {
        if (pauseCounter == 0)
        {
            pauseCounter++;
            return;
        }
        else if (isDataSend == false && canStartGame)
        {
            sendThisPlayerData.player_id = otherPlayer.playerId;
            sendThisPlayerData.winning_details.thisplayerScore = gameManager.Bowler.score;
            sendThisPlayerData.wallet_amt = AndroidtoUnityJSON.instance.game_fee;
            sendThisPlayerData.player_id = AndroidtoUnityJSON.instance.player_id;
            sendThisPlayerData.game_mode = AndroidtoUnityJSON.instance.game_mode;

            if (AndroidtoUnityJSON.instance.game_mode == "tour")
                sendThisPlayerData.battle_tournament_id = AndroidtoUnityJSON.instance.tour_id;
            else if (AndroidtoUnityJSON.instance.game_mode == "battle")
                sendThisPlayerData.battle_tournament_id = AndroidtoUnityJSON.instance.battle_id;


            sendThisPlayerData.game_status = "LEFT";

            sendWinningDetailsData = JsonUtility.ToJson(winning_details);
            sendNewData1 = JsonUtility.ToJson(sendThisPlayerData);
            WebRequestHandler.Instance.Post(sendDataURL, sendNewData1, (response, status) =>
            {
                Debug.Log(response + "HitNewApi");
            });

            isDataSend = true;
        }

        Application.Quit();
    }

    //startReloadGameTime();
    //                    // StartCoroutine(CallLeaveRoom());
    //                    sendWinningDetailsData = JsonUtility.ToJson(winningDetails);
    //                    sendNewData1 = JsonUtility.ToJson(sendThisPlayerData);
    //                    //Debug.Log(sendNewData+"sendNewData");
    //                    WebRequestHandler.Instance.Post(sendDataURL, sendNewData1, (response, status) =>
    //                    {
    //                        Debug.Log(response + "HitNewApi");
    //                    });

    //                    WebRequestHandler.Instance.Post(sendDataURL, sendWinningDetailsData, (response, status) =>
    //                    {
    //                        Debug.Log(response + "For Winning Details");
    //                    });

    //                    isDataSend = true;
    //                }
    //                else
    //                {
    //                    Debug.Log("Waiting for other player to finish playing");
    //                }



    //        });
    //    }
    //}



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

    //public void ReloadScene()
    //{
    //    Debug.Log("ChangeScene");
    //    SceneManager.LoadScene(0);

    //}

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
    //    WebRequestHandler.Instance.Get("http://localhost:3000/removeFromRooms/" + myRoomId + "/" + thisPlayer.playerName, (response, status) =>
    //    {
    //        thisPlayer.RoomID = "";
    //        myRoomId = "";
    //        otherPlayer = JsonUtility.FromJson<NetworkingPlayer>(response);
    //        Debug.Log(response);
    //        if (otherPlayer.playerName != null && otherPlayer.playerName != "")
    //        {
    //            foundOtherPlayer = true;
    //            otherText.text = otherPlayer.playerName;
    //        }
    //        otherPlayer = JsonUtility.FromJson<NetworkingPlayer>(response);

    //    });

    //}
}
