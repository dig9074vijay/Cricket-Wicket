using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class SendData1 
{
    public string player_id;
    public string room_id;
    public string game_mode;
    //public string winning_details;//new class to be send as json 
    public string game_end_time;
    public string wallet_amt;//to be sent by base app
    public string game_status;//fully played or left
}
