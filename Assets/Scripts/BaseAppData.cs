using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseAppData : MonoBehaviour
{
    public Text data;
    //string data;
    //public string jsonString;
    //public string jsonString1;

    public string player_id;
    public string token;
    public string wallet_amount;
    public string profile_image;
    public string user_name;
    public string battle_id;
    public string game_id;

    //string s ;

    //public string[] subs ;


   // public AndroidJSON androidJsonData;

    private void Awake()
    {
#if (UNITY_EDITOR)

        player_id = "32323";
        token = "32kjsanjjdnskd434";
        wallet_amount = "4";
        profile_image = "https://picsum.photos/400";
        user_name = "John Doe";
        battle_id = "32";
        game_id = "32";
#endif
        getIntentData();
    }

    private bool getIntentData()
    {
#if (!UNITY_EDITOR && UNITY_ANDROID)
    return CreatePushClass (new AndroidJavaClass ("com.unity3d.player.UnityPlayer"));
#endif
        return false;
    }

    public bool CreatePushClass(AndroidJavaClass UnityPlayer)
    {
#if UNITY_ANDROID


        AndroidJavaObject currentActivity = UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaObject intent = currentActivity.Call<AndroidJavaObject>("getIntent");
        AndroidJavaObject extras = GetExtras(intent);

        if (extras != null)
        {
            //        string jsonString = GetProperty(extras, "data");
            //        Debug.Log(jsonString+"Android data from base app");
            //        //androidJsonData = JsonUtility.FromJson<AndroidJSON>(jsonString);
            //        subs = jsonString.Split(',');

            ////use this => androidJsonData.
            //        text.text = jsonString;
            //        foreach (var sub in subs)
            //        {
            //            Debug.Log(sub+"1");
            //        }

           // string jsonString = GetProperty(extras, "data");

          //  data.text = "Base app data error!";

            player_id = GetProperty(extras, "player_id");
            token = GetProperty(extras, "token");
            user_name = GetProperty(extras, "user_name");
            game_id = GetProperty(extras, "game_id");
            battle_id = GetProperty(extras, "battle_id");
            profile_image = GetProperty(extras, "profile_image");
            wallet_amount = GetProperty(extras, "wallet_amount");

         //   data.text = "";


            return true;
        }
        else
        {
            data.text = "NULL";
        }
#endif
        return false;
    }

    //[Serializable]
    //public class AndroidJSON
    //{
    //    public string player_id;
    //    public string token;
    //    public string wallet_amount;
    //    public string profile_image;
    //    public string user_name;
    //    public string battle_id;
    //    public string game_id;
    //}

    //public void OnClick()
    //{
    //    text.text = androidJsonData.battle_id;
    //}



    #region HELPER CLASSES FOR JAVA

    private AndroidJavaObject GetExtras(AndroidJavaObject intent)
    {
        AndroidJavaObject extras = null;

        try
        {
            extras = intent.Call<AndroidJavaObject>("getExtras");
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }

        return extras;
    }

    private string GetProperty(AndroidJavaObject extras, string name)
    {
        string s = string.Empty;

        try
        {
            s = extras.Call<string>("getString", name);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }

        return s;
    }

    #endregion


}
