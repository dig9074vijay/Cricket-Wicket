using System;
using System.Collections;
using System.Collections.Generic;
[Serializable]
public class NetworkingPlayer 
{
    public string gameName;//to be set from editor
    public string playerName;
    public string playerId;
    public string imageURL;
    public string RoomID;
    public int score;
    public bool isGameStarted;
    public bool finishedPlaying;
    public bool iWon = false;
    public int playerDisconnectionCounter = 3;
    public bool isConnected = false;
    public bool isBot = false;
    public float[] incrementFactor;
}


